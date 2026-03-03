using Company.Project.Data;
using Company.Project.Data.Entities;
using Microsoft.Extensions.Logging;

namespace Company.Project.Infrastructure.Data;

public static class DbSeeder
{
    public static async Task SeedAccountsAsync(AppDbContext db, ILogger logger, CancellationToken cancellationToken = default)
    {
        await db.Database.EnsureCreatedAsync(cancellationToken);

        if (!db.Accounts.Any())
        {
            var basePath = AppContext.BaseDirectory;
            var accountsPath = Path.Combine(basePath, "Data", "Test_Accounts.csv");

            if (!File.Exists(accountsPath))
            {
                logger.LogWarning("Test_Accounts.csv not found at {Path}", accountsPath);
            }
            else
            {
                var lines = await File.ReadAllLinesAsync(accountsPath, cancellationToken);
                foreach (var line in lines.Skip(1))
                {
                    if (string.IsNullOrWhiteSpace(line)) continue;
                    var parts = line.Split(',', StringSplitOptions.TrimEntries);

                    if (parts.Length < 3) continue;
                    if (!int.TryParse(parts[0], out var accountId)) continue;

                    db.Accounts.Add(new Account
                    {
                        AccountId = accountId,
                        FirstName = parts[1],
                        LastName = parts[2]
                    });
                }

                await db.SaveChangesAsync(cancellationToken);
                logger.LogInformation("Seeded Accounts from Test_Accounts.csv");
            }
        }
    }
}
