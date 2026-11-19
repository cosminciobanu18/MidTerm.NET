using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.CreateRoom;

public class CreateRoomHandler(
    IRoomRepository rooms,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateRoomCommand, Result<Room>>
{
    public async Task<Result<Room>> HandleAsync(CreateRoomCommand command, CancellationToken ct = default)
    {
        var result = Room.Create(command.Name, command.Capacity);
        if (result.IsFailure)
        {
            return result;
        }

        rooms.Add(result.Value);
        await unitOfWork.SaveChangesAsync(ct);

        return result;
    }
}
