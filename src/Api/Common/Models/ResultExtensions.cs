using Api.Common.Enums;

namespace Api.Common.Models;

public static class ResultExtensions
{
    extension(Result result)
    {
        public IResult ToHttpResult()
        {
            if (result.IsSuccess)
            {
                return Results.Ok();
            }

            return result.Error!.ErrorType switch
            {
                ErrorType.NotFound => Results.NotFound(result.Error),
                ErrorType.Validation => Results.BadRequest(result.Error),
                ErrorType.BadRequest => Results.BadRequest(result.Error),
                ErrorType.Unauthorized => Results.Json(result.Error, statusCode: StatusCodes.Status401Unauthorized),
                _ => Results.Json(result.Error, statusCode: StatusCodes.Status500InternalServerError)
            };
        }
    }
}