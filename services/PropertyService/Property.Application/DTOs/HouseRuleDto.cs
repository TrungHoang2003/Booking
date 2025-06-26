namespace Property.Application.DTOs;

public record HouseRuleDto
(
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