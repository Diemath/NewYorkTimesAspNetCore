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
        /// /// <exception cref="UnvalidShortUrlException">Throws if short url parameter is not in format XXXXXXX.</exception>
        /// <returns></returns>
        Task<ArticleDto> GetArticleAsync(string shortUrl);
        /// <summary>
        /// Filters by article section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns>Returns first found article and returns first found article.</returns>
        Task<ArticleDto> GetArticleAsync(Section section);
        /// <summary>
        /// Filters by article section.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section);
        /// <summary>
        /// Filters by article section and updated date.
        /// </summary>
        /// <param name="section"></param>
        /// <param name="updatedDateTime"></param>
        /// <exception cref="UndefinedEnumException">Throws if section parameter is undefined.</exception>
        /// <returns></returns>
        Task<IEnumerable<ArticleDto>> FilterArticlesAsync(Section section, DateTime updatedDateTime);
        /// <summary>
        /// Groups articles by updated dates.
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        Task<IEnumerable<ArticleGroupByDateDto>> GetGroupsAsync(Section section);
    }
}
