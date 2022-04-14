﻿using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using SimpleTimer.Models.HelperClasses;

namespace SimpleTimer.Resources.Converter
{
  public class TimeExceededConverter : IValueConverter
  {
    #region IValueConverter Members

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
      string HoursLimitProp = (string)value;

      TimeSpan res;
      bool result = TimeSpan.TryParseExact(HoursLimitProp, @"hh\:mm\:ss", CultureInfo.InvariantCulture, out res);

      TimeSpan RingTime = HelperClass.ParseToTimeSpanRingTime();

      if (res > RingTime)
      {
        return Brushes.Red;
      }
      else
      {
        return Brushes.LightGreen;
      }

    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
      return Binding.DoNothing;
    }

    #endregion
  }
}
