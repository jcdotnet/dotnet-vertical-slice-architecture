namespace Api.Common.Models;

/*
 * new response body with Result pattern:
 * 
 * Success (HTTP 200):
 * { "isSuccess": true, "error": null, "value": { ... } }
 *
 * Not Found (HTTP 404): {
 * "isSuccess": false,
 * "error": {
 *    "code": "ProjectTask.NotFound",
 *    "description": "Task b0bfa332-35cc-434c-b26d-8b834de49660 not found.",
 *    "errorType": "NotFound"
 *   }
 * }
 * 
 * Validation Error (Bad Request, HTTP 400):
 * {
 *   "isSuccess": false, "error": { ... } "extensions": { ... } }
 * }
 */
public class Result
{
    protected Result(bool isSuccess, Error? error)
    {
        if (isSuccess && error != null || !isSuccess && error == null)
        {
            throw new ArgumentException("Invalid error state", nameof(error));
        }
        IsSuccess = isSuccess;
        Error = error;
    }

    public bool IsSuccess { get; }
    public Error? Error { get; }

    public static Result SuccessResult() => new(true, null);
    public static Result FailureResult(Error error) => new(false, error);

    public static Result<T> SuccessResult<T>(T value) => new(value, true, null);
    public static Result<T> FailureResult<T>(Error error) => new(default, false, error);
    public static T CreateFailure<T>(Error error)
    {
        if (typeof(T) == typeof(Result))
        {
            return (T)(object)FailureResult(error);
        }
        var method = typeof(T).GetMethod("Failure", [typeof(Error)]);
        return (T)method!.Invoke(null, [error])!;
    }
}

public class Result<T>(T? value, bool isSuccess, Error? error) : Result(isSuccess, error)
{
    public T? Value => value;
    public static Result<T> Failure(Error error) => new(default, false, error);

    public static implicit operator Result<T>(T? value) =>
        value is not null ? SuccessResult(value) : Failure(Error.NullValue);
}