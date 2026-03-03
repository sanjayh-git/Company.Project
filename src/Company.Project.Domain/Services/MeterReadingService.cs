using System.Globalization;
using Company.Project.Data.Entities;
using Company.Project.Data.Repositories.Interfaces;
using Company.Project.Domain.Csv.Interfaces;
using Company.Project.Domain.DTOs;

namespace Company.Project.Application.Services;

public class MeterReadingService : IMeterReadingService
{
    private readonly IAccountRepository _accountRepository;
    private readonly IMeterReadingRepository _meterReadingRepository;
    private readonly ICsvParser _csvParser;

    public MeterReadingService(
        IAccountRepository accountRepository,
        IMeterReadingRepository meterReadingRepository,
        ICsvParser csvParser)
    {
        _accountRepository = accountRepository;
        _meterReadingRepository = meterReadingRepository;
        _csvParser = csvParser;
    }

    public async Task<MeterReadingUploadResultDto> ProcessCsvAsync(Stream csvStream, CancellationToken cancellationToken = default)
    {
        var rows = await _csvParser.ParseAsync(csvStream, cancellationToken);

        var success = 0;
        var failure = 0;

        foreach (var row in rows)
        {
            if (row.Length < 3)
            {
                failure++;
                continue;
            }

            if (!int.TryParse(row[0], out var accountId))
            {
                failure++;
                continue;
            }

            // Account must exist
            var account = await _accountRepository.GetByAccountIdAsync(accountId, cancellationToken);
            if (account is null)
            {
                failure++;
                continue;
            }

            // Parse date/time
            if (!DateTime.TryParseExact(
                    row[1].Trim(),
                    "dd/MM/yyyy HH:mm",
                    CultureInfo.GetCultureInfo("en-GB"),
                    DateTimeStyles.AssumeLocal,
                    out var readingDateTime))
            {
                failure++;
                continue;
            }

            var readingValue = row[2].Trim();

            // Reading value must be NNNNN (5 digits)
            if (readingValue.Length != 5 || !readingValue.All(char.IsDigit))
            {
                failure++;
                continue;
            }

            // Reject duplicate readings for same account + timestamp
            var isDuplicate = await _meterReadingRepository.ExistsAsync(accountId, readingDateTime, cancellationToken);
            if (isDuplicate)
            {
                failure++;
                continue;
            }

            // Reject if new reading is older than latest existing reading
            var latest = await _meterReadingRepository.GetLatestReadingAsync(accountId, cancellationToken);
            if (latest is not null && readingDateTime <= latest.ReadingDateTime)
            {
                failure++;
                continue;
            }

            var entity = new MeterReading
            {
                AccountId = accountId,
                ReadingDateTime = readingDateTime,
                ReadingValue = readingValue
            };

            await _meterReadingRepository.AddAsync(entity, cancellationToken);
            success++;
        }

        await _meterReadingRepository.SaveChangesAsync(cancellationToken);

        return new MeterReadingUploadResultDto(success, failure);
    }
}
