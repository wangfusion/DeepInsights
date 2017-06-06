using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using System.Collections.Generic;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public class QuoteSelectionNotification : Confirmation
    {
        #region Constructor

        public QuoteSelectionNotification()
        {
            Quotes = new List<string>();
            SelectedQuote = null;
        }

        public QuoteSelectionNotification(IEnumerable<string> quotes)
            : this()
        {
            foreach (string quote in quotes)
            {
                Quotes.Add(quote);
            }
        }

        #endregion

        #region Properties

        public IList<string> Quotes
        {
            get;
            private set;
        }

        public string SelectedQuote
        {
            get;
            set;
        }

        #endregion
    }
}
