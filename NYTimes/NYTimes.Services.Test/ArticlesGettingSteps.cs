using Microsoft.Extensions.Options;
using Moq;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Enums;
using NYTimes.Services.Configurations;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace NYTimes.Services.Test
{
    [Binding]
    public class ArticlesGettingSteps
    {
        private Mock<IOptions<ApiOptions>> _mockApiConfig = new Mock<IOptions<ApiOptions>>();
        private Mock<IRestClientFactory> _mockRestClientFactory = new Mock<IRestClientFactory>();
        private Mock<IRestClient> _mockRestClient = new Mock<IRestClient>();
        private Mock<IRestResponse> _mockRestResponse = new Mock<IRestResponse>();

        public ArticlesGettingSteps()
        {
            SetupSuccessfulEmptyResult();
        }

        [Given(@"backend configuration file has such base API url - ""(.*)"", such API key - ""(.*)"" and such resource ""(.*)""")]
        public void GivenBackendConfigurationFileHasSuchBaseAPIUrl_SuchAPIKey_AndSuchResource(string baseUrl, string id, string resource)
        {
            _mockApiConfig.SetupGet(c => c.Value).Returns(new ApiOptions { BaseUrl = baseUrl, Id = id, Resource = resource });
        }
        
        [When(@"I get articles by section ""(.*)""")]
        public async Task WhenIGetArticlesBySection(Section section)
        {
            await new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object)
                .GetArticlesAsync(section);
        }
        
        [When(@"I get articles by section ""(.*)"" and date ""(.*)""")]
        public async Task WhenIGetArticlesBySectionAndDate(Section section, DateTime dateTime)
        {
            await new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object)
                .GetArticlesAsync(section, dateTime);
        }
        
        [Then(@"backend rest client will use ""(.*)"" like base API url, ""(.*)"" like resource and will has a parameter ""(.*)"" with value ""(.*)""")]
        public void ThenBackendRestClientWillUseLikeBaseAPIUrlLikeResourceAndWillHasAParameterWithValue(string baseUrl, string resource, string parameterName, string parameterValue)
        {
            _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == baseUrl), Times.Once);
            _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r => r.Resource == resource &&
                r.Parameters.Any(p => p.Name == parameterName && p.Value.ToString() == parameterValue))), Times.Once);
        }

        private void SetupSuccessfulEmptyResult()
        {
            _mockRestClientFactory.Setup(f => f.GetRestClient()).Returns(_mockRestClient.Object);
            _mockRestClient.Setup(c => c.ExecuteTaskAsync(It.IsAny<IRestRequest>())).ReturnsAsync(_mockRestResponse.Object);
            _mockRestResponse.SetupGet(r => r.IsSuccessful).Returns(true);
            _mockRestResponse.SetupGet(r => r.Content).Returns("{ results:[] }");
        }
    }
}
