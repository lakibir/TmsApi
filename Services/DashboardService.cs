using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Services;

public class DashboardService
{
    private readonly TmsDbContext _db;

    public DashboardService(TmsDbContext db) => _db = db;

    // Exercise 3: Paged student list
    // SQL: SELECT ... FROM "Students" ORDER BY "Name" LIMIT 20 OFFSET ...
    public Task<List<StudentRow>> GetStudentsPagedAsync(
        int page, int pageSize = 20, CancellationToken ct = default)
    {
        return _db.Students
                  .AsNoTracking()
                  .OrderBy(s => s.Name)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .Select(s => new StudentRow(s.Id, s.RegistrationNumber, s.Name, s.GPA))
                  .ToListAsync(ct);
    }

    // Exercise 3: Top 5 courses by enrollment count
    // SQL: SELECT ... FROM "Enrollments" GROUP BY ... ORDER BY COUNT DESC LIMIT 5
    public Task<List<CourseCountRow>> GetTopCoursesAsync(CancellationToken ct = default)
    {
        return _db.Enrollments
                  .AsNoTracking()
                  .GroupBy(e => new { e.CourseId, e.Course.Title, e.Course.Code })
                  .Select(g => new CourseCountRow(g.Key.CourseId, g.Key.Code, g.Key.Title, g.Count()))
                  .OrderByDescending(r => r.EnrollmentCount)
                  .Take(5)
                  .ToListAsync(ct);
    }
}

// Lightweight read-only projections
public record StudentRow(int Id, string RegistrationNumber, string Name, decimal GPA);
public record CourseCountRow(int CourseId, string Code, string Title, int EnrollmentCount);