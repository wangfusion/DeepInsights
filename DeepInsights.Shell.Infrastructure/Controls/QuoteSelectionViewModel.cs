using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Interactivity.InteractionRequest;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.Windows.Input;

namespace DeepInsights.Shell.Infrastructure.Controls
{
    public class QuoteSelectionViewModel : BindableBase, IInteractionRequestAware
    {
        #region Private Fields

        private QuoteSelectionNotification _Notification;

        #endregion

        #region Constructor

        public QuoteSelectionViewModel()
        {
            InitializeCommands();
        }

        #endregion

        #region Properties

        public Action FinishInteraction
        {
            get;
            set;
        }

        public INotification Notification
        {
            get
            {
                return _Notification;
            }
            set
            {
                if (value is QuoteSelectionNotification)
                {
                    _Notification = value as QuoteSelectionNotification;
                    OnPropertyChanged(() => Notification);
                }
            }
        }

        public string SelectedQuote
        {
            get;
            set;
        }

        #endregion

        #region Commands

        public ICommand SelectQuoteCommand
        {
            get;
            private set;
        }

        public ICommand CancelCommand
        {
            get;
            private set;
        }

        #endregion

        #region Private Methods

        private void InitializeCommands()
        {
            SelectQuoteCommand = new DelegateCommand(AcceptSelectedQuote);
            CancelCommand = new DelegateCommand(CancelInteraction);
        }

        private void AcceptSelectedQuote()
        {
            if (_Notification != null)
            {
                _Notification.SelectedQuote = SelectedQuote;
                _Notification.Confirmed = true;
            }

            FinishInteraction();
        }

        private void CancelInteraction()
        {
            if (_Notification != null)
            {
                _Notification.SelectedQuote = null;
                _Notification.Confirmed = false;
            }

            FinishInteraction();
        }

        #endregion
    }
}
