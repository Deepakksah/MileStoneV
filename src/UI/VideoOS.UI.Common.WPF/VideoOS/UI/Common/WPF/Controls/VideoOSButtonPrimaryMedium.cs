using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonPrimaryMedium : VideoOSButtonBase
{
	static VideoOSButtonPrimaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimaryMedium)));
	}
}
