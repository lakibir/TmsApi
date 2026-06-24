using Microsoft.EntityFrameworkCore;
using TmsApi.Entities;
namespace TmsApi.Data;
public class TmsDbContext(DbContextOptions<TmsDbContext> options) : DbContext(options)
{
public DbSet<Student> Students => Set<Student>();
public DbSet<Course> Courses => Set<Course>();
public DbSet<Enrollment> Enrollments => Set<Enrollment>();
} Configurations/
    │       ├── StudentConfiguration.cs      ← Exercise 4
    │       ├── CourseConfiguration.cs       ← Exercise 4
    │       └── EnrollmentConfiguration.cs  ← Exerc
    Services/DashboardService.cs