using RoomHub.Api.Models;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CreateBooking;

public record CreateBookingRequest(
    string? BookedBy,
    DateTime StartUtc,
    DateTime EndUtc,
    int Attendees);

public record CreateBookingResponse(
    Guid Id,
    Guid RoomId,
    BookingStatus Status);

public static class CreateBookingEndpoint
{
    public static void MapCreateBooking(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms/{roomId:guid}/bookings", (
            Guid roomId,
            CreateBookingRequest? request,
            RoomRepository rooms,
            BookingRepository bookings) =>
        {
            var room = rooms.Get(roomId);
            if (room is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "NotFound",
                    detail: "The room does not exist.");
            }

            if (request is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "Request body is required.");
            }

            if (string.IsNullOrWhiteSpace(request.BookedBy))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "BookedBy is required.");
            }

            if (request.EndUtc <= request.StartUtc)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "The end must be after the start.");
            }

            if (request.Attendees <= 0)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "Attendees must be positive.");
            }

            if (request.Attendees > room.Capacity)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "The room does not have enough seats.");
            }

            foreach (var existing in bookings.ListForRoom(roomId))
            {
                if (existing.Status == BookingStatus.Active &&
                    existing.StartUtc < request.EndUtc &&
                    request.StartUtc < existing.EndUtc)
                {
                    return Results.Problem(
                        statusCode: StatusCodes.Status409Conflict,
                        title: "Conflict",
                        detail: "The room is occupied in this interval.");
                }
            }

            var booking = new Booking
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                BookedBy = request.BookedBy.Trim(),
                StartUtc = request.StartUtc,
                EndUtc = request.EndUtc,
                Attendees = request.Attendees,
                Status = BookingStatus.Active
            };

            bookings.Add(booking);

            return Results.Created(
                $"/api/bookings/{booking.Id}",
                new CreateBookingResponse(booking.Id, booking.RoomId, booking.Status));
        }).WithTags("Bookings");
    }
}
