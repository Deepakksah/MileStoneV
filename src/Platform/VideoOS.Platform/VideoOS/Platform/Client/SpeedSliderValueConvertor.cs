using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.Platform.Client;

public class SpeedSliderValueConvertor : IValueConverter
{
	public static Dictionary<double, double> PositionToSpeed = new Dictionary<double, double>
	{
		{ 0.0, 0.1 },
		{ 1.0, 0.25 },
		{ 2.0, 0.5 },
		{ 3.0, 1.0 },
		{ 4.0, 2.0 },
		{ 5.0, 4.0 },
		{ 6.0, 8.0 },
		{ 7.0, 16.0 },
		{ 8.0, 32.0 }
	};

	public static Dictionary<double, double> SpeedToPosition = new Dictionary<double, double>
	{
		{ 0.1, 0.0 },
		{ 0.25, 1.0 },
		{ 0.5, 2.0 },
		{ 1.0, 3.0 },
		{ 2.0, 4.0 },
		{ 4.0, 5.0 },
		{ 8.0, 6.0 },
		{ 16.0, 7.0 },
		{ 32.0, 8.0 }
	};

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = PositionToSpeed[(double)value];
		if (num == 0.25)
		{
			return "1/4x";
		}
		if (num == 0.5)
		{
			return "1/2x";
		}
		return num.ToString(CultureInfo.InvariantCulture) + "x";
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
