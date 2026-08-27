using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonPrimaryDestructiveSmall : VideoOSButtonBase
{
	static VideoOSButtonPrimaryDestructiveSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonPrimaryDestructiveSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonPrimaryDestructiveSmall)));
	}
}
