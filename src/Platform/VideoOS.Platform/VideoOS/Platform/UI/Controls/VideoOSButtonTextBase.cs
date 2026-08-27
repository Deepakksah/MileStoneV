using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSButtonTextBase : VideoOSButtonBase
{
	public static readonly DependencyProperty IsDestructiveProperty = DependencyProperty.Register("IsDestructive", typeof(bool), typeof(VideoOSButtonTextBase), new PropertyMetadata(false));

	public bool IsDestructive
	{
		get
		{
			return (bool)GetValue(IsDestructiveProperty);
		}
		set
		{
			SetValue(IsDestructiveProperty, value);
		}
	}

	public VideoOSButtonTextBase()
	{
	}
}
