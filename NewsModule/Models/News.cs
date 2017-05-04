using System;

namespace DeepInsights.Components.MarketNews.Models
{
    public class News
    {
        public News(DateTime publishDate, string title, string summary, string content)
        {
            PublishDate = publishDate;
            Title = title;
            Summary = summary;
            Content = content;
        }

        public DateTime PublishDate
        {
            get;
            private set;
        }

        public string Title
        {
            get;
            private set;
        }

        public string Summary
        {
            get;
            private set;
        }

        public string Content
        {
            get;
            private set;
        }
    }
}
