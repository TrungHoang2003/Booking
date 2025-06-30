namespace Property.Application.DTOs;

public record LocationDto(
    string Address,
    string City,
    string Country,
    int PostCode
    );
    