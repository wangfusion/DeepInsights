using System.Collections.Generic;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace DeepInsights.Services
{
    public interface INewsFeedService
    {
        Task<IEnumerable<SyndicationItem>> GetNewsFeed(string newsFeedURL);
    }
}
