using System.Net.Mime;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rent_estimator.Shared.Mvc.Documentation.Attributes;
using rent_estimator.Shared.Mvc.Models;

namespace rent_estimator.Shared.Mvc.Controllers;

[ApiController]
[Consumes(MediaTypeNames.Application.Json)]
[SwaggerCustomResponse(StatusCodes.Status400BadRequest, "<strong>Failed</strong><p>Request Validation failures - syntax, regex failures, required fields etc.</p><p>required request headers</p><p>Any failure resulting from the request submitted before the actual processing of the request</p>", typeof(StandardErrorResponse), typeof(StandardResponseHeader), MediaTypeNames.Application.Json)]
[SwaggerCustomResponse(StatusCodes.Status401Unauthorized, "<strong>AuthFailure</strong><p>Failures resulting from authentication</p><p>Invalid claims or Token</p>", typeof(StandardErrorResponse), typeof(StandardResponseHeader), MediaTypeNames.Application.Json)]
[SwaggerCustomResponse(StatusCodes.Status500InternalServerError, "<strong>Error | Timeout | Unknown</strong><dl><dt>Error</dt><dd><p>Any server side exception or error</p></dd><dt>Timeout</dt><dd><p>Timeout experienced while processing the request</p></dd><dt>Unknown</dt><dd><p>Generic error response</p></dd></dl>", typeof(StandardErrorResponse), typeof(StandardResponseHeader), MediaTypeNames.Application.Json)]
public class ApiControllerBase : ControllerBase
{
}
