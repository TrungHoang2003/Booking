using BuildingBlocks.Interfaces;

namespace Contracts.Messages;

public class AddRentalUnit : IMessage
{
    public Guid CorrelationId { get; set; }
    public string Name { get; set; }
    public int PropertyId { get; set; }
    public int MaxAdults { get; set; }
    public int MaxChildren { get; set; }
    public int Size { get; set; }
    public int BedroomsCount { get; set; }
    public int BathroomsCount { get; set; }
    public bool IsRoomBasedProperty { get; set; }
    public int Quantity { get; set; }
    public bool SharedBathroom { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; }
    public List<int>? AmenityIds { get; set; }
}