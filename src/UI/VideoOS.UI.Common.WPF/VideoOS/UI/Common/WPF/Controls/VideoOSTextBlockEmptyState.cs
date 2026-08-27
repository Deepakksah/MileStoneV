using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSTextBlockEmptyState : TooltipSupportTextBlock
{
	static VideoOSTextBlockEmptyState()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSTextBlockEmptyState), new FrameworkPropertyMetadata(typeof(VideoOSTextBlockEmptyState)));
	}
}
