using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace NYTimes.Services.Test
{
    [Binding]
    public class ArticlesFiltrationSteps : TestBase
    {
        private string articles = 
        @"
        {
            results: [
                {
                    updated_date: '{updatedDate1}'
                },
                {
                    updated_date: '{updatedDate2}'
                }
            ]
        }
        ";
        private IEnumerable<ArticleDto> result;

        [Given(@"New York Times API returns two articles\. First one has updated date ""(.*)""")]
        public void GivenNewYorkTimesAPIReturnsTwoArticles_FirstOneHasUpdatedDate(string updatedDate)
        {
            articles = articles.Replace("{updatedDate1}", updatedDate);
        }
        
        [Given(@"second one has updated date ""(.*)""")]
        public void GivenSecondOneHasUpdatedDate(string updatedDate)
        {
            articles = articles.Replace("{updatedDate2}", updatedDate);
            SetupSuccessfulResult(articles);
        }

        [When(@"I filter articles by updated date ""(.*)""")]
        public async Task WhenIFilterArticlesByUpdatedDate(DateTime updatedDate)
        {
            result = await targetArticleService.GetArticlesAsync(Section.Arts, updatedDate);
        }


        [Then(@"the result will be ""(.*)"" article")]
        public void ThenTheResultWillBeArticle(int count)
        {
            Assert.Equal(count, result.Count());
        }
        
        [Then(@"its updated date will be ""(.*)""")]
        public void ThenItsUpdatedDateWillBe(DateTime updatedDate)
        {
            Assert.Equal(updatedDate, result.First().UpdatedDateTime);
        }
    }
}
