using DeepInsights.Components.Account.Models;
using DeepInsights.Services.ForexServices;
using DeepInsights.Shell.Infrastructure.Utilities;
using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace DeepInsights.Components.Account.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class AccountMainViewModel : BindableBase
    {
        #region Private Fields

        private ModuleStatus _ModuleStatus = new ModuleStatus();
        private readonly IForexAccountService _ForexAccountService;

        #endregion

        #region Constructor

        [ImportingConstructor]
        public AccountMainViewModel(IForexAccountService forexAccountService)
        {
            forexAccountService.ThrowIfNull("forexAccountService");
            _ForexAccountService = forexAccountService;

            InitializeCommands();
            InitializeProperties();
        }

        #endregion

        #region Properties

        public ModuleStatus ModuleStatus
        {
            get { return _ModuleStatus; }
            set { SetProperty(ref _ModuleStatus, value); }
        }

        public RangeObservableCollection<KeyValuePair<string, string>> AccountKeyValuePairs
        {
            get;
            set;
        }

        public string ModuleHeader
        {
            get { return "Account Info"; }
        }

        #endregion

        #region Commands

        public ICommand ViewLoadedCommand
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
            try
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
                ModuleStatus.IsLoaded = true;
            }
            catch (Exception)
            {
                ModuleStatus.HasErrors = true;
            }
        }

        #endregion
    }
}