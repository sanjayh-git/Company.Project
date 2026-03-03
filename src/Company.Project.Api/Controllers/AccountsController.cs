using Company.Project.Application.Accounts.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Company.Project.Api.Controllers;

[ApiController]
[Route("accounts")]
public class AccountsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<AccountsController> _logger;

    public AccountsController(IMediator mediator, ILogger<AccountsController> logger)
    {
        _mediator = mediator;
        _logger = logger;

    }

    [HttpGet]
    public async Task<ActionResult> Get(CancellationToken cancellationToken)
    {
        throw new Exception("This is a test exception to verify global error handling.");
        _logger.LogInformation("This is an Information log");
        _logger.LogWarning("This is a Warning log");
        _logger.LogError("This is an Error log");
        var items = await _mediator.Send(new GetAllAccountsQuery(), cancellationToken);
        return Ok(items);
    }
}
