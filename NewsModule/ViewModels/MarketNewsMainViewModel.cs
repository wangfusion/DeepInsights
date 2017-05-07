using DeepInsights.Components.MarketNews.Models;
using DeepInsights.Services;
using DeepInsights.Shell.Infrastructure;
using DeepInsights.Shell.Infrastructure.Utility;
using Microsoft.Practices.Prism.Commands;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.DataAnnotations;
using System.ServiceModel.Syndication;
using System.Threading.Tasks;

namespace DeepInsights.Components.MarketNews.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class MarketNewsMainViewModel : ValidatableBindableBase
    {
        #region Private Fields

        private readonly INewsFeedService _NewsFeedService;
        private List<string> _Errors;
        private bool _HasNewsLoaded;
        private string _Mobile;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public MarketNewsMainViewModel(INewsFeedService newsFeedService)
        {
            if (newsFeedService == null)
            {
                throw new ArgumentNullException("newsFeedService");
            }
            _NewsFeedService = newsFeedService;
            DailyNews = new RangeObservableCollection<News>();

            InitializeCommands();
        }

        #endregion

        #region Properties

        public string ModuleHeader
        {
            get { return "Market News"; }
        }

        public RangeObservableCollection<News> DailyNews
        {
            get;
            set;
        }

        public bool HasNewsLoaded
        {
            get { return _HasNewsLoaded; }
            set
            {
                if (_HasNewsLoaded != value)
                {
                    SetProperty(ref _HasNewsLoaded, value);
                    OnPropertyChanged(() => HasNewsLoaded);
                }
            }
        }

        [MaxLength(10, ErrorMessage = "Max Length is 10 chars."), Required]
        public string Mobile
        {
            get { return _Mobile; }
            set
            {
                if (_Mobile != value)
                {
                    SetProperty(ref _Mobile, value);
                    OnPropertyChanged(() => Mobile);
                    Errors = FlattenErrors();
                }
            }
        }

        public List<string> Errors
        {
            get { return _Errors; }
            set { SetProperty(ref _Errors, value); }
        }

        #endregion

        #region Commands

        public DelegateCommand ViewLoadedCommand
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            ViewLoadedCommand = new DelegateCommand(ViewLoaded);
        }

        private async void ViewLoaded()
        {
            await GetNewsFeeds();
        }

        private List<string> FlattenErrors()
        {
            var errors = new List<string>();
            Dictionary<string, List<string>> allErrors = ErrorsContainer.GetAllErrors();
            foreach (string propertyName in allErrors.Keys)
            {
                foreach (var errorString in allErrors[propertyName])
                {
                    errors.Add(propertyName + ": " + errorString);
                }
            }

            return errors;
        }

        private async Task GetNewsFeeds()
        {
            var newsItems = new List<News>();
            IEnumerable<SyndicationItem> items = await _NewsFeedService.GetNewsFeed(NewsConstants.REUTER_MONEY);

            foreach (SyndicationItem item in items)
            {
                string title = item.Title == null ? string.Empty : item.Title.Text;
                string summary = item.Summary == null ? string.Empty : item.Summary.Text;
                string content = item.Content == null ? string.Empty : item.Content.ToString();
                newsItems.Add(new News(item.PublishDate.UtcDateTime.ToLocalTime(), title, summary, content));
            }

            DailyNews.ClearAndAddRange(newsItems);
            HasNewsLoaded = true;
        }
        #endregion
    }
}
