using BuildingBlocks.Commons;
using Property.Application.DTOs;
using Property.Application.Errors;
using Property.Domain.Aggregates.AggregateRoot;
using Property.Infrastructure.Repositories;

namespace Property.Application.CQRS.Commands;

public sealed record SetUpPropertyAmenityCommand(
    int propertyId,
    List<SetupPropertyAmenityDto> listSetupPropertyAmenityDto): ICommand;

public class SetUpPropertyAmenityCommandHandler(
    IAmenityRepository amenityRepo, 
    IPropertyRepository propertyRepo, 
    IPropertyAmenityRepository propertyAmenityRepo,
    IUnitOfWork unitOfWork) : ICommandHandler<SetUpPropertyAmenityCommand>
{
    public async Task<Result> Handle(SetUpPropertyAmenityCommand command, CancellationToken cancellationToken)
    {
        var listSetupPropertyAmenityDto = command.listSetupPropertyAmenityDto;

        var listPropertyAmenity = new List<PropertyAmenity>();

        foreach (var setupPropertyAmenityDto in listSetupPropertyAmenityDto)
        {
            var property = await propertyRepo.GetByIdAsync(command.propertyId);
            if (property == null)
            {
                return Result.Failure(PropertyErrors.PropertyNotFound);
            }

            var amenity = await amenityRepo.GetByIdAsync(setupPropertyAmenityDto.amenityId);
            if (amenity == null)
            {
                return Result.Failure(AmenityErrors.AmenityNotFound);
            }

            var propertyAmenity = new PropertyAmenity(property.Id, amenity.Id, setupPropertyAmenityDto.description, setupPropertyAmenityDto.Price);
            listPropertyAmenity.Add(propertyAmenity);
        }

        await propertyAmenityRepo.AddRangeAsync(listPropertyAmenity);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}
