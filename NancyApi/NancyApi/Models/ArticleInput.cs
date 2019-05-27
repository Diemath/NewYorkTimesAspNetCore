using Services.Abstractions.Enums;
using System;

namespace NancyApi.Models
{
  public class ArticleInput
  {
    public ArticleSection Section { get; set; }
    public string UpdatedDate { get; set; }
    public string ShortUrl { get; set; }
  }
}
