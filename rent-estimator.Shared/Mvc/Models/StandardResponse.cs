using Swashbuckle.AspNetCore.Annotations;

namespace rent_estimator.Shared.Mvc;

public class StandardResponse
{
    [SwaggerSchema(Description = "The standard response status like success, failure, error, etc.")]
    public string Status { get; set; } = "Success";
}