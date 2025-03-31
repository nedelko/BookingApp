namespace BookingApp.Models
{
    public class RoomType
    {
        public string Code { get; set; }
        public int Size { get; set; }
        public string Descriptor { get; set; }
        public List<string> Amenities { get; set; }
        public List<string> Features { get; set; }

        public RoomType()
        {
            Amenities = new List<string>();
            Features = new List<string>();
        }
    }
}
