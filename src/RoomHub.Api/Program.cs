using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Common;
using RoomHub.Api.Domain;
using RoomHub.Api.Features.CancelBooking;
using RoomHub.Api.Features.CreateBooking;
using RoomHub.Api.Features.CreateRoom;
using RoomHub.Api.Features.GetBooking;
using RoomHub.Api.Features.GetRoom;
using RoomHub.Api.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddSingleton(TimeProvider.System);

builder.Services.AddDbContext<RoomHubDbContext>(options =>
    options.UseSqlite("Data Source=roomhub.db"));

builder.Services.AddScoped<IUnitOfWork>(sp => sp.GetRequiredService<RoomHubDbContext>());
builder.Services.AddScoped<IRoomRepository, EfRoomRepository>();
builder.Services.AddScoped<IBookingRepository, EfBookingRepository>();

builder.Services.AddScoped<ICommandHandler<CreateRoomCommand, Result<Room>>, CreateRoomHandler>();
builder.Services.AddScoped<IQueryHandler<GetRoomQuery, Result<Room>>, GetRoomHandler>();
builder.Services.AddScoped<ICommandHandler<CreateBookingCommand, Result<Booking>>, CreateBookingHandler>();
builder.Services.AddScoped<IQueryHandler<GetBookingQuery, Result<Booking>>, GetBookingHandler>();
builder.Services.AddScoped<ICommandHandler<CancelBookingCommand, Result>, CancelBookingHandler>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<RoomHubDbContext>();
    db.Database.EnsureCreated();
}

app.MapGet("/", () => "RoomHub API is running.");
app.MapCreateRoom();
app.MapGetRoom();
app.MapCreateBooking();
app.MapGetBooking();
app.MapCancelBooking();

app.Run();
