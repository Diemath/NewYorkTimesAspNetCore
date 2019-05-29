using System;

namespace NYTimes.NancyApi.Models
{
    public class ArticleView
    {
        public string Heading { get; set; }
        public DateTime Updated { get; set; }
        public string Link { get; set; }
    }
}
