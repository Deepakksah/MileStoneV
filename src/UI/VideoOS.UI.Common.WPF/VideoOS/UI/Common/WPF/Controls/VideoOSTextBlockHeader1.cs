using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockHeader1 : TooltipSupportTextBlock
{
	static VideoOSTextBlockHeader1()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockHeader1), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockHeader1)));
	}
}
