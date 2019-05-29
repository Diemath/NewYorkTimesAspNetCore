using Moq;
using NYTimes.Services.Abstractions;
using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;

namespace NYTimes.NancyApi.Test
{
  public class ArticlesModuleTestBase
  {
    protected Mock<IArticleService> _mockArticleService = new Mock<IArticleService>(MockBehavior.Loose);

    public ArticlesModuleTestBase()
    {
      // Setup default article service behavior
      _mockArticleService.Setup(g => g.GetArticleAsync(It.IsAny<string>()))
        .ReturnsAsync(new ArticleDto { });
      _mockArticleService.Setup(g => g.GetArticleAsync(It.IsAny<Section>()))
        .ReturnsAsync(new ArticleDto { });
    }
  }
}
