using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionSelectControlHolder : ContentControl, IActionControlHolder, IComponentConnector
{
	public static string CheckBoxExtraSmallDataTemplateName = "ActionSelectControlHolderCheckBoxExtraSmallDataTemplateName";

	public static string CheckBoxSmallDataTemplateName = "ActionSelectControlHolderCheckBoxSmallDataTemplateName";

	public static string CheckBoxMediumDataTemplateName = "ActionSelectControlHolderCheckBoxMediumDataTemplateName";

	public static string ToggleButtonIconExtraSmallDataTemplateName = "ActionSelectControlHolderToggleButtonIconExtraSmallDataTemplateName";

	public static string ToggleButtonIconSmallDataTemplateName = "ActionSelectControlHolderToggleButtonIconSmallDataTemplateName";

	public static string ToggleButtonIconMediumDataTemplateName = "ActionSelectControlHolderToggleButtonIconMediumDataTemplateName";

	public static string ToggleButtonSwitchExtraSmallDataTemplateName = "ActionSelectControlHolderToggleButtonSwitchExtraSmallDataTemplateName";

	public static string ToggleButtonSwitchSmallDataTemplateName = "ActionSelectControlHolderToggleButtonSwitchSmallDataTemplateName";

	public static string ToggleButtonSwitchMediumDataTemplateName = "ActionSelectControlHolderToggleButtonSwitchMediumDataTemplateName";

	public static IMultiValueConverter ContentDataTemplateConverter { get; } = new LambdaConverter2<ActionSelectControlHolder, VideoOSActionSelect.Appearances>(delegate(ActionSelectControlHolder self, VideoOSActionSelect.Appearances appearance)
	{
		string resourceKey = string.Format("{0}{1}{2}DataTemplateName", "ActionSelectControlHolder", appearance, self.ControlSize);
		return (DataTemplate)self.FindResource(resourceKey);
	}, "ContentDataTemplateConverter");

	public ActionsPresenter.ControlSizes ControlSize { get; set; }

	public FrameworkElement ActionableElement => this.FindDescendant<ToggleButton>();

	public ActionSelectControlHolder()
	{
		InitializeComponent();
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new ActionControlHolderAutomationPeer(this);
	}
}
