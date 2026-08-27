using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonPrimarySmall : VideoOSButtonBase
{
	static VideoOSButtonPrimarySmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimarySmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimarySmall)));
	}
}
