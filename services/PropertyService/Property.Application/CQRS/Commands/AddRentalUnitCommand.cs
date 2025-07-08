using BuildingBlocks.Commons;
using Property.Application.Errors;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.CQRS.Commands;

public record AddRentalUnitCommand(
    int PropertyId,
    int MaxAdults,
    int MaxChildren,
    int Size,
    int BedroomsCount,
    int BathroomsCount,
    Price Price
    ):ICommand;

public class AddRentalUnitCommandHandler(IPropertyRepository propertyRepository, IUnitOfWork unitOfWork): ICommandHandler<AddRentalUnitCommand>
{
    public async Task<Result> Handle(AddRentalUnitCommand request, CancellationToken cancellationToken)
    {
        var rentalUnit = new EntirePropertyRentalUnit(
            null,
            request.Price,
            request.MaxAdults,
            request.MaxChildren,
            request.Size,
            request.BedroomsCount,
            request.BathroomsCount 
        );
        
        var property = await propertyRepository.GetByIdAsync(request.PropertyId);
        if (property is null) return PropertyErrors.PropertyNotFound;
        
        property.AddRentalUnit(rentalUnit);
        await propertyRepository.Update(property);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}