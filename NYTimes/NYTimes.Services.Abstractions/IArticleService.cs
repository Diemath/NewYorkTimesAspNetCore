using NYTimes.Services.Abstractions.Dto;
using NYTimes.Services.Abstractions.Enums;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace NYTimes.Services.Abstractions
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
        /// <returns></returns>
        Task<IEnumerable<ArticleDto>> GetArticlesAsync(Section section);
        /// <summary>
        /// Filters by article section and updated date.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="updatedDateTime"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleDto>> GetArticlesAsync(Section section, DateTime updatedDateTime);
        /// <summary>
        /// Groups articles by updated dates. Filters by article section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(Section section);
    }
}
