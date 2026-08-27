using System;
using System.Globalization;
using System.Windows.Data;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

[ValueConversion(typeof(bool), typeof(string))]
public class EnabledTextConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (targetType != typeof(string))
		{
			throw new InvalidOperationException("The target must be a string");
		}
		if (!System.Convert.ToBoolean(value, CultureInfo.InvariantCulture))
		{
			return Resources.HardwareSelectionDisabled;
		}
		return Resources.HardwareSelectionEnabled;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotSupportedException();
	}
}
