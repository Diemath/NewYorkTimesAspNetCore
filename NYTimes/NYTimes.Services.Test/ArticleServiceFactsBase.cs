using Microsoft.Extensions.Options;
using Moq;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Configurations;
using RestSharp;

namespace NYTimes.Services.Test
{
    public class ArticleServiceFactsBase
    {
        protected Mock<IOptions<ApiOptions>> _mockApiConfig = new Mock<IOptions<ApiOptions>>();

        protected Mock<IRestClientFactory> _mockRestClientFactory = new Mock<IRestClientFactory>();
        protected Mock<IRestClient> _mockRestClient = new Mock<IRestClient>();
        protected Mock<IRestResponse> _mockRestResponse = new Mock<IRestResponse>();

        public ArticleServiceFactsBase()
        {
            // Setup default mock configurations
            _mockApiConfig.SetupGet(c => c.Value).Returns(new ApiOptions
            {
                BaseUrl = "https://api.sometesturl.com/",
                Key = "test-unique-identifier",
                ShortUrlTemplate = "https://nyti.ms/{ShortUrlId}"
            }
            );

            SetupRestSharp();
        }

        private void SetupRestSharp()
        {
            _mockRestClientFactory.Setup(f => f.GetRestClient()).Returns(_mockRestClient.Object);
            _mockRestClient.Setup(c => c.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(_mockRestResponse.Object);
            _mockRestResponse.SetupGet(r => r.IsSuccessful).Returns(true);
            _mockRestResponse.SetupGet(r => r.Content).Returns("{results:[]}");
        }
    }
}

