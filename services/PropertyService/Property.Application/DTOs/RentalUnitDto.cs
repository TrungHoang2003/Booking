namespace Property.Application.DTOs;

public record RentalUnitDto
(
    int Id,
    string Name,
    string Description,
    decimal Price,
    int Capacity,
    string Location,
    string ImageUrl,
    DateTime CreatedAt,
    DateTime UpdatedAt
);