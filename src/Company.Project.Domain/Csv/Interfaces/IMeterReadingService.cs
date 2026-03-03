using Company.Project.Domain.DTOs;

namespace Company.Project.Domain.Csv.Interfaces;


public interface IMeterReadingService
{
    Task<MeterReadingUploadResultDto> ProcessCsvAsync(Stream csvStream, CancellationToken cancellationToken = default);
}
