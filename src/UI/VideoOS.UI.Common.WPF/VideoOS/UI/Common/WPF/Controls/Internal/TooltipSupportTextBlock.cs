using System.Windows;
using System.Windows.Controls;

namespace VideoOS.UI.Common.WPF.Controls.Internal;

public abstract class TooltipSupportTextBlock : TextBlock
{
	static TooltipSupportTextBlock()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(TooltipSupportTextBlock), new FrameworkPropertyMetadata(typeof(TooltipSupportTextBlock)));
	}

	public bool IsTextConstrained()
	{
		Size size = MeasureOverride(new Size(double.PositiveInfinity, double.PositiveInfinity));
		Size size2 = MeasureOverride(new Size(base.ActualWidth, base.ActualHeight));
		if (base.TextWrapping != TextWrapping.NoWrap && size.Height < size2.Height)
		{
			return size2.Height / size.Height * size2.Width < size.Width;
		}
		return size != size2;
	}
}
