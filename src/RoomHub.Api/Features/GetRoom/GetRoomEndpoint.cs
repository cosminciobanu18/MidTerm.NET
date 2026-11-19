using RoomHub.Api.Common;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Features.GetRoom;

public record GetRoomResponse(Guid Id, string Name, int Capacity);

public static class GetRoomEndpoint
{
    public static void MapGetRoom(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/rooms/{id:guid}", async (
            Guid id,
            IQueryHandler<GetRoomQuery, Result<Room>> handler,
            CancellationToken ct) =>
        {
            var query = new GetRoomQuery(id);
            var result = await handler.HandleAsync(query, ct);

            return result.IsSuccess
                ? Results.Ok(new GetRoomResponse(result.Value.Id, result.Value.Name, result.Value.Capacity))
                : result.Error.ToProblem();
        }).WithTags("Rooms");
    }
}
