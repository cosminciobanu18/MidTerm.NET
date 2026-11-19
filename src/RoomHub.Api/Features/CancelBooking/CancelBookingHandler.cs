using RoomHub.Api.Common;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CancelBooking;

public class CancelBookingHandler(BookingRepository bookings)
    : ICommandHandler<CancelBookingCommand, Result>
{
    public Task<Result> HandleAsync(CancelBookingCommand command, CancellationToken ct = default)
    {
        var booking = bookings.Get(command.Id);
        if (booking is null)
        {
            return Task.FromResult(Result.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist.")));
        }

        var result = booking.Cancel();
        return Task.FromResult(result);
    }
}
