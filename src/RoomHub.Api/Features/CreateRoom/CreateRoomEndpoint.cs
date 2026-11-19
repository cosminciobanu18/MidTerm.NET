using RoomHub.Api.Models;
using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.CreateRoom;

public record CreateRoomRequest(string? Name, int Capacity);

public record CreateRoomResponse(Guid Id, string Name, int Capacity);

public static class CreateRoomEndpoint
{
    public static void MapCreateRoom(this IEndpointRouteBuilder app)
    {
        app.MapPost("/api/rooms", (CreateRoomRequest? request, RoomRepository rooms) =>
        {
            if (request is null || string.IsNullOrWhiteSpace(request.Name))
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "Room name is required.");
            }

            if (request.Capacity <= 0)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status400BadRequest,
                    title: "ValidationFailed",
                    detail: "Capacity must be positive.");
            }

            var room = new Room
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Capacity = request.Capacity
            };

            rooms.Add(room);

            return Results.Created(
                $"/api/rooms/{room.Id}",
                new CreateRoomResponse(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
