using Company.Project.Application.MeterReadings.Commands;
using Company.Project.Domain.Csv.Interfaces;
using Company.Project.Domain.DTOs;
using MediatR;

namespace Company.Project.Application.MeterReadings.Handlers;

public class UploadMeterReadingsCommandHandler : IRequestHandler<UploadMeterReadingsCommand, MeterReadingUploadResultDto>
{
    private readonly IMeterReadingService _service;

    public UploadMeterReadingsCommandHandler(IMeterReadingService service)
    {
        _service = service;
    }

    public async Task<MeterReadingUploadResultDto> Handle(UploadMeterReadingsCommand request, CancellationToken cancellationToken)
    {
        using var stream = new MemoryStream(request.FileBytes);
        return await _service.ProcessCsvAsync(stream, cancellationToken);
    }
}
