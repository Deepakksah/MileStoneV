using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockLabel2 : TooltipSupportTextBlock
{
	static VideoOSTextBlockLabel2()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockLabel2), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockLabel2)));
	}
}
