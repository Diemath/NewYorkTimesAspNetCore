using Services.Abstractions.Enums;

namespace NancyApi.Models
{
  public class ArticlesBySectionAndDateQueryParams
  {
    public string Section { get; set; }
    public string UpdatedDate { get; set; }
  }
}
