using System.Windows.Input;
using BuildingBlocks.Commons;
using MediatR;
using Property.Application.DTOs;
using Property.Domain.Aggregates.RentalUnitAggregate;
using Property.Domain.ValueObjects;
using ICommand = BuildingBlocks.Commons.ICommand;

namespace Property.Application.CQRS.Commands;

public sealed record CreatePropertyCommand
(
    HouseRuleDto houseRuleDto, 
    BasicInformationsDto basicInfoDto,
    RentalUnitDto rentalUnitDto
):ICommand;

public class CreatePropertyCommandHandler : ICommandHandler<CreatePropertyCommand>
{
    public async Task<Result> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
    {
        var basicInfoDto = command.basicInfoDto;
        var houseRuleDto = command.houseRuleDto;
        
        var location = new Location(basicInfoDto.Address, basicInfoDto.City, basicInfoDto.Country, basicInfoDto.PostCode);
        var houseRule = new HouseRule(houseRuleDto.CheckInTimeFrom, houseRuleDto.CheckInTimeUntil,
            houseRuleDto.CheckOutTimeFrom, houseRuleDto.CheckOutTimeUntil, houseRuleDto.PetAllowed, houseRuleDto.SmokingAllowed,
            houseRuleDto.PartyAllowed, houseRuleDto.AgeRestriction);

        var rentalUnit = new EntirePropertyRentalUnit(rentalUnitDto.PricePerNight, 
            rentalUnitDto.MaxGuests, 
            rentalUnitDto.NumberOfBedrooms, 
            rentalUnitDto.NumberOfBeds, 
            rentalUnitDto.NumberOfBathrooms, 
            rentalUnitDto.Amenities, 
            rentalUnitDto.Images
        );
        
        var property = new Domain.Aggregates.AggregateRoot.Property(
            basicInfoDto.Name, 
            basicInfoDto.Description, 
            basicInfoDto.MaxGuests, 
            basicInfoDto.NumberOfBedrooms, 
            basicInfoDto.NumberOfBeds, 
            basicInfoDto.NumberOfBathrooms,
            location, 
            houseRule, 
            rentalUnitDto.RentalType, 
            rentalUnitDto.PricePerNight);
    }
}