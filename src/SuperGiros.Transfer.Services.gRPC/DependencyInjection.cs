using Microsoft.Extensions.DependencyInjection;
using SuperGiros.Transfer.Services.gRPC.Commons.Auth;
using SuperGiros.Transfer.Services.gRPC.Commons.GlobalExceptions;
using System.Reflection;

namespace SuperGiros.Transfer.Services.gRPC
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPresentationLayer(this IServiceCollection services)
        {
            // 1. Registro de AutoMapper cargando AMBOS perfiles
            services.AddAutoMapper(cfg => {
                // Carga los mapeos de la capa gRPC (Requests -> Commands)
                cfg.AddMaps(Assembly.GetExecutingAssembly());

                // Carga los mapeos de la capa Application (Entidades -> DTOs)
                // Reemplaza 'CreateUserCommand' por cualquier clase que esté en tu proyecto Application
                cfg.AddMaps(typeof(SuperGiros.Transfer.Application.UseCases.Features.Users.Commands.CreateUser.CreateUserCommand).Assembly);
            });

            // 2. Registro de gRPC con Interceptores
            services.AddGrpc(options =>
            {
                options.Interceptors.Add<GlobalExceptionHandler>();
                options.Interceptors.Add<AuthInterceptor>();
            });

            services.AddGrpcSwagger();

            return services;
        }
    }
}
