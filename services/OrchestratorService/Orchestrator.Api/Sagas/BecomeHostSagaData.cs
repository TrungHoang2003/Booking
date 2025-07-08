using MassTransit;

namespace Orchestrator.Api.Sagas;

public class BecomeHostSagaData: SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public int PropertyId { get; set; }
    public string CurrentState { get; set; }
    public bool PropertyCreated { get; set; }
    public bool UserProfileUpdated { get; set; }
}