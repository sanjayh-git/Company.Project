using Company.Project.Data.Entities;
using MediatR;

namespace Company.Project.Application.MeterReadings.Queries;

public record GetAllMeterReadingsQuery() : IRequest<IReadOnlyList<MeterReading>>;
