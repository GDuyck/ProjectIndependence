using Microsoft.AspNetCore.Mvc;
using ProjectIndependence.API.Core.Errors;
using ProjectIndependence.API.Core.Response;
using System.Reflection.Metadata.Ecma335;

namespace ProjectIndependence.API.Controllers.Base
{
    [ApiController]
    [Produces("application/json")]
    public abstract class ApiBaseController : Controller
    {
        protected ActionResult OkResponse<T>(T data)
            => Ok(ApiResponse<T>.FromSucces(data));

        protected ActionResult CreatedAtResponse<T>(T data, string routeName, object routeValues)
            => CreatedAtRoute(routeName, routeValues, ApiResponse<T>.FromSucces(data));

        protected ActionResult NotFoundResponse(string entityName, object id)
        {
            var problem = new ProblemDetails
            {
                Title = ValidationErrors.NotFoundTitle,
                Detail = $"{entityName} with ID {id} was not found",
                Status = StatusCodes.Status404NotFound
            };

            return NotFound(ApiResponse<object>.FromError(problem));
        }

        protected ActionResult MismatchResponse()
        {
            var problem = new ProblemDetails
            {
                Title = ValidationErrors.IdsNotMatchingTitle,
                Detail = ValidationErrors.IdsNotMatching,
                Status = StatusCodes.Status400BadRequest
            };

            return BadRequest(ApiResponse<object>.FromError(problem));
        }
    }
}
