using System.Collections.Generic;
using System.Threading.Tasks;
using Services.Abstractions;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Services.Concrete.Models;
using Mapster;
using Services.Abstractions.Configurations;
using System;
using System.Linq;

namespace Services.Concrete
{
  public class ArticleService : IArticleService
  {
    private readonly IHttpGetter _httpGetter;
    private readonly NyTimesApiConfig _config;

    public ArticleService(IHttpGetter httpGetter, IConfigProvider configProvider)
    {
      _httpGetter = httpGetter;
      _config = configProvider.Config.NyTimesApi;
    }

    public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(ArticleSection section)
      => await FilterArticlesBySectionAsync(section);

    public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(ArticleSection section, DateTime updatedDate)
      => (await FilterArticlesBySectionAsync(section))
        .Where(dto => dto.UpdatedDateTime.Date == updatedDate.Date);

    public Task<ArticleDto> GetArticleAsync(string shortUrl)
    {
      throw new NotImplementedException();
    }

    public Task<ArticleDto> GetArticleAsync(ArticleSection section)
    {
      throw new NotImplementedException();
    }

    public Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(ArticleSection section)
    {
      throw new NotImplementedException();
    }

    private async Task<IEnumerable<ArticleDto>> FilterArticlesBySectionAsync(ArticleSection section)
    {
      string responseMessage = await _httpGetter.GetAsync($"{_config.BaseUrl}{section}.json", 
        new KeyValuePair<string, string>("format", "json"),
        new KeyValuePair<string, string>("api-key", _config.Id));

      return JsonConvert.DeserializeObject<ArticlesJsonResult>(responseMessage).Results.Adapt<IEnumerable<ArticleDto>>();
    }
  }
}
