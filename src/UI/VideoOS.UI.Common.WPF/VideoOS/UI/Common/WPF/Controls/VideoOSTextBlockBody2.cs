using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockBody2 : TooltipSupportTextBlock
{
	static VideoOSTextBlockBody2()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockBody2), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockBody2)));
	}
}
