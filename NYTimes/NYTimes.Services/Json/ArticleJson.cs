using Newtonsoft.Json;
using System;

namespace NYTimes.Services.Json
{
    public class ArticleJson
    {
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("url")]
        public string Url { get; set; }
        [JsonProperty("updated_date")]
        public DateTime UpdatedDateTime { get; set; }
        [JsonProperty("short_url")]
        public string ShortUrl { get; set; }
    }
}
