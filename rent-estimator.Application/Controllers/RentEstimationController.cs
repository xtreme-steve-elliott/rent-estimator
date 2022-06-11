using MediatR;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.RentEstimation.Queries;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Controllers;

[Route("properties")]
public class RentEstimationController: ApiControllerBase, IRentEstimationController
{
    private readonly IMediator _mediator;

    public RentEstimationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("city/state", Name = "FetchRentalsByCityState")]
    [SwaggerOperation(Summary = "Fetches rentals from Rent Estimation Service with given city and state")]
    public async Task<ActionResult<FetchRentalsByCityStateResponse>> FetchRentalsByCityState(FetchRentalsByCityStateRequest request, CancellationToken token)
    {
        var validator = new FetchRentalsByCityStateValidator();
        var results = await validator.ValidateAsync(request);
        if (!results.IsValid)
        {
            return new BadRequestObjectResult(results.Errors);
        }
        return new OkObjectResult(await _mediator.Send(request, token));
    }
}

public interface IRentEstimationController
{
    Task<ActionResult<FetchRentalsByCityStateResponse>> FetchRentalsByCityState(FetchRentalsByCityStateRequest request, CancellationToken token);
}