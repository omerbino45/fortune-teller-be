using FortuneTeller.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FortuneTeller.Infrastructure.Persistence.Configurations;

public class WorryConfiguration : IEntityTypeConfiguration<Worry>
{
    public void Configure(EntityTypeBuilder<Worry> builder)
    {
        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .ValueGeneratedNever();

        builder.Property(w => w.Title)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(w => w.Prophecy)
            .IsRequired();

        builder.Property(w => w.Status)
            .HasConversion<string>()
            .IsRequired();

        builder.Property(w => w.PreAnxietyLevel).IsRequired();
        builder.Property(w => w.Assurance).IsRequired();
        builder.Property(w => w.CreatedAt).IsRequired();
        builder.Property(w => w.AssuranceUpdatedAt).IsRequired();

        builder.Property(w => w.Description).IsRequired(false);
        builder.Property(w => w.Factors).IsRequired(false);
        builder.Property(w => w.ActualOutcome).IsRequired(false);
        builder.Property(w => w.PostAnxietyLevel).IsRequired(false);

        // Immutability lock: EF will never include these in an UPDATE statement
        builder.Property(w => w.PreAnxietyLevel)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(w => w.Prophecy)
            .Metadata.SetAfterSaveBehavior(PropertySaveBehavior.Ignore);

        builder.Property(w => w.UserId).IsRequired();

        builder.ToTable("Worries");
    }
}
