using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonTextSmall : VideoOSButtonBase
{
	static VideoOSButtonTextSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTextSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTextSmall)));
	}
}
