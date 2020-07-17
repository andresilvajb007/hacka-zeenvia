using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models
{
    
    public partial class Content
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("payload", NullValueHandling = NullValueHandling.Ignore)]
        public Payload Payload { get; set; }

        [JsonProperty("text", NullValueHandling = NullValueHandling.Ignore)]
        public String Text { get; set; }
    }



}
