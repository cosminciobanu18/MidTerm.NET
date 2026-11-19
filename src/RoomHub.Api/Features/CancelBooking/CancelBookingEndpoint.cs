using RoomHub.Api.Common;

namespace RoomHub.Api.Features.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", async (
            Guid id,
            ICommandHandler<CancelBookingCommand, Result> handler,
            CancellationToken ct) =>
        {
            var command = new CancelBookingCommand(id);
            var result = await handler.HandleAsync(command, ct);

            return result.IsSuccess
                ? Results.NoContent()
                : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
