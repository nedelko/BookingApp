using BookingApp.Managers;
using BookingApp.Models;
using System.Text.Json;

class HotelReservationApp
{
    private static List<Hotel> hotels = new();
    private static List<Booking> bookings = new();

    static void Main(string[] args)
    {
        //string hotelsPath = "C:\\Users\\..\\source\\repos\\BookingApp\\hotels.json";
        string hotelsPath = GetPath(args, "--hotels");

        if (!File.Exists(hotelsPath))
        {
            Console.WriteLine("Hotel file not found");
            return;
        }

        //string bookingsPath = "C:\\Users\\..\\source\\repos\\BookingApp\\bookings.json";
        string bookingsPath = GetPath(args, "--bookings");

        if (!File.Exists(bookingsPath))
        {
            Console.WriteLine("Booking file not found.");
            return;
        }

        hotels = JsonSerializer.Deserialize<List<Hotel>>(File.ReadAllText(hotelsPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        bookings = JsonSerializer.Deserialize<List<Booking>>(File.ReadAllText(bookingsPath), new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        Console.WriteLine("Application is ready. Enter commands:");

        ReservationsManager reservationsManager = new ReservationsManager();
        reservationsManager.Init(hotels, bookings);

        while (true)
        {
            Console.Write("> ");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
            {
                break;
            }

            if (input.StartsWith("Availability"))
            {
                reservationsManager.HandleAvailability(input);
            }
            else if (input.StartsWith("RoomTypes"))
            {
                reservationsManager.HandleRoomTypes(input);
            }
            else
            {
                Console.WriteLine("Unknown command.");
            }
        }
    }

    static string GetPath(string[] args, string name)
    {
        var idx = Array.IndexOf(args, name);
        return (idx != -1 && idx < args.Length - 1) ? args[idx + 1] : "";
    }
}