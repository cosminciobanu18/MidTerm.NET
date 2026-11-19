using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.GetRoom;

public class GetRoomHandler(RoomRepository rooms)
    : IQueryHandler<GetRoomQuery, Result<Room>>
{
    public Task<Result<Room>> HandleAsync(GetRoomQuery query, CancellationToken ct = default)
    {
        var room = rooms.Get(query.Id);
        if (room is null)
        {
            return Task.FromResult(Result<Room>.Failure(
                Error.NotFound("RoomNotFound", "The room does not exist.")));
        }

        return Task.FromResult(Result<Room>.Success(room));
    }
}
