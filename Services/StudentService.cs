using Microsoft.EntityFrameworkCore;
using TmsApi.Data;
using TmsApi.Entities;

namespace TmsApi.Services;

public class StudentService
{
    private readonly TmsDbContext _db;
    public StudentService(TmsDbContext db) => _db = db;

    // Exercise 7 Part A — N+1 (demo only, never ship this)
    public async Task DemonstrateNPlusOneAsync(CancellationToken ct = default)
    {
        var students = await _db.Students
                                .AsNoTracking()
                                .ToListAsync(ct);         // 1 query

        foreach (var s in students)
        {
            var count = await _db.Enrollments
                                 .AsNoTracking()
                                 .CountAsync(e => e.StudentId == s.Id, ct); // 1 query per student

            Console.WriteLine($"{s.Name}: {count} enrollments");
        }
    }

    // Exercise 7 Part B — Fixed: single query with projection
    public async Task GetEnrollmentCountsAsync(CancellationToken ct = default)
    {
        var report = await _db.Students
                              .AsNoTracking()
                              .Select(s => new
                              {
                                  s.Name,
                                  s.RegistrationNumber,
                                  EnrollmentCount = s.Enrollments.Count  // → SQL subquery
                              })
                              .ToListAsync(ct);

        foreach (var r in report)
            Console.WriteLine($"[{r.RegistrationNumber}] {r.Name}: {r.EnrollmentCount} enrollments");
    }

    // Exercise 8: update triggers LastUpdated + Version concurrency check
    public async Task UpdateStudentNameAsync(int id, string newName, CancellationToken ct = default)
    {
        var student = await _db.Students.FindAsync(new object[] { id }, ct);
        if (student is null) return;

        student.Name = newName;
        // TmsDbContext.SaveChangesAsync sets LastUpdated automatically.
        // Throws DbUpdateConcurrencyException if another request saved first.
        await _db.SaveChangesAsync(ct);
    }

    // Exercise 9: soft-delete
    public async Task SoftDeleteStudentAsync(int id, CancellationToken ct = default)
    {
        await _db.Students
                 .Where(s => s.Id == id)
                 .ExecuteUpdateAsync(s => s.SetProperty(e => e.IsDeleted, true), ct);
    }

    // Exercise 9: admin restore — bypasses HasQueryFilter
    public async Task<List<Student>> GetDeletedStudentsAsync(CancellationToken ct = default)
    {
        return await _db.Students
                        .IgnoreQueryFilters()
                        .Where(s => s.IsDeleted)
                        .AsNoTracking()
                        .ToListAsync(ct);
    }
}