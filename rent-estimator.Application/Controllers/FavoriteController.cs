using MediatR;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.Account.Commands;
using rent_estimator.Modules.Favorite.Commands;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Controllers;

[Route("favorites")]
public class FavoriteController : ApiControllerBase, IFavoriteController
{
    private readonly IMediator _mediator;
    public FavoriteController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost(Name = "CreateFavorite")]
    [SwaggerOperation(Summary = "Creates Favorite Search with request body parameter")]
    public async Task<ActionResult<CreateAccountResponse>> CreateFavorite(CreateFavoriteRequest request, CancellationToken cancellationToken)
    {
        var validator = new CreateFavoriteRequestValidator();
        var results = await validator.ValidateAsync(request, cancellationToken);
        if (!results.IsValid)
        {
            return new BadRequestObjectResult(results.Errors);
        }
        return new OkObjectResult(await _mediator.Send(request, cancellationToken));
    }
}

public interface IFavoriteController
{
    Task<ActionResult<CreateAccountResponse>> CreateFavorite(CreateFavoriteRequest request, CancellationToken token);
}