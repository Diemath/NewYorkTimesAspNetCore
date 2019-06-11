using System;

namespace NYTimes.Services.Abstractions.Dto
{
    public class ArticleDto
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string ShortUrl { get; set; }
        public DateTime UpdatedDateTime { get; set; }
    }
}
