using Company.Project.Data.Entities;

namespace Company.Project.Data.Repositories.Interfaces;

public interface IMeterReadingRepository
{
    Task<MeterReading?> GetLatestReadingAsync(int accountId, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(int accountId, DateTime readingDateTime, CancellationToken cancellationToken = default);
    Task AddAsync(MeterReading reading, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<MeterReading>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
