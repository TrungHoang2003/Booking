namespace Contracts.Commands;

public class CreateAccommodation
{
    public Guid CorrelationId { get; init; }
    public required string AccommodationName { get; init; }
    public required string Description { get; init; }
}