using System;

namespace NYTimes.Services.Abstractions.Dto
{
  public class ArticleGroupByDateDto
  {
    public int Total { get; set; }
    public DateTime UpdatedDate { get; set; }
  }
}
