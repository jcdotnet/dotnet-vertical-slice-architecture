using Api.Common.Enums;

namespace Api.Common.Models;

public static class ResultExtensions
{
    extension<T>(Result<T> result)
    {
        public IResult ToHttpResult()
        {
            return result switch
            {
                T value => Results.Ok(value),

                Error error => error.ErrorType switch
                {
                    ErrorType.NotFound => Results.NotFound(error),
                    ErrorType.Validation => Results.BadRequest(error),
                    ErrorType.BadRequest => Results.BadRequest(error),
                    ErrorType.Unauthorized => Results.Json(error, statusCode: 401),
                    _ => Results.Json(error, statusCode: 500)
                },
                null => Results.Json(Error.NullValue, statusCode: 500)
            };
        }
    }
}