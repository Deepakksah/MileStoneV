using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class IntToVisibilityConverter : MarkupExtension, IValueConverter
{
	public int MinMatchValue { get; set; } = int.MinValue;

	public int MaxMatchValue { get; set; } = int.MaxValue;

	public Visibility MatchedResult { get; set; }

	public Visibility NotMatchedResult { get; set; } = Visibility.Collapsed;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is int num && num >= MinMatchValue && num <= MaxMatchValue) ? MatchedResult : NotMatchedResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
