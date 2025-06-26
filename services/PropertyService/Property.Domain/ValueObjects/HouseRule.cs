namespace Property.Domain.ValueObjects;

public record HouseRule
{
    public TimeSpan CheckInTimeFrom { get; init; }
    public TimeSpan CheckInTimeUntil{ get; init; }
    private TimeSpan CheckOutTimeFrom { get; init; }
    public TimeSpan CheckOutTimeUntil{ get; init; }
    public bool PetAllowed { get; init; }
    public bool SmokingAllowed { get; init; }
    public bool PartyAllowed { get; init; }
    public int AgeRestriction { get; init; }

    public HouseRule(TimeSpan checkInTimeFrom, TimeSpan checkInTimeUntil, TimeSpan checkOutTimeFrom, TimeSpan checkOutTimeUntil, 
                     bool petAllowed, bool smokingAllowed, bool partyAllowed, int ageRestriction)
    {
        CheckInTimeFrom = checkInTimeFrom;
        CheckInTimeUntil = checkInTimeUntil;
        CheckOutTimeFrom = checkOutTimeFrom;
        CheckOutTimeUntil = checkOutTimeUntil;
        PetAllowed = petAllowed;
        SmokingAllowed = smokingAllowed;
        PartyAllowed = partyAllowed;
        AgeRestriction = ageRestriction;

        if(CheckInTimeFrom > CheckInTimeUntil) throw new ArgumentException("Check-in time from cannot be later than check-in time until", nameof(checkInTimeFrom));
        if(CheckOutTimeFrom > CheckOutTimeUntil) throw new ArgumentException("Check-out time from cannot be later than check-out time until", nameof(checkOutTimeFrom));
    }
    
    // Constructor mặc định hoặc với ít tham số hơn nếu muốn tạo các instance "default"
    // Chỉ để phục vụ EF Core nếu cần hoặc khi khởi tạo default từ DB
    private HouseRule() : this(
        TimeSpan.FromHours(15), TimeSpan.FromHours(23),
        TimeSpan.FromHours(7), TimeSpan.FromHours(11),
        false, false, false, 0){}
}