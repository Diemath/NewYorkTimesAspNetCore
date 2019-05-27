using Services.Abstractions.Dto;
using Services.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Abstractions
{
  public interface IArticleService
  {
    /// <summary>
    /// Filters by short url.
    /// </summary>
    /// <param name="shortUrl"></param>
    /// <returns></returns>
    Task<ArticleDto> GetArticleAsync(string shortUrl);
    /// <summary>
    /// Filters by article section.
    /// </summary>
    /// <param name="section"></param>
    /// <returns>Returns first found article and returns first found article.</returns>
    Task<ArticleDto> GetArticleAsync(ArticleSection section);
    /// <summary>
    /// Filters by article section.
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    Task<IEnumerable<ArticleDto>> FilterArticlesAsync(ArticleSection section);
    /// <summary>
    /// Filters by article section and updated date.
    /// </summary>
    /// <param name="section"></param>
    /// <param name="updatedDateTime"></param>
    /// <returns></returns>
    Task<IEnumerable<ArticleDto>> FilterArticlesAsync(ArticleSection section, DateTime updatedDateTime);
    /// <summary>
    /// Groups articles by updated dates.
    /// </summary>
    /// <param name="section"></param>
    /// <returns></returns>
    Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(ArticleSection section);
  }
}
