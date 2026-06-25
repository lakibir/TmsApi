using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class StudentConfiguration : IEntityTypeConfiguration<Student>
{
    public void Configure(EntityTypeBuilder<Student> builder)
    {
        builder.HasKey(s => s.Id);

        // Natural key must be unique
        builder.HasIndex(s => s.RegistrationNumber)
               .IsUnique();

        builder.Property(s => s.RegistrationNumber)
               .IsRequired()
               .HasMaxLength(20);

        builder.Property(s => s.Name)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(s => s.GPA)
               .HasPrecision(3, 2);

        // Exercise 8: shadow audit column — exists in DB only, not on the C# model
        builder.Property<DateTime>("LastUpdated");

        // Exercise 8: row-version concurrency token
        // Npgsql maps IsRowVersion() to PostgreSQL's built-in xmin column — no extra column needed
        builder.Property(s => s.Version)
               .IsRowVersion();

        // Exercise 9: soft-delete global query filter
        // Automatically appends WHERE "IsDeleted" = false to every normal query
        builder.HasQueryFilter(s => !s.IsDeleted);
    }
}