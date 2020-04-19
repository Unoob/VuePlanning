using MessagePack;

namespace VuePlanning.Models
{
    [MessagePackObject]
    public class User
    {
        [Key("connectionId")]
        public string ConnectionId { get; set; }
        [Key("name")]
        public string Name { get; set; }
        [Key("isHost")]
        public bool IsHost { get; set; }
        [Key("roomId")]
        public string RoomId { get; set; }
        [Key("vote")]
        public string Vote { get; set; }
        [Key("userState")]
        public int UserState { get; set; }
        [Key("subscription")]
        public Subscription Subscription { get; set; }
    }
}