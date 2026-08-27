using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class ValueParameterCompareToBoolConverter : MarkupExtension, IValueConverter
{
	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return value?.Equals(parameter);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(bool)value)
		{
			return Binding.DoNothing;
		}
		return parameter;
	}
}
