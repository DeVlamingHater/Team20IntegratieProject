using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Tweet
    {
        [JsonProperty("profile")]
        public Profile Profile { get; set; }

        [JsonProperty("id")]
        public long TweetId { get; set; }

        [JsonProperty("urls")]
        public List<string> Urls { get; set; }

        [JsonProperty("sentiment")]
        public double[] Sentiment { get; set; }

        [JsonProperty("persons")]
        public List<string> Persons { get; set; }

        [JsonProperty("hashtags")]
        public List<string> Hashtags { get; set; }

        [JsonProperty("retweet")]
        public bool Retweet { get; set; }

        [JsonProperty("themes")]
        public List<string> Themes { get; set; }

        [JsonProperty("source")]
        public string source { get; set; }

        [JsonProperty("words")]
        public List<string> Words { get; set; }

        [JsonProperty("date")]
        public DateTime Date { get; set; }

        [JsonProperty("mentions")]
        public List<string> Mentions { get; set; }

        //TODO
        //[JsonProperty("geo")]
        //public string geo { get; set; }
    }
}
