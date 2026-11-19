using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.CreateBooking;

public class CreateBookingHandler(
    IRoomRepository rooms,
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CreateBookingCommand, Result<Booking>>
{
    public async Task<Result<Booking>> HandleAsync(CreateBookingCommand command, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(command.RoomId, ct);
        if (room is null)
        {
            return Result<Booking>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist."));
        }

        var overlappingBookings = await bookings.GetActiveOverlappingAsync(
            command.RoomId, command.StartUtc, command.EndUtc, ct);

        var participantBookings = !string.IsNullOrWhiteSpace(command.BookedBy)
            ? await bookings.ListActiveForParticipantOnDateAsync(command.BookedBy.Trim(), command.StartUtc, ct)
            : [];

        var result = Booking.Create(
            room,
            command.BookedBy,
            command.StartUtc,
            command.EndUtc,
            command.Attendees,
            overlappingBookings,
            participantBookings,
            timeProvider);

        if (result.IsFailure)
        {
            return result;
        }

        bookings.Add(result.Value);
        await unitOfWork.SaveChangesAsync(ct);

        return result;
    }
}
