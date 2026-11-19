using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.CreateRoom;

public record CreateRoomRequest(string? Name, int Capacity);

public record CreateRoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", async (
            CreateRoomRequest request,
            ICommandHandler<CreateRoomCommand, Result<Room>> handler,
            CancellationToken ct) =>
        {
            var command = new CreateRoomCommand(request.Name, request.Capacity);
            var result = await handler.HandleAsync(command, ct);

            return result.IsSuccess
                ? Results.Created(
                    $"/api/rooms/{result.Value.Id}",
                    new CreateRoomResponse(result.Value.Id, result.Value.Name, result.Value.Capacity))
                : result.Error.ToProblem();
        }).WithTags("Rooms");
    }
}
