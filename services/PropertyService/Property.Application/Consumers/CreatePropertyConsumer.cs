using BuildingBlocks.Interfaces;
using BuildingBlocks.Services;
using Contracts.Events;
using Contracts.Messages;
using MassTransit;
using Property.Domain.Aggregates.ImageAggregate;
using Property.Domain.Aggregates.LanguageAggregate;
using Property.Domain.ValueObjects;
using Property.Infrastructure.Repositories;

namespace Property.Application.Consumers;

public class CreatePropertyConsumer(ILanguageRepository languageRepo, 
    IPropertyRepository repo, IPropertyTypeRepository propertyTypeRepo,
    IUnitOfWork unitOfWork, CloudinaryService cloudinary, IImageRepository imageRepo) : IConsumer<CreateProperty>
{
    public async Task Consume(ConsumeContext<CreateProperty> context)
    {
        var houseRules = new HouseRule(
            context.Message.CheckInTimeFrom,
            context.Message.CheckInTimeUntil,
            context.Message.CheckOutTimeFrom,
            context.Message.CheckOutTimeUntil,
            context.Message.PetAllowed,
            context.Message.SmokingAllowed,
            context.Message.PartyAllowed,
            context.Message.AgeRestriction);

        var location = new Location(
            context.Message.Address,
            context.Message.City,
            context.Message.Country,
            context.Message.PostCode);
        
        var property = new Domain.Aggregates.AggregateRoot.Property(
            context.Message.PropertyTypeId,
            context.Message.HostId,
            context.Message.Name,
            context.Message.Description,
            context.Message.FloorNumber,
            null,
            context.Message.NeighborhoodDescription);
        
        property.UpdateHouseRule(houseRules);
        property.UpdateLocation(location);
        
        if(context.Message.Base64Images != null)
            await AddImages(context, property.Id); 
        
        if(context.Message.LanguageIds!= null)
            await AddLanguages(context, property); 
        
        await AddType(context, property); 
        
        await repo.Create(property);
        await unitOfWork.SaveChangesAsync();

        await context.RespondAsync<PropertyCreated>(new
        {
            PropertyId = property.Id,
            Correlationid = context.Message.CorrelationId,
        });
    }
    
    private async Task AddType(ConsumeContext<CreateProperty> context, Domain.Aggregates.AggregateRoot.Property property)
    {
        var propertyType = await propertyTypeRepo.GetById(context.Message.PropertyTypeId) ?? throw new Exception("Property type not found");
        property.AddType(propertyType);
    }

    private async Task AddImages(ConsumeContext<CreateProperty> context, int propertyId)
    {
        var listImage = new List<Image>(); // Init List Image
        
        foreach (var base64Image in context.Message.Base64Images!)
        {
            var imageUrl = await cloudinary.UploadImage(base64Image); 
            var image = new Image(propertyId, imageUrl, false);
            
            var type = EntityType.Property;
            image.SetEntityType(type);
            
            listImage.Add(image);
        }
        await imageRepo.AddRangeAsync(listImage);
    }
    
    private async Task AddLanguages(ConsumeContext<CreateProperty> context, Domain.Aggregates.AggregateRoot.Property property)
    {
        var listLanguage = new List<Language>(); // Init List Language
        
        foreach (var languageId in context.Message.LanguageIds!)
        {
            var language = await languageRepo.GetById(languageId) ?? throw new Exception("Language not found");
            listLanguage.Add(language);
        }
        property.AddListLanguage(listLanguage);
    }
}