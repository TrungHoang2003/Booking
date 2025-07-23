
using Contracts.DTOs;

namespace Contracts.Events;

public record BecomeHostStarted
(
    Guid CorrelationId,
    int HostId
);