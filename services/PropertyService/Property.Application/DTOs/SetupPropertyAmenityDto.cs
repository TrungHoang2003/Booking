using Property.Domain.ValueObjects;

namespace Property.Application.DTOs;

public record SetupPropertyAmenityDto
(
    int AmenityId,
    string? Description,
    decimal Amount = 0,
    string Currency = "VND"
);