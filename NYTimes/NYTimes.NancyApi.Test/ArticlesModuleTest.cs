using Moq;
using Nancy;
using Nancy.Testing;
using Newtonsoft.Json;
using NYTimes.NancyApi.NancyModules;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Xunit;

namespace NYTimes.NancyApi.Test
{
  // IArticlesService should be mock
  public class ArticlesModuleTest : ArticlesModuleTestBase
  {
    [Theory]
    [InlineData("/")]
    [InlineData("/list/arts")]
    [InlineData("/list/arts/first")]
    [InlineData("/list/arts/2019-12-11")]
    [InlineData("/article/short-url")]
    [InlineData("/group/arts")]
    public async Task ReturnsStatusOkWhenRouteExists(string route)
    {
      var configurableBootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency(_mockArticleService.Object);
      });

      var browser = new Browser(configurableBootstrapper);

      // When
      var result = await browser.Get(route, with =>
      {
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
      mockArticleService.Setup(g => g.FilterArticlesAsync(Section.Arts))
        .ReturnsAsync(new ArticleDto[] { });

      var configurableBootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(configurableBootstrapper);

      // When
      var result = await browser.Get("/unexisting-route", with =>
      {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionListRequested()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>();
      mockArticleService.Setup(g => g.FilterArticlesAsync(Section.Arts))
        .ReturnsAsync(new ArticleDto[] {
          new ArticleDto
          {
            Title = "test-title",
            Url = "test-url",
            UpdatedDateTime = DateTime.Parse("5/24/2019 6:24:16 PM", CultureInfo.InvariantCulture)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });
      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts", with =>
      {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest[]>(result.Body.AsString());

      Assert.Equal("test-title", body[0].heading);
      Assert.Equal("test-url", body[0].link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body[0].updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFirstFilteredByExistingSectionRequested()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetArticleAsync(Section.Arts))
        .ReturnsAsync(new ArticleDto
        {
          Title = "test-title",
          Url = "test-url",
          UpdatedDateTime = DateTime.Parse("5/24/2019 6:24:16 PM", CultureInfo.InvariantCulture)
        });

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts/first", with =>
      {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest>(result.Body.AsString());

      Assert.Equal("test-title", body.heading);
      Assert.Equal("test-url", body.link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body.updated);
    }

    [Fact]
    public async Task ReturnsNotFoundIfFirstFilteredByExistingSectionArticleDoesNotExist()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetArticleAsync(Section.Arts))
        .ReturnsAsync((ArticleDto)null);

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts/first", with =>
      {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionAndUpdatedDateRequested()
    {
      // Given
      var testUpdatedDate = DateTime.Parse("5/24/2019 6:24:16 PM", CultureInfo.InvariantCulture);
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.FilterArticlesAsync(Section.Arts, testUpdatedDate.Date))
        .ReturnsAsync(new ArticleDto[] {
          new ArticleDto
          {
            Title = "test-title",
            Url = "test-url",
            UpdatedDateTime = testUpdatedDate
          },
          new ArticleDto
          {
            Title = "test-title",
            Url = "test-url",
            UpdatedDateTime = testUpdatedDate.AddHours(1)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/list/arts/2019-05-24", with =>
      {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest[]>(result.Body.AsString());

      Assert.Equal("test-title", body[0].heading);
      Assert.Equal("test-url", body[0].link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body[0].updated);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenArticleByShortUrlRequested()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetArticleAsync("test-short-url"))
        .ReturnsAsync(new ArticleDto
        {
          Title = "test-title",
          Url = "test-url",
          UpdatedDateTime = DateTime.Parse("5/24/2019 6:24:16 PM", CultureInfo.InvariantCulture)
        });

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/article/test-short-url", with =>
      {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleTest>(result.Body.AsString());

      Assert.Equal("test-title", body.heading);
      Assert.Equal("test-url", body.link);
      Assert.Equal("2019-05-24T18:24:16.0000000+03:00", body.updated);
    }

    [Fact]
    public async Task ReturnsNotFoundIfArticleByShortUrlDoesNotExist()
    {
      // Given
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetArticleAsync("test-short-url"))
        .ReturnsAsync((ArticleDto)null);

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/article/test-short-url", with =>
      {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }

    [Fact]
    public async Task ShouldReturnJsonWhenFilteredByExistingSectionGroupsRequested()
    {
      // Given
      var testUpdatedDate = DateTime.Parse("5/24/2019", CultureInfo.InvariantCulture);
      var mockArticleService = new Mock<IArticleService>(MockBehavior.Strict);
      mockArticleService.Setup(g => g.GetGroupsAsync(Section.Arts))
        .ReturnsAsync(new ArticleGroupByDateDto[] {
          new ArticleGroupByDateDto
          {
            Total = 10,
            UpdatedDate = testUpdatedDate
          },
          new ArticleGroupByDateDto
          {
            Total = 11,
            UpdatedDate = testUpdatedDate.AddDays(1)
          }
        });

      var bootstrapper = new ConfigurableBootstrapper(with =>
      {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(mockArticleService.Object);
      });

      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get($"/group/arts", with =>
      {
        with.HttpsRequest();
      });

      // Then
      var body = JsonConvert.DeserializeObject<ArticleGroupTest[]>(result.Body.AsString());

      Assert.Equal("2019-05-24", body[0].date);
      Assert.Equal(10.ToString(), body[0].total);
      Assert.Equal("2019-05-25", body[1].date);
      Assert.Equal(11.ToString(), body[1].total);
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
