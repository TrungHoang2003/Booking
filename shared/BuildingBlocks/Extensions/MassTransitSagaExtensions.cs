using BuildingBlocks.Commons;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Extensions;

public static class MassTransitSagaExtensions
{
    /// <summary>
    /// Add MassTransit with RabbitMQ and Entity Framework Saga support
    /// </summary>
    /// <typeparam name="TDbContext">DbContext type for saga persistence</typeparam>
    /// <param name="services">Service collection</param>
    /// <param name="configure">Action to configure MassTransit (register consumers, sagas, etc.)</param>
    /// <returns>Service collection for chaining</returns>
    public static IServiceCollection AddMassTransitWithRabbitMqAndSaga<TDbContext>(
        this IServiceCollection services,
        Action<IBusRegistrationConfigurator> configure)
        where TDbContext : DbContext
    {
        services.AddMassTransit(busConfigurator =>
        {
            configure(busConfigurator);
            
            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                var settings = context.GetRequiredService<MessageBrokerSettings>();
                
                cfg.Host(new Uri(settings.Host), h =>
                {
                    h.Username(settings.Username);
                    h.Password(settings.Password);
                });
                
                // Configure endpoints for consumers and sagas
                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }

    /// <summary>
    /// Helper method to configure Saga with Entity Framework
    /// </summary>
    /// <typeparam name="TSaga">Saga state machine type</typeparam>
    /// <typeparam name="TSagaData">Saga data type</typeparam>
    /// <typeparam name="TDbContext">DbContext type</typeparam>
    /// <param name="configurator">Bus registration configurator</param>
    /// <param name="concurrencyMode">Concurrency mode for saga</param>
    /// <returns>Saga configuration</returns>
    public static ISagaRegistrationConfigurator<TSagaData> AddSagaWithEntityFramework<TSaga, TSagaData, TDbContext>(
        this IBusRegistrationConfigurator configurator,
        ConcurrencyMode concurrencyMode = ConcurrencyMode.Pessimistic)
        where TSaga : MassTransitStateMachine<TSagaData>
        where TSagaData : class, SagaStateMachineInstance
        where TDbContext : DbContext
    {
        return configurator.AddSagaStateMachine<TSaga, TSagaData>()
            .EntityFrameworkRepository(r =>
            {
                r.ConcurrencyMode = concurrencyMode;
                r.UsePostgres();
                r.ExistingDbContext<TDbContext>();
            });
    }
}
