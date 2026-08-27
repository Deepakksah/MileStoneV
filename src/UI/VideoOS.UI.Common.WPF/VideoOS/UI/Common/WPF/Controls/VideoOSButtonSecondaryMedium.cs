using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonSecondaryMedium : VideoOSButtonBase
{
	static VideoOSButtonSecondaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondaryMedium)));
	}
}
