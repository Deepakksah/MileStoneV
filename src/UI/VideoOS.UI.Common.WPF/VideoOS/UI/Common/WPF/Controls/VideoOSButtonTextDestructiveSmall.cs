using System.Windows;
using VideoOS.UI.Common.WPF.Controls.Internal;

namespace VideoOS.UI.Common.WPF.Controls;

public class VideoOSButtonTextDestructiveSmall : VideoOSButtonBase
{
	static VideoOSButtonTextDestructiveSmall()
	{
		FrameworkElement.DefaultStyleKeyProperty.OverrideMetadata(typeof(VideoOSButtonTextDestructiveSmall), new FrameworkPropertyMetadata(typeof(VideoOSButtonTextDestructiveSmall)));
	}
}
