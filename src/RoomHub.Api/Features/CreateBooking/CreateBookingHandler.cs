using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CreateBooking;

public class CreateBookingHandler(
    RoomRepository rooms,
    BookingRepository bookings,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<Booking>>
{
    public Task<Result<Booking>> HandleAsync(CreateBookingCommand command, CancellationToken ct = default)
    {
        var room = rooms.Get(command.RoomId);
        if (room is null)
        {
            return Task.FromResult(Result<Booking>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist.")));
        }

        var existingRoomBookings = bookings.ListForRoom(command.RoomId);
        var existingParticipantBookings = !string.IsNullOrWhiteSpace(command.BookedBy)
            ? bookings.ListActiveForParticipantOnDate(command.BookedBy.Trim(), command.StartUtc)
            : [];

        var result = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            existingRoomBookings,
            existingParticipantBookings,
            timeProvider);

        if (result.IsSuccess)
        {
            bookings.Add(result.Value);
        }

        return Task.FromResult(result);
    }
}
