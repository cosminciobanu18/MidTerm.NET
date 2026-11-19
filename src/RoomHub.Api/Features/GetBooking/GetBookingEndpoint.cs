using RoomHub.Api.Models;
using RoomHub.Api.Repositories;

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
        app.MapGet("/api/bookings/{id:guid}", (Guid id, BookingRepository bookings) =>
        {
            var booking = bookings.Get(id);
            if (booking is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "NotFound",
                    detail: "The booking does not exist.");
            }

            return Results.Ok(new GetBookingResponse(
                booking.Id,
                booking.RoomId,
                booking.BookedBy,
                booking.StartUtc,
                booking.EndUtc,
                booking.Attendees,
                booking.Status));
        }).WithTags("Bookings");
    }
}
