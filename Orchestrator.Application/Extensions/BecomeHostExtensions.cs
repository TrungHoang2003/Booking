using Contracts.Drafts;
using Contracts.Messages;

namespace Orchestrator.Api.Extensions;

public static class BecomeHostExtensions
{
    public static CreateProperty ToCreateProperty(this BecomeHostDraft draft)
    {
        return new CreateProperty
        {
            CorrelationId = draft.DraftId,
            HostId = draft.HostId,
            Name = draft.PropertyName,
            PropertyTypeId = draft.PropertyTypeId,
            Address = draft.LocationDto.Address,
            City = draft.LocationDto.City,
            Country = draft.LocationDto.Country,
            PostCode = draft.LocationDto.PostCode,
            CheckInTimeFrom = draft.HouseRuleDto.CheckInTimeFrom,
            CheckInTimeUntil = draft.HouseRuleDto.CheckInTimeUntil,
            CheckOutTimeFrom = draft.HouseRuleDto.CheckOutTimeFrom,
            CheckOutTimeUntil = draft.HouseRuleDto.CheckOutTimeUntil,
            PetAllowed = draft.HouseRuleDto.PetAllowed,
            SmokingAllowed = draft.HouseRuleDto.SmokingAllowed,
            PartyAllowed = draft.HouseRuleDto.PartyAllowed,
            AgeRestriction = draft.HouseRuleDto.AgeRestriction,
            FloorNumber = draft.HouseRuleDto.FloorNumber,
            LanguageIds = draft.LanguageIds,
            Base64Images = draft.Base64Images,
            Description = draft.HostProfileDto?.PropertyDescription,
            NeighborhoodDescription = draft.HostProfileDto?.NeighborhoodDescription
        };
    }

    public static AddRentalUnit ToAddRentalUnit(this BecomeHostDraft draft, int propertyId)
    {
        return new AddRentalUnit
        {
            Name = draft.RentalUnitDto.Name,
            CorrelationId = draft.DraftId,
            PropertyId = propertyId,
            MaxAdults = draft.RentalUnitDto.MaxAdults,
            MaxChildren = draft.RentalUnitDto.MaxChildren,
            Size = draft.RentalUnitDto.Size,
            BedroomsCount = draft.RentalUnitDto.BedroomsCount,
            BathroomsCount = draft.RentalUnitDto.BathroomsCount,
            IsRoomBasedProperty = draft.RentalUnitDto.IsRoomBasedProperty,
            Quantity = draft.RentalUnitDto.Quantity,
            SharedBathroom = draft.RentalUnitDto.SharedBathroom,
            Amount = draft.RentalUnitDto.Amount,
            Currency = draft.RentalUnitDto.Currency,
            AmenityIds = draft.AmenityIds
        };
    }

    public static AddBedroom ToAddBedroom(this BecomeHostDraft draft, int rentalUnitId)
    {
        return new AddBedroom
        {
            CorrelationId = draft.DraftId,
            RentalUnitId = rentalUnitId,
            ListBedrooms= draft.ListBedroomDtos
        };
    }

    public static UpdateHostProfile ToUpdateHostProfile(this BecomeHostDraft draft, int hostId)
    {
        return new UpdateHostProfile
        {
            CorrelationId = draft.DraftId,
            HostId = hostId,
            HostName= draft.HostProfileDto.HostName,
            HostDescription = draft.HostProfileDto.HostDescription
        };
    }
}
