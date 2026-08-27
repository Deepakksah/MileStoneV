using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class EnumToEnumConverter : MarkupExtension, IValueConverter
{
	public Enum MatchValue { get; set; }

	public Enum MatchedResult { get; set; }

	public Enum NotMatchedResult { get; set; }

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (!(value is Enum obj) || !obj.Equals(MatchValue))
		{
			return NotMatchedResult;
		}
		return MatchedResult;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
