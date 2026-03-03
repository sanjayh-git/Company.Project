using Company.Project.Application.MeterReadings.Commands;
using Company.Project.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Project.Api.Controllers;

[ApiController]
[Route("meter-reading-uploads")]
public class MeterReadingUploadsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeterReadingUploadsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    [ProducesResponseType(typeof(MeterReadingUploadResultDto), StatusCodes.Status200OK)]
    public async Task<IActionResult> Upload(IFormFile file, CancellationToken cancellationToken)
    {
        if (file == null)
        {
            return BadRequest("File is required.");
        }

        if (file.Length == 0)
        {
            return BadRequest("Uploaded file is empty.");
        }

        await using var fileStream = file.OpenReadStream();
        using var ms = new MemoryStream();
        await fileStream.CopyToAsync(ms, cancellationToken);
        var bytes = ms.ToArray();

        var command = new UploadMeterReadingsCommand(bytes);
        var result = await _mediator.Send(command, cancellationToken);

        return Ok(result);
    }
}
