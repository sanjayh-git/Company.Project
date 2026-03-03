using Company.Project.Application.MeterReadings.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Project.Api.Controllers;

[ApiController]
[Route("meter-readings")]
public class MeterReadingsController : ControllerBase
{
    private readonly IMediator _mediator;

    public MeterReadingsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        var items = await _mediator.Send(new GetAllMeterReadingsQuery(), cancellationToken);
        return Ok(items);
    }
}
