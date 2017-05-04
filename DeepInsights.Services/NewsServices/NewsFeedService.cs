using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;
using System.Xml;

namespace DeepInsights.Services
{
    [Export(typeof(INewsFeedService))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    public class NewsFeedService : INewsFeedService
    {
        public async Task<IEnumerable<SyndicationItem>> GetNewsFeed(string newsFeedURL)
        {
            return await Task.Factory.StartNew(() => 
            {
                using (XmlReader reader = XmlReader.Create(newsFeedURL))
                {
                    SyndicationFeed feed = SyndicationFeed.Load(reader);

                    return feed.Items;
                }
            });
        }
    }
}
