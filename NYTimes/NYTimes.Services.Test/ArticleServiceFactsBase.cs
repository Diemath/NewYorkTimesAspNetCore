using Moq;
using Services.Abstractions;
using Services.Abstractions.Configurations;

namespace Services.Test
{
  public class ArticleServiceFactsBase
  {
    protected Mock<IConfigProvider> _mockConfigProvider = new Mock<IConfigProvider>();

    public ArticleServiceFactsBase()
    {
      // Setup default mock configurations
      _mockConfigProvider.SetupGet(c => c.Config).Returns(new AppConfig
      {
        Api = new ApiConfig
        {
          BaseUrl = "https://api.sometesturl.com/",
          Id = "test-unique-identifier",
          ShortUrlTemplate = "https://nyti.ms/{ShortUrlId}"
        }
      });
    }
  }
}

