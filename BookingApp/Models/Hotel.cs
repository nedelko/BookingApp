namespace BookingApp.Models
{
    public class Hotel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public List<RoomType> RoomTypes { get; set; }
        public List<Room> Rooms { get; set; }

        public Hotel()
        {
            RoomTypes = new List<RoomType>();
            Rooms = new List<Room>();
        }
    }
}
