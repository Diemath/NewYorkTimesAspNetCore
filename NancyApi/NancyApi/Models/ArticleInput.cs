using System;

namespace NancyApi.Models
{
  public class ArticleInput
  {
    public string Section { get; set; }
    public DateTime UpdatedDate { get; set; }
    public string ShortUrl { get; set; }
  }
}
