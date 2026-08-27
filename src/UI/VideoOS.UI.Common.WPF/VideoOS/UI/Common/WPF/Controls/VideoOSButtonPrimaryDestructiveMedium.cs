using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonPrimaryDestructiveMedium : VideoOSButtonBase
{
	static VideoOSButtonPrimaryDestructiveMedium()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimaryDestructiveMedium), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimaryDestructiveMedium)));
	}
}
