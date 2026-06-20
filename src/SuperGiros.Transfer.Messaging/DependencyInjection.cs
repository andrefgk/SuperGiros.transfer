using MassTransit;
using Microsoft.Extensions.DependencyInjection;
using SuperGiros.Transfer.Messaging.Consumers;
using SuperGiros.Transfer.Messaging.Publishers;

namespace SuperGiros.Transfer.Messaging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddMessaging(this IServiceCollection services)
        {
            services.AddMassTransit(cfg =>
            {
                cfg.AddConsumer<TransactionCreatedConsumer>();
                cfg.AddConsumer<CustomerCreatedConsumer>();
                cfg.AddConsumer<TransactionCanceledConsumer>();

                // In-Memory para desarrollo. Para RabbitMQ: reemplaza UsingInMemory por UsingRabbitMq
                cfg.UsingInMemory((context, busCfg) =>
                {
                    busCfg.ConfigureEndpoints(context);
                });
            });

            services.AddScoped<IEventPublisher, EventPublisher>();
            return services;
        }
    }
}
