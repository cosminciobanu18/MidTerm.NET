using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.GetBooking;

public class GetBookingHandler(BookingRepository bookings)
    : IQueryHandler<GetBookingQuery, Result<Booking>>
{
    public Task<Result<Booking>> HandleAsync(GetBookingQuery query, CancellationToken ct = default)
    {
        var booking = bookings.Get(query.Id);
        if (booking is null)
        {
            return Task.FromResult(Result<Booking>.Failure(
                Error.NotFound("BookingNotFound", "The booking does not exist.")));
        }

        return Task.FromResult(Result<Booking>.Success(booking));
    }
}
