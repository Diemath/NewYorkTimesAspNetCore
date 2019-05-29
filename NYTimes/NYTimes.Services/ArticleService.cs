using Mapster;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Configurations;
using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using NYTimes.Services.Abstractions.Exceptions;
using NYTimes.Services.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NYTimes.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IRestClient _restClient;
        private readonly ApiConfig _apiConfig;

        public ArticleService(IRestClientFactory restClientFactory, IOptions<ApiConfig> options)
        {
            _restClient = restClientFactory.GetRestClient();
            _apiConfig = options.Value;
        }

        public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section)
        {
            CheckExceptions(section);

            var result = (await FilterArticlesBySectionAsync(section))
              .Adapt<IEnumerable<ArticleDto>>();
            return result;
        }

        public async Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section, DateTime updatedDate)
        {
            CheckExceptions(section);

            var result = (await FilterArticlesBySectionAsync(section))
              .Where(x => x.UpdatedDateTime.Date == updatedDate.Date)
              .Adapt<IEnumerable<ArticleDto>>();
            return result;
        }

        public async Task<ArticleDto> GetArticleAsync(string shortUrl)
        {
            CheckExceptions(shortUrl);

            var result = (await FilterArticlesBySectionAsync(Section.Home))
              .FirstOrDefault(x => x.ShortUrl == _apiConfig.ShortUrlTemplate.Replace("{ShortUrlId}", shortUrl))
              ?.Adapt<ArticleDto>();
            return result;
        }

        public async Task<ArticleDto> GetArticleAsync(Section section)
        {
            CheckExceptions(section);

            var result = (await FilterArticlesBySectionAsync(section))
              .FirstOrDefault()
              ?.Adapt<ArticleDto>();
            return result;
        }

        public async Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(Section section)
        {
            CheckExceptions(section);

            var result = (await FilterArticlesBySectionAsync(section))
              .GroupBy(x => x.UpdatedDateTime.Date)
              .Select(x => new ArticleGroupByDateDto
              {
                  Total = x.Count(),
                  UpdatedDate = x.Key
              });
            return result;
        }

        private async Task<IEnumerable<ArticleJson>> FilterArticlesBySectionAsync(Section section)
        {
            _restClient.BaseUrl = new Uri(_apiConfig.BaseUrl);

            var request = new RestRequest("svc/topstories/v2/{section}.json", Method.GET);

            request.AddUrlSegment("section", section.ToString().ToLower());
            request.AddParameter("api-key", _apiConfig.Id);

            var restResponse = await _restClient.ExecuteTaskAsync(request);

            return JsonConvert.DeserializeObject<ArticlesJson>(restResponse.Content).Results;
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
