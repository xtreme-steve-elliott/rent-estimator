using FluentValidation;
using rent_estimator.Shared.Documentation;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.RentEstimation.Queries;

public class FetchRentalsByCityStateRequest
{
    [SwaggerSchema(Description = "First name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("John")]
    public string city { get; set; }
    
    [SwaggerSchema(Description = "First name of created user", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("John")]
    public string stateAbbrev { get; set; }
}

public class FetchRentalsByCityStateResponse : StandardResponse
{
    [SwaggerSchema(Description = "Rent estimation response content as string", Format = "{xxx: xxx}", ReadOnly = true)]
    [SwaggerSchemaExample("{ propertyId: 'testPropertyId'}")]
    public string content { get; set; }
}

public class FetchRentalsByCityStateValidator : AbstractValidator<FetchRentalsByCityStateRequest>
{
    public FetchRentalsByCityStateValidator()
    {
        RuleFor(request => request.city)
            .NotEmpty()
            .WithMessage("City must not be empty.");
        
        RuleFor(request => request.stateAbbrev)
            .NotEmpty()
            .WithMessage("State abbreviation must not be empty.");
    }
}