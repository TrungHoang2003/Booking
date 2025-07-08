using Contracts.Commands;
using Contracts.Events;
using MassTransit;

namespace Orchestrator.Api.Sagas;

public class BecomeHostSaga: MassTransitStateMachine<BecomeHostSagaData>
{
    public State CreateProperty { get; set; }
    public State AddRentalUnit { get; set; }
    public State UpdateUserProfile { get; set; }
    
    public Event<PropertyCreated> PropertyCreated { get; set; }
    public Event<RentalUnitAdded> RentalUnitAdded { get; set; }
    public Event<UserProfileUpdated> UserProfileUpdated { get; set; }
   
    public BecomeHostSaga()
    {
        InstanceState(x => x.CurrentState);

        Event(() => UserProfileUpdated, x => 
            x.CorrelateById(context => context.Message.CorrelationId));

        Initially(
            When(PropertyCreated)
                .Then(context =>
                {
                    context.Saga.PropertyId = context.Message.PropertyId;
                })
                .TransitionTo(CreateProperty)
        );
    }
}