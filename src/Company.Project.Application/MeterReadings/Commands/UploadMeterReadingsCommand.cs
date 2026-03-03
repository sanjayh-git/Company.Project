using Company.Project.Domain.DTOs;
using MediatR;

namespace Company.Project.Application.MeterReadings.Commands;

public record UploadMeterReadingsCommand(byte[] FileBytes) : IRequest<MeterReadingUploadResultDto>;
