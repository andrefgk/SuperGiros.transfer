using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SuperGiros.Transfer.Persistence.Contexts;
using SuperGiros.Transfer.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;
using SuperGiros.Transfer.Persistence.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using SuperGiros.Transfer.Application.Interfaces.Persistence;
using SuperGiros.Transfer.Application.Interfaces.Persistence.Repositories;

namespace SuperGiros.Transfer.Persistence
{
    public static class DependencyInjection
    {
        // Cambiamos el nombre a uno estándar y corregimos la ortografía
        public static IServiceCollection AddPersistenceLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("TransferConnection"),
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

            services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());

            // Registro del repositorio de usuarios (Estilo Pacagroup)
            services.AddScoped<IUserRepository, UserRepository>();

            return services;
        }
    }
}
