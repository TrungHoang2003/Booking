namespace Contracts.DTOs;

public record BedroomDto(
    int SingleBedCount,
    int DoubleBedCount,
    int KingBedCount,
    int SofaBedCount
    );