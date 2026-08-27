using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class MaxWidthConverter : MarkupExtension, IValueConverter, IMultiValueConverter
{
	public double MaxWidth { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		double num = System.Convert.ToDouble(value, CultureInfo.InvariantCulture);
		return System.Convert.ChangeType(Math.Max(0.0, MaxWidth - num), targetType);
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		double num = values.Select((object x) => System.Convert.ToDouble(x, CultureInfo.InvariantCulture)).Sum();
		return System.Convert.ChangeType(Math.Max(0.0, MaxWidth - num), targetType);
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
