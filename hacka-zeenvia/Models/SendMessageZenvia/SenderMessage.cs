using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models.SendMessageZenvia
{
    public partial class SenderMessageRequest
    {
        [JsonProperty("from")]
        public string From { get; set; }

        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("contents")]
        public List<Content> Contents { get; set; }

    }


}
