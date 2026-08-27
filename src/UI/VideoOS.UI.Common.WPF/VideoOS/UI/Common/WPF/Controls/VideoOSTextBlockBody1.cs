using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockBody1 : TooltipSupportTextBlock
{
	static VideoOSTextBlockBody1()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockBody1), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockBody1)));
	}
}
