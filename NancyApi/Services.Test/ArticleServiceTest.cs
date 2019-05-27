
using Microsoft.Extensions.Options;
using Moq;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using Services.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Services.Test
{
  public class ArticleServiceTest
  {
    public Mock<IConfigProvider> mockConfigProvider;

    public ArticleServiceTest()
    {
      mockConfigProvider = new Mock<IConfigProvider>();
      mockConfigProvider.SetupGet(c => c.Config).Returns(new AppConfig());
    }

    public class GetArticlesBySectionAndUpdatedDateMethods
    {
      public Mock<IConfigProvider> _mockConfigProvider;
      public GetArticlesBySectionAndUpdatedDateMethods()
      {
        _mockConfigProvider = new Mock<IConfigProvider>();
        _mockConfigProvider.SetupGet(c => c.Config).Returns(new AppConfig
        {
          NyTimesApi = new NyTimesApiConfig
          {
            BaseUrl = "https://api.sometesturl.com/",
            Id = "test-unique-identifier"
          }
        });
      }

      [Fact]
      public async Task BuildsRequestUrlLookingAtConfigurationsAndPassedSection()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpGetter>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{results: []}");

        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);

        // Act
        await target.FilterArticlesAsync(ArticleSection.Arts);
        await target.FilterArticlesAsync(ArticleSection.Arts, DateTime.Now);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Exactly(2));
      }

      [Fact]
      public async Task CreatesDtoForEachArticle()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpGetter>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
            {
              results: [
                {
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM'
                }
              ]
            }
          ");

        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);

        // Act
        var result = await target.FilterArticlesAsync(ArticleSection.Arts);

        // Assert
        void assert(ArticleDto articleDto)
        {
          Assert.Equal("test-title", articleDto.Title);
          Assert.Equal("test-url", articleDto.Url);
          Assert.Equal("5/24/2019 6:24:16 PM", articleDto.UpdatedDateTime.ToString());
        }

        var singleArticle = result.Single();
        assert(singleArticle);

        // Act
        result = await target.FilterArticlesAsync(ArticleSection.Arts, DateTime.Parse("5/24/2019 6:24:20 PM", CultureInfo.InvariantCulture));

        // Assert
        singleArticle = result.Single();
        assert(singleArticle);
      }

      [Fact]
      public async Task FiltersInMemoryByUpdatedDate()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpGetter>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync($@"
            {{
              results: [
                {{
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM'
                }}
              ]
            }},
            {{
              results: [
                {{
                  title: 'test-title2',
                  url: 'test-url2',
                  updated_date: '5/25/2019 6:24:16 PM'
                }}
              ]
            }}
          ");

        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);

        // Act
        var result = await target.FilterArticlesAsync(ArticleSection.Arts, DateTime.Parse("5/24/2019 6:35:20 PM", CultureInfo.InvariantCulture)); // Another time but date the same

        // Assert
        var singleArticle = result.Single();
        Assert.Equal("5/24/2019 6:24:16 PM", singleArticle.UpdatedDateTime.ToString());
      }

      //[Fact]
      //public async Task Test001()
      //{
      //  //https://api.nytimes.com/svc/topstories/v2/science.json?api-key=yourkey
      //  var mockNyTimesApiConfig = new Mock<IConfigProvider>();
      //  mockNyTimesApiConfig.SetupGet(c => c.Config)
      //    .Returns(new AppConfig
      //    {
      //      NyTimesApi = new NyTimesApiConfig
      //      {
      //        BaseUrl = "https://api.nytimes.com/svc/topstories/v2/",
      //        Id = "k0XA0k0jJGAVuv8Jr5wAIcKDGPuznmRJ"
      //      }
      //    });

      //  IArticleService articleAccessor = new ArticleService(new HttpGetter(), mockNyTimesApiConfig.Object);
      //  var result = await articleAccessor.GetBySectionAsync(ArticleSection.Science);
      //}
    }
  }
}

