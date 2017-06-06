using Microsoft.Practices.Prism.Commands;
using Microsoft.Practices.Prism.Mvvm;
using System;
using System.ComponentModel.Composition;
using System.Windows.Threading;

namespace DeepInsights.Components.TopDashboard.ViewModels
{
    [Export]
    [PartCreationPolicy(CreationPolicy.NonShared)]
    public class TopDashboardMainViewModel : BindableBase
    {
        #region Private Fields

        private string _LondonTime;
        private TimeZoneInfo _LondonZone;

        private string _NewYorkTime;
        private TimeZoneInfo _NewYorkZone;

        private string _TokyoTime;
        private TimeZoneInfo _TokyoZone;

        private DispatcherTimer timer = new DispatcherTimer(); 

        #endregion

        #region Constructor

        public TopDashboardMainViewModel()
        {
            InitializeCommands();
        }

        #endregion

        #region Properties

        public string LondonTime
        {
            get { return _LondonTime; }
            set { SetProperty(ref _LondonTime, value); }
        }

        public string NewYorkTime
        {
            get { return _NewYorkTime; }
            set { SetProperty(ref _NewYorkTime, value); }
        }

        public string TokyoTime
        {
            get { return _TokyoTime; }
            set { SetProperty(ref _TokyoTime, value); }
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

        private void ViewLoaded()
        {
            _LondonZone = TimeZoneInfo.FindSystemTimeZoneById("GMT Standard Time");
            _NewYorkZone = TimeZoneInfo.FindSystemTimeZoneById("Eastern Standard Time");
            _TokyoZone = TimeZoneInfo.FindSystemTimeZoneById("Tokyo Standard Time");

            UpdateTime();
            timer.Tick += new EventHandler(OnTimedEvent);
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Start();
        }

        private void OnTimedEvent(object source, EventArgs e)
        {
            UpdateTime();
        }

        private void UpdateTime()
        {
            LondonTime = string.Format("{0:H:mm:ss}", TimeZoneInfo.ConvertTime(DateTime.Now, _LondonZone));
            NewYorkTime = string.Format("{0:H:mm:ss}", TimeZoneInfo.ConvertTime(DateTime.Now, _NewYorkZone));
            TokyoTime = string.Format("{0:H:mm:ss}", TimeZoneInfo.ConvertTime(DateTime.Now, _TokyoZone));
        }

        #endregion
    }
}
