using SimpleTimer.Models;
using SimpleTimer.Models.HelperClasses;
using System;
using System.Diagnostics;
using System.Windows.Input;

namespace SimpleTimer.ViewModels
{
  public class ViewModel : BaseViewModel, IViewModel
  {
    // Set the timer offset to X seconds
    private static readonly TimeSpan StartFromSecConfProp = TimeSpan.FromSeconds(0);
    private TimeSpan SecondsAlreadyPassed = TimeSpan.Zero;

    private static Action TimerAction;
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

      // Initialize the current time elapsed field by adding the offset
      this.SecondsAlreadyPassed = this.SecondsAlreadyPassed.Add(ViewModel.StartFromSecConfProp);

      // We specify this method to be executed every 1 srcond // BACKGROUND WORK
      this.BackgroundWorkTimerInterval = TimeSpan.FromSeconds(1);
      ExecutableProcess process = new ExecutableProcess(this.BackgroundWorkTimerInterval, this.LogOutTimer);
      process.Start();

      // We specify this method to be executed every 1 srcond // DISPLAYED IN LABEL
      this.LabelTimerInterval = TimeSpan.FromSeconds(1);
      this.newProcess = new ExecutableProcess(this.LabelTimerInterval, this.LabelTimer);
      this.newProcess.Start();

      this.Initialize();
    }

    public ExecutableProcess newProcess = new ExecutableProcess(ViewModel.SecondsBetweenRun, ViewModel.TimerAction);

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
      this.CurrentTime = this.SecondsAlreadyPassed;

      DateTime ringTime = HelperClass.DateTimeConverter(this.HoursLimitProp);

      if (this.CurrentTime.TotalSeconds == ringTime.TimeOfDay.TotalSeconds)
      {
        HelperClass.PlaySound();
      }
    }

    private void LogOutTimer()
    {
      //Debug.Write($"Running {DateTime.Now}" + "\n");
    }

    /// <summary>
    /// Timer implementation, working time today
    /// <summary>
    private TimeSpan currentTime;

    public TimeSpan CurrentTime
    {
      get
      {
        return this.currentTime;
      }
      set
      {
        if (this.currentTime == value)
        {
          return;
        }
        this.currentTime = value;
        this.OnPropertyChanged();
      }
    }

    private string hoursLimit;

    public string HoursLimitProp
    {
      get { return this.hoursLimit; }
      set
      {
        if (this.hoursLimit != value)
        {
          this.hoursLimit = value;
          this.OnPropertyChanged();
          this.UpdateTimeLimit();
        }
      }
    }
  }
}
