using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public class EfBookingRepository(RoomHubDbContext context) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Bookings.FirstOrDefaultAsync(b => b.Id == id, ct);
    }

    public void Add(Booking booking)
    {
        context.Bookings.Add(booking);
    }

    public async Task<List<Booking>> GetActiveOverlappingAsync(
        Guid roomId,
        DateTime startUtc,
        DateTime endUtc,
        CancellationToken ct = default)
    {
        return await context.Bookings
            .Where(b => b.RoomId == roomId &&
                        b.Status == BookingStatus.Active &&
                        b.StartUtc < endUtc &&
                        startUtc < b.EndUtc)
            .ToListAsync(ct);
    }

    public async Task<List<Booking>> ListActiveForParticipantOnDateAsync(
        string bookedBy,
        DateTime date,
        CancellationToken ct = default)
    {
        var startOfDay = date.Date;
        var endOfDay = startOfDay.AddDays(1);

        return await context.Bookings
            .Where(b => b.Status == BookingStatus.Active &&
                        b.BookedBy.ToLower() == bookedBy.ToLower() &&
                        b.StartUtc >= startOfDay &&
                        b.StartUtc < endOfDay)
            .ToListAsync(ct);
    }
}
