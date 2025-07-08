using BuildingBlocks.Commons;
using Contracts.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Property.Application.Consumers;

public class CreatePropertyConsumer(ILogger<CreatePropertyConsumer> logger, IEventBus bus): IConsumer<CreateProperty>
{
    public async Task Consume(ConsumeContext<CreateProperty> context)
    {
        
        
    }
}