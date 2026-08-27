using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonSecondarySmall : VideoOSButtonBase
{
	static VideoOSButtonSecondarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondarySmall)));
	}
}
