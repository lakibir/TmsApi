using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TMS.Api.Models;

namespace TMS.Api.Data.Configurations;

public class EnrollmentConfiguration : IEntityTypeConfiguration<Enrollment>
{
    public void Configure(EntityTypeBuilder<Enrollment> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Grade)
               .HasPrecision(5, 2);

        builder.Property(e => e.EnrolledOn)
               .IsRequired();

        // Student → Enrollment (one-to-many)
        builder.HasOne(e => e.Student)
               .WithMany(s => s.Enrollments)
               .HasForeignKey(e => e.StudentId)
               // A student's enrollment history must not vanish when a Course is deleted.
               // Application code must remove enrollments explicitly before deleting a course.
               .OnDelete(DeleteBehavior.Restrict);

        // Course → Enrollment (one-to-many)
        builder.HasOne(e => e.Course)
               .WithMany(c => c.Enrollments)
               .HasForeignKey(e => e.CourseId)
               .OnDelete(DeleteBehavior.Restrict);
    }
}