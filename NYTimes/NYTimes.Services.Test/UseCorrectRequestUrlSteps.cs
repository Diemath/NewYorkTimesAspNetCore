using Microsoft.Extensions.Options;
using Moq;
using NYTimes.Services.Abstractions.Enums;
using NYTimes.Services.Configurations;
using System;
using TechTalk.SpecFlow;

namespace NYTimes.Services.Test
{
    [Binding]
    public class UseCorrectRequestUrlSteps
    {
        private Mock<IOptions<ApiOptions>> _mockApiConfig = new Mock<IOptions<ApiOptions>>();

        [Given(@"injected configuration has such base API url - ""(.*)"", such API identifier - ""(.*)"" and such resource ""(.*)""")]
        public void GivenInjectedConfigurationHasSuchBaseAPIUrl_SuchAPIIdentifier_AndSuchResource(string baseUrl, string id, string resource)
        {
            _mockApiConfig.SetupGet(c => c.Value).Returns(new ApiOptions
            {
                BaseUrl = baseUrl,
                Id = id,
                Resource = resource
            }
            );
        }
        
        [When(@"I get articles by section ""(.*)""")]
        public void WhenIGetArticlesBySection(Section section)
        {
            _section = section;
        }
        
        [Then(@"the rest client will use ""(.*)"" like base API url, ""(.*)"" like resource and will have a parameter ""(.*)"" with value ""(.*)""")]
        public void ThenTheRestClientWillUseLikeBaseAPIUrlLikeResourceAndWillHaveAParameterWithValue(string p0, string p1, string p2, string p3)
        {
            ScenarioContext.Current.Pending();
        }
    }
}
