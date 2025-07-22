using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;
using ICommand = BuildingBlocks.Interfaces.ICommand;
using Interfaces_ICommand = BuildingBlocks.Interfaces.ICommand;

namespace Property.Application.UseCases.Commands;

public sealed record CreatePropertyCommand
(
    int HostId,
    string Name,
    int PropertyTypeId,
    string Address,
    string City,
    string Country,
    int PostCode,
    TimeSpan CheckInTimeFrom,
    TimeSpan CheckInTimeUntil,
    TimeSpan CheckOutTimeFrom,
    TimeSpan CheckOutTimeUntil,
    bool PetAllowed,
    bool SmokingAllowed,
    bool PartyAllowed,
    int AgeRestriction,
    string? NeighborhoodDescription,
    int FloorNumber
):Interfaces_ICommand;

public class CreatePropertyCommandHandler(IPropertyRepository propertyRepo, IUnitOfWork unitOfWork) : ICommandHandler<CreatePropertyCommand>
{
    public async Task<Result> Handle(CreatePropertyCommand command, CancellationToken cancellationToken)
    {
        var houseRule = new HouseRule(
            command.CheckInTimeFrom, command.CheckInTimeUntil,
            command.CheckOutTimeFrom, command.CheckOutTimeUntil,
            command.PetAllowed, command.SmokingAllowed,
            command.PartyAllowed, command.AgeRestriction);
       
        var location = new Location(
            command.Address, command.City, command.Country, command.PostCode);
        
        var property = new Domain.Aggregates.AggregateRoot.Property(
            command.PropertyTypeId,
            command.HostId, 
            command.Name,
            null,
            null,
            null,
            houseRule,
            location
        );

        await propertyRepo.Create(property);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        
        return Result.Success();
    }
}