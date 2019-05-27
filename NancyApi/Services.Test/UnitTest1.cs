
using Microsoft.Extensions.Options;
using Moq;
using Services.Abstractions;
using Services.Abstractions.Enums;
using Services.Concrete;
using Services.Concrete.Configurations;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Services.Test
{
  public class ArticleServiceTest
  {
    public class GetArticlesBySectionMethod
    {
      [Fact]
      public async Task GatherCorrectRequestUrlToGetArticlesBySectionUsingConfig()
      {
        var testBaseUrl = "https://api.sometesturl.com/";
        var testUniqueIdentifier = "test-unique-identifier";
        var section = ArticleSection.Arts;

        var mockNyTimesApiConfig = new Mock<IOptions<NyTimesApiConfig>>();
        mockNyTimesApiConfig.SetupGet(c => c.Value)
          .Returns(new NyTimesApiConfig
          {
            BaseUrl = testBaseUrl,
            UniqueIdentifier = testUniqueIdentifier
          });

        var mockHttpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"{testBaseUrl}arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == testUniqueIdentifier))))
          .ReturnsAsync("{}");

        var target = new ArticleService(mockNyTimesApiConfig.Object, new ArticleAccessor(mockHttpGetter.Object));

        // Act
        var result = await target.GetArticlesBySectionAsync(section);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDtoForEachArticle()
      {
        var testTitle = "test-title";
        var testUrl = "test-url";
        var testUpdatedDate = "5/24/2019 6:24:16 PM";

        var mockNyTimesApiConfig = new Mock<IOptions<NyTimesApiConfig>>();
        mockNyTimesApiConfig.SetupGet(c => c.Value).Returns(new NyTimesApiConfig());

        var mockHttpGetter = new Mock<IHttpGetter>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync($@"
            {{
              results: [
                {{
                  title: '{testTitle}',
                  url: '{testUrl}',
                  updated_date: '{testUpdatedDate}'
                }}
              ]
            }}
          ");

        IArticleService target = new ArticleService(mockNyTimesApiConfig.Object, new ArticleAccessor(mockHttpGetter.Object));

        // Act
        var result = await target.GetArticlesBySectionAsync(ArticleSection.Arts);

        // Assert
        var singleArticle = result.Single();
        Assert.Equal(testTitle, singleArticle.Title);
        Assert.Equal(testUrl, singleArticle.Url);
        Assert.Equal(testUpdatedDate, singleArticle.UpdatedDateTime.ToString());
      }

      //[Fact]
      //public async Task Test001()
      //{
      //  //https://api.nytimes.com/svc/topstories/v2/science.json?api-key=yourkey
      //  IArticleAccessor articleAccessor = new ArticleAccessor(new HttpGetter());
      //  var result = await articleAccessor.GetBySectionAsync("https://api.nytimes.com/svc/topstories/v2/", "k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ", "science");
      //}
    }
  }
}
