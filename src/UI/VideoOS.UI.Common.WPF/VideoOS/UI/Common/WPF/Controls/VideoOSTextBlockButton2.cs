using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockButton2 : TooltipSupportTextBlock
{
	static VideoOSTextBlockButton2()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockButton2), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockButton2)));
	}
}
