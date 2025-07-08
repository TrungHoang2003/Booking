using Property.Domain.ValueObjects;

namespace Property.Application.DTOs;

public record BasicInformationsDto
(
    string Name,
    int PropertyTypeId,
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