using RoomHub.Api.Common;
using RoomHub.Api.Domain;

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
        app.MapPost("/api/rooms/{roomId:guid}/bookings", async (
            Guid roomId,
            CreateBookingRequest request,
            ICommandHandler<CreateBookingCommand, Result<Booking>> handler,
            CancellationToken ct) =>
        {
            var command = new CreateBookingCommand(
                roomId,
                request.BookedBy,
                request.StartUtc,
                request.EndUtc,
                request.Attendees);

            var result = await handler.HandleAsync(command, ct);

            return result.IsSuccess
                ? Results.Created(
                    $"/api/bookings/{result.Value.Id}",
                    new CreateBookingResponse(result.Value.Id, result.Value.RoomId, result.Value.Status))
                : result.Error.ToProblem();
        }).WithTags("Bookings");
    }
}
