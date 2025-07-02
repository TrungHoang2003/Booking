using System.Windows.Input;
using BuildingBlocks.Commons;
using Property.Application.DTOs;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;
using ICommand = BuildingBlocks.Commons.ICommand;

namespace Property.Application.CQRS.Commands;

public sealed record SignUpPropertyCommand
(
    int hostId,
    HouseRuleDto houseRuleDto, 
    BasicInformationsDto basicInfoDto,
    EntirePropertyRentalUnitDto entireRentalUnitDto,
    HostProfileDto? hostProfileDto 
):ICommand;

public class SignUpPropertyCommandHandler(IPropertyRepository propertyRepo, IUnitOfWork unitOfWork) : ICommandHandler<SignUpPropertyCommand>
{
    public async Task<Result> Handle(SignUpPropertyCommand command, CancellationToken cancellationToken)
    {
        var basicInfoDto = command.basicInfoDto;
        var houseRuleDto = command.houseRuleDto;
        var entireRentalUnitDto= command.entireRentalUnitDto;

        var location = new Location(basicInfoDto.Address, basicInfoDto.City, basicInfoDto.Country, basicInfoDto.PostCode);
        
        var houseRule = new HouseRule(
            houseRuleDto.CheckInTimeFrom, houseRuleDto.CheckInTimeUntil,
            houseRuleDto.CheckOutTimeFrom, houseRuleDto.CheckOutTimeUntil,
            houseRuleDto.PetAllowed, houseRuleDto.SmokingAllowed,
            houseRuleDto.PartyAllowed, houseRuleDto.AgeRestriction);
        
        var rentalUnit = new EntirePropertyRentalUnit(
            null,
            entireRentalUnitDto.Price,
            entireRentalUnitDto.MaxAdults,
            entireRentalUnitDto.MaxChildren,
            entireRentalUnitDto.Size,
            entireRentalUnitDto.BedroomsCount,
            entireRentalUnitDto.BathroomsCount 
        );
        
        var property = new Domain.Aggregates.AggregateRoot.Property(
            basicInfoDto.PropertyTypeId,
            command.hostId, 
            basicInfoDto.Name,
            null,
            null,
            null,
            houseRule,
            location
        );
        property.AddRentalUnit(rentalUnit);

        if (command.hostProfileDto != null)
        {
            var hostProfileDto = command.hostProfileDto;
            property.AddDescription(hostProfileDto.PropertyDescription);
        }
        
        await propertyRepo.Create(property);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}