using Mapster;
using Newtonsoft.Json;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using Services.Concrete.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Services.Abstractions.Exceptions;

namespace Services.Concrete
{
  public class ArticleService : IArticleService
  {
    private readonly IHttpRequestHelper _httpGetter;
    private readonly ApiConfig _apiConfig;

    public ArticleService(IHttpRequestHelper httpGetter, IConfigProvider configProvider)
    {
      _httpGetter = httpGetter;
      _apiConfig = configProvider.Config.Api;
    }

    public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section)
    {
      CheckExceptions(section);

      return (await FilterArticlesBySectionAsync(section))
        .Adapt<IEnumerable<ArticleDto>>();
    }

    public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section, DateTime updatedDate)
    {
      CheckExceptions(section);

      return (await FilterArticlesBySectionAsync(section))
        .Where(x => x.UpdatedDateTime.Date == updatedDate.Date)
        .Adapt<IEnumerable<ArticleDto>>();
    }

    public async Task<ArticleDto> GetArticleAsync(string shortUrl)
    {
      CheckExceptions(shortUrl);

      return (await FilterArticlesBySectionAsync(Section.Home))
        .FirstOrDefault(x => x.ShortUrl == _apiConfig.ShortUrlTemplate.Replace("{ShortUrlId}", shortUrl))
        ?.Adapt<ArticleDto>();
    }

    public async Task<ArticleDto> GetArticleAsync(Section section)
    {
      CheckExceptions(section);

      return (await FilterArticlesBySectionAsync(section))
        .FirstOrDefault()
        ?.Adapt<ArticleDto>();
    }

    public async Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(Section section)
    {
      CheckExceptions(section);

      return (await FilterArticlesBySectionAsync(section))
        .GroupBy(x => x.UpdatedDateTime.Date)
        .Select(x => new ArticleGroupByDateDto
        {
          Total = x.Count(),
          UpdatedDate = x.Key
        });
    }

    private async Task<IEnumerable<ArticleJson>> FilterArticlesBySectionAsync(Section section)
    {
      string responseMessage = await _httpGetter.GetAsync($"{_apiConfig.BaseUrl}{section.ToString().ToLower()}.json", 
        new KeyValuePair<string, string>("format", "json"),
        new KeyValuePair<string, string>("api-key", _apiConfig.Id)
      );
      return JsonConvert.DeserializeObject<ArticlesJson>(responseMessage).Results;
    }

    private void CheckExceptions(Section section)
    {
      if (!Enum.IsDefined(typeof(Section), section))
      {
        throw new UndefinedEnumException($"{nameof(UndefinedEnumException)}: {section}.");
      }
    }

    private void CheckExceptions(string shortUrl)
    {
      if (shortUrl.Length != 7)
      {
        throw new UnvalidShortUrlException($"{nameof(UnvalidShortUrlException)}: valid format is XXXXXXX.");
      }
    }
  }
}
