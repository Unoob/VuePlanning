namespace VuePlanning.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
        public bool IsHost { get; set; }
        public string RoomId { get; set; }
        public double Vote { get; set; }
    }
}