using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.ValueConverters;

public class ColorToSolidColorBrushConverter : IValueConverter
{
	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value == null)
		{
			return null;
		}
		if (value is Color)
		{
			SolidColorBrush solidColorBrush = new SolidColorBrush((Color)value);
			solidColorBrush.Freeze();
			return solidColorBrush;
		}
		Type type = value.GetType();
		throw new InvalidOperationException("Unsupported type [" + type.Name + "]");
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
