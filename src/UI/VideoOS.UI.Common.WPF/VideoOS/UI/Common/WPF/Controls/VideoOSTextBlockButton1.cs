using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockButton1 : TooltipSupportTextBlock
{
	static VideoOSTextBlockButton1()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockButton1), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockButton1)));
	}
}
