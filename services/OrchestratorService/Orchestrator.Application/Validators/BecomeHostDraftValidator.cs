
using Contracts.Drafts;

namespace Orchestrator.Api.Validators;

public static class BecomeHostDraftValidator
{
    public static (bool IsValid, string ErrorMessage) ValidateForSubmission(BecomeHostDraft draft)
    {
        if (draft == null)
            return (false, "Draft is required");

        if (draft.PropertyTypeId <= 0)
            return (false, "Property type is required");

        if (string.IsNullOrWhiteSpace(draft.PropertyName))
            return (false, "Property name is required");

        if (draft.LocationDto == null)
            return (false, "Location information is required");

        if (draft.RentalUnitDto == null)
            return (false, "Rental unit information is required");

        if (draft.ListBedroomDtos == null || !draft.ListBedroomDtos.Any())
            return (false, "At least one bedroom is required");

        return (true, string.Empty);
    }

    public static (bool IsValid, string ErrorMessage) ValidateUserId(string userIdHeader, out int userId)
    {
        userId = 0;

        if (string.IsNullOrEmpty(userIdHeader))
            return (false, "User ID header is required");

        if (!int.TryParse(userIdHeader, out userId))
            return (false, "User ID must be a valid number");

        if (userId <= 0)
            return (false, "User ID must be greater than 0");

        return (true, string.Empty);
    }

    public static (bool IsValid, string ErrorMessage) ValidateDraftId(Guid draftId)
    {
        if (draftId == Guid.Empty)
            return (false, "Draft ID is required");

        return (true, string.Empty);
    }

    public static (bool IsValid, string ErrorMessage) ValidatePropertyType(int propertyTypeId)
    {
        if (propertyTypeId <= 0)
            return (false, "Property type ID must be greater than 0");

        return (true, string.Empty);
    }

    public static (bool IsValid, string ErrorMessage) ValidatePropertyName(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return (false, "Property name is required");

        if (propertyName.Length > 100)
            return (false, "Property name cannot exceed 100 characters");

        return (true, string.Empty);
    }
}
