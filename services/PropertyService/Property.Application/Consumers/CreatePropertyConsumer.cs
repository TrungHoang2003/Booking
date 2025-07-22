using Contracts.Commands;
using Contracts.Events;
using MassTransit;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class CreatePropertyConsumer(IPropertyRepository repo) : IConsumer<CreateProperty>
{
    public async Task Consume(ConsumeContext<CreateProperty> context)
    {
        var property = new Domain.Aggregates.AggregateRoot.Property(
            context.Message.PropertyTypeId,
            context.Message.HostId,
            context.Message.Name,
            null,
            context.Message.FloorNumber,
            null,
            new HouseRule(
                context.Message.CheckInTimeFrom,
                context.Message.CheckInTimeUntil,
                context.Message.CheckOutTimeFrom,
                context.Message.CheckOutTimeUntil,
                context.Message.PetAllowed,
                context.Message.SmokingAllowed,
                context.Message.PartyAllowed,
                context.Message.AgeRestriction),
            new Location(
                context.Message.Address,
                context.Message.City,
                context.Message.Country,
                context.Message.PostCode)
        );
        await repo.Create(property);

        await context.RespondAsync<PropertyCreated>(new
        {
            PropertyId = property.Id,
            Correlationid = context.Message.CorrelationId,
        });
    }
}