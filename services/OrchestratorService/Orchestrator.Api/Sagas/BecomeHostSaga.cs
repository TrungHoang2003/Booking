using Contracts.Events;
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
    private readonly ILogger<BecomeHostSaga> _logger;
    
    // States
    private State CreatingProperty { get; set; }
    private State AddingRentalUnit { get; set; }
    private State AddingBedroom { get; set; }
    private State UpdatingHostProfile { get; set; }
    
    // Events
    private Event<BecomeHostStarted> Started { get; set; }
    private Event<PropertyCreated> PropertyCreated { get; set; }
    private Event<RentalUnitAdded> RentalUnitAdded { get; set; }
    private Event<BedroomAdded> BedroomAdded { get; set; }
    private Event<HostProfileUpdated> HostProfileUpdated { get; set; }
    
    //Failed Events
    private Event<Fault<CreateProperty>> CreatePropertyFailed { get; set; }
    private Event<Fault<UpdateHostProfile>> UpdateHostProfileFailed { get; set; }
    private Event<Fault<AddRentalUnit>> AddRentalUnitFailed { get; set; }
    private Event<Fault<AddBedroom>> AddBedroomFailed { get; set; }

    public BecomeHostSaga(ILogger<BecomeHostSaga> logger)
    {
        _logger = logger;
        InstanceState(x => x.CurrentState);

        Event(() => Started, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => PropertyCreated, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => RentalUnitAdded, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => BedroomAdded, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => HostProfileUpdated, x
            => x.CorrelateById(m => m.Message.CorrelationId));

        Event(() => CreatePropertyFailed, x =>
            x.CorrelateById(m => m.Message.Message.CorrelationId));
        
        Event(() => AddRentalUnitFailed, x
        => x.CorrelateById(m=>m.Message.Message.CorrelationId));

        Event(() => AddBedroomFailed, x
            => x.CorrelateById(m => m.Message.Message.CorrelationId));
        
        Event(() => UpdateHostProfileFailed, x
            => x.CorrelateById(m=>m.Message.Message.CorrelationId));

        Initially(
            When(Started)
                .Then(context =>
                {
                    _logger.LogInformation("Saga started");
                    context.Saga.HostId = context.Message.HostId;
                })
                .TransitionTo(CreatingProperty)
                .Publish(context => context.Saga.Draft.ToCreateProperty())
        );

        During(CreatingProperty,
            When(PropertyCreated)
                .Then(context => { context.Saga.PropertyId = context.Message.PropertyId; })
                .TransitionTo(AddingRentalUnit)
                .Publish(context => context.Saga.Draft.ToAddRentalUnit(context.Saga.PropertyId)),
            When(CreatePropertyFailed)
                .Then(ctx =>
                {
                    var reason = ctx.Message.Exceptions.FirstOrDefault()?.Message;
                    _logger.LogError("Create Property Failed: {reason}", reason);
                }).Finalize()
        );

        During(AddingRentalUnit,
            When(RentalUnitAdded)
                .TransitionTo(AddingBedroom)
                .Publish(context => context.Saga.Draft.ToAddBedroom(context.Message.RentalUnitId)),
            When(AddRentalUnitFailed)
                .Then(ctx =>
                {
                    var reason = ctx.Message.Exceptions.FirstOrDefault()?.Message;
                    _logger.LogError("Add rental unit failed: {reason}", reason);
                }).Finalize()
        );

        During(AddingBedroom,
            When(BedroomAdded)
                .TransitionTo(UpdatingHostProfile)
                .Publish(context => context.Saga.Draft.ToUpdateHostProfile(context.Saga.HostId)),
            When(AddBedroomFailed)
                .Then(context =>
                {
                    var reason = context.Message.Exceptions.FirstOrDefault()?.Message;
                    _logger.LogError("Add bedroom failed: {reason}", reason);
                }).Finalize());

        During(UpdatingHostProfile,
            When(HostProfileUpdated)
                .Then(context =>
                {
                    _logger.LogInformation("Saga completed");
                })
                .Finalize(),
            When(UpdateHostProfileFailed).Then(context =>
            {
                var reason = context.Message.Exceptions.FirstOrDefault()?.Message;
                _logger.LogInformation("Host profile update failed: {reason}", reason);
            }).Finalize());

        SetCompletedWhenFinalized();
    }
}