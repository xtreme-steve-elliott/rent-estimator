using System.Net.Mime;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Shared.Documentation;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Controllers;

[Route("accounts")]
public class AccountController : ApiControllerBase, IAccountController
{
    private readonly IMediator _mediator;

    public AccountController(IMediator mediator) {
        _mediator = mediator;
    }
    
    [HttpPost(Name = "CreateAccount")]
    [SwaggerOperation(Summary = "Creates Account with request body parameter")]
    [SwaggerCustomResponse(
        StatusCodes.Status200OK, "Successfully creates user account",
        typeof(CreateAccountResponse),
        typeof(StandardResponseHeader), 
        MediaTypeNames.Application.Json
    )]
    public async Task<ActionResult<CreateAccountResponse>> CreateAsync(
        [FromBody] CreateAccountRequest request,
        [FromHeader] StandardRequestHeader header,
        CancellationToken cancellationToken
    ) {
        return new OkObjectResult(await _mediator.Send(request, cancellationToken));
    }

}

public interface IAccountController
{
    Task<ActionResult<CreateAccountResponse>> CreateAsync(
        CreateAccountRequest request, 
        StandardRequestHeader requestHeader, 
        CancellationToken cancellationToken
    );
}