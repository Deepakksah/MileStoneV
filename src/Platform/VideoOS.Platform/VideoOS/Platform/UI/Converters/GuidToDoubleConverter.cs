using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class GuidToDoubleConverter : MarkupExtension, IValueConverter
{
	public Guid MatchValue { get; set; }

	public double MatchedResult { get; set; }

	public double NotMatchedResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		return (value is Guid guid && guid == MatchValue) ? MatchedResult : NotMatchedResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
