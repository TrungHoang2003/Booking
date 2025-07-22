using BuildingBlocks.Interfaces;
using MassTransit;

namespace BuildingBlocks.Services;

public class MassTransitEventBus(IPublishEndpoint publishEndpoint) : IEventBus
{
    public async Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default)
    {
       await publishEndpoint.Publish(@event, cancellationToken);
    }
}