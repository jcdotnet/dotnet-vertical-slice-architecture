using Api.Common.Enums;

namespace Api.Common.Models;

public record Error(string Code, string Description, ErrorType ErrorType, object? Extensions = null)
{
    public static readonly Error None = Failure(string.Empty, string.Empty);
    public static readonly Error NullValue = Failure("Error.NullValue", "Null Value Provided");
    public static readonly Error TitleRequired = Validation("ProjectTask.TitleRequired", "Title is required");

    // Factory methods
    public static Error NotFound(string code, string desc) => new(code, desc, ErrorType.NotFound);

    public static Error Validation(string code, string desc, object? extensions = null) =>
        new(code, desc, ErrorType.Validation, extensions);
    public static Error Validation(string code, string desc) => new(code, desc, ErrorType.Validation);
    public static Error BadRequest(string code, string desc) => new(code, desc, ErrorType.BadRequest);
    public static Error Unauthorized(string code, string desc) => new(code, desc, ErrorType.Unauthorized);
    public static Error Failure(string code, string desc) => new(code, desc, ErrorType.Failure);
}