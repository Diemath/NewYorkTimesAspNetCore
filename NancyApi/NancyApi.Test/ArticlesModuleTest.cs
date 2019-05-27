using Nancy;
using Nancy.Testing;
using NancyApi.Modules;
using Services.Abstractions;
using Services.Concrete;
using System.Threading.Tasks;
using Xunit;

namespace NancyApi.Test
{
  public class ArticlesModuleTest
  {
    [Theory]
    [InlineData("/")]
    [InlineData("/list/test-section")]
    [InlineData("/list/test-section/first")]
    [InlineData("/list/test-section/test-updated-date")]
    [InlineData("/article/short-url")]
    [InlineData("/group/test-section")]
    public async Task ShouldReturnStatusOkWhenRouteExists(string route)
    {
      // Given
      var bootstrapper = new ConfigurableBootstrapper(with => {
          with.Module<ArticlesModule>();
          with.Dependency<IArticleService>(typeof(ArticleService));
        });
      var browser = new Browser(bootstrapper);

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
      var bootstrapper = new ConfigurableBootstrapper(with => {
        with.Module<ArticlesModule>();
        with.Dependency<IArticleService>(typeof(ArticleService));
      });
      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get("/unexisting-route", with => {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.NotFound, result.StatusCode);
    }
  }
}
