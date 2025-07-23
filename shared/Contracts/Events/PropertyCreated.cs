namespace Contracts.Events;

public record PropertyCreated
(
   Guid CorrelationId,
   int PropertyId
);