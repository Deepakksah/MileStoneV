using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace VideoOS.UI.Common.WPF.Utils.Converters;

public class CountToVisibilityConverter : MarkupExtension, IValueConverter, IMultiValueConverter
{
	public int Threshold { get; set; }

	public Visibility EqualOrLessVisibility { get; set; }

	public Visibility GreaterThanVisibility { get; set; }

	public CountToVisibilityConverter()
	{
		Threshold = 0;
		EqualOrLessVisibility = Visibility.Collapsed;
		GreaterThanVisibility = Visibility.Visible;
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			return (System.Convert.ToInt32(value, CultureInfo.InvariantCulture) > Threshold) ? GreaterThanVisibility : EqualOrLessVisibility;
		}
		catch (InvalidCastException)
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			return (values.Select((object i) => System.Convert.ToInt32(i, CultureInfo.InvariantCulture)).Sum() > Threshold) ? GreaterThanVisibility : EqualOrLessVisibility;
		}
		catch (InvalidCastException)
		{
			return DependencyProperty.UnsetValue;
		}
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}

	public override object ProvideValue(IServiceProvider serviceProvider)
	{
		return this;
	}
}
