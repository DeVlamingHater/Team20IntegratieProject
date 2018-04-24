using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POC_IntegratieProject_framework
{
   public class PersoonParser
    {
        [JsonProperty("first_name")]
        public string firstName { get; set; }
        [JsonProperty("last_name")]
        public string lastName { get; set; }
        [JsonProperty("district")]
        public string district { get; set; }
        [JsonProperty("level")]
        public string level { get; set; }
        [JsonProperty("gender")]
        public string gender { get; set; }
        [JsonProperty("twitter")]
        public string twitter { get; set; }
        [JsonProperty("site")]
        public string site { get; set; }
        [JsonProperty("dateOfBirth")]
        public string dateOfBirth { get; set; }
        [JsonProperty("facebook")]
        public string facebook { get; set; }
        [JsonProperty("postal_code")]
        public string postal_code { get; set; }
        [JsonProperty("full_name")]
        public string full_name { get; set; }
        [JsonProperty("position")]
        public string position { get; set; }
        [JsonProperty("organisation")]
        public string organisation { get; set; }
        [JsonProperty("id")]
        public string id { get; set; }
        [JsonProperty("town")]
        public string town { get; set; }
    }
}
