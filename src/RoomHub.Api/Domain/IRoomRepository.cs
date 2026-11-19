namespace RoomHub.Api.Domain;

public interface IRoomRepository
{
    Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default);
    void Add(Room room);
}
