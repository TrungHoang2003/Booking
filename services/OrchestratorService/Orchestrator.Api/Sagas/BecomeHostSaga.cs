using Contracts.Events;
using Contracts.DTOs;
using Contracts.Messages;
using MassTransit;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.Extensions;

namespace Orchestrator.Api.Sagas;

public class BecomeHostSagaData: SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public State CurrentState { get; set; } = null!;
    public int HostId { get; set; }
    public int PropertyId { get; set; }
    
    // Lưu toàn bộ draft data
    public BecomeHostDraft Draft { get; set; }
}

public class BecomeHostSaga : MassTransitStateMachine<BecomeHostSagaData>
{
    private State CreatingProperty { get; set; }
    private State AddingRentalUnit { get; set; }
    private State UpdatingHostProfile { get; set; }
    private Event<BecomeHostStarted> Started { get; set; }
    private Event<PropertyCreated> PropertyCreated { get; set; }
    private Event<RentalUnitAdded> RentalUnitAdded { get; set; }
    private Event<HostProfileUpdated> HostProfileUpdated { get; set; }

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
                .Then(context => { context.Saga.PropertyId = context.Message.PropertyId; })
                .TransitionTo(AddingRentalUnit)
                .Publish(context => context.Saga.Draft.ToAddRentalUnit(context.Saga.PropertyId))
        );

        During(AddingRentalUnit,
            When(RentalUnitAdded)
                .Publish(context => new UpdateHostProfile
                (
                    context.Saga.CorrelationId,
                    context.Saga.HostId,
                    context.Saga.Draft.HostProfileDto?.HostDescription,
                    context.Saga.Draft.HostProfileDto?.HostName
                ))
        );
        
        During(UpdatingHostProfile,
            When(HostProfileUpdated)
                .Publish(context=> new UpdateB))
        
        SetCompletedWhenFinalized();
    }
}