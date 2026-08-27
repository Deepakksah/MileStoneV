using System;
using System.Globalization;
using System.Windows;

namespace VideoOS.Mobile.UI.Converters;

public class ProgressBarOutOfRangeVisibilityConverter : ValueConverterBase
{
	public static readonly Visibility DefaultOutOfRangeVisibility = Visibility.Hidden;

	public static readonly int DefaultMinRange = 0;

	public static readonly int DefaultMaxRange = 100;

	public int Min { get; set; }

	public int Max { get; set; }

	public Visibility OutOfRangeVisibility { get; set; }

	public ProgressBarOutOfRangeVisibilityConverter()
	{
		OutOfRangeVisibility = DefaultOutOfRangeVisibility;
		Min = DefaultMinRange;
		Max = DefaultMaxRange;
	}

	public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		if (value is int num && Min <= num && num <= Max)
		{
			return Visibility.Visible;
		}
		return OutOfRangeVisibility;
	}
}
