using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;

namespace SimpleTimer.Models.HelperClasses
{
  public static class HelperClass
  {
    public static DateTime DateTimeConverter(string timeAsString)
    {
      if (!DateTime.TryParse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime ringTime))
      {
        // handle validation error
      }
      TimeSpan time = ringTime.TimeOfDay;

      return ringTime;
    }

    public static TimeSpan ParseToTimeSpan(string TimeAsString)
    {
      TimeSpan timeSpan = new TimeSpan(0, 0, 0, 0, 0);

      if (!string.IsNullOrEmpty(TimeAsString))
      {
        List<int> TimeSplit = TimeAsString.Split(':').Where(x => int.TryParse(x, out _)).Select(int.Parse).ToList();
        timeSpan = new TimeSpan(0, TimeSplit[0], TimeSplit[1], 0, 0);
      }

      return timeSpan;
    }

    public static TimeSpan ParseToTimeSpanRingTime()
    {
      TimeSpan result = new SettingsConnector().TryReadString(Properties.StringResources.TimeLimit,
               out string timeAsString) && DateTime.TryParse(timeAsString, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime timeAsTimeSpan)
                    ? timeAsTimeSpan.TimeOfDay
                    : TimeSpan.Zero;

      return result;
    }

    public static void PlaySound()
    {
      try
      {
        System.Media.SoundPlayer player = new System.Media.SoundPlayer(string.Concat(System.IO.Directory.GetCurrentDirectory(),
                @"\Miscellaneous\Sounds\bell.wav"));
        player.Play();

        //Debug.WriteLine("+++++++++ bell ring +++++++++");

      }
      catch (Exception)
      {
        throw;
      }
    }
  }
}
