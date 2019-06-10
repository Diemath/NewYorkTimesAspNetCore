using Mapster;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using NYTimes.Services.Configurations;
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
        private readonly ApiOptions _apiConfig;

        public ArticleService(IRestClientFactory restClientFactory, IOptions<ApiOptions> options)
        {
            _restClient = restClientFactory.GetRestClient();
            _apiConfig = options.Value;
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync(Section section)
        {
            var articles = await GetArticlesBySectionAsync(section);
            return articles.Adapt<IEnumerable<ArticleDto>>();
        }

        public async Task<IEnumerable<ArticleDto>> GetArticlesAsync(Section section, DateTime updatedDate)
        {
            var articles = await GetArticlesBySectionAsync(section);
            return articles
                .Where(x => x.UpdatedDateTime.Date == updatedDate.Date)
                .Adapt<IEnumerable<ArticleDto>>();
        }

        public async Task<ArticleDto> GetArticleAsync(string shortUrl)
        {
            var articles = await GetArticlesBySectionAsync(Section.Home);
            return articles
              .SingleOrDefault(x => x.ShortUrl == _apiConfig.ShortUrlTemplate.Replace("{ShortUrlId}", shortUrl))
              ?.Adapt<ArticleDto>();
        }

        public async Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(Section section)
        {
            var articles = await GetArticlesBySectionAsync(section);
            return articles
              .GroupBy(x => x.UpdatedDateTime.Date)
              .Select(x => new ArticleGroupByDateDto
              {
                  Total = x.Count(),
                  UpdatedDate = x.Key
              });
        }

        private async Task<IEnumerable<ArticleJson>> GetArticlesBySectionAsync(Section section)
        {
            _restClient.BaseUrl = new Uri(_apiConfig.BaseUrl);

            var resource = _apiConfig.Resource.Replace("{section}", section.ToString().ToLower());
            var request = new RestRequest(resource, Method.GET);

            request.AddParameter("api-key", _apiConfig.Id);

            var restResponse = await _restClient.ExecuteTaskAsync(request);

            if (!restResponse.IsSuccessful)
                throw new Exception(restResponse.ErrorMessage);

            return JsonConvert.DeserializeObject<ArticlesJson>(restResponse.Content).Results;
        }
    }
}
