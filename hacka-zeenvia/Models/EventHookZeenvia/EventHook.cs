using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models
{
    public partial class EventHook
    {
        public int EventHookId { get; set; }

        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("timestamp")]
        public DateTimeOffset Timestamp { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("subscriptionId")]
        public Guid SubscriptionId { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("message")]
        public Message Message { get; set; }
    }

    


}
