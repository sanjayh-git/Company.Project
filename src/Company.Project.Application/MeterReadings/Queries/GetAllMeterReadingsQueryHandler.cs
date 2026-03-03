using Company.Project.Application.MeterReadings.Queries;
using Company.Project.Data.Entities;
using Company.Project.Data.Repositories.Interfaces;
using MediatR;

namespace Company.Project.Application.MeterReadings.Handlers;

public class GetAllMeterReadingsQueryHandler : IRequestHandler<GetAllMeterReadingsQuery, IReadOnlyList<MeterReading>>
{
    private readonly IMeterReadingRepository _repository;

    public GetAllMeterReadingsQueryHandler(IMeterReadingRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<MeterReading>> Handle(GetAllMeterReadingsQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetAllAsync(cancellationToken);
    }
}
