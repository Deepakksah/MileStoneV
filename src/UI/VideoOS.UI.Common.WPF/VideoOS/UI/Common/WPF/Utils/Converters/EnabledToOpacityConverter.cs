using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class EnabledToOpacityConverter : MarkupExtension, IValueConverter
{
	public double DisabledOpacityValue { get; set; } = 0.3;

	public double EnabledOpacityValue { get; set; } = 1.0;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(bool)value)
		{
			return DisabledOpacityValue;
		}
		return EnabledOpacityValue;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
