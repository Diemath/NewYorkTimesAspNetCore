using System.Collections.Generic;

namespace NYTimes.Services.Json
{
  public class ArticlesJson
  {
    public IEnumerable<ArticleJson> Results { get; set; }
  }
}
