using BuildingBlocks.Interfaces;
using Contracts.Messages;
using MassTransit;
using Property.Application.Errors;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class AddRentalUnitConsumer(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork) : IConsumer<AddRentalUnit>
{
    public async Task Consume(ConsumeContext<AddRentalUnit> context)
    {
        var price = new Price(context.Message.Amount, context.Message.Currency);

        var property = await propertyRepository.GetByIdAsync(context.Message.PropertyId);
        if (property is null) throw new Exception("Property not found");

        if (context.Message.IsRoomBasedProperty)
        {
            var roomRentalUnit = new RoomRentalUnit(
                null,
                price,
                context.Message.MaxAdults,
                context.Message.MaxChildren,
                context.Message.Quantity,
                context.Message.SharedBathroom);

            property.AddRentalUnit(roomRentalUnit);
        }
        else
        {
            var entireRentalUnit = new EntirePropertyRentalUnit(
                price,
                context.Message.MaxAdults,
                context.Message.MaxChildren,
                context.Message.Size,
                context.Message.BedroomsCount,
                context.Message.BathroomsCount);

            property.AddRentalUnit(entireRentalUnit);
        }

        await propertyRepository.Update(property);
        await unitOfWork.SaveChangesAsync();
    }
}