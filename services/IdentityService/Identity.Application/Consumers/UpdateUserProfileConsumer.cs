using BuildingBlocks.Commons;
using Contracts.Commands;
using Contracts.Events;
using MassTransit;
using Identity.Application.UseCases.Commands;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Consumers;

public class UpdateUserProfileConsumer(IMediator mediator, IEventBus bus, ILogger<UpdateUserProfileConsumer> logger):IConsumer<UpdateUserProfile>
{
    public async Task Consume(ConsumeContext<UpdateUserProfile> context)
    {
        logger.LogInformation("[IdentityService] Updating User Profile {MessageUserId}", context.Message.UserId);

        var command = new UpdateUserProfileCommand(
            context.Message.UserId,
            context.Message.FullName,
            context.Message.Description
        );

        var result = await mediator.Send(command);

        await bus.PublishAsync(new UserProfileUpdated
        {
            CorrelationId = context.Message.CorrelationId,
            UserId = context.Message.UserId,
            FullName = context.Message.FullName,
            Description = context.Message.Description
        });
    }
}