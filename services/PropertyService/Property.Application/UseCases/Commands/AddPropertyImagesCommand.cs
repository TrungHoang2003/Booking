using BuildingBlocks.Commons;
using BuildingBlocks.Interfaces;
using BuildingBlocks.Services;
using Property.Application.Errors;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.UseCases.Commands;

public sealed record AddPropertyImagesCommand(List<string> Images, int PropertyId): ICommand;

public class AddPropertyImagesCommandHandler(IUnitOfWork unitOfWork, IImageRepository imageRepository, IPropertyRepository propertyRepository, CloudinaryService cloudinary) : ICommandHandler<AddPropertyImagesCommand>
{
    public async Task<Result> Handle(AddPropertyImagesCommand command, CancellationToken cancellationToken)
    {
        var property = await propertyRepository.GetByIdAsync(command.PropertyId);
        if (property == null) return PropertyErrors.PropertyNotFound;
        
        var imageList = new List<Image>();
        foreach(var image in command.Images)
        {
            var uploadResult = await cloudinary.UploadImage(image);
            var newImage = new Image(command.PropertyId, EntityType.Property, uploadResult, false);
            imageList.Add(newImage);
        } 
        imageList.First().SetPrimary(); // First image is set as primary
        
        await imageRepository.AddRangeAsync(imageList);
        await unitOfWork.SaveChangesAsync(cancellationToken);
        return Result.Success();
    }
}


