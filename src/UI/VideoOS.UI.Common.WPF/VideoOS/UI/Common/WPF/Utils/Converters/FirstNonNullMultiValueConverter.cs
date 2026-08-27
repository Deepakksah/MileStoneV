using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class FirstNonNullMultiValueConverter : MarkupExtension, IMultiValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		if (values == null)
		{
			return DependencyProperty.UnsetValue;
		}
		foreach (object obj in values)
		{
			if (obj != null)
			{
				return obj;
			}
		}
		return DependencyProperty.UnsetValue;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
