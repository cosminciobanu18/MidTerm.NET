using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.GetBooking;

public record GetBookingResponse(
    Guid Id,
    Guid RoomId,
    string BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees,
    BookingStatus Status);

public static class GetBookingEndpoint
{
    public static void MapGetBooking(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/bookings/{id:guid}", async (
            Guid id,
            IQueryHandler<GetBookingQuery, Result<Booking>> handler,
            CancellationToken ct) =>
        {
            var query = new GetBookingQuery(id);
            var result = await handler.HandleAsync(query, ct);

            return result.IsSuccess
                ? Results.Ok(new GetBookingResponse(
                    result.Value.Id,
                    result.Value.RoomId,
                    result.Value.BookedBy,
                    result.Value.StartUtc,
                    result.Value.EndUtc,
                    result.Value.Attendees,
                    result.Value.Status))
                : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
