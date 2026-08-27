using System.Windows;
using System.Windows.Automation.Peers;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls;

public abstract class VideoOSTabControlPrimaryBase : VideoOSTabControlBase
{
	public static readonly DependencyProperty ItemIconSourcePropertyNameProperty = DependencyProperty.Register("ItemIconSourcePropertyName", typeof(string), typeof(VideoOSTabControlPrimaryBase), new PropertyMetadata(null));

	public static readonly DependencyProperty ItemActionsPropertyNameProperty = DependencyProperty.Register("ItemActionsPropertyName", typeof(string), typeof(VideoOSTabControlPrimaryBase), new PropertyMetadata(null));

	public static readonly DependencyProperty ItemAlertPropertyNameProperty = DependencyProperty.Register("ItemAlertPropertyName", typeof(string), typeof(VideoOSTabControlPrimaryBase), new PropertyMetadata(null));

	public string ItemIconSourcePropertyName
	{
		get
		{
			return (string)GetValue(ItemIconSourcePropertyNameProperty);
		}
		set
		{
			SetValue(ItemIconSourcePropertyNameProperty, value);
		}
	}

	public string ItemActionsPropertyName
	{
		get
		{
			return (string)GetValue(ItemActionsPropertyNameProperty);
		}
		set
		{
			SetValue(ItemActionsPropertyNameProperty, value);
		}
	}

	public string ItemAlertPropertyName
	{
		get
		{
			return (string)GetValue(ItemAlertPropertyNameProperty);
		}
		set
		{
			SetValue(ItemAlertPropertyNameProperty, value);
		}
	}

	public VideoOSTabControlPrimaryBase()
	{
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new VideoOSTabControlPrimaryBaseAutomationPeer(this);
	}
}
