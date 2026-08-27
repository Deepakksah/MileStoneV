using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonSecondaryDestructiveMedium : VideoOSButtonBase
{
	static VideoOSButtonSecondaryDestructiveMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondaryDestructiveMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondaryDestructiveMedium)));
	}
}
