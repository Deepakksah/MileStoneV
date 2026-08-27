using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class CountToStringConverter : MarkupExtension, IValueConverter
{
	public int Threshold { get; set; }

	public string EqualOrLessString { get; set; }

	public string GreaterThanString { get; set; }

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			return (System.Convert.ToInt32(value, CultureInfo.InvariantCulture) > Threshold) ? GreaterThanString : EqualOrLessString;
		}
		catch
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
