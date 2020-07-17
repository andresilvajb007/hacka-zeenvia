using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models
{
    
    public partial class Message
    {
        [JsonProperty("id")]
        public Guid Id { get; set; }

        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("direction")]
        public string Direction { get; set; }

        [JsonProperty("channel")]
        public string Channel { get; set; }

        [JsonProperty("visitor")]
        public Visitor Visitor { get; set; }

        [JsonProperty("contents")]
        public Content[] Contents { get; set; }
    }

    

}
