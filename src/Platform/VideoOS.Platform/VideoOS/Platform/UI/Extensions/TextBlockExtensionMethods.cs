using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace VideoOS.Platform.UI.Extensions;

public static class TextBlockExtensionMethods
{
	public static Size MeasureString(this TextBlock textBlock, string text = null)
	{
		FormattedText formattedText = new FormattedText(text ?? textBlock.Text, CultureInfo.CurrentCulture, FlowDirection.LeftToRight, new Typeface(textBlock.FontFamily, textBlock.FontStyle, textBlock.FontWeight, textBlock.FontStretch), textBlock.FontSize, Brushes.Black, new NumberSubstitution(), 1.0);
		return new Size(formattedText.Width, formattedText.Height);
	}
}
