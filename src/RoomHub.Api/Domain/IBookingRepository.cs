namespace RoomHub.Api.Domain;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct = default);
    void Add(Booking booking);
    Task<List<Booking>> GetActiveOverlappingAsync(Guid roomId, DateTime startUtc, DateTime endUtc, CancellationToken ct = default);
    Task<List<Booking>> ListActiveForParticipantOnDateAsync(string bookedBy, DateTime date, CancellationToken ct = default);
}
