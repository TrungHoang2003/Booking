namespace Property.Domain.ValueObjects;

public record Price
{
   public decimal Amount { get; init; }
   public string Currency { get; init; }
   
    public Price(decimal amount, string currency)
    {
        if (amount < 0)
            throw new ArgumentException("Price amount cannot be negative", nameof(amount));
        if (string.IsNullOrWhiteSpace(currency))
            throw new ArgumentException("Currency cannot be null or empty", nameof(currency));
    
        Amount = amount;
        Currency = currency;
    }
}