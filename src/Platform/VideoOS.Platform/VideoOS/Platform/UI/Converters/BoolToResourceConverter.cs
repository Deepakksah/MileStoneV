using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Converters;

public class BoolToResourceConverter : MarkupExtension, IMultiValueConverter
{
	public object FalseResult { get; set; } = string.Empty;

	public object TrueResult { get; set; } = string.Empty;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values != null && values.Length > 1 && values[0] is DependencyObject self)
		{
			object obj = values[1];
			if (obj is bool)
			{
				object resourceKey = (((bool)obj) ? TrueResult : FalseResult);
				return self.FindResource(resourceKey);
			}
		}
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
