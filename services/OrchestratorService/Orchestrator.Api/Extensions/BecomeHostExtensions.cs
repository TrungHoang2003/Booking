using Contracts.Events;
using Contracts.Messages;
using Contracts.DTOs;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.Sagas;

namespace Orchestrator.Api.Extensions;

public static class BecomeHostExtensions{
    public static CreateProperty ToCreateProperty(this BecomeHostDraft draft)
    {
        return new CreateProperty(
            draft.DraftId,
            draft.HostId,
            draft.PropertyName,
            draft.PropertyTypeId,
            draft.LocationDto.Address,
            draft.LocationDto.City,
            draft.LocationDto.Country,
            draft.LocationDto.PostCode,
            draft.HouseRuleDto.CheckInTimeFrom,
            draft.HouseRuleDto.CheckInTimeUntil,
            draft.HouseRuleDto.CheckOutTimeFrom,
            draft.HouseRuleDto.CheckOutTimeUntil,
            draft.HouseRuleDto.PetAllowed,
            draft.HouseRuleDto.SmokingAllowed,
            draft.HouseRuleDto.PartyAllowed,
            draft.HouseRuleDto.AgeRestriction,
            draft.HouseRuleDto.FloorNumber
        );
    }

    public static AddRentalUnit ToAddRentalUnit(this BecomeHostDraft draft)
    {
        return new AddRentalUnit(
            draft.DraftId,
            draft.RentalUnitDto.PropertyId,
            draft.RentalUnitDto.MaxAdults,
            draft.RentalUnitDto.MaxChildren,
            draft.RentalUnitDto.Size,
            draft.RentalUnitDto.BedroomsCount,
            draft.RentalUnitDto.BathroomsCount,
            draft.RentalUnitDto.IsRoomBasedProperty,
            draft.RentalUnitDto.Quantity,
            draft.RentalUnitDto.SharedBathroom,
            draft.PricePerNight,
            "EUR"
        );
    }
}
