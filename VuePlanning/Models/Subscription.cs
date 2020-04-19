using MessagePack;

namespace VuePlanning.Models
{
    [MessagePackObject]
    public class Subscription
    {
        [Key("endpoint")]
        public string Endpoint { get; set; }
        [Key("keys")]
        public Keys Keys { get; set; }
    }

    [MessagePackObject]
    public class Keys
    {
        [Key("p256dh")]
        public string P256dh { get; set; }
        [Key("auth")]
        public string Auth { get; set; }
    }

}
