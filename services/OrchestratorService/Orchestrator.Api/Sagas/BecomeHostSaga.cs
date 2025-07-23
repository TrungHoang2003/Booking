using Contracts.Events;
using Contracts.DTOs;
using MassTransit;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.Extensions;

namespace Orchestrator.Api.Sagas;

public class BecomeHostSagaData: SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; } = null!;
    public int HostId { get; set; }
    
    // Lưu toàn bộ draft data
    public BecomeHostDraft Draft { get; set; }
}

public class BecomeHostSaga : MassTransitStateMachine<BecomeHostSagaData>
{
    private State CreatingProperty { get; set; } = null!;
    private State AddingRentalUnit { get; set; } = null!;
    private Event<BecomeHostStarted> Started { get; set; } = null!;
    private Event<PropertyCreated> PropertyCreated { get; set; } = null!;
    private Event<RentalUnitAdded> RentalUnitAdded { get; set; } = null!;

    public BecomeHostSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => Started, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => PropertyCreated, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => RentalUnitAdded, x
            => x.CorrelateById(m => m.Message.CorrelationId)); 

        Initially(
            When(Started)
                .Then(context => { context.Saga.HostId = context.Message.HostId; })
                .TransitionTo(CreatingProperty)
                .Publish(context => context.Saga.Draft.ToCreateProperty())
        );
        
        During(CreatingProperty,
            When(PropertyCreated)
                .TransitionTo(AddingRentalUnit)
                .Publish(context=>context.Saga.Draft.ToAddRentalUnit())
        );
        
        During(AddingRentalUnit,
            When(RentalUnitAdded)
                .Finalize()
        );

        SetCompletedWhenFinalized();
    }
Ư