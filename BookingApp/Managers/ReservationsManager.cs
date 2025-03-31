using BookingApp.Models;

namespace BookingApp.Managers
{
    public class ReservationsManager
    {
        private List<Hotel> Hotels { get; set; }
        private List<Booking> Bookings { get; set; }

        public ReservationsManager()
        {
            Hotels = new List<Hotel>();
            Bookings = new List<Booking>();
        }

        public void Init(List<Hotel> hotels, List<Booking> bookings)
        {
            Hotels = hotels;
            Bookings = bookings;
        }

        public void HandleAvailability(string input)
        {
            try
            {
                var args = input["Availability".Length..].Trim('(', ')', ' ').Split(',');
                string hotelId = args[0].Trim();
                string dateRange = args[1].Trim();
                string roomType = args[2].Trim();

                DateTime startDate, endDate;
                if (dateRange.Contains("-"))
                {
                    var dates = dateRange.Split('-');
                    startDate = ParseDate(dates[0]);
                    endDate = ParseDate(dates[1]);
                }
                else
                {
                    startDate = ParseDate(dateRange);
                    endDate = startDate;
                }

                var hotel = Hotels.FirstOrDefault(h => h.Id == hotelId);
                if (hotel is null)
                {
                    Console.WriteLine("Hotel not found.");
                    return;
                }

                int totalRooms = hotel.Rooms.Count(r => r.RoomType == roomType);
                int booked = 0;

                foreach (var booking in Bookings.Where(b => b.HotelId == hotelId && b.RoomType == roomType))
                {
                    var arrival = ParseDate(booking.Arrival);
                    var departure = ParseDate(booking.Departure);

                    if (arrival < endDate.AddDays(1) && departure > startDate)
                    {
                        booked++;
                    }
                }

                Console.WriteLine(totalRooms - booked);
            }
            catch
            {
                Console.WriteLine("Error parsing Availability command.");
            }
        }

        public void HandleRoomTypes(string input)
        {
            try
            {
                var args = input["RoomTypes".Length..].Trim('(', ')', ' ').Split(',');
                string hotelId = args[0].Trim();
                string dateRange = args[1].Trim();
                int guests = int.Parse(args[2].Trim());

                DateTime startDate, endDate;
                if (dateRange.Contains("-"))
                {
                    var dates = dateRange.Split('-');
                    startDate = ParseDate(dates[0]);
                    endDate = ParseDate(dates[1]);
                }
                else
                {
                    startDate = ParseDate(dateRange);
                    endDate = startDate;
                }

                var hotel = Hotels.FirstOrDefault(h => h.Id == hotelId);
                if (hotel is null)
                {
                    Console.WriteLine("Hotel not found.");
                    return;
                }

                var roomTypeAvailability = new Dictionary<string, int>();
                foreach (var roomType in hotel.RoomTypes)
                {
                    int available = hotel.Rooms.Count(r => r.RoomType == roomType.Code);
                    foreach (var booking in Bookings.Where(b => b.HotelId == hotelId && b.RoomType == roomType.Code))
                    {
                        var arrival = ParseDate(booking.Arrival);
                        var departure = ParseDate(booking.Departure);
                        if (arrival < endDate.AddDays(1) && departure > startDate)
                            available--;
                    }
                    roomTypeAvailability[roomType.Code] = available;
                }

                // Sort room types by size descending
                var sortedTypes = hotel.RoomTypes.OrderByDescending(rt => rt.Size).ToList();
                var allocation = new List<string>();

                foreach (var rt in sortedTypes)
                {
                    while (roomTypeAvailability[rt.Code] > 0 && guests >= rt.Size)
                    {
                        allocation.Add(rt.Code);
                        roomTypeAvailability[rt.Code]--;
                        guests -= rt.Size;
                    }
                }

                if (guests > 0)
                {
                    // Try to fit the remaining guests partially
                    var partial = sortedTypes.FirstOrDefault(rt => roomTypeAvailability[rt.Code] > 0 && guests <= rt.Size);
                    if (partial != null)
                    {
                        allocation.Add(partial.Code + "!");
                        guests = 0;
                    }
                }

                if (guests > 0)
                {
                    Console.WriteLine("Cannot allocate enough rooms for the group.");
                    return;
                }

                Console.WriteLine($"{hotel.Id}: {string.Join(", ", allocation)}");
            }
            catch
            {
                Console.WriteLine("Error parsing RoomTypes command.");
            }
        }

        private DateTime ParseDate(string yyyymmdd)
        {
            return DateTime.ParseExact(yyyymmdd, "yyyyMMdd", null);
        }
    }
}
