using Moq;
using NYTimes.Services.Abstractions.Dto;
using RestSharp;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace NYTimes.Services.Test
{
    [Binding]
    public class ArticleGettingByShortUrlSteps : StepDefinitionsBase
    {
        private string articles =
        @"
        {
            results: [
                {
                    short_url: '{shortUrl1}'
                },
                {
                    short_url: '{shortUrl2}'
                }
            ]
        }
        ";
        private ArticleDto result;

        [Given(@"New York Times API returns two articles\. First one has short url ""(.*)""")]
        public void GivenNewYorkTimesAPIReturnsTwoArticles_FirstOneHasShortUrl(string shortUrl)
        {
            articles = articles.Replace("{shortUrl1}", shortUrl);
        }
        
        [Given(@"second one has short url ""(.*)""")]
        public void GivenSecondOneHasShortUrl(string shortUrl)
        {
            articles = articles.Replace("{shortUrl2}", shortUrl);
            SetupSuccessfulResult(articles);
        }
        
        [When(@"I get article by key ""(.*)""")]
        public async Task WhenIGetArticleByKey(string key)
        {
            result = await targetArticleService.GetArticleAsync(key);
        }

        [Then(@"rest client will use ""(.*)"" like resource")]
        public void ThenRestClientWillUseLikeResource(string resource)
        {
            mockRestClient.Verify(
                c => c.ExecuteTaskAsync(It.Is<RestRequest>(
                    r => r.Resource == resource)),
                Times.Once
            );
        }

        [Then(@"the result will be an article with short url ""(.*)""")]
        public void ThenTheResultWillBeAnArticleWithShortUrl(string shortUrl)
        {
            Assert.Equal(shortUrl, result.ShortUrl);
        }
    }
}
