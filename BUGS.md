# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | RoomHub.Api.Services.BookingService (metoda Create) | AI | added a check in Create method for time to be in the future|
| R6 | RoomHub.Api.Services.BookingService (metoda Create) | AI | added the missing validation in Create method |
| R7 | RoomHub.Api.Services.BookingService (liniile 26-30 in foreach) | AI | changed from <= to < so two cosecutive intervals are not considered overlapping |
| R7 | RoomHub.Api.Services.BookingService (liniile 26-30 in foreach) | AI | checks to not have status cancelled |
| R8 | RoomHub.Api.Services.BookingService (liniile 56-61 in functia Cancel) | AI | if it's already cancelled return 409 |
| R8 | RoomHub.Api.Models.Booking (liniile 56-61 in functia Cancel) | AI | made private set on BookingStatus and only modify it in Cancel method |