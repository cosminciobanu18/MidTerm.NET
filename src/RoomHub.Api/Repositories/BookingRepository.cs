using RoomHub.Api.Domain;

namespace RoomHub.Api.Repositories;

public class BookingRepository
{
    private readonly List<Booking> _bookings = new();
    private readonly object _lock = new();

    public void Add(Booking booking)
    {
        lock (_lock)
        {
            _bookings.Add(booking);
        }
    }

    public Booking? Get(Guid id)
    {
        lock (_lock)
        {
            return _bookings.FirstOrDefault(b => b.Id == id);
        }
    }

    public List<Booking> ListForRoom(Guid roomId)
    {
        lock (_lock)
        {
            return _bookings.Where(b => b.RoomId == roomId).ToList();
        }
    }

    public List<Booking> ListActiveForParticipantOnDate(string bookedBy, DateTime date)
    {
        lock (_lock)
        {
            return _bookings.Where(b =>
                b.Status == BookingStatus.Active &&
                string.Equals(b.BookedBy, bookedBy, StringComparison.OrdinalIgnoreCase) &&
                b.StartUtc.Date == date.Date).ToList();
        }
    }
}
