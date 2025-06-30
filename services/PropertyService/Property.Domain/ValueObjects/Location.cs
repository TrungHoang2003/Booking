namespace Property.Domain.ValueObjects;

public record Location
{
    public Location(string address, string city, string country, int postCode)
    {
        Address = address;
        City = city;
        Country = country;
        PostCode = postCode;
        
        if(string.IsNullOrEmpty(address)) throw new ArgumentException("Address cannot be null or empty", nameof(address));
        if(string.IsNullOrEmpty(city)) throw new ArgumentException("City cannot be null or empty", nameof(city));
        if(string.IsNullOrEmpty(country)) throw new ArgumentException("Country cannot be null or empty", nameof(country));
    }

    public string Address { get; init; }
    public string City { get; init; }
    public string Country { get; init; }
    public int? PostCode{ get; init; }
}