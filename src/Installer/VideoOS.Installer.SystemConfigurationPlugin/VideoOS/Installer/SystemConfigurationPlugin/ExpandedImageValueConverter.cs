using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ExpandedImageValueConverter : IMultiValueConverter
{
	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null)
		{
			throw new ArgumentNullException("values");
		}
		if (targetType != typeof(ImageSource))
		{
			throw new InvalidOperationException("The target must be a Visibility");
		}
		if (!System.Convert.ToBoolean(values[0], CultureInfo.InvariantCulture))
		{
			return values[2];
		}
		return values[1];
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
