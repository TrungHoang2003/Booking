using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.AggregateRoot;

namespace Property.Infrastructure.Configurations;

public class PropertyLanguageConfiguration:IEntityTypeConfiguration<PropertyLanguage>
{
    public void Configure(EntityTypeBuilder<PropertyLanguage> builder)
    {
        builder.ToTable("PropertyLanguages");

        builder.HasKey(pl => pl.Id);
        builder.Property(pl => pl.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}