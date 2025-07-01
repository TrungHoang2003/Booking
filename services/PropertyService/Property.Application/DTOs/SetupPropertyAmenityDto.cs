using Property.Domain.ValueObjects;

namespace Property.Application.DTOs;

public record SetupPropertyAmenityDto
(
    int amenityId,
    string? description ,
    Price? Price
);