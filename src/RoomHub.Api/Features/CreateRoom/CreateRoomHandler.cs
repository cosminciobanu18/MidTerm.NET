using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CreateRoom;

public class CreateRoomHandler(RoomRepository rooms)
    : ICommandHandler<CreateRoomCommand, Result<Room>>
{
    public Task<Result<Room>> HandleAsync(CreateRoomCommand command, CancellationToken ct = default)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsSuccess)
        {
            rooms.Add(result.Value);
        }

        return Task.FromResult(result);
    }
}
