﻿using System.Windows;
using System.Windows.Input;

namespace SimpleTimer.Main
{
  /// <summary>
  /// Interaction logic for MainWindow.xaml
  /// </summary>
  public partial class MainWindow : Window
  {
    public MainWindow()
    {
      this.InitializeComponent();
    }

    private void Window_MouseDown(object sender, MouseButtonEventArgs e)
    {
      if (e.ChangedButton == MouseButton.Left)
        this.DragMove();
    }

    private void Button_PreviewMouseDown(object sender, MouseButtonEventArgs e)
    {
      this.Close();
    }
  }
}
