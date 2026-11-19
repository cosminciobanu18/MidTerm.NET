namespace RoomHub.Api.Common;

public static class ErrorHttpExtensions
{
    public static IResult ToProblem(this Error? error)
    {
        if (error is null)
        {
            return Results.Problem(statusCode: StatusCodes.Status500InternalServerError);
        }

        var statusCode = error.Type switch
        {
            ErrorType.Validation => StatusCodes.Status400BadRequest,
            ErrorType.NotFound => StatusCodes.Status404NotFound,
            ErrorType.Conflict => StatusCodes.Status409Conflict,
            _ => StatusCodes.Status500InternalServerError
        };

        return Results.Problem(
            statusCode: statusCode,
            title: error.Code,
            detail: error.Message);
    }
}
