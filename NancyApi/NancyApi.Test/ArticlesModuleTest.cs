using Moq;
using Nancy;
using Nancy.Json;
using Nancy.Testing;
using NancyApi.Modules;
using Newtonsoft.Json;
using Services.Abstractions;
using Services.Abstractions.Configurations;
using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using Services.Concrete;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace NancyApi.Test
{
  public class ArticlesModuleTest
  {
    [Theory]
    [InlineData("/")]
    [InlineData("/list/art")]
    [InlineData("/list/art/first")]
    [InlineData("/list/art/2019-12-11")]
    [InlineData("/article/short-url")]
    [InlineData("/group/art")]
    public async Task ShouldReturnStatusOkWhenRouteExists(string route)
    {
      // Given
      var mockArticleService = new Mock<IArticleService>();
      mockArticleService.Setup(g => g.GetBySectionAsync(It.IsAny<ArticleSection>()))
        .ReturnsAsync(new ArticleDto[] { });
      mockArticleService.Setup(g => g.GetGroupsBySectionAsync(It.IsAny<ArticleSection>()))
        .ReturnsAsync(new ArticleGroupByDateDto[] { });
      mockArticleService.Setup(g => g.GetAsync(It.IsAny<string>()))
        .ReturnsAsync(new ArticleDto { });
      mockArticleService.Setup(g => g.GetFirstBySectionAsync(It.IsAny<ArticleSection>()))
        .ReturnsAsync(new ArticleDto { });

      var configurableBootstrapper = new ConfigurableBootstrapper(with => {
          with.Module<ArticlesModule>();
          with.Dependency<IArticleService>(mockArticleService.Object);
        });

      var browser = new Browser(configurableBootstrapper);

      // When
      var result = await browser.Get(route, with => {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnStatusNotFoundWhenRouteDoesNotExist()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>();
      mockArticleService.Setup(g => g.GetBySectionAsync(ArticleSection.Arts))
        .ReturnsAsync(new ArticleDto[] { });

      var configurableBootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(configurableBootstrapper);

      // When
      var result = await browser.Get("/unexisting-route", with => {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionListRequested()
    {
      // Given
      var testTitle = "test-title";
      var testUrl = "test-url";
      var testUpdatedDate = "5/24/2019 6:24:16 PM";

      var mockArticleService = new Mock<IArticleService>();
      mockArticleService.Setup(g => g.GetBySectionAsync(ArticleSection.Arts))
        .ReturnsAsync(new ArticleDto[] {
          new ArticleDto
          {
            Title = testTitle,
            Url = testUrl,
            UpdatedDateTime = DateTime.Parse(testUpdatedDate, CultureInfo.InvariantCulture)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });
      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts", with => {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest[]>(result.Body.AsString());

      Assert.Equal(testTitle, body[0].heading);
      Assert.Equal(testUrl, body[0].link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body[0].updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFirstFilteredByExistingSectionRequested()
    {
      // Given
      var testTitle = "test-title";
      var testUrl = "test-url";
      var testUpdatedDate = "5/24/2019 6:24:16 PM";

      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetFirstBySectionAsync(ArticleSection.Arts))
        .ReturnsAsync(new ArticleDto {
            Title = testTitle,
            Url = testUrl,
            UpdatedDateTime = DateTime.Parse(testUpdatedDate, CultureInfo.InvariantCulture)
        });

      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts/first", with => {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest>(result.Body.AsString());

      Assert.Equal(testTitle, body.heading);
      Assert.Equal(testUrl, body.link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body.updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionAndUpdatedDateRequested()
    {
      // Given
      var testTitle = "test-title";
      var testUrl = "test-url";
      var testUpdatedDate = DateTime.Parse("5/24/2019 6:24:16 PM", CultureInfo.InvariantCulture);

      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetBySectionAndUpdatedDateTimeAsync(ArticleSection.Arts, testUpdatedDate.Date))
        .ReturnsAsync(new ArticleDto[] {
          new ArticleDto
          {
            Title = testTitle,
            Url = testUrl,
            UpdatedDateTime = testUpdatedDate
          },
          new ArticleDto
          {
            Title = testTitle,
            Url = testUrl,
            UpdatedDateTime = testUpdatedDate.AddHours(1)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts/2019-05-24", with => {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest[]>(result.Body.AsString());

      Assert.Equal(testTitle, body[0].heading);
      Assert.Equal(testUrl, body[0].link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body[0].updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenArticleByShortUrlRequested()
    {
      // Given
      var testTitle = "test-title";
      var testUrl = "test-url";
      var testUpdatedDate = "5/24/2019 6:24:16 PM";
      var testShortUrl = "test-short-url";

      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetAsync(testShortUrl))
        .ReturnsAsync(new ArticleDto
        {
          Title = testTitle,
          Url = testUrl,
          UpdatedDateTime = DateTime.Parse(testUpdatedDate, CultureInfo.InvariantCulture)
        });

      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/article/{testShortUrl}", with => {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest>(result.Body.AsString());

      Assert.Equal(testTitle, body.heading);
      Assert.Equal(testUrl, body.link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body.updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionGroupsRequested()
    {
      // Given
      var testTotal = 10;
      var testUpdatedDate = DateTime.Parse("5/24/2019", CultureInfo.InvariantCulture);

      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetGroupsBySectionAsync(ArticleSection.Arts))
        .ReturnsAsync(new ArticleGroupByDateDto[] {
          new ArticleGroupByDateDto
          {
            Total = testTotal,
            UpdatedDate = testUpdatedDate
          },
          new ArticleGroupByDateDto
          {
            Total = testTotal + 1,
            UpdatedDate = testUpdatedDate.AddDays(1)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/group/arts", with => {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleGroupTest[]>(result.Body.AsString());

      Assert.Equal("2019-05-24", body[0].date);
      Assert.Equal(testTotal.ToString(), body[0].total);
      Assert.Equal("2019-05-25", body[1].date);
      Assert.Equal((testTotal + 1).ToString(), body[1].total);
    }
  }

  class ArticleTest
  {
    public string heading { get; set; }
    public string updated { get; set; }
    public string link { get; set; }
  }

  class ArticleGroupTest
  {
    public string date { get; set; }
    public string total { get; set; }
  }
}
