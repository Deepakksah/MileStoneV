using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonTertiarySmall : VideoOSButtonBase
{
	static VideoOSButtonTertiarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTertiarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTertiarySmall)));
	}
}
