using RoomHub.Api.Models;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CancelBooking;

public static class CancelBookingEndpoint
{
    public static void MapCancelBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/bookings/{id:guid}/cancel", (Guid id, BookingRepository bookings) =>
        {
            var booking = bookings.Get(id);
            if (booking is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "NotFound",
                    detail: "The booking does not exist.");
            }

            if (booking.Status == BookingStatus.Cancelled)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status409Conflict,
                    title: "Conflict",
                    detail: "The booking is already cancelled.");
            }

            booking.Status = BookingStatus.Cancelled;

            return Results.NoContent();
        }).WithTags("Bookings");
    }
}
