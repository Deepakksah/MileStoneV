using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionSeparatorControlHolder : ContentControl, IComponentConnector
{
	public static readonly DependencyProperty ControlSizeProperty = DependencyProperty.Register("ControlSize", typeof(ActionsPresenter.ControlSizes), typeof(ActionSeparatorControlHolder), new PropertyMetadata(ActionsPresenter.ControlSizes.ExtraSmall));

	public ActionsPresenter.ControlSizes ControlSize
	{
		get
		{
			return (ActionsPresenter.ControlSizes)GetValue(ControlSizeProperty);
		}
		set
		{
			SetValue(ControlSizeProperty, value);
		}
	}

	public ActionSeparatorControlHolder()
	{
		InitializeComponent();
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new ActionControlHolderAutomationPeer(this);
	}
}
