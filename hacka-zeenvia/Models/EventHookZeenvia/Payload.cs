using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models
{
    
    public partial class Payload
    {
        [JsonProperty("visitor")]
        public Visitor Visitor { get; set; }
    }
    

}
