using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;

namespace TmsApi.Data;

public class TmsDbContext(DbContextOptions<TmsDbContext> options) : DbContext(options)
{
    public DbSet<Student>    Students    => Set<Student>();
    public DbSet<Course>     Courses     => Set<Course>();
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // All configuration is discovered automatically from the three
        // IEntityTypeConfiguration classes in the Configurations folder.
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TmsDbContext).Assembly);
    }

    // Exercise 8: auto-stamp LastUpdated shadow property on every save
    public override Task<int> SaveChangesAsync(CancellationToken ct = default)
    {
        var now = DateTime.UtcNow;

        foreach (var entry in ChangeTracker.Entries<Student>()
                     .Where(e => e.State is EntityState.Added or EntityState.Modified))
        {
            entry.Property("LastUpdated").CurrentValue = now;
        }

        return base.SaveChangesAsync(ct);
    }
}