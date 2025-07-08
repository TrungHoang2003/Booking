namespace Contracts.Commands;

public class CreateProperty
{
    public Guid CorrelationId { get; init; }
    public string PropertyName { get; init; }
    public string Description { get; init; }
}