using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionHoldControlHolder : ContentControl, IActionControlHolder, IComponentConnector
{
	public static string ButtonPrimaryExtraSmallDataTemplateName = "ActionHoldControlHolderButtonPrimaryExtraSmallDataTemplateName";

	public static string ButtonPrimarySmallDataTemplateName = "ActionHoldControlHolderButtonPrimarySmallDataTemplateName";

	public static string ButtonPrimaryMediumDataTemplateName = "ActionHoldControlHolderButtonPrimaryMediumDataTemplateName";

	public static string ButtonSecondaryExtraSmallDataTemplateName = "ActionHoldControlHolderButtonSecondaryExtraSmallDataTemplateName";

	public static string ButtonSecondarySmallDataTemplateName = "ActionHoldControlHolderButtonSecondarySmallDataTemplateName";

	public static string ButtonSecondaryMediumDataTemplateName = "ActionHoldControlHolderButtonSecondaryMediumDataTemplateName";

	public static string ButtonTertiaryExtraSmallDataTemplateName = "ActionHoldControlHolderButtonTertiaryExtraSmallDataTemplateName";

	public static string ButtonTertiarySmallDataTemplateName = "ActionHoldControlHolderButtonTertiarySmallDataTemplateName";

	public static string ButtonTertiaryMediumDataTemplateName = "ActionHoldControlHolderButtonTertiaryMediumDataTemplateName";

	public static string ButtonTransparentExtraSmallDataTemplateName = "ActionHoldControlHolderButtonTransparentExtraSmallDataTemplateName";

	public static string ButtonTransparentSmallDataTemplateName = "ActionHoldControlHolderButtonTransparentSmallDataTemplateName";

	public static string ButtonTransparentMediumDataTemplateName = "ActionHoldControlHolderButtonTransparentMediumDataTemplateName";

	public static string ButtonTextExtraSmallDataTemplateName = "ActionHoldControlHolderButtonTextExtraSmallDataTemplateName";

	public static string ButtonTextSmallDataTemplateName = "ActionHoldControlHolderButtonTextSmallDataTemplateName";

	public static string ButtonTextMediumDataTemplateName = "ActionHoldControlHolderButtonTextMediumDataTemplateName";

	public static string ButtonHyperlinkExtraSmallDataTemplateName = "ActionHoldControlHolderButtonHyperlinkExtraSmallDataTemplateName";

	public static string ButtonHyperlinkSmallDataTemplateName = "ActionHoldControlHolderButtonHyperlinkSmallDataTemplateName";

	public static string ButtonHyperlinkMediumDataTemplateName = "ActionHoldControlHolderButtonHyperlinkMediumDataTemplateName";

	public static string ButtonAccentExtraSmallDataTemplateName = "ActionHoldControlHolderButtonAccentExtraSmallDataTemplateName";

	public static string ButtonAccentSmallDataTemplateName = "ActionHoldControlHolderButtonAccentSmallDataTemplateName";

	public static string ButtonAccentMediumDataTemplateName = "ActionHoldControlHolderButtonAccentMediumDataTemplateName";

	public static IMultiValueConverter ContentDataTemplateConverter { get; } = new LambdaConverter2<ActionHoldControlHolder, VideoOSActionHold.Appearances>(delegate(ActionHoldControlHolder self, VideoOSActionHold.Appearances appearance)
	{
		string resourceKey = string.Format("{0}{1}{2}DataTemplateName", "ActionHoldControlHolder", appearance, self.ControlSize);
		return (DataTemplate)self.FindResource(resourceKey);
	}, "ContentDataTemplateConverter");

	public ActionsPresenter.ControlSizes ControlSize { get; set; }

	public FrameworkElement ActionableElement => this.FindDescendant<Button>();

	public ActionHoldControlHolder()
	{
		InitializeComponent();
	}

	private void ActionControl_OnIsPressedChanged(object sender, RoutedEventArgs e)
	{
		VideoOSButtonBase videoOSButtonBase = (VideoOSButtonBase)sender;
		((VideoOSActionHold)videoOSButtonBase.DataContext).IsHeld = videoOSButtonBase.IsPressed;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new ActionControlHolderAutomationPeer(this);
	}
}
