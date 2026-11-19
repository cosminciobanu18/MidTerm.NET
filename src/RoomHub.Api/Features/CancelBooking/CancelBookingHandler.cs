using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.CancelBooking;

public class CancelBookingHandler(
    IBookingRepository bookings,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider)
    : ICommandHandler<CancelBookingCommand, Result>
{
    public async Task<Result> HandleAsync(CancelBookingCommand command, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdAsync(command.Id, ct);
        if (booking is null)
        {
            return Result.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist."));
        }

        var result = booking.Cancel(timeProvider);
        if (result.IsFailure)
        {
            return result;
        }

        await unitOfWork.SaveChangesAsync(ct);

        return result;
    }
}
