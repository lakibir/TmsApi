using Microsoft.EntityFrameworkCore;
using TmsApi.Data;

namespace TmsApi.Services;

public class EnrollmentArchiveService
{
    private readonly TmsDbContext _db;
    public EnrollmentArchiveService(TmsDbContext db) => _db = db;

    // Exercise 9: single UPDATE statement — no rows loaded into memory
    public async Task ArchiveOldEnrollmentsAsync(DateTime cutoff, CancellationToken ct = default)
    {
        await _db.Enrollments
                 .Where(e => e.EnrolledAt < cutoff && !e.IsArchived)
                 .ExecuteUpdateAsync(
                     s => s.SetProperty(e => e.IsArchived, true),
                     ct);
    }
}