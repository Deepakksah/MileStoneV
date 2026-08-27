using System;
using System.Globalization;
using System.Windows.Data;

namespace VideoOS.UI.Common.WPF.VideoText;

public sealed class ObjectToBoolValueConverter : IValueConverter
{
	object IValueConverter.Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (value == null)
			{
				return false;
			}
			return true;
		}
		catch
		{
			return null;
		}
	}

	object IValueConverter.ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
