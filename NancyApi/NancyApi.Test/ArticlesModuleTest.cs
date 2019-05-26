using Nancy;
using Nancy.Testing;
using System.Threading.Tasks;
using Xunit;

namespace NancyApi.Test
{
  public class ArticlesModuleTest
  {
    [Fact]
    public async Task ShouldReturnStatusOkWhenRouteExists()
    {
      // Given
      var bootstrapper = new ConfigurableBootstrapper(with => with.Module<ArticlesModule>());
      var browser = new Browser(bootstrapper);

      // When
      var result = await browser.Get("/", with => {
        with.HttpsRequest();
      });

      // Then
      Assert.Equal(HttpStatusCode.OK, result.StatusCode);
    }
  }
}
