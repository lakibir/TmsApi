using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class CourseConfiguration : IEntityTypeConfiguration<Course>
{
    public void Configure(EntityTypeBuilder<Course> builder)
    {
        builder.HasKey(c => c.Id);

        // Natural key must be unique
        builder.HasIndex(c => c.Code)
               .IsUnique();

        builder.Property(c => c.Code)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(c => c.Title)
               .IsRequired()
               .HasMaxLength(300);

        builder.Property(c => c.Capacity)
               .IsRequired();
    }
}