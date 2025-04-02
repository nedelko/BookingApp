BookingApp

A simple C# console application to manage hotel room availability and reservations.
This app loads hotel and booking data from JSON files and supports two commands via the terminal:  
- `Availability(...)`
- `RoomTypes(...)`

How to Run

1. Clone or download the repository
git clone https://github.com/nedelko/BookingApp.git
cd BookingApp/BookingApp

2. Run with .NET CLI
dotnet run -- --hotels hotels.json --bookings bookings.json


Commands

1) Availability
Check how many rooms are available for a given date and room type.

Example:
Availability(H1, 20240901, SGL)
Availability(H1, 20240901-20240903, DBL)

2) RoomTypes
Calculate which room types are needed to accommodate a given number of people.

Example:
RoomTypes(H1, 20240904, 3)
RoomTypes(H1, 20240905-20240907, 5)
