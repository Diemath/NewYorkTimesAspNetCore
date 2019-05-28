using System;

namespace Services.Abstractions.Dto
{
  public class ArticleDto
  {
    public string Title { get; set; }
    public string Url { get; set; }
    public DateTime UpdatedDateTime { get; set; }
  }
}
