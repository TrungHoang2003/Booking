namespace Shared.Contracts.Events;

public class PropertyDetailsSagaData
{
    public Guid CorrelationId { get; set; }
    public int UserId { get; set; }
    public int PropertyTypeId { get; set; }
    public DateTime StartedAt { get; set; }

    // Current step in the saga
    public int CurrentStep { get; set; } = 0;

    // Draft data for each step
    public string PropertyNameDraft { get; set; }
    public LocationData LocationDraft { get; set; }
    public string HouseRulesDraft { get; set; }
    public string AmenitiesDraft { get; set; }
}