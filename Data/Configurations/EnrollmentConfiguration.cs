using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TmsApi.Entities;

namespace TmsApi.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Grade)
               .HasPrecision(5, 2);

        builder.Property(e => e.EnrolledAt)
               .IsRequired();

        builder.Property(e => e.IsArchived)
               .HasDefaultValue(false);

        // Student → Enrollment
        // Restrict: enrollment history must not vanish when a student is deleted.
        builder.HasOne(e => e.Student)
               .WithMany(s => s.Enrollments)
               .HasForeignKey(e => e.StudentId)
               .OnDelete(DeleteBehavior.Restrict);

        // Course → Enrollment
        // Restrict: grade history must not vanish when a course is retired.
        builder.HasOne(e => e.Course)
               .WithMany(c => c.Enrollments)
               .HasForeignKey(e => e.CourseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}