using Microsoft.Practices.Prism.Mvvm;

namespace DeepInsights.Shell.Infrastructure.Utilities
{
    public class ModuleStatus : BindableBase
    {
        #region Private Fields

        private bool _IsLoaded;
        private bool _HasErrors;
        private string _ErrorMessage;

        #endregion

        public bool IsLoaded
        {
            get { return _IsLoaded; }
            set
            {
                if (_IsLoaded != value)
                {
                    SetProperty(ref _IsLoaded, value);
                    OnPropertyChanged(() => IsLoaded);
                }
            }
        }

        public bool HasErrors
        {
            get { return _HasErrors; }
            set
            {
                if (_HasErrors != value)
                {
                    SetProperty(ref _HasErrors, value);
                    OnPropertyChanged(() => HasErrors);
                }
            }
        }

        public string ErrorMessage
        {
            get { return _ErrorMessage; }
            set
            {
                if (_ErrorMessage != value)
                {
                    SetProperty(ref _ErrorMessage, value);
                    OnPropertyChanged(() => ErrorMessage);
                }
            }
        }
    }
}
