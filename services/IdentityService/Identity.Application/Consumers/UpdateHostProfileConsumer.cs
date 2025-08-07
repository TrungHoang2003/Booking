using BuildingBlocks.Middlewares;
using Contracts.Events;
using Contracts.Messages;
using Identity.Domain.Entities;
using MassTransit;
using Microsoft.AspNetCore.Identity;

namespace Identity.Application.Consumers;

public class UpdateHostProfileConsumer(UserManager<User> userManager) : IConsumer<UpdateHostProfile>
{
    public async Task Consume(ConsumeContext<UpdateHostProfile> context)
    {
        var user = await userManager.FindByIdAsync(context.Message.HostId.ToString());
        if (user == null) throw new BusinessException("Authentication Error", "User not found");

        user.FullName = context.Message.HostName;
        user.Description = context.Message.HostDescription;

        await userManager.UpdateAsync(user);

        await context.RespondAsync(new HostProfileUpdated
        {
            CorrelationId = context.Message.CorrelationId
        });
    }
}