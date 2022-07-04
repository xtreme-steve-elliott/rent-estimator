using MediatR;
using rent_estimator.Shared.Mvc.Documentation.Attributes;
using rent_estimator.Shared.Mvc.Models;
using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Modules.RentEstimation.Queries;

public class GetRentalDetailRequest : IRequest<GetRentalDetailResponse>
{
    [SwaggerSchema(Description = "Property indentifier for the rental detail being fetched", Format = "xxxxx", ReadOnly = true)]
    [SwaggerSchemaExample("M7952539079")]
    public string PropertyId { get; set; }
}

public class GetRentalDetailResponse : StandardResponse
{
    [SwaggerSchema(Description = "Rent estimation response content as string", Format = "{xxx: xxx}", ReadOnly = true)]
    [SwaggerSchemaExample("{ propertyDetail: {propertyDetailObject} }")]
    public string Content { get; set; }
}

public class GetRentalDetailHandler : IRequestHandler<GetRentalDetailRequest, GetRentalDetailResponse>
{
    private readonly IRentEstimatorClient _client;

    public GetRentalDetailHandler(IRentEstimatorClient client)
    {
        _client = client;
    }
    
    public async Task<GetRentalDetailResponse> Handle(GetRentalDetailRequest request, CancellationToken token)
    {
        var content = await _client.FetchRental(request.PropertyId);
        return new GetRentalDetailResponse
        {
            Content = content
        };
    }
}
