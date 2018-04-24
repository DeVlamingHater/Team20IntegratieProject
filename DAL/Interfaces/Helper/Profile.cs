using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Profile
    {
        [JsonProperty("gender")]
        public string gender { get; set; }

        [JsonProperty("age")]
        public string age { get; set; }

        [JsonProperty("education")]
        public string education { get; set; }

        [JsonProperty("language")]
        public string language { get; set; }

        [JsonProperty("personality")]
        public string personality { get; set; }
    }
}