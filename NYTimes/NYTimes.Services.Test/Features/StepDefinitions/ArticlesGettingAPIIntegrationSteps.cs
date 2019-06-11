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
    public class ArticlesGettingAPIIntegrationSteps : StepDefinitionsBase
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
        
        [When(@"I get articles by some section")]
        public async Task WhenIGetArticlesBySomeSection()
        {
            await targetArticleService.GetArticlesAsync(Section.Arts);
        }
        
        [When(@"I get articles by some section and some updated date")]
        public async Task WhenIGetArticlesBySomeSectionAndSomeUpdatedDate()
        {
            await targetArticleService.GetArticlesAsync(Section.Arts, DateTime.Now);
        }
        
        [When(@"I get article groups by some section")]
        public async Task WhenIGetArticleGroupsBySomeSection()
        {
            await targetArticleService.GetGroupsAsync(Section.Arts);
        }

        [When(@"I get article by some key")]
        public async Task WhenIGetArticleBySomeKey()
        {
            await targetArticleService.GetArticleAsync("2YNxSD2");
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
