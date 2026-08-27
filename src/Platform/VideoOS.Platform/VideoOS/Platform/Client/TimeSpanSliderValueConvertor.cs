using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.Platform.Client;

public class TimeSpanSliderValueConvertor : IValueConverter
{
	public static Dictionary<int, int> TimeSpanTotalMinutesToPosition = new Dictionary<int, int>
	{
		{ 1, 0 },
		{ 2, 1 },
		{ 5, 2 },
		{ 10, 3 },
		{ 20, 4 },
		{ 60, 5 },
		{ 120, 6 },
		{ 240, 7 },
		{ 480, 8 },
		{ 720, 9 },
		{ 960, 10 },
		{ 1200, 11 },
		{ 1440, 12 },
		{ 2880, 13 },
		{ 5760, 14 },
		{ 10080, 15 },
		{ 20160, 16 },
		{ 40320, 17 }
	};

	public static int[] PositionToTimeSpanTotalMinutes = new int[18]
	{
		1, 2, 5, 10, 20, 60, 120, 240, 480, 720,
		960, 1200, 1440, 2880, 5760, 10080, 20160, 40320
	};

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return TimeSpanTotalMinutesToPosition[(int)value];
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return PositionToTimeSpanTotalMinutes[(int)(double)value];
	}
}
