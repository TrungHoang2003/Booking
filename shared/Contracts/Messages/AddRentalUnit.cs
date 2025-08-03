namespace Contracts.Messages;

public record AddRentalUnit(
    string Name,
    Guid CorrelationId,
    int PropertyId,
    int MaxAdults,
    int MaxChildren,
    int Size,
    int BedroomsCount,
    int BathroomsCount,
    bool IsRoomBasedProperty,
    int Quantity,
    bool SharedBathroom,
    decimal Amount,
    string Currency,
    List<int> AmenityIds);