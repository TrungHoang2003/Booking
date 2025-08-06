using System.Text.Json;
using BuildingBlocks.Interfaces;
using Contracts.Drafts;
using Contracts.Events;
using Contracts.Messages;
using MassTransit;
using Property.Application.Errors;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Domain.Aggregates.AmenityAggregate;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class AddRentalUnitConsumer(IPropertyTypeRepository propertyTypeRepos, IAmenityRepository amenityRepo,
    IPropertyRepository propertyRepo, IUnitOfWork unitOfWork) : IConsumer<AddRentalUnit>
{
    public async Task Consume(ConsumeContext<AddRentalUnit> context)
    {
        var addRentalUnit = context.Message;
        int rentalUnitId;
        
        var unitPrice = new Price(context.Message.Amount, context.Message.Currency);
        
        var listAmenity = new List<Amenity>();

        foreach (var amenityId in context.Message.AmenityIds)
        {
            var amenity = await amenityRepo.GetById(amenityId);
            listAmenity.Add(amenity);
        }
        
        var property = await propertyRepo.GetById(context.Message.PropertyId);
        var propertyType = await propertyTypeRepos.GetById(context.Message.PropertyTypeId);

        if (propertyType.IsRoomBased)
        {
            var roomRentalUnit = new RoomRentalUnit(
                addRentalUnit.Name,
                addRentalUnit.MaxAdults,
                addRentalUnit.MaxChildren,
                addRentalUnit.Quantity,
                addRentalUnit.SharedBathroom,
                unitPrice,
                RentalUnitType.Room());
            
            roomRentalUnit.AddListAmenity(listAmenity);
            property.AddRentalUnit(roomRentalUnit);
        }
        else
        {
            if (addRentalUnit.IsRoomBasedProperty)
                throw new Exception("Entire Property Type cant have Room based rental unit");
            
            var entireRentalUnit = new EntirePropertyRentalUnit(addRentalUnit.MaxAdults,
                addRentalUnit.MaxChildren,
                addRentalUnit.Size,
                addRentalUnit.BedroomsCount,
                addRentalUnit.BathroomsCount,
                unitPrice,
                RentalUnitType.EntireProperty());
            
            entireRentalUnit.AddListAmenity(listAmenity);
            property.AddRentalUnit(entireRentalUnit);
        }

        await propertyRepo.Update(property);
        await unitOfWork.SaveChangesAsync();
        rentalUnitId = property.RentalUnits.First().Id;

        await context.RespondAsync(new RentalUnitAdded
        {
            CorrelationId = context.Message.CorrelationId,
            RentalUnitId = rentalUnitId
        });
    }
}