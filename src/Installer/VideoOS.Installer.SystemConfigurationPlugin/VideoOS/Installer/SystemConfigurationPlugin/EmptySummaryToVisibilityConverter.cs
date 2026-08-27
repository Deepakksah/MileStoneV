using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace VideoOS.Installer.SystemConfigurationPlugin;

[ValueConversion(typeof(ObservableCollection<string>), typeof(Visibility))]
public class EmptySummaryToVisibilityConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType != typeof(Visibility))
		{
			throw new InvalidOperationException("The target must be a Visibility");
		}
		return ((value as ObservableCollection<string>).Count > 0) ? Visibility.Collapsed : Visibility.Visible;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
