using FluentValidation;
using MediatR;
using rent_estimator.Shared.Mvc.Documentation.Attributes;
using rent_estimator.Shared.Mvc.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.RentEstimation.Queries;

public class FetchRentalsByCityStateRequest: IRequest<FetchRentalsByCityStateResponse>
{
    [SwaggerSchema(Description = "First name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("John")]
    public string City { get; set; }
    
    [SwaggerSchema(Description = "Abbreviated state name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("NY")]
    public string StateAbbreviation { get; set; }
}

public class FetchRentalsByCityStateResponse : StandardResponse
{
    // TODO: Can this be an object?
    [SwaggerSchema(Description = "Rent estimation response content as string", Format = "{xxx: xxx}", ReadOnly = true)]
    [SwaggerSchemaExample("{ propertyId: 'testPropertyId'}")]
    public string Content { get; set; }
}

public class FetchRentalsByCityStateHandler : IRequestHandler<FetchRentalsByCityStateRequest, FetchRentalsByCityStateResponse>
{
    private readonly IRentEstimatorClient _client;
    
    public FetchRentalsByCityStateHandler(IRentEstimatorClient client)
    {
        _client = client;
    }
    
    public async Task<FetchRentalsByCityStateResponse> Handle(FetchRentalsByCityStateRequest request, CancellationToken token)
    {
        var content = await _client.FetchRentalsByCityState(request.City, request.StateAbbreviation);
        return new FetchRentalsByCityStateResponse { Content = content };
    }
}

public class FetchRentalsByCityStateValidator : AbstractValidator<FetchRentalsByCityStateRequest>
{
    public FetchRentalsByCityStateValidator()
    {
        RuleFor(request => request.City)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty.");
        
        RuleFor(request => request.StateAbbreviation)
            .NotEmpty()
            .WithMessage("{PropertyName} must not be empty.");
    }
}
