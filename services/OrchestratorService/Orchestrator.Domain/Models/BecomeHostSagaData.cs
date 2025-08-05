using Contracts.Drafts;
using MassTransit;

namespace Orchestrator.Domain.Models;

public class BecomeHostSagaData: SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public string CurrentState { get; set; }
    public int HostId { get; set; }
    public int PropertyId { get; set; }
    
    // Lưu toàn bộ draft data
    public BecomeHostDraft Draft { get; set; }
}
