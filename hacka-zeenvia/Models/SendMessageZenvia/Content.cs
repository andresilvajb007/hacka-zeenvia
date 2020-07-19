using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models.SendMessageZenvia
{
    public partial class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("payload")]
        public string Payload { get; set; }
    }
}
