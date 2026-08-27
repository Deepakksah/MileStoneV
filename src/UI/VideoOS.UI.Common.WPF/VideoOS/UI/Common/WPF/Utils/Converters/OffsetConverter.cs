using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class OffsetConverter : MarkupExtension, IValueConverter
{
	public double FixedOffset { get; set; }

	public double Multiplier { get; set; } = 1.0;

	public double MinValue { get; set; } = double.MinValue;

	public double MaxValue { get; set; } = double.MaxValue;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double? num = value as double?;
		if (!num.HasValue)
		{
			return DependencyProperty.UnsetValue;
		}
		return Math.Min(Math.Max(FixedOffset + num.Value * Multiplier, MinValue), MaxValue);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
