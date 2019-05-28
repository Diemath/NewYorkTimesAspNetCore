using Moq;
using NYTimes.Services.Abstractions;
using RestSharp;
using Services.Abstractions;
using Services.Abstractions.Configurations;

namespace Services.Test
{
  public class ArticleServiceFactsBase
  {
    protected Mock<IConfigProvider> _mockConfigProvider = new Mock<IConfigProvider>();

    protected Mock<IRestClientFactory> _mockRestClientFactory = new Mock<IRestClientFactory>();
    protected Mock<IRestClient> _mockRestClient = new Mock<IRestClient>();
    protected Mock<IRestResponse> _mockRestResponse = new Mock<IRestResponse>();

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

      SetupRestSharp();
    }

    private void SetupRestSharp()
    {
      _mockRestClientFactory.Setup(f => f.GetRestClient()).Returns(_mockRestClient.Object);
      _mockRestClient.Setup(c => c.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(_mockRestResponse.Object);
      _mockRestResponse.SetupGet(r => r.Content).Returns("{results:[]}");
    }
  }
}

