using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Shared.Mvc.Models;

public class StandardErrorResponse : StandardResponse
{
    [SwaggerSchema(Description = "List of status message details")]
    public IList<StatusDetail>? StatusDetails { get; init; }
}
