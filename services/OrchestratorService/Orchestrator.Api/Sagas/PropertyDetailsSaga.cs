using MassTransit;
using Shared.Contracts.Events;

namespace Orchestrator.Api.Sagas;

public class PropertyDetailsSaga
{
    public State CollectingPropertyName { get; set; }
    public State CollectingPropertyLocation { get; set; }
    public State CollectingHouseRules { get; set; }
    public State CollectingAmenities { get; set; }
    public State CollectingRentalUnit { get; set; }

    public Event<BecomeHostStarted> BecomeHostStarted { get; set; }
    public Event<PropertyNameSubmitted> PropertyNameSubmitted { get; set; }
    public Event<LocationAdded> LocationAdded{ get; set; } 
    public Event<HouseRulesAdded> HouseRulesAdded { get; set; }
}

