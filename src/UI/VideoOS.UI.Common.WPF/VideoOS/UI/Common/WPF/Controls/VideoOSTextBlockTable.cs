using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockTable : TooltipSupportTextBlock
{
	static VideoOSTextBlockTable()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockTable), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockTable)));
	}
}
