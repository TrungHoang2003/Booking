using Property.Domain.ValueObjects;

namespace Property.Application.DTOs;

public record EntirePropertyRentalUnitDto
(
    int MaxAdults,
    int MaxChildren,
    int Size,
    int BedroomsCount,
    int BathroomsCount,
    Price Price
);