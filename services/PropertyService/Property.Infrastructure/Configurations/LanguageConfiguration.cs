using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Property.Domain.Aggregates.LanguageAggregate;

namespace Property.Infrastructure.Configurations;

public class LanguageConfiguration: IEntityTypeConfiguration<Language>
{
    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.ToTable("Languages");

        builder.HasKey(l => l.Id);
        builder.Property(l => l.Id)
            .ValueGeneratedOnAdd()
            .IsRequired();

        builder.HasData(
            new Language(1, "English"),
            new Language(2, "French"),
            new Language(3, "German"),
            new Language(4, "Italian"),
            new Language(5, "Spanish"),
            new Language(6, "Japanese"),
            new Language(7, "Chinese"),
            new Language(8, "Korean"),
            new Language(9, "Russian"),
            new Language(10, "Arabic")
        );
    }
}