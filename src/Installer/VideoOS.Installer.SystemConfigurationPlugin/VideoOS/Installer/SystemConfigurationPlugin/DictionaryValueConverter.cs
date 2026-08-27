using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class DictionaryValueConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return string.Empty;
		}
		return ((KeyValuePair<UserPageViewModel.UserType, string>)value).Key;
	}
}
