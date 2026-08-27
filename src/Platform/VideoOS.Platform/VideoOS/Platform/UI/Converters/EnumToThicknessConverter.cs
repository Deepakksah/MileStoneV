using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class EnumToThicknessConverter : MarkupExtension, IValueConverter
{
	public Enum MatchValue { get; set; }

	public Thickness MatchedResult { get; set; } = new Thickness(0.0);

	public Thickness NotMatchedResult { get; set; } = new Thickness(0.0);

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
