using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionActivateControlHolder : ContentControl, IActionControlHolder, IComponentConnector
{
	public static string ButtonPrimaryExtraSmallDataTemplateName = "ActionActivateControlHolderButtonPrimaryExtraSmallDataTemplateName";

	public static string ButtonPrimarySmallDataTemplateName = "ActionActivateControlHolderButtonPrimarySmallDataTemplateName";

	public static string ButtonPrimaryMediumDataTemplateName = "ActionActivateControlHolderButtonPrimaryMediumDataTemplateName";

	public static string ButtonSecondaryExtraSmallDataTemplateName = "ActionActivateControlHolderButtonSecondaryExtraSmallDataTemplateName";

	public static string ButtonSecondarySmallDataTemplateName = "ActionActivateControlHolderButtonSecondarySmallDataTemplateName";

	public static string ButtonSecondaryMediumDataTemplateName = "ActionActivateControlHolderButtonSecondaryMediumDataTemplateName";

	public static string ButtonTertiaryExtraSmallDataTemplateName = "ActionActivateControlHolderButtonTertiaryExtraSmallDataTemplateName";

	public static string ButtonTertiarySmallDataTemplateName = "ActionActivateControlHolderButtonTertiarySmallDataTemplateName";

	public static string ButtonTertiaryMediumDataTemplateName = "ActionActivateControlHolderButtonTertiaryMediumDataTemplateName";

	public static string ButtonTransparentExtraSmallDataTemplateName = "ActionActivateControlHolderButtonTransparentExtraSmallDataTemplateName";

	public static string ButtonTransparentSmallDataTemplateName = "ActionActivateControlHolderButtonTransparentSmallDataTemplateName";

	public static string ButtonTransparentMediumDataTemplateName = "ActionActivateControlHolderButtonTransparentMediumDataTemplateName";

	public static string ButtonTextExtraSmallDataTemplateName = "ActionActivateControlHolderButtonTextExtraSmallDataTemplateName";

	public static string ButtonTextSmallDataTemplateName = "ActionActivateControlHolderButtonTextSmallDataTemplateName";

	public static string ButtonTextMediumDataTemplateName = "ActionActivateControlHolderButtonTextMediumDataTemplateName";

	public static string ButtonHyperlinkExtraSmallDataTemplateName = "ActionActivateControlHolderButtonHyperlinkExtraSmallDataTemplateName";

	public static string ButtonHyperlinkSmallDataTemplateName = "ActionActivateControlHolderButtonHyperlinkSmallDataTemplateName";

	public static string ButtonHyperlinkMediumDataTemplateName = "ActionActivateControlHolderButtonHyperlinkMediumDataTemplateName";

	public static string ButtonAccentExtraSmallDataTemplateName = "ActionActivateControlHolderButtonAccentExtraSmallDataTemplateName";

	public static string ButtonAccentSmallDataTemplateName = "ActionActivateControlHolderButtonAccentSmallDataTemplateName";

	public static string ButtonAccentMediumDataTemplateName = "ActionActivateControlHolderButtonAccentMediumDataTemplateName";

	public static IMultiValueConverter ContentDataTemplateConverter { get; } = new LambdaConverter2<ActionActivateControlHolder, VideoOSActionActivate.Appearances>(delegate(ActionActivateControlHolder self, VideoOSActionActivate.Appearances appearance)
	{
		string resourceKey = string.Format("{0}{1}{2}DataTemplateName", "ActionActivateControlHolder", appearance, self.ControlSize);
		return (DataTemplate)self.FindResource(resourceKey);
	}, "ContentDataTemplateConverter");

	public ActionsPresenter.ControlSizes ControlSize { get; set; }

	public FrameworkElement ActionableElement => this.FindDescendant<Button>();

	public ActionActivateControlHolder()
	{
		InitializeComponent();
	}

	private void ActionControlActivated(object sender, RoutedEventArgs e)
	{
		((VideoOSActionActivate)((VideoOSButtonBase)sender).DataContext).RaiseActivatedEvent();
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new ActionControlHolderAutomationPeer(this);
	}
}
