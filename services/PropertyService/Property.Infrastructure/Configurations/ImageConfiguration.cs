using System.Net.Mime;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.ImageAggregate;

namespace Property.Infrastructure.Configurations;

public class ImageConfiguration:IEntityTypeConfiguration<Image>
{
    public void Configure(EntityTypeBuilder<Image> builder)
    {
        builder.ToTable("Images");

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();
    }
}