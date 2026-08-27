using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class EnumToVisibilityConverter : MarkupExtension, IValueConverter
{
	public Enum MatchValue { get; set; }

	public Visibility MatchedResult { get; set; }

	public Visibility NotMatchedResult { get; set; } = Visibility.Collapsed;

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is Enum obj && obj.Equals(MatchValue)) ? MatchedResult : NotMatchedResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
