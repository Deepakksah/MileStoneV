using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonSecondaryDestructiveSmall : VideoOSButtonBase
{
	static VideoOSButtonSecondaryDestructiveSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonSecondaryDestructiveSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonSecondaryDestructiveSmall)));
	}
}
