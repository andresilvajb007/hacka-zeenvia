using System;
using Newtonsoft.Json;

namespace hacka_zeenvia.Models
{
   
    public partial class Visitor
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("firstName")]
        public string FirstName { get; set; }

        [JsonProperty("lastName")]
        public string LastName { get; set; }
    }


}
