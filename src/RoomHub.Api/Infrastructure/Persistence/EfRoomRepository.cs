using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public class EfRoomRepository(RoomHubDbContext context) : IRoomRepository
{
    public async Task<Room?> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await context.Rooms.FirstOrDefaultAsync(r => r.Id == id, ct);
    }

    public void Add(Room room)
    {
        context.Rooms.Add(room);
    }
}
