using Company.Project.Data.Entities;
using Company.Project.Data.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Company.Project.Data.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AppDbContext _db;

    public AccountRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Account?> GetByAccountIdAsync(int accountId, CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.AccountId == accountId, cancellationToken);
    }

    public async Task<IReadOnlyList<Account>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        return await _db.Accounts
            .AsNoTracking()
            .OrderBy(a => a.AccountId)
            .ToListAsync(cancellationToken);
    }
}
