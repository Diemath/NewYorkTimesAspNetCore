using Moq;
using NYTimes.Services.Abstractions.Enums;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NYTimes.Services.Test
{
    public class ArticleServiceTest : ArticleServiceFactsBase
    {
        public class GetArticleByShortUrlMethod : ArticleServiceFactsBase
        {

            [Fact]
            public async Task ThrowsExceptionIfArticleNotSingle()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          title: 'test-title',
                          url: 'test-url',
                          updated_date: '5/24/2019 6:24:16 PM',
                          short_url: 'https://nyti.ms/2YNxSD2'
                        },
                        {
                          title: 'test-title',
                          url: 'test-url',
                          updated_date: '5/24/2019 6:24:16 PM',
                          short_url: 'https://nyti.ms/2YNxSD2'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                await Assert.ThrowsAsync<InvalidOperationException>(() => target.GetArticleAsync("2YNxSD2"));
            }

            [Fact]
            public async Task ReturnsNullForNotFoundArticle()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          title: 'another-title',
                          url: 'another-url',
                          updated_date: '6/24/2019 6:24:16 PM',
                          short_url: 'https://nyti.ms/2YNxSD2'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticleAsync("XXXXXXX");

                // Assert
                Assert.Null(result);
            }
        }

        public class GetArticleGroupsBySectionMethod : ArticleServiceFactsBase
        {
            [Fact]
            public async Task CreatesDtos()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          updated_date: '5/24/2019 6:24:16 PM'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetGroupsAsync(Section.Arts);

                // Assert
                var singleGroup = result.Single();
                Assert.Equal(1, singleGroup.Total);
                Assert.Equal(DateTime.Parse("5/24/2019"), singleGroup.UpdatedDate);
            }

            [Fact]
            public async Task GroupsByDate()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          updated_date: '5/24/2019 6:24:16 PM'
                        },
                        {
                          updated_date: '5/24/2019 7:24:16 PM'
                        },
                        {
                          updated_date: '6/24/2019 6:24:16 PM'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetGroupsAsync(Section.Arts);

                // Assert
                Assert.Equal(2, result.Count());

                var group = result.ElementAt(0);
                Assert.Equal(2, group.Total);
                Assert.Equal(DateTime.Parse("5/24/2019"), group.UpdatedDate);

                group = result.ElementAt(1);
                Assert.Equal(1, group.Total);
                Assert.Equal(DateTime.Parse("6/24/2019"), group.UpdatedDate);
            }
        }

        [Fact]
        public async Task ThrowsExceptionIfResponseNotSuccessful()
        {
            // Arrange
            _mockRestResponse.SetupGet(r => r.IsSuccessful).Returns(false);
            _mockRestResponse.SetupGet(r => r.ErrorMessage).Returns("test-error-message");
            _mockRestResponse.SetupGet(r => r.Content)
              .Returns(@"
                    {
                      results: []
                    }
                  ");

            // Act
            var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);

            async Task assert(Func<Task> act)
            {
                var exception = await Assert.ThrowsAsync<Exception>(act);
                Assert.Equal("test-error-message", exception.Message);
            }

            await assert(() => target.GetArticleAsync("2YNxSD2"));
            await assert(() => target.GetArticlesAsync(Section.Arts));
            await assert(() => target.GetArticlesAsync(Section.Arts, DateTime.Now));
            await assert(() => target.GetGroupsAsync(Section.Arts));
        }
    }
}

