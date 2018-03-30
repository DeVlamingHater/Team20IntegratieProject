using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class Tweet
    {
        [JsonProperty("id")]
        public long TweetId { get; set; }
        [JsonProperty("user_id")]
        public string User_id { get; set; }
        [JsonProperty("geo")]
        public string geo { get; set; }
        [JsonProperty("mentions")]
        public List<string> Mentions { get; set; }
        [JsonProperty("retweet")]
        public bool Retweet { get; set; }
        [JsonProperty("date")]
        public DateTime Date { get; set; }
        [JsonProperty("words")]
        public List<string> Words { get; set; }
        [JsonProperty("sentiment")]
        public double[] Sentiment { get; set; }
        [JsonProperty("hashtags")]
        public List<string> Hashtags { get; set; }
        [JsonProperty("urls")]
        public List<string> Urls { get; set; }
        [JsonProperty("politician")]
        public string[] Politician { get; set; }


    }
}
