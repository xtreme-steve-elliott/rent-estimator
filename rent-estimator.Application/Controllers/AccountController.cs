using MediatR;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Shared.Mvc;
using rent_estimator.Shared.Mvc.Validation;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Controllers;

[Route("accounts")]
public class AccountController : ApiControllerBase, IAccountController
{
    private readonly IMediator _mediator;
    private readonly IValidatorWrapper<CreateAccountRequest> _validator;

    public AccountController(IMediator mediator, IValidatorWrapper<CreateAccountRequest> validator) {
        _mediator = mediator;
        _validator = validator;
    }
    
    [HttpPost(Name = "CreateAccount")]
    [SwaggerOperation(Summary = "Creates Account with request body parameter")]
    public async Task<ActionResult<CreateAccountResponse>> CreateAsync(
        [FromBody] CreateAccountRequest request,
        CancellationToken cancellationToken
    )
    {
        var results = await _validator.Validate(request, cancellationToken);
        if (!results.IsValid)
        {
            return new BadRequestObjectResult(results.Errors);
        }
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