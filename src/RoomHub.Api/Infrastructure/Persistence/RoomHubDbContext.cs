using Microsoft.EntityFrameworkCore;
using RoomHub.Api.Domain;

namespace RoomHub.Api.Infrastructure.Persistence;

public class RoomHubDbContext(DbContextOptions<RoomHubDbContext> options)
    : DbContext(options), IUnitOfWork
{
    public DbSet<Room> Rooms => Set<Room>();
    public DbSet<Booking> Bookings => Set<Booking>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Room>(builder =>
        {
            builder.HasKey(r => r.Id);
            builder.Property(r => r.Name).IsRequired();
            builder.Property(r => r.Capacity).IsRequired();
        });

        modelBuilder.Entity<Booking>(builder =>
        {
            builder.HasKey(b => b.Id);
            builder.Property(b => b.RoomId).IsRequired();
            builder.Property(b => b.BookedBy).IsRequired();
            builder.Property(b => b.StartUtc).IsRequired();
            builder.Property(b => b.EndUtc).IsRequired();
            builder.Property(b => b.Attendees).IsRequired();
            builder.Property(b => b.Status)
                .HasConversion<string>()
                .IsRequired();
        });
    }
}
