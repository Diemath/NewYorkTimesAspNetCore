using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Services.Concrete.Models
{
  public class ArticlesJsonResult
  {
    public IEnumerable<ArticleJsonResult> Results { get; set; }
  }

  public class ArticleJsonResult
  {
    [JsonProperty("title")]
    public string Title { get; set; }
    [JsonProperty("url")]
    public string Url { get; set; }
    [JsonProperty("updated_date")]
    public DateTime UpdatedDateTime { get; set; }
  }
}
