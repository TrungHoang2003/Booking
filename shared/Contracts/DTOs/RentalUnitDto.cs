namespace Contracts.DTOs;

public record RentalUnitDto
(
    string Name,
    int MaxAdults,
    int MaxChildren,
    int Size,
    int BedroomsCount,
    int BathroomsCount,
    bool IsRoomBasedProperty,
    int Quantity,
    bool SharedBathroom,
    decimal Amount,
    string Currency
);