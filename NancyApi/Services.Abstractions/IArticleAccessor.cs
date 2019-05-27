using Services.Abstractions.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IArticleAccessor
  {
    Task<IEnumerable<ArticleDto>> GetBySectionAsync(string baseUrl, string uniqueIdentifier, string section);
  }
}
