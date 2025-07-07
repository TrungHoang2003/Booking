using BuildingBlocks.Commons;
using Contracts.Events;
using Identity.Application.CQRS.Commands;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Consumers;

public class UpdateUserProfileConsumer(IEventBus bus, ILogger<UpdateUserProfileConsumer> logger):IConsumer<UpdateUserProfileCommand>
{
    public async Task Consume(ConsumeContext<UpdateUserProfileCommand> context)
    {
       logger.LogInformation("[IdentityService] Updating User Profile {MessageUserId}", context.Message.UserId); 
       
       await bus.PublishAsync(new UpdateUserProfileEvent(
           context.Message.))
    }
}