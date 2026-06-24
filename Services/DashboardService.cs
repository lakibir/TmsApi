using Microsoft.EntityFrameworkCore;
using TMS.Api.Data;

namespace TMS.Api.Services;

public class DashboardService
{
    private readonly TmsDbContext _db;

    public DashboardService(TmsDbContext db) => _db = db;

    // TODO 1 — Paged student list
    // OrderBy gives PostgreSQL a stable sort; Skip/Take become OFFSET/LIMIT in SQL.
    public Task<List<StudentRow>> GetStudentsPagedAsync(
        int page, int pageSize = 20, CancellationToken ct = default)
    {
        return _db.Students
                  .AsNoTracking()
                  .OrderBy(s => s.Name)
                  .Skip((page - 1) * pageSize)
                  .Take(pageSize)
                  .Select(s => new StudentRow(s.Id, s.Name, s.Email))
                  .ToListAsync(ct);
    }

    // TODO 2 — Top 5 courses by enrollment count
    // GroupBy + Count() translate to GROUP BY … ORDER BY … DESC LIMIT 5 in SQL.
    public Task<List<CourseCountRow>> GetTopCoursesAsync(CancellationToken ct = default)
    {
        return _db.Enrollments
                  .AsNoTracking()
                  .GroupBy(e => new { e.CourseId, e.Course.Title })
                  .Select(g => new CourseCountRow(g.Key.CourseId, g.Key.Title, g.Count()))
                  .OrderByDescending(r => r.EnrollmentCount)
                  .Take(5)
                  .ToListAsync(ct);
    }
}

// Lightweight read-only projections (no need for full entity tracking)
public record StudentRow(int Id, string Name, string Email);
public record CourseCountRow(int CourseId, string Title, int EnrollmentCount);