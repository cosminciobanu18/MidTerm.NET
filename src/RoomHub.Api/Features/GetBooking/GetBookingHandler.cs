using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.GetBooking;

public class GetBookingHandler(IBookingRepository bookings)
    : IQueryHandler<GetBookingQuery, Result<Booking>>
{
    public async Task<Result<Booking>> HandleAsync(GetBookingQuery query, CancellationToken ct = default)
    {
        var booking = await bookings.GetByIdAsync(query.Id, ct);
        if (booking is null)
        {
            return Result<Booking>.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist."));
        }

        return Result<Booking>.Success(booking);
    }
}
