using Moq;
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
    public class ArticlesGettingAPIIntegrationSteps : TestBase
    {
        public ArticlesGettingAPIIntegrationSteps()
        {
            SetupSuccessfulResult("{ results:[] }"); // Result is not important for this feature 
        }

        [Given(@"backend configuration file has such base API url ""(.*)""")]
        public void GivenBackendConfigurationFileHasSuchBaseAPIUrl(string baseUrl)
        {
            apiOptions.BaseUrl = baseUrl;
        }
        
        [Given(@"such API key ""(.*)""")]
        public void GivenSuchAPIKey(string key)
        {
            apiOptions.Key = key;
        }
        
        [Given(@"such resource ""(.*)""")]
        public void GivenSuchResource(string resource)
        {
            apiOptions.Resource = resource;
        }
        
        [When(@"I get articles by section ""(.*)""")]
        public async Task WhenIGetArticlesBySection(Section section)
        {
            await targetArticleService.GetArticlesAsync(section);
        }
        
        [When(@"I get articles by section ""(.*)"" and updated date ""(.*)""")]
        public async Task WhenIGetArticlesBySectionAndUpdatedDate(Section section, DateTime updatedDate)
        {
            await targetArticleService.GetArticlesAsync(section, updatedDate);
        }
        
        [When(@"I get article groups by section ""(.*)""")]
        public async Task WhenIGetArticleGroupsBySection(Section section)
        {
            await targetArticleService.GetGroupsAsync(section);
        }
        
        [Then(@"backend rest client will use ""(.*)"" like base API url")]
        public void ThenBackendRestClientWillUseLikeBaseAPIUrl(string baseUrl)
        {
            mockRestClient.VerifySet(
                f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == baseUrl),
                Times.Once
            );
        }
        
        [Then(@"""(.*)"" like resource")]
        public void ThenLikeResource(string resource)
        {
            mockRestClient.Verify(
                c => c.ExecuteTaskAsync(It.Is<RestRequest>(
                    r => r.Resource == resource)),
                Times.Once
            );
        }
        
        [Then(@"will has a parameter ""(.*)"" with value ""(.*)""")]
        public void ThenWillHasAParameterWithValue(string parameterName, string parameterValue)
        {
            mockRestClient.Verify(
                c => c.ExecuteTaskAsync(It.Is<RestRequest>(
                    r => r.Parameters.Any(p => p.Name == parameterName && p.Value.ToString() == parameterValue))),
                Times.Once
            );
        }
    }
}
