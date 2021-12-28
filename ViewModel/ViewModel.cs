using Microsoft.Win32;
using SimpleTimer.Models;
using SimpleTimer.Models.HelperClasses;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace SimpleTimer.ViewModels
{
    public class ViewModel : IViewModel
    {
        public TimerViewModel TimerViewModel { get; set; }

        // Set the timer offset to 50 seconds
        private static readonly TimeSpan StartFromSecConfProp = TimeSpan.FromSeconds(50);
        private TimeSpan SecondsAlreadyPassed = TimeSpan.Zero;

        private static Action _timerAction;
        private static TimeSpan SecondsBetweenRun;

        public ICommand PauseTimerCommand => new RelayCommand(param => this.PauseTimer());

        public ICommand UpdateTimeLimitCommand => new RelayCommand(param => this.UpdateTimeLimit());

        public ICommand EnterKeyCommand => new RelayCommand(param => this.EnterKeyMethod());

        private TimeSpan BackgroundWorkTimerInterval { get; set; }
        private TimeSpan LabelTimerInterval { get; set; }

        private IGeneralDataProvider GeneralDataProvider { get; }

        private void UpdateTimeLimit() => this.GeneralDataProvider.SetTimeLimit(this.HoursLimitProp);

        public ViewModel(IGeneralDataProvider generalDataProvider)
        {
            this.GeneralDataProvider = generalDataProvider;

            //this.TimerViewModel = new TimerViewModel();

            // Initialize the current time elapsed field by adding the offset
            this.SecondsAlreadyPassed = this.SecondsAlreadyPassed.Add(ViewModel.StartFromSecConfProp);

            // We specify this method to be executed every 1 srcond // BACKGROUND WORK
            this.BackgroundWorkTimerInterval = TimeSpan.FromSeconds(1);
            var process = new ExecutableProcess(this.BackgroundWorkTimerInterval, ViewModel.MyProcessToExecute);
            process.Start();

            // We specify this method to be executed every 1 srcond // DISPLAYED IN LABEL
            this.LabelTimerInterval = TimeSpan.FromSeconds(1);
            this.newProcess = new ExecutableProcess(this.LabelTimerInterval, LabelTimer);
            this.newProcess.Start();

            this.Initialize();
        }

        // ExecutableProcess executableProcess = new ExecutableProcess();
        public ExecutableProcess newProcess = new ExecutableProcess(ViewModel.SecondsBetweenRun, ViewModel._timerAction);

        public void PauseTimer()
        {
            this.newProcess.TogglePause();
        }

        private void Initialize()
        {
            this.HoursLimitProp = this.GeneralDataProvider.GetTimeLimit();
        }

        private void EnterKeyMethod()
        {
            this.UpdateTimeLimit();
            Debug.WriteLine("Enter key pressed");
        }

        private void LabelTimer()
        {
            this.SecondsAlreadyPassed = this.SecondsAlreadyPassed.Add(this.LabelTimerInterval);
            this.CurrentTime = this.SecondsAlreadyPassed.ToString(@"hh\:mm\:ss"); // DateTime.Now.ToLongTimeString()

            // HINT::Wrong namespace. After refactoring (moving types to new projects/folders) you forgot to adjust the namespaces, 
            // so the type DateTimeConverter could not be resolved.
            // Old namespace: SimpleTimer.HelperClass. Fixed namespace:SimpleTimer.Model.HelperClass. I lived up to the occasion 
            // to rename the namespace/folder from ..Calsses to ..Classes (fixed typo)
            DateTime RingTime = HelperClass.DateTimeConverter(this.HoursLimitProp);

            //Debug.WriteLine("Current time: " + CurrentTime);
            //Debug.WriteLine("Ring time: " + RingTime.ToLongTimeString());

            if (this.CurrentTime == RingTime.ToLongTimeString())
            {
                // HINT::Not implemented.
                // HINT::Created method. Implementation pending.
                HelperClass.PlaySound();
            }
        }

        private static void MyProcessToExecute()
        {
            //Debug.Write($"Running {DateTime.Now}" + "\n");
        }

        /// <summary>
        /// Timer implementation, working time today
        /// <summary>
        private string _currentTime;

        public string CurrentTime
        {
            get
            {
                return this._currentTime;
            }
            set
            {
                // HINT::Use always curly braces to prevent difficult to find bugs!! I wonder StyleCop is not complaining...
                if (this._currentTime == value)
                    return;
                this._currentTime = value;
                this.OnPropertyChanged();
            }
        }

        private string _hoursLimit;

        public string HoursLimitProp
        {
            get { return this._hoursLimit; }
            set
            {
                if (this._hoursLimit != value)
                {
                    DateTime dt;
                    bool res = DateTime.TryParseExact(value, "M/d/yyyy h:mm:ss tt",
                        CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

                    this._hoursLimit = dt.ToString("HH:mm");
                    this.OnPropertyChanged();
                    this.UpdateTimeLimit();
                    //TimePickedString();
                }
            }
        }

        //private DateTime? _time;
        //public DateTime? Time
        //{
        //    get => this._time;
        //    set
        //    {
        //        if (this._time == value)
        //        {
        //            return;
        //        }
        //        this._time = value;
        //        OnPropertyChanged(); // Note that this line is missing in your code
        //        TimePicked();
        //    }
        //}

        //public void TimePicked()
        //{
        //    if (_time.HasValue)
        //    {
        //        Debug.WriteLine(_time.Value.ToString("HH:mm"));
        //    }
        //}

        //public void TimePickedString()
        //{
        //    if (!string.IsNullOrEmpty(_hoursLimit))
        //    {
        //        DateTime dt;
        //        bool res = DateTime.TryParseExact(_hoursLimit, "M/d/yyyy h:mm:ss tt", 
        //            CultureInfo.InvariantCulture, DateTimeStyles.None, out dt);

        //        Debug.WriteLine(dt.ToString("HH:mm"));
        //    }
        //}

        #region Implementation of INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion Implementation of INotifyPropertyChanged
    }
}
