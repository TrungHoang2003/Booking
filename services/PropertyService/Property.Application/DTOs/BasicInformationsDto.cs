using Property.Domain.ValueObjects;

namespace Property.Application.DTOs;

public record BasicInformationsDto
(
    string Name,
    int PropertyTypeId,
    string Address,
    string City,
    string Country,
    string PostCode
);