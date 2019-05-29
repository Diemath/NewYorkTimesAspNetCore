using Moq;
using NYTimes.Services.Abstractions;
using RestSharp;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Abstractions.Enums;
using Services.Abstractions.Exceptions;
using Services.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace Services.Test
{
  public class ArticleServiceTest
  {
    public class GetArticlesBySectionAndCreatedDateMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await target.FilterArticlesAsync(Section.Arts, DateTime.Now);

        // Assert
        _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
        _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
          r.Resource == "svc/topstories/v2/{section}.json" &&
          r.Parameters.Any(p => 
            p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" || 
            p.Name == "section" && p.Value.ToString() == "arts"
          ))), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
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

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.FilterArticlesAsync(Section.Arts, DateTime.Parse("5/24/2019"));
        var firstArticle = result.First();

        // Assert
        Assert.Equal("test-title", firstArticle.Title);
        Assert.Equal("test-url", firstArticle.Url);
        Assert.Equal("5/24/2019 6:24:16 PM", firstArticle.UpdatedDateTime.ToString());
      }

      [Fact]
      public async Task FiltersArticlesByUpdatedDate()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM'
                },
                {
                  title: 'another-title',
                  url: 'another-url',
                  updated_date: '6/24/2019 6:24:16 PM',
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.FilterArticlesAsync(Section.Arts, DateTime.Parse("5/24/2019"));

        // Assert
        Assert.Equal("test-title", result.Single().Title);
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Act Assert: Pass unexisting section
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.FilterArticlesAsync((Section)1000, DateTime.Parse("5/24/2019")));
      }
    }

    public class GetFirstArticleBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await target.GetArticleAsync(Section.Arts);

        // Assert
        _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
        _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
          r.Resource == "svc/topstories/v2/{section}.json" &&
          r.Parameters.Any(p =>
            p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
            p.Name == "section" && p.Value.ToString() == "arts"
          ))), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
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

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync(Section.Arts);

        // Assert
        Assert.Equal("test-title", result.Title);
        Assert.Equal("test-url", result.Url);
        Assert.Equal("5/24/2019 6:24:16 PM", result.UpdatedDateTime.ToString());
      }

      [Fact]
      public async Task TakesFirstFoundArticle()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM'
                },
                {
                  title: 'another-title',
                  url: 'another-url',
                  updated_date: '6/24/2019 6:24:16 PM',
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync(Section.Arts);

        // Assert
        Assert.Equal("test-title", result.Title);
      }

      [Fact]
      public async Task ReturnsNullForNotFoundArticle()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [ ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync(Section.Arts);

        // Assert
        Assert.Null(result);
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Act Assert: Pass unexisting section
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.GetArticleAsync((Section)1000));
      }
    }

    public class GetArticlesBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await target.FilterArticlesAsync(Section.Arts);
        
        // Assert
        _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
        _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
          r.Resource == "svc/topstories/v2/{section}.json" &&
          r.Parameters.Any(p =>
            p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
            p.Name == "section" && p.Value.ToString() == "arts"
          ))), Times.Once);
      }

      [Fact]
      public async Task CreatesDtos()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
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

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.FilterArticlesAsync(Section.Arts);
        var firstArticle = result.First();

        // Assert
        Assert.Equal("test-title", firstArticle.Title);
        Assert.Equal("test-url", firstArticle.Url);
        Assert.Equal("5/24/2019 6:24:16 PM", firstArticle.UpdatedDateTime.ToString());
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Act Assert: Pass unexisting section
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.FilterArticlesAsync((Section)1000));
      }
    }

    public class GetArticleByShortUrlMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await target.FilterArticlesAsync(Section.Home);

        // Assert
        _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
        _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
          r.Resource == "svc/topstories/v2/{section}.json" &&
          r.Parameters.Any(p =>
            p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
            p.Name == "section" && p.Value.ToString() == "home"
          ))), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM',
                  short_url: 'https://nyti.ms/2YNxSD2'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync("2YNxSD2");

        // Assert
        Assert.Equal("test-title", result.Title);
        Assert.Equal("test-url", result.Url);
        Assert.Equal("5/24/2019 6:24:16 PM", result.UpdatedDateTime.ToString());
      }

      [Fact]
      public async Task TakesFirstFoundArticle()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  title: 'test-title',
                  url: 'test-url',
                  updated_date: '5/24/2019 6:24:16 PM',
                  short_url: 'https://nyti.ms/2YNxSD2'
                },
                {
                  title: 'another-title',
                  url: 'another-url',
                  updated_date: '6/24/2019 6:24:16 PM',
                  short_url: 'https://nyti.ms/2YNxSD2'
                },
                {
                  title: 'another-title',
                  url: 'another-url',
                  updated_date: '7/24/2019 6:24:16 PM',
                  short_url: 'https://nyti.ms/XXXXXXX'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync("2YNxSD2");

        // Assert
        Assert.Equal("test-title", result.Title);
      }

      [Fact]
      public async Task ReturnsNullForNotFoundArticle()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  title: 'another-title',
                  url: 'another-url',
                  updated_date: '6/24/2019 6:24:16 PM',
                  short_url: 'https://nyti.ms/2YNxSD2'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetArticleAsync("XXXXXXX");

        // Assert
        Assert.Null(result);
      }

      [Fact]
      public async Task ThrowsExceptionIfShortUrlUnvalid()
      {
        // Act Assert: Pass unvalid short url. short_url: https://nyti.ms/2YNxSD2
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await Assert.ThrowsAsync<UnvalidShortUrlException>(() => target.GetArticleAsync("unvalid-short-url"));
      }
    }

    public class GetArticleGroupsBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await target.GetGroupsAsync(Section.Arts);

        // Assert
        _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
        _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
          r.Resource == "svc/topstories/v2/{section}.json" &&
          r.Parameters.Any(p =>
            p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
            p.Name == "section" && p.Value.ToString() == "arts"
          ))), Times.Once);
      }

      [Fact]
      public async Task CreatesDtos()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  updated_date: '5/24/2019 6:24:16 PM'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetGroupsAsync(Section.Arts);

        // Assert
        var singleGroup = result.Single();
        Assert.Equal(1, singleGroup.Total);
        Assert.Equal(DateTime.Parse("5/24/2019"), singleGroup.UpdatedDate);
      }

      [Fact]
      public async Task GroupsByDate()
      {
        // Arrange
        _mockRestResponse.SetupGet(r => r.Content)
          .Returns(@"
            {
              results: [
                {
                  updated_date: '5/24/2019 6:24:16 PM'
                },
                {
                  updated_date: '5/24/2019 7:24:16 PM'
                },
                {
                  updated_date: '6/24/2019 6:24:16 PM'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        var result = await target.GetGroupsAsync(Section.Arts);

        // Assert
        Assert.Equal(2, result.Count());

        var group = result.ElementAt(0);
        Assert.Equal(2, group.Total);
        Assert.Equal(DateTime.Parse("5/24/2019"), group.UpdatedDate);

        group = result.ElementAt(1);
        Assert.Equal(1, group.Total);
        Assert.Equal(DateTime.Parse("6/24/2019"), group.UpdatedDate);
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Act Assert: Pass unexisting section
        var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.GetGroupsAsync((Section)1000));
      }
    }
  }
}

