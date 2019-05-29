using Moq;
using NYTimes.Services.Abstractions.Enums;
using RestSharp;
using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace NYTimes.Services.Test
{
    public class ArticleServiceTest
    {
        public class GetArticlesBySectionAndCreatedDateMethod : ArticleServiceFactsBase
        {
            [Fact]
            public async Task UsesValidRequestUrl()
            {
                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                await target.GetArticlesAsync(Section.Arts, DateTime.Now);

                // Assert
                _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
                _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
                  r.Resource == "svc/topstories/v2/{section}.json" &&
                  r.Parameters.Any(p =>
                    p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
                    p.Name == "section" && p.Value.ToString() == "arts"
                  ))), Times.Once);
            }

            [Fact]
            public async Task CreatesDto()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          title: 'test-title',
                          url: 'test-url',
                          updated_date: '5/24/2019 6:24:16 PM'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticlesAsync(Section.Arts, DateTime.Parse("5/24/2019"));
                var firstArticle = result.First();

                // Assert
                Assert.Equal("test-title", firstArticle.Title);
                Assert.Equal("test-url", firstArticle.Url);
                Assert.Equal("5/24/2019 6:24:16 PM", firstArticle.UpdatedDateTime.ToString());
            }

            [Fact]
            public async Task FiltersArticlesByUpdatedDate()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          title: 'test-title',
                          url: 'test-url',
                          updated_date: '5/24/2019 6:24:16 PM'
                        },
                        {
                          title: 'another-title',
                          url: 'another-url',
                          updated_date: '6/24/2019 6:24:16 PM',
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticlesAsync(Section.Arts, DateTime.Parse("5/24/2019"));

                // Assert
                Assert.Equal("test-title", result.Single().Title);
            }
        }

        public class GetArticlesBySectionMethod : ArticleServiceFactsBase
        {
            [Fact]
            public async Task UsesValidRequestUrl()
            {
                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                await target.GetArticlesAsync(Section.Arts);

                // Assert
                _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
                _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
                  r.Resource == "svc/topstories/v2/{section}.json" &&
                  r.Parameters.Any(p =>
                    p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
                    p.Name == "section" && p.Value.ToString() == "arts"
                  ))), Times.Once);
            }

            [Fact]
            public async Task CreatesDtos()
            {
                // Arrange
                _mockRestResponse.SetupGet(r => r.Content)
                  .Returns(@"
                    {
                      results: [
                        {
                          title: 'test-title',
                          url: 'test-url',
                          updated_date: '5/24/2019 6:24:16 PM'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticlesAsync(Section.Arts);
                var firstArticle = result.First();

                // Assert
                Assert.Equal("test-title", firstArticle.Title);
                Assert.Equal("test-url", firstArticle.Url);
                Assert.Equal("5/24/2019 6:24:16 PM", firstArticle.UpdatedDateTime.ToString());
            }
        }

        public class GetArticleByShortUrlMethod : ArticleServiceFactsBase
        {
            [Fact]
            public async Task UsesValidRequestUrl()
            {
                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                await target.GetArticlesAsync(Section.Home);

                // Assert
                _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
                _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
                  r.Resource == "svc/topstories/v2/{section}.json" &&
                  r.Parameters.Any(p =>
                    p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
                    p.Name == "section" && p.Value.ToString() == "home"
                  ))), Times.Once);
            }

            [Fact]
            public async Task CreatesDto()
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
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticleAsync("2YNxSD2");

                // Assert
                Assert.Equal("test-title", result.Title);
                Assert.Equal("test-url", result.Url);
                Assert.Equal("5/24/2019 6:24:16 PM", result.UpdatedDateTime.ToString());
            }

            [Fact]
            public async Task TakesFirstFoundArticle()
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
                          title: 'another-title',
                          url: 'another-url',
                          updated_date: '6/24/2019 6:24:16 PM',
                          short_url: 'https://nyti.ms/2YNxSD2'
                        },
                        {
                          title: 'another-title',
                          url: 'another-url',
                          updated_date: '7/24/2019 6:24:16 PM',
                          short_url: 'https://nyti.ms/XXXXXXX'
                        }
                      ]
                    }
                  ");

                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                var result = await target.GetArticleAsync("2YNxSD2");

                // Assert
                Assert.Equal("test-title", result.Title);
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
            public async Task UsesValidRequestUrl()
            {
                // Act
                var target = new ArticleService(_mockRestClientFactory.Object, _mockApiConfig.Object);
                await target.GetGroupsAsync(Section.Arts);

                // Assert
                _mockRestClient.VerifySet(f => f.BaseUrl = It.Is<Uri>(u => u.ToString() == "https://api.sometesturl.com/"), Times.Once);
                _mockRestClient.Verify(c => c.ExecuteTaskAsync(It.Is<RestRequest>(r =>
                  r.Resource == "svc/topstories/v2/{section}.json" &&
                  r.Parameters.Any(p =>
                    p.Name == "api-key" && p.Value.ToString() == "test-unique-identifier" ||
                    p.Name == "section" && p.Value.ToString() == "arts"
                  ))), Times.Once);
            }

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
    }
}

