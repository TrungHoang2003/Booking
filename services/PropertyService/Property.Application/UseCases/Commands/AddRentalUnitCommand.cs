using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Property.Application.Errors;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.UseCases.Commands;

public record AddRentalUnitCommand(
    int PropertyId,
    int MaxAdults,
    int MaxChildren,
    int Size,
    int BedroomsCount,
    int BathroomsCount,
    bool IsRoomBasedProperty,
    int Quantity,
    bool SharedBathroom,
    Price Price
    ):ICommand;

public class AddRentalUnitCommandHandler(
    IPropertyRepository propertyRepository,
    IUnitOfWork unitOfWork) : ICommandHandler<AddRentalUnitCommand>
{
    public async Task<Result> Handle(AddRentalUnitCommand command, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdAsync(command.PropertyId);
        if (property is null) return PropertyErrors.PropertyNotFound;

        if (command.IsRoomBasedProperty)
        {
            var roomRentalUnit = new RoomRentalUnit(
                null,
                command.Price,
                command.MaxAdults,
                command.MaxChildren,
                command.Quantity,
                command.SharedBathroom);

            property.AddRentalUnit(roomRentalUnit);
        }
        else
        {
            var entireRentalUnit = new EntirePropertyRentalUnit(
                command.Price,
                command.MaxAdults,
                command.MaxChildren,
                command.Size,
                command.BedroomsCount,
                command.BathroomsCount);

            property.AddRentalUnit(entireRentalUnit);
        }

        await propertyRepository.Update(property);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}