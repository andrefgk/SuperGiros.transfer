using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SuperGiros.Transfer.Application.UseCases.Commons.Behaviors;
using System.Reflection;


namespace SuperGiros.Transfer.Application.UseCases
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services)
        {
            // 1. Registro de MediatR
            services.AddMediatR(cfg => {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                // 2. REGISTRO DEL BEHAVIOR DE LOGGING
                // Esto hará que cada vez que uses MediatR, se guarde un log en consola
                cfg.AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehaviours<,>));
            });

            // 3. REGISTRO DE AUTOMAPPER
            // Busca automáticamente tu MappingProfile.cs en este ensamblado
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
