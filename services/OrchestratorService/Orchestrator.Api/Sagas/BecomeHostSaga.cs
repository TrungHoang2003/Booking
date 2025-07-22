using Contracts.Commands;
using Contracts.Events;
using MassTransit;

namespace Orchestrator.Api.Sagas;

public class BecomeHostSagaData: SagaStateMachineInstance
{
    public Guid CorrelationId { get; set; }
    public CreateProperty Property { get; set; }
    public int CurrentStep { get; set; }
}

public class BecomeHostSaga : MassTransitStateMachine<BecomeHostSagaData>
{
    public State CreatingProperty { get; private set; } = null!;
    public Event<BecomeHostStarted> Started { get; private set; } = null!;
    public Event<PropertyCreated> PropertyCreated { get; private set; } = null!;

    public BecomeHostSaga()
    {
        InstanceState(x => x.CurrentStep);

        Event(() => Started, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => PropertyCreated, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Initially(
            When(Started)
                .Then(context => { 
                    context.Saga.CurrentStep = 1; 
                })
                .TransitionTo(CreatingProperty)
                .Publish(context => context.Saga.Property with
                {
                    CorrelationId = context.Saga.CorrelationId,
                    PropertyTypeId = context.Saga.Property.PropertyTypeId,
                    HostId = context.Message.Saga.HostId,
                    Name = context.Message.Saga.Name,
                    FloorNumber = context.Message.Property.FloorNumber,
                    Address = context.Message.Property.Address,
                    City = context.Message.Property.City,
                    Country = context.Message.Property.Country,
                    PostCode = context.Message.Property.PostCode,
                    CheckInTimeFrom = context.Message.Property.CheckInTimeFrom,
                    CheckInTimeUntil = context.Message.Property.CheckInTimeUntil,
                    CheckOutTimeFrom = context.Message.Property.CheckOutTimeFrom,
                    CheckOutTimeUntil = context.Message.Property.CheckOutTimeUntil,
                    PetAllowed = context.Message.Property.PetAllowed,
                    SmokingAllowed = context.Message.Property.SmokingAllowed,
                    PartyAllowed = context.Message.Property.PartyAllowed,
                    AgeRestriction = context.Message.Property.AgeRestriction
                })
        );
    
        During(CreatingProperty,
            When(PropertyCreated)
                .Then(context => { context.Saga.CurrentStep = 2; })
                .Finalize()
        );
    }
}