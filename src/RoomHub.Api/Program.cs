using System.Text.Json;
using System.Text.Json.Serialization;
using RoomHub.Api.Features.CancelBooking;
using RoomHub.Api.Features.CreateBooking;
using RoomHub.Api.Features.CreateRoom;
using RoomHub.Api.Features.GetBooking;
using RoomHub.Api.Features.GetRoom;
using RoomHub.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton<RoomRepository>();
builder.Services.AddSingleton<BookingRepository>();

var app = builder.Build();

app.MapGet("/", () => "RoomHub API is running.");
app.MapCreateRoom();
app.MapGetRoom();
app.MapCreateBooking();
app.MapGetBooking();
app.MapCancelBooking();

app.Run();
