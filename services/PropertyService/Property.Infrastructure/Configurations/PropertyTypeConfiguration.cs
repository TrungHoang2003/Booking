using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AggregateRoot;

namespace Property.Infrastructure.Configurations;

public class PropertyTypeConfiguration: IEntityTypeConfiguration<PropertyType>
{
    public void Configure(EntityTypeBuilder<PropertyType> builder)
    {
        builder.ToTable("PropertyTypes");

        builder.HasKey(pt => pt.Id);
        builder.Property(pt => pt.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.Property(pt => pt.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(pt => pt.Description)
            .HasMaxLength(500);

        builder.HasData(
            // Entire-Place Rental Types
            new PropertyType(1, "Apartment", "Furnished and self-catering accommodation available for short- and long-term rental", false),
            new PropertyType(2, "Holiday home", "Free-standing home with private, external entrance and rented specifically for holidays", false),
            new PropertyType(4, "Villa", "Private self-standing and self-catering home with luxury feel", false),
            new PropertyType(5, "Chalet", "Free-standing home characterised by sloped roof and rented specifically for holidays", false),
            new PropertyType(6, "Holiday park", "Private self-catering residences located on a shared grounds with shared facilities or recreational activities", false),
            new PropertyType(7, "Aparthotel", "A self-catering apartment with some hotel facilities like a reception desk", false),
            
            // Room-based Types
            new PropertyType(8, "Hotel", "Accommodation for travellers often offering restaurants, meeting rooms and other guest services", true),
            new PropertyType(9, "Guest house", "Private home with separate living facilities for host and guest", true),
            new PropertyType(10, "Bed and breakfast", "Private home offering overnight stays and breakfast", true),
            new PropertyType(11, "Homestay", "Private home with shared living facilities for host and guest", true),
            new PropertyType(12, "Hostel", "Budget accommodation with mostly dorm-style bedding and a social atmosphere", true),
            new PropertyType(13, "Aparthotel", "A self-catering apartment with some hotel facilities like a reception desk", true),
            new PropertyType(14, "Capsule hotel", "Extremely small units or capsules offering cheap and basic overnight accommodation", true),
            new PropertyType(15, "Country house", "Private home with simple accommodation in the countryside", true),
            new PropertyType(16, "Farm stay", "Private farm with simple accommodation", true),
            new PropertyType(17, "Inn", "Small and basic accommodation with a rustic feel", true),
            new PropertyType(18, "Love hotel", "Adult-only accommodation rented per hour or night", true),
            new PropertyType(19, "Motel", "Roadside hotel usually for motorists, with direct access to parking and little to no amenities", true),
            new PropertyType(20, "Riad", "Traditional Moroccan accommodation with a courtyard and luxury feel", true),
            new PropertyType(21, "Resort", "A place for relaxation with onsite restaurants, activities and often with a luxury feel", true),
            new PropertyType(22, "Ryokan", "Traditional Japanese-style accommodation with meal options", true),
            new PropertyType(23, "Lodge", "Private home with accommodation surrounded by nature, such as mountains or forest", true)
            );

    }
}