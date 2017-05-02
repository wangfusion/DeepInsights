using System.ComponentModel.DataAnnotations;
using DeepInsights.Shell.Infrastructure;
using System.Collections.Generic;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Text;

namespace DeepInsights.Components.MarketNews.ViewModels
{
    public class MarketNewsMainViewModel : ValidatableBindableBase
    {
        #region Private Fields

        private string _Mobile;
        private List<string> _Errors;

        #endregion

        #region Constructor

        public MarketNewsMainViewModel() { ReadNewsFeeds(); }

        #endregion

        #region Properties

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

        #region Private Methods

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

        private void ReadNewsFeeds()
        {
            XmlReader reader = XmlReader.Create(NewsConstants.REUTER_MONEY);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            var newsBuilder = new StringBuilder();

            foreach (SyndicationItem item in feed.Items)
            {
                newsBuilder.Append(item.PublishDate).Append(" - ")
                           .Append(item.Title.Text).AppendLine();
            }

            string news = newsBuilder.ToString();
        }

        #endregion
    }
}
