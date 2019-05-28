using Moq;
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
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{results: []}");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await target.FilterArticlesAsync(Section.Arts, DateTime.Now);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
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

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        var result = await target.FilterArticlesAsync(Section.Arts, DateTime.Parse("5/24/2019"));

        // Assert
        Assert.Equal("test-title", result.Single().Title);
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();

        // Act Assert: Pass unexisting section
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.FilterArticlesAsync((Section)1000, DateTime.Parse("5/24/2019")));
      }
    }

    public class GetFirstArticleBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{results: []}");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await target.GetArticleAsync(Section.Arts);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
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

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        var result = await target.GetArticleAsync(Section.Arts);

        // Assert
        Assert.Equal("test-title", result.Title);
      }

      [Fact]
      public async Task ReturnsNullForNotFoundArticle()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
            {
              results: [ ]
            }
          ");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        var result = await target.GetArticleAsync(Section.Arts);

        // Assert
        Assert.Null(result);
      }

      [Fact]
      public async Task ThrowsExceptionIfSectionUnvalid()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();

        // Act Assert: Pass unexisting section
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.GetArticleAsync((Section)1000));
      }
    }

    public class GetArticlesBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{results: []}");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await target.FilterArticlesAsync(Section.Arts);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDtos()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
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

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();

        // Act Assert: Pass unexisting section
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.FilterArticlesAsync((Section)1000));
      }
    }

    public class GetArticleByShortUrlMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/home.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{results: []}");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await target.GetArticleAsync("XXXXXXX");

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDto()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        var result = await target.GetArticleAsync("2YNxSD2");

        // Assert
        Assert.Equal("test-title", result.Title);
      }

      [Fact]
      public async Task ReturnsNullForNotFoundArticle()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        var result = await target.GetArticleAsync("XXXXXXX");

        // Assert
        Assert.Null(result);
      }

      [Fact]
      public async Task ThrowsExceptionIfShortUrlUnvalid()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();

        // Act Assert: Pass unvalid short url. short_url: https://nyti.ms/2YNxSD2
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await Assert.ThrowsAsync<UnvalidShortUrlException>(() => target.GetArticleAsync("unvalid-short-url"));
      }
    }

    public class GetArticleGroupsBySectionMethod : ArticleServiceFactsBase
    {
      [Fact]
      public async Task UsesValidRequestUrl()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>(MockBehavior.Strict);
        mockHttpGetter.Setup(g => g.GetAsync($"https://api.sometesturl.com/arts.json", It.Is<KeyValuePair<string, string>[]>(x => x.Any(y => y.Key == "api-key" && y.Value == "test-unique-identifier"))))
          .ReturnsAsync("{ results: [ ] }");


        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await target.GetGroupsAsync(Section.Arts);

        // Assert
        mockHttpGetter.Verify(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()), Times.Once);
      }

      [Fact]
      public async Task CreatesDtos()
      {
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
            {
              results: [
                {
                  updated_date: '5/24/2019 6:24:16 PM'
                }
              ]
            }
          ");

        // Act
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        var mockHttpGetter = new Mock<IHttpRequestHelper>();
        mockHttpGetter.Setup(g => g.GetAsync(It.IsAny<string>(), It.IsAny<KeyValuePair<string, string>[]>()))
          .ReturnsAsync(@"
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
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
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
        // Arrange
        var mockHttpGetter = new Mock<IHttpRequestHelper>();

        // Act Assert: Pass unexisting section
        var target = new ArticleService(mockHttpGetter.Object, _mockConfigProvider.Object);
        await Assert.ThrowsAsync<UndefinedEnumException>(() => target.GetGroupsAsync((Section)1000));
      }
    }
  }
}

