using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class DoubleToBoolConverter : MarkupExtension, IValueConverter
{
	public double MinMatchValue { get; set; } = double.MinValue;

	public double MaxMatchValue { get; set; } = double.MaxValue;

	public bool MatchedResult { get; set; } = true;

	public bool NotMatchedResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is double num && num >= MinMatchValue && num <= MaxMatchValue) ? MatchedResult : NotMatchedResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
