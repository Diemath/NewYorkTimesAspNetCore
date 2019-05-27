using Newtonsoft.Json;
using Services.Abstractions;
using Services.Abstractions.Dto;
using Services.Concrete.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mapster;

namespace Services.Concrete
{
  public class ArticleAccessor : IArticleAccessor
  {
    private readonly IHttpGetter _httpGetter;

    public ArticleAccessor(IHttpGetter httpGetter)
    {
      _httpGetter = httpGetter;
    }

    public async Task<IEnumerable<ArticleDto>> GetBySectionAsync(string baseUrl, string uniqueIdentifier, string section)
    {
      var parameters = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("format", "json") };
      if (uniqueIdentifier != null)
        parameters.Add(new KeyValuePair<string, string>("api-key", uniqueIdentifier));

      string responseMessage = await _httpGetter.GetAsync($"{baseUrl}{section}.json", parameters.ToArray());
      
      return JsonConvert.DeserializeObject<ArticlesJsonResult>(responseMessage).Results.Adapt<IEnumerable<ArticleDto>>();
    }
  }
}
