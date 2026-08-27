using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using VideoOS.Platform.Resources;

namespace VideoOS.Platform.Client;

public class TimeSpanSliderTextConvertor : IValueConverter
{
	public static Dictionary<int, string> TimeSpanTotalMinutesToText = new Dictionary<int, string>
	{
		{
			1,
			Strings.PlaybackUserControlMini_SingleMinute
		},
		{
			2,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiMinutes, 2)
		},
		{
			5,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiMinutes, 5)
		},
		{
			10,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiMinutes, 10)
		},
		{
			20,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiMinutes, 20)
		},
		{
			60,
			Strings.PlaybackUserControlMini_SingleHour
		},
		{
			120,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 2)
		},
		{
			240,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 4)
		},
		{
			480,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 8)
		},
		{
			720,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 12)
		},
		{
			960,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 16)
		},
		{
			1200,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiHours, 20)
		},
		{
			1440,
			Strings.PlaybackUserControlMini_SingleDay
		},
		{
			2880,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiDays, 2)
		},
		{
			5760,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiDays, 4)
		},
		{
			10080,
			Strings.PlaybackUserControlMini_SingleWeek
		},
		{
			20160,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiWeeks, 2)
		},
		{
			40320,
			string.Format(CultureInfo.CurrentCulture, Strings.PlaybackUserControlMini_MultiWeeks, 4)
		}
	};

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return TimeSpanTotalMinutesToText[(int)value];
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
