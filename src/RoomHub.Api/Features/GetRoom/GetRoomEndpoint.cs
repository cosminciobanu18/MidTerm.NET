using RoomHub.Api.Repositories;

namespace RoomHub.Api.Features.GetRoom;

public record GetRoomResponse(Guid Id, string Name, int Capacity);

public static class GetRoomEndpoint
{
    public static void MapGetRoom(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", (Guid id, RoomRepository rooms) =>
        {
            var room = rooms.Get(id);
            if (room is null)
            {
                return Results.Problem(
                    statusCode: StatusCodes.Status404NotFound,
                    title: "NotFound",
                    detail: "The room does not exist.");
            }

            return Results.Ok(new GetRoomResponse(room.Id, room.Name, room.Capacity));
        }).WithTags("Rooms");
    }
}
