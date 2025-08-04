using BuildingBlocks.Interfaces;
using Contracts.DTOs;

namespace Contracts.Messages;

public class AddBedroom: IMessage
{
    public Guid CorrelationId { get; set; }
    public int RentalUnitId { get; set; }
    public List<BedroomDto> ListBedrooms { get; set; }
}