using FluentValidation;
using Property.Application.UseCases.Commands;

namespace Property.Application.Validators;

public class AddPropertyImagesValidator: AbstractValidator<AddPropertyImagesCommand>
{
   public AddPropertyImagesValidator()
   {
      RuleFor(command => command.images)
         .NotEmpty().WithMessage("Image URLs cannot be empty.")
         .Must(urls => urls.All(url => Uri.IsWellFormedUriString(url, UriKind.Absolute)))
         .Must(urls => urls.Count >=5)
         .WithMessage("All image URLs must be valid absolute URIs.");
   } 
}