using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Property.Application.DTOs;
using Property.Application.Errors;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.UseCases.Commands;

public sealed record SetUpPropertyAmenityCommand(
    int PropertyId,
    List<SetupPropertyAmenityDto> ListSetupPropertyAmenityDto): ICommand;

public class SetUpPropertyAmenityCommandHandler(
    IAmenityRepository amenityRepo, 
    IPropertyRepository propertyRepo, 
    IPropertyAmenityRepository propertyAmenityRepo,
    IUnitOfWork unitOfWork) : ICommandHandler<SetUpPropertyAmenityCommand>
{
    public async Task<Result> Handle(SetUpPropertyAmenityCommand command, CancellationToken cancellationToken)
    {
        var listSetupPropertyAmenityDto = command.ListSetupPropertyAmenityDto;

        var listPropertyAmenity = new List<PropertyAmenity>();

        foreach (var setupPropertyAmenityDto in listSetupPropertyAmenityDto)
        {
            var property = await propertyRepo.GetByIdAsync(command.PropertyId);
            if (property == null)
                return Result.Failure(PropertyErrors.PropertyNotFound);

            var amenity = await amenityRepo.GetByIdAsync(setupPropertyAmenityDto.AmenityId);
            if (amenity == null)
                return Result.Failure(AmenityErrors.AmenityNotFound);

            var propertyAmenity = new PropertyAmenity(property.Id, amenity.Id, setupPropertyAmenityDto.Description);
            var price = new Price(setupPropertyAmenityDto.Amount, setupPropertyAmenityDto.Currency);
            propertyAmenity.SetPrice(price); 
            
            listPropertyAmenity.Add(propertyAmenity);
        }

        await propertyAmenityRepo.AddRangeAsync(listPropertyAmenity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
