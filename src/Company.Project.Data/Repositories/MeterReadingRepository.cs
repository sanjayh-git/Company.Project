using Company.Project.Data.Entities;
using Company.Project.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Data.Repositories;

public class MeterReadingRepository : IMeterReadingRepository
{
    private readonly AppDbContext _db;

    public MeterReadingRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<MeterReading?> GetLatestReadingAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await _db.MeterReadings
            .Where(m => m.AccountId == accountId)
            .OrderByDescending(m => m.ReadingDateTime)
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<bool> ExistsAsync(int accountId, DateTime readingDateTime, CancellationToken cancellationToken = default)
    {
        return await _db.MeterReadings
            .AnyAsync(m => m.AccountId == accountId && m.ReadingDateTime == readingDateTime, cancellationToken);
    }

    public async Task AddAsync(MeterReading reading, CancellationToken cancellationToken = default)
    {
        await _db.MeterReadings.AddAsync(reading, cancellationToken);
    }

    public async Task<IReadOnlyList<MeterReading>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.MeterReadings
            .AsNoTracking()
            .OrderBy(m => m.AccountId)
            .ThenBy(m => m.ReadingDateTime)
            .ToListAsync(cancellationToken);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _db.SaveChangesAsync(cancellationToken);
    }
}
