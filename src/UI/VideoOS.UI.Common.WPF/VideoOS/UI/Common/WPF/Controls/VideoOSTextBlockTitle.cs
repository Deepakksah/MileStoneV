using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockTitle : TooltipSupportTextBlock
{
	static VideoOSTextBlockTitle()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockTitle), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockTitle)));
	}
}
