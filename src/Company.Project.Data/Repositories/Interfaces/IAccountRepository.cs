using Company.Project.Data.Entities;

namespace Company.Project.Data.Repositories.Interfaces;

public interface IAccountRepository
{
    Task<Account?> GetByAccountIdAsync(int accountId, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default);
}
