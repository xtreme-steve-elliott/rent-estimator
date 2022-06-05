using MediatR;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.Account.Commands;
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
    public async Task<ActionResult<CreateAccountResponse>> CreateAsync(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken
    ) {
        return new OkObjectResult(await _mediator.Send(request, cancellationToken));
    }

}

public interface IAccountController
{
    Task<ActionResult<CreateAccountResponse>> CreateAsync(
        CreateAccountRequest request, 
        CancellationToken cancellationToken
    );
}