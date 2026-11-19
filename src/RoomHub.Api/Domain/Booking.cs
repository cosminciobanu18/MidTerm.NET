using RoomHub.Api.Common;

namespace RoomHub.Api.Domain;

public class Booking
{
    public Guid Id { get; private set; }
    public Guid RoomId { get; private set; }
    public string BookedBy { get; private set; } = string.Empty;
    public DateTime StartUtc { get; private set; }
    public DateTime EndUtc { get; private set; }
    public int Attendees { get; private set; }
    public BookingStatus Status { get; private set; }

    private Booking() { }

    private Booking(
        Guid id,
        Guid roomId,
        string bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        BookingStatus status)
    {
        Id = id;
        RoomId = roomId;
        BookedBy = bookedBy;
        StartUtc = startUtc;
        EndUtc = endUtc;
        Attendees = attendees;
        Status = status;
    }

    public static Result<Booking> Create(
        Room room,
        string? bookedBy,
        DateTime startUtc,
        DateTime endUtc,
        int attendees,
        IEnumerable<Booking> existingRoomBookings,
        IEnumerable<Booking> existingParticipantBookings,
        TimeProvider timeProvider)
    {
        if (string.IsNullOrWhiteSpace(bookedBy))
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidBookedBy", "BookedBy is required."));
        }

        if (endUtc <= startUtc)
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidRange", "The end must be after the start."));
        }

        if (attendees <= 0)
        {
            return Result<Booking>.Failure(
                Error.Validation("InvalidAttendees", "Attendees must be positive."));
        }

        if (attendees > room.Capacity)
        {
            return Result<Booking>.Failure(
                Error.Validation("AttendeesExceedCapacity", "The room does not have enough seats."));
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        if (startUtc < nowUtc)
        {
            return Result<Booking>.Failure(
                Error.Validation("PastStart", "Booking cannot start in the past."));
        }

        var maxDuration = room.Capacity > 10
            ? TimeSpan.FromHours(2)
            : TimeSpan.FromHours(4);

        if (endUtc - startUtc > maxDuration)
        {
            var message = room.Capacity > 10
                ? "Rooms with capacity greater than 10 can be booked for at most 2 hours."
                : "Maximum booking duration is 4 hours.";

            return Result<Booking>.Failure(
                Error.Validation("DurationExceeded", message));
        }

        foreach (var existing in existingRoomBookings)
        {
            if (existing.RoomId == room.Id &&
                existing.Status == BookingStatus.Active &&
                existing.StartUtc < endUtc &&
                startUtc < existing.EndUtc)
            {
                return Result<Booking>.Failure(
                    Error.Conflict("RoomOccupied", "The room is occupied in this interval."));
            }
        }

        var activeParticipantBookingsCount = existingParticipantBookings
            .Count(b => b.Status == BookingStatus.Active &&
                        string.Equals(b.BookedBy, bookedBy.Trim(), StringComparison.OrdinalIgnoreCase) &&
                        b.StartUtc.Date == startUtc.Date);

        if (activeParticipantBookingsCount >= 2)
        {
            return Result<Booking>.Failure(
                Error.Conflict("ParticipantDailyLimitExceeded", "A participant cannot have more than 2 active bookings per day."));
        }

        var booking = new Booking(
            Guid.NewGuid(),
            room.Id,
            bookedBy.Trim(),
            startUtc,
            endUtc,
            attendees,
            BookingStatus.Active);

        return Result<Booking>.Success(booking);
    }

    public Result Cancel(TimeProvider timeProvider)
    {
        if (Status == BookingStatus.Cancelled)
        {
            return Result.Failure(
                Error.Conflict("AlreadyCancelled", "The booking is already cancelled."));
        }

        var nowUtc = timeProvider.GetUtcNow().UtcDateTime;
        if (StartUtc - nowUtc < TimeSpan.FromMinutes(60))
        {
            return Result.Failure(
                Error.Conflict("TooLateToCancel", "A booking can be cancelled only if it starts at least 60 minutes from now."));
        }

        Status = BookingStatus.Cancelled;
        return Result.Success();
    }
}
