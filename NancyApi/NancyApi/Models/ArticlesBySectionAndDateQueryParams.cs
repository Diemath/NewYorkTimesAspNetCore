using Services.Abstractions.Enums;

namespace NancyApi.Models
{
  public class ArticlesBySectionAndDateQueryParams
  {
    public Section Section { get; set; }
    public string UpdatedDate { get; set; }
  }
}
