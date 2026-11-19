# BUGS

The starter contains behaviors that contradict `ASSIGNMENT.md`. Write here every discrepancy you found.
Fill it in yourself, in your own words. There is no required number: what counts is that they are real.

| Which rule from ASSIGNMENT.md is violated (R1–R8) | Where the problem was in the starter (file, approximate line) | How you discovered it (test, reading, AI) | What you changed |
|---|---|---|---|
| R5 | RoomHub.Api.Services.BookingService (metoda Create) | AI | _(fill in)_ startUtc poate fi in trecut|
| R6 | RoomHub.Api.Services.BookingService (metoda Create) | AI | _(fill in)_ nu se verifica durata de 4h |
| R7 | RoomHub.Api.Services.BookingService (liniile 26-30 in foreach) | AI | _(fill in)_ <= la suprapunere|
| R7 | RoomHub.Api.Services.BookingService (liniile 26-30 in foreach) | AI | _(fill in)_ itereaza inclusiv prin bookingurile anulate|
| R8 | RoomHub.Api.Services.BookingService (liniile 56-61 in functia Cancel) | AI | _(fill in)_ nu se returneaza 409 daca anulam de 2 ori|
| R8 | RoomHub.Api.Models.Booking (liniile 56-61 in functia Cancel) | AI | _(fill in)_ status are setter public|