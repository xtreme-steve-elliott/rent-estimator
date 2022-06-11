using MediatR;
using rent_estimator.Shared.Documentation;
using rent_estimator.Shared.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.RentEstimation.Queries;

public class GetRentalDetailRequest : IRequest<GetRentalDetailResponse>
{
    [SwaggerSchema(Description = "PropertyId for the favorite being fetched", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("M7952539079")]
    public string propertyId { get; set; }
}

public class GetRentalDetailResponse : StandardResponse
{
    [SwaggerSchema(Description = "Rent estimation response content as string", Format = "{xxx: xxx}", ReadOnly = true)]
    [SwaggerSchemaExample("{ propertyDetail: {propertyDetailObject} }")]
    public string content { get; set; }
}