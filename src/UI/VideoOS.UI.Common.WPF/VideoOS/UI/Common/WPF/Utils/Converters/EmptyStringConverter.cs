using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class EmptyStringConverter : MarkupExtension, IValueConverter
{
	public object ValueIfEmpty { get; set; }

	public object ValueIfNotEmpty { get; set; }

	public EmptyStringConverter()
	{
		ValueIfEmpty = "Empty";
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		string value2 = System.Convert.ToString(value, CultureInfo.InvariantCulture);
		object result = ((ValueIfNotEmpty == null) ? value : ValueIfNotEmpty);
		if (!string.IsNullOrEmpty(value2))
		{
			return result;
		}
		return ValueIfEmpty;
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new InvalidOperationException("'EmptyStringToNullConverter' can only used in one way bindings");
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
