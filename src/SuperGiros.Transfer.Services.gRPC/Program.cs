using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SuperGiros.Transfer.Application.UseCases;
using SuperGiros.Transfer.Messaging;
using SuperGiros.Transfer.Persistence;
using SuperGiros.Transfer.Persistence.Contexts;
using SuperGiros.Transfer.Services.gRPC.Commons.Auth;
using SuperGiros.Transfer.Services.gRPC.Commons.GlobalExceptions;
using SuperGiros.Transfer.Services.gRPC.Services;
using System.Text;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 1. KESTREL: HTTP1 para Swagger/REST + HTTP2 para gRPC
builder.WebHost.ConfigureKestrel(options => {
    options.ListenAnyIP(5220, o => o.Protocols = HttpProtocols.Http1AndHttp2); // Antes 5240
    options.ListenAnyIP(5221, o => o.Protocols = HttpProtocols.Http2); // Antes 5241
});

// =========================================================
// 🔴 NUEVO: CONFIGURACIÓN DE CORS PARA REACT
// =========================================================
builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactPolicy", policy =>
    {
        // Para entorno de Minikube permite cualquier origen, o pon explícitamente la IP de Minikube
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .WithExposedHeaders("Grpc-Status", "Grpc-Message", "Grpc-Encoding", "Grpc-Accept-Encoding");
    });
});
// =========================================================

// 2. JWT con eventos de error personalizados (mensajes claros en Postman)
builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("JwtSettings"));
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options => {
        var jwtSettings = builder.Configuration.GetSection("JwtSettings").Get<JwtSettings>();
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings!.Issuer,
            ValidAudience = jwtSettings.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.SecretKey)),
            ClockSkew = TimeSpan.Zero
        };

        // ✅ CORRECCIÓN: Mensajes JSON claros cuando falla la autenticación REST
        options.Events = new JwtBearerEvents
        {
            OnChallenge = async context =>
            {
                // Evita la respuesta por defecto (que abre Visual Studio o muestra HTML)
                context.HandleResponse();
                context.Response.StatusCode = 401;
                context.Response.ContentType = "application/json";
                var mensaje = string.IsNullOrEmpty(context.ErrorDescription)
                    ? "Token JWT requerido. Genera uno en POST /api/Auth/login"
                    : $"Token inválido o expirado: {context.ErrorDescription}. Genera uno nuevo en POST /api/Auth/login";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "No autorizado",
                        mensaje,
                        status = 401,
                        hint = "Envía el header: Authorization: Bearer <tu_token>   "
                    }));
            },
            OnForbidden = async context =>
            {
                // ✅ CORRECCIÓN: Mensaje claro cuando el token es válido pero el rol no alcanza
                context.Response.StatusCode = 403;
                context.Response.ContentType = "application/json";
                await context.Response.WriteAsync(
                    System.Text.Json.JsonSerializer.Serialize(new
                    {
                        error = "Acceso denegado",
                        mensaje = "No tienes permisos suficientes para este recurso. Se requiere rol Admin.",
                        status = 403,
                        hint = "Inicia sesión con una cuenta Admin para realizar esta operación"
                    }));
            }
        };
    });
builder.Services.AddAuthorization();

// 3. gRPC + Interceptores
builder.Services.AddScoped<GlobalExceptionHandler>();
builder.Services.AddScoped<AuthInterceptor>();
builder.Services.AddGrpc(options =>
{
    options.Interceptors.Add<GlobalExceptionHandler>();
    options.Interceptors.Add<AuthInterceptor>();
}).AddJsonTranscoding();

// 4. Capas de la aplicación
builder.Services.AddApplicationLayer();
builder.Services.AddPersistenceLayer(builder.Configuration);

// 5. Mensajería EVENT-DRIVEN (MassTransit In-Memory)
builder.Services.AddMessaging();

// 6. AutoMapper + Swagger
builder.Services.AddAutoMapper(
    typeof(SuperGiros.Transfer.Application.UseCases.Commons.Mapping.MappingProfile),
    typeof(SuperGiros.Transfer.Services.gRPC.Commons.Mappings.MappingProfile));

builder.Services.AddControllers();
builder.Services.AddGrpcSwagger();
builder.Services.AddSwaggerGen(c => {
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "SuperGiros API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Ingresa tu token JWT. Obtenlo en POST /api/Auth/login"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement {
        {
            new OpenApiSecurityScheme {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});

var app = builder.Build();

// Pipeline
// if (app.Environment.IsDevelopment())
// {
app.UseSwagger();
app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SuperGiros v1"));
// }

// =========================================================
// 🔴 NUEVO: APLICAR CORS Y GRPC-WEB (ANTES DE AUTHENTICATION)
// =========================================================
app.UseCors("ReactPolicy");
app.UseGrpcWeb(new GrpcWebOptions { DefaultEnabled = true });
// =========================================================

app.UseAuthentication();
app.UseAuthorization();

// =========================================================
// 🔴 NUEVO: AÑADIR .EnableGrpcWeb() A TUS SERVICIOS EXISTENTES
// =========================================================
app.MapGrpcService<UserService>().EnableGrpcWeb();
app.MapGrpcService<CustomerService>().EnableGrpcWeb();
app.MapGrpcService<OfficeService>().EnableGrpcWeb();
app.MapGrpcService<TransactionService>().EnableGrpcWeb();
app.MapGrpcService<GreeterService>().EnableGrpcWeb();
// =========================================================

app.MapControllers();
app.MapGet("/", () => "✅ SuperGiros Transfer Service activo");

// Auto-migración al iniciar en Docker
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate(); // Esto crea las tablas automáticamente
}

app.Run();