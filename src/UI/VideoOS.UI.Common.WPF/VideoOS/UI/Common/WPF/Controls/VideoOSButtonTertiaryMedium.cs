using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonTertiaryMedium : VideoOSButtonBase
{
	static VideoOSButtonTertiaryMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTertiaryMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonTertiaryMedium)));
	}
}
