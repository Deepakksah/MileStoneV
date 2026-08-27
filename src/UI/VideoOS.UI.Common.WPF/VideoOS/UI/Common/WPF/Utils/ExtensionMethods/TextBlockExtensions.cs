using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.UI.Common.WPF.Utils.ExtensionMethods;

public static class TextBlockExtensions
{
	public static Size MeasureString(this TextBlock textBlock)
	{
		return textBlock.Text.MeasureString(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch, textBlock.FontSize, VisualTreeHelper.GetDpi(textBlock).PixelsPerDip);
	}
}
