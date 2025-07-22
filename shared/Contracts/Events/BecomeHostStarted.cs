using Contracts.Commands;

namespace Contracts.Events;

public record BecomeHostStarted
(
    Guid CorrelationId
);