using Microsoft.Extensions.Options;
using Moq;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Configurations;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Text;

namespace NYTimes.Services.Test
{
    public abstract class TestBase
    {
        protected readonly Mock<IOptions<ApiOptions>> mockApiConfig = new Mock<IOptions<ApiOptions>>();
        protected readonly Mock<IRestClientFactory> mockRestClientFactory = new Mock<IRestClientFactory>();
        protected readonly Mock<IRestClient> mockRestClient = new Mock<IRestClient>();
        protected readonly Mock<IRestResponse> mockRestResponse = new Mock<IRestResponse>();
        protected readonly ApiOptions apiOptions = new ApiOptions
        {
            BaseUrl = "https://api.someapi.com/",
            Key = string.Empty,
            Resource = string.Empty
        };

        public TestBase()
        {
            mockApiConfig.SetupGet(c => c.Value).Returns(apiOptions);
        }

        protected ArticleService targetArticleService => new ArticleService(mockRestClientFactory.Object, mockApiConfig.Object);

        protected void SetupSuccessfulResult(string result)
        {
            mockRestClientFactory.Setup(f => f.GetRestClient()).Returns(mockRestClient.Object);
            mockRestClient.Setup(c => c.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(mockRestResponse.Object);
            mockRestResponse.SetupGet(r => r.IsSuccessful).Returns(true);
            mockRestResponse.SetupGet(r => r.Content).Returns(result);
        }
    }
}
