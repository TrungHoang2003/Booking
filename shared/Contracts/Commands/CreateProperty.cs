namespace Contracts.Commands;

public record CreateProperty
(
    Guid CorrelationId,
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
    int FloorNumber
);