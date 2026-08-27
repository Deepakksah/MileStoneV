using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockH3High : TooltipSupportTextBlock
{
	static VideoOSTextBlockH3High()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockH3High), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockH3High)));
	}
}
