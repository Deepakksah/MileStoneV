using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockLabel1 : TooltipSupportTextBlock
{
	static VideoOSTextBlockLabel1()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockLabel1), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockLabel1)));
	}
}
