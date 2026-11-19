using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.GetRoom;

public class GetRoomHandler(IRoomRepository rooms)
    : IQueryHandler<GetRoomQuery, Result<Room>>
{
    public async Task<Result<Room>> HandleAsync(GetRoomQuery query, CancellationToken ct = default)
    {
        var room = await rooms.GetByIdAsync(query.Id, ct);
        if (room is null)
        {
            return Result<Room>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist."));
        }

        return Result<Room>.Success(room);
    }
}
