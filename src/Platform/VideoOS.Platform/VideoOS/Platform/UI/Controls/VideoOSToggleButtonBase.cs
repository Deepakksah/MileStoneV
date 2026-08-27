using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSToggleButtonBase : ToggleButton
{
	public static readonly DependencyProperty EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty;

	public bool EnsureFocusIsNotLostWhenActivatedUsingSpaceKey
	{
		get
		{
			return (bool)GetValue(EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty);
		}
		set
		{
			SetValue(EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty, value);
		}
	}

	static VideoOSToggleButtonBase()
	{
		EnsureFocusIsNotLostWhenActivatedUsingSpaceKeyProperty = DependencyProperty.Register("EnsureFocusIsNotLostWhenActivatedUsingSpaceKey", typeof(bool), typeof(VideoOSToggleButtonBase), new PropertyMetadata(false));
		FrameworkElement.CursorProperty.OverrideMetadata(typeof(VideoOSToggleButtonBase), new FrameworkPropertyMetadata(Cursors.Hand));
	}

	public VideoOSToggleButtonBase()
	{
		base.PreviewKeyDown += VideoOSToggleButtonBase_PreviewKeyDown;
	}

	private void VideoOSToggleButtonBase_PreviewKeyDown(object sender, KeyEventArgs e)
	{
		if (EnsureFocusIsNotLostWhenActivatedUsingSpaceKey && e.Key == Key.Space)
		{
			base.IsChecked = !base.IsChecked;
			e.Handled = true;
		}
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSToggleButtonBaseAutomationPeer(this);
	}
}
