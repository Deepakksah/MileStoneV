using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.Installer.UI.Framework;

public sealed class BooleanToVisibilityConverter : IValueConverter
{
	public bool IsReversed { get; set; }

	public bool UseHidden { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = System.Convert.ToBoolean(value, CultureInfo.InvariantCulture);
		if (IsReversed)
		{
			flag = !flag;
		}
		if (flag)
		{
			return Visibility.Visible;
		}
		return UseHidden ? Visibility.Hidden : Visibility.Collapsed;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
