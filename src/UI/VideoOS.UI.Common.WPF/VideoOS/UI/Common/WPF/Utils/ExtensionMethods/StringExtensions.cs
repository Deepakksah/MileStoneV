using System;
using System.Globalization;
using System.Windows;
using System.Windows.Media;
using VideoOS.UI.Common.WPF.Utils.RightToLeft;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class StringExtensions
{
	public static Size MeasureString(this string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize)
	{
		FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, RtlUtility.SelectedFlowDirection, new Typeface(fontFamily, fontStyle, fontWeight, fontStretch), fontSize, Brushes.Black, new NumberSubstitution());
		return new Size(formattedText.Width, formattedText.Height);
	}

	public static Size MeasureString(this string text, FontFamily fontFamily, FontStyle fontStyle, FontWeight fontWeight, FontStretch fontStretch, double fontSize, double PixelsPerDip)
	{
		FormattedText formattedText = new FormattedText(text, CultureInfo.CurrentCulture, RtlUtility.SelectedFlowDirection, new Typeface(fontFamily, fontStyle, fontWeight, fontStretch), fontSize, Brushes.Black, new NumberSubstitution(), PixelsPerDip);
		return new Size(formattedText.Width, formattedText.Height);
	}

	public static SolidColorBrush ToSolidColorBrush(this string hexColor)
	{
		return (SolidColorBrush)new BrushConverter().ConvertFrom(hexColor);
	}

	public static bool TryCreateUri(this string text, out Uri uri, string fallbackScheme = "https")
	{
		if (Uri.TryCreate(text, UriKind.Absolute, out uri) || Uri.TryCreate(fallbackScheme + "://" + text, UriKind.Absolute, out uri))
		{
			return true;
		}
		return false;
	}
}
