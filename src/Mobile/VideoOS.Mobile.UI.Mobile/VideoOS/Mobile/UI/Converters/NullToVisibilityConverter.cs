using System;
using System.Globalization;
using System.Windows;

namespace VideoOS.Mobile.UI.Converters;

public class NullToVisibilityConverter : ValueConverterBase
{
	public static readonly Visibility DefaultNullVisibility = Visibility.Collapsed;

	public bool ReverseConversion { get; set; }

	public Visibility? NullVisibility { get; set; }

	public NullToVisibilityConverter()
	{
		NullVisibility = DefaultNullVisibility;
		ReverseConversion = false;
	}

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		Visibility? visibility = ((value == null) ? NullVisibility : new Visibility?(Visibility.Visible));
		if (ReverseConversion)
		{
			visibility = ((visibility == Visibility.Visible) ? NullVisibility : new Visibility?(Visibility.Visible));
		}
		return visibility;
	}
}
