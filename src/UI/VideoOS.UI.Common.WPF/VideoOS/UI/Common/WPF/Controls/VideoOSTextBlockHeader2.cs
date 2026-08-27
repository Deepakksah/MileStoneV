using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockHeader2 : TooltipSupportTextBlock
{
	static VideoOSTextBlockHeader2()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockHeader2), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockHeader2)));
	}
}
