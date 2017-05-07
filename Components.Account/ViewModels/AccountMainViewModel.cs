using DeepInsights.Components.Account.Models;
using DeepInsights.Services.ForexServices;
using DeepInsights.Shell.Infrastructure.Utility;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DeepInsights.Components.Account.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountMainViewModel : BindableBase
    {
        #region Private Fields

        private readonly IForexAccountService _ForexAccountService;
        private bool _HasAccountInfoLoaded;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AccountMainViewModel(IForexAccountService forexAccountService)
        {
            if (forexAccountService == null)
            {
                throw new ArgumentNullException("forexAccountService");
            }
            _ForexAccountService = forexAccountService;

            InitializeCommands();
            InitializeProperties();
        }

        #endregion

        #region Properties

        public RangeObservableCollection<KeyValuePair<string, string>> AccountKeyValuePairs
        {
            get;
            set;
        }

        public bool HasAccountInfoLoaded
        {
            get { return _HasAccountInfoLoaded; }
            set
            {
                if (_HasAccountInfoLoaded != value)
                {
                    SetProperty(ref _HasAccountInfoLoaded, value);
                    OnPropertyChanged(() => HasAccountInfoLoaded);
                }
            }
        }

        public string ModuleHeader
        {
            get { return "Account Info"; }
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

        private void InitializeProperties()
        {
            AccountKeyValuePairs = new RangeObservableCollection<KeyValuePair<string, string>>();
        }

        private async void ViewLoaded()
        {
            await GetAccountData();
        }

        private async Task GetAccountData()
        {
            var accountProperties = new List<KeyValuePair<string, string>>();
            string accountJson = await _ForexAccountService.GetAccountData();
            AccountInfo accountInfo = JsonConvert.DeserializeObject<Response>(accountJson).account;
            PropertyInfo[] properties = accountInfo.GetType().GetProperties();
            foreach (var p in properties)
            {
                string key = Regex.Replace(p.Name, "([a-z])([A-Z])", "$1 $2");
                string val = p.GetValue(accountInfo).ToString();
                accountProperties.Add(new KeyValuePair<string, string>(key, val));
            }

            AccountKeyValuePairs.ClearAndAddRange(accountProperties);
            HasAccountInfoLoaded = true;
        }

        #endregion
    }
}