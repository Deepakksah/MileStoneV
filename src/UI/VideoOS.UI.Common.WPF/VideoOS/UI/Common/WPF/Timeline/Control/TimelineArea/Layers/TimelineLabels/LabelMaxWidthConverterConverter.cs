using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.Timeline.Control.TimelineArea.Layers.TimelineLabels;

public class LabelMaxWidthConverterConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = 200.0;
		if (value is double num2)
		{
			num = Math.Min(num, num2 / 4.0);
		}
		return num;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
