using System;
using System.Globalization;
using System.Windows;

namespace VideoOS.Mobile.UI.Converters;

public class BooleanToVisibilityConverter : ValueConverterBase
{
	public static readonly Visibility DefaultFalseVisibility = Visibility.Collapsed;

	public Visibility? FalseVisibility { get; set; }

	public bool ReverseConversion { get; set; }

	public BooleanToVisibilityConverter()
	{
		FalseVisibility = DefaultFalseVisibility;
		ReverseConversion = false;
	}

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Visibility? visibility = ((value as bool? == true) ? new Visibility?(Visibility.Visible) : FalseVisibility);
		if (ReverseConversion)
		{
			visibility = ((visibility == Visibility.Visible) ? FalseVisibility : new Visibility?(Visibility.Visible));
		}
		return visibility;
	}

	public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		bool flag = value as Visibility? == Visibility.Visible;
		if (ReverseConversion)
		{
			flag = !flag;
		}
		return flag;
	}
}
