using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.Platform.UI.Converters;

public class EnumToDataTemplateSelectorConverter : MarkupExtension, IValueConverter
{
	public Enum MatchValue { get; set; }

	public DataTemplateSelector MatchedResult { get; set; }

	public DataTemplateSelector NotMatchedResult { get; set; }

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
