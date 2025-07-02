namespace BuildingBlocks.Commons;

public interface IEventBus
{
    // TEvent là kiểu của sự kiện, phải là một class.
    Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken = default);
}