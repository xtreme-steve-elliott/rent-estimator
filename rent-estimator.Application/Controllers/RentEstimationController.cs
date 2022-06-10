using Microsoft.AspNetCore.Mvc;
using rent_estimator.Modules.RentEstimation;
using rent_estimator.Modules.RentEstimation.Queries;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Controllers;

[Route("properties")]
public class RentEstimationController: ApiControllerBase, IRentEstimationController
{
    private readonly IRentEstimatorClient _client;

    public RentEstimationController(IRentEstimatorClient client)
    {
        _client = client;
    }

    [HttpPost("city/state", Name = "FetchRentalsByCityState")]
    [SwaggerOperation(Summary = "Fetches rentals from Rent Estimation Service with given city and state")]
    public async Task<ActionResult<FetchRentalsByCityStateResponse>> FetchRentalsByCityState(FetchRentalsByCityStateRequest request)
    {
        var validator = new FetchRentalsByCityStateValidator();
        var results = await validator.ValidateAsync(request);
        if (!results.IsValid)
        {
            return new BadRequestObjectResult(results.Errors);
        }
        var content = await _client.FetchRentalListingsByCityState(request.city, request.stateAbbrev);
        return new OkObjectResult(new FetchRentalsByCityStateResponse {content = content});
    }
}

public interface IRentEstimationController
{
    Task<ActionResult<FetchRentalsByCityStateResponse>> FetchRentalsByCityState(FetchRentalsByCityStateRequest request);
}