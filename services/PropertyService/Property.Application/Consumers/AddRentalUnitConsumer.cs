using BuildingBlocks.Interfaces;
using Contracts.Events;
using Contracts.Messages;
using MassTransit;
using Property.Application.Errors;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class AddRentalUnitConsumer(IAmenityRepository amenityRepo, IPropertyRepository propertyRepository, IUnitOfWork unitOfWork) : IConsumer<AddRentalUnit>
{
    public async Task Consume(ConsumeContext<AddRentalUnit> context)
    {
        int rentalUnitId;
        
        var addRentalUnit = context.Message;
        var unitPrice = new Price(context.Message.Amount, context.Message.Currency);
        
        var listAmenity = new List<Amenity>();

        foreach (var amenityId in context.Message.AmenityIds)
        {
            var amenity = await amenityRepo.GetByIdAsync(amenityId) ?? throw new Exception("Amenity not found");
            listAmenity.Add(amenity);
        }
        
        var property = await propertyRepository.GetByIdAsync(context.Message.PropertyId);
        if (property is null) throw new Exception("Property not found");

        if (property.Type.IsRoomBased)
        {
            var roomRentalUnit = new RoomRentalUnit(
                addRentalUnit.Name,
                addRentalUnit.MaxAdults,
                addRentalUnit.MaxChildren,
                addRentalUnit.Quantity,
                addRentalUnit.SharedBathroom);
            
            roomRentalUnit.SetPrice(unitPrice);
            roomRentalUnit.AddListAmenity(listAmenity);
            property.AddRentalUnit(roomRentalUnit);

            rentalUnitId = roomRentalUnit.Id;
        }
        else
        {
            if (addRentalUnit.IsRoomBasedProperty)
                throw new Exception("Entire Property Type cant have Room based rental unit");
            
            var entireRentalUnit = new EntirePropertyRentalUnit(addRentalUnit.MaxAdults,
                addRentalUnit.MaxChildren,
                addRentalUnit.Size,
                addRentalUnit.BedroomsCount,
                addRentalUnit.BathroomsCount);

            entireRentalUnit.AddListAmenity(listAmenity);
            property.AddRentalUnit(entireRentalUnit);

            rentalUnitId = entireRentalUnit.Id;
        }

        await propertyRepository.Update(property);
        await unitOfWork.SaveChangesAsync();
        
        await context.RespondAsync(new RentalUnitAdded
        {
            CorrelationId = context.Message.CorrelationId,
            RentalUnitId = rentalUnitId
        });
    }
}