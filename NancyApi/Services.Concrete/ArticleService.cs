using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Abstractions;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using Microsoft.Extensions.Options;
using Services.Concrete.Configurations;

namespace Services.Concrete
{
  public class ArticleService : IArticleService
  {
    private readonly NyTimesApiConfig _config;
    private readonly IArticleAccessor _articleAccessor;

    public ArticleService(IOptions<NyTimesApiConfig> config, IArticleAccessor articleAccessor)
    {
      _config = config.Value;
      _articleAccessor = articleAccessor;
    }

    public async Task<IEnumerable<ArticleDto>> GetArticlesBySectionAsync(ArticleSection section)
    {
      return await _articleAccessor.GetBySectionAsync(_config.BaseUrl, _config.UniqueIdentifier, section.ToString().ToLower());
    }
  }
}
