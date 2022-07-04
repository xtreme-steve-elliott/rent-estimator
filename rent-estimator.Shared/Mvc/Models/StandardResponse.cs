using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Shared.Mvc.Models;

// TODO: Should consider getting rid of this in favour of status codes at the controller level.
// TODO (Cont'd): It's not particularly discoverable in the body vs proper HTTP responses
public class StandardResponse
{
    [SwaggerSchema(Description = "The standard response status like success, failure, error, etc.")]
    public string Status { get; set; } = "Success";
}
