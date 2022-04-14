using log4net;
using log4net.Config;
using log4net.Repository;
using Microsoft.Win32;
using SimpleTimer.Models;
using SimpleTimer.ViewModels;
using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using System.Xml;

namespace SimpleTimer.Main
{
  /// <summary>
  /// Interaction logic for App.xaml
  /// </summary>
  public partial class App : Application
  {
    public App()
    {
    }

    public ViewModel ViewModel { get; set; }

    private void Run(object sender, StartupEventArgs e)
    {
      SystemEvents.SessionSwitch += SystemEvents_SessionSwitch;

      InitializeLogger();

      this.ViewModel = new ViewModel(new GeneralDataProvider());

      var mainWindow = new MainWindow()
      {
        DataContext = ViewModel
      };

      mainWindow.Show();
    }

    public void SystemEvents_SessionSwitch(object sender, SessionSwitchEventArgs e)
    {
      if (e.Reason == SessionSwitchReason.SessionLock)
      {
        //Debug.Print("I am locked: " + DateTime.Now);
        this.Logger.Debug("I am locked: " + DateTime.Now);
        ViewModel.newProcess.TogglePause();
      }
      else if (e.Reason == SessionSwitchReason.SessionUnlock)
      {
        //Debug.Print("I am unlocked: " + DateTime.Now);
        this.Logger.Debug("I am unlocked: " + DateTime.Now);
        ViewModel.newProcess.TogglePause();
      }
    }

    private ILog Logger { get; } =
        LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

    private void InitializeLogger()
    {
      using FileStream fileStream = File.OpenRead("Properties/log4net.config");
      var log4netConfig = new XmlDocument();
      log4netConfig.Load(fileStream);

      ILoggerRepository repo = LogManager.CreateRepository(
          Assembly.GetEntryAssembly(),
          typeof(log4net.Repository.Hierarchy.Hierarchy));

      XmlConfigurator.Configure(repo, log4netConfig["log4net"]);
    }

    private void OnUnhandledException(object sender, UnhandledExceptionEventArgs e) => LogUnhandledException(e.ExceptionObject as Exception);

    private void OnUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e) => LogUnhandledException(e.Exception);

    private void LogUnhandledException(Exception exception) =>
        this.Logger.Fatal("Unhandled exception.", exception);
  }
}
