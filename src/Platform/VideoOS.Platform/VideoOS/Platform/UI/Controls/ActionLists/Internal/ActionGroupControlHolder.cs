using System;
using System.Windows;
using System.Windows.Automation.Peers;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using VideoOS.Platform.UI.Automation.AutomationPeers;
using VideoOS.Platform.UI.Commands;
using VideoOS.Platform.UI.Converters;
using VideoOS.Platform.UI.Extensions;

namespace VideoOS.Platform.UI.Controls.ActionLists.Internal;

public partial class ActionGroupControlHolder : ContentControl, IActionControlHolder, IComponentConnector
{
	public static string MenuButtonIconExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonIconExtraSmallDataTemplateName";

	public static string MenuButtonIconSmallDataTemplateName = "ActionGroupControlHolderMenuButtonIconSmallDataTemplateName";

	public static string MenuButtonIconMediumDataTemplateName = "ActionGroupControlHolderMenuButtonIconMediumDataTemplateName";

	public static string MenuButtonTextExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonTextExtraSmallDataTemplateName";

	public static string MenuButtonTextSmallDataTemplateName = "ActionGroupControlHolderMenuButtonTextSmallDataTemplateName";

	public static string MenuButtonTextMediumDataTemplateName = "ActionGroupControlHolderMenuButtonTextMediumDataTemplateName";

	public static string MenuButtonPrimaryExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonPrimaryExtraSmallDataTemplateName";

	public static string MenuButtonPrimarySmallDataTemplateName = "ActionGroupControlHolderMenuButtonPrimarySmallDataTemplateName";

	public static string MenuButtonPrimaryMediumDataTemplateName = "ActionGroupControlHolderMenuButtonPrimaryMediumDataTemplateName";

	public static string MenuButtonSecondaryExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonSecondaryExtraSmallDataTemplateName";

	public static string MenuButtonSecondarySmallDataTemplateName = "ActionGroupControlHolderMenuButtonSecondarySmallDataTemplateName";

	public static string MenuButtonSecondaryMediumDataTemplateName = "ActionGroupControlHolderMenuButtonSecondaryMediumDataTemplateName";

	public static string MenuButtonTertiaryExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonTertiaryExtraSmallDataTemplateName";

	public static string MenuButtonTertiarySmallDataTemplateName = "ActionGroupControlHolderMenuButtonTertiarySmallDataTemplateName";

	public static string MenuButtonTertiaryMediumDataTemplateName = "ActionGroupControlHolderMenuButtonTertiaryMediumDataTemplateName";

	public static string MenuButtonTransparentExtraSmallDataTemplateName = "ActionGroupControlHolderMenuButtonTransparentExtraSmallDataTemplateName";

	public static string MenuButtonTransparentSmallDataTemplateName = "ActionGroupControlHolderMenuButtonTransparentSmallDataTemplateName";

	public static string MenuButtonTransparentMediumDataTemplateName = "ActionGroupControlHolderMenuButtonTransparentMediumDataTemplateName";

	public static readonly DependencyProperty MenuButtonOpenedClosedCommandProperty = DependencyProperty.Register("MenuButtonOpenedClosedCommand", typeof(DelegateCommand), typeof(ActionGroupControlHolder), new PropertyMetadata(null));

	public bool __isOpen;

	public static IMultiValueConverter ContentDataTemplateConverter { get; } = new LambdaConverter2<ActionGroupControlHolder, VideoOSActionGroup.Appearances>(delegate(ActionGroupControlHolder self, VideoOSActionGroup.Appearances appearance)
	{
		string resourceKey = string.Format("{0}{1}{2}DataTemplateName", "ActionGroupControlHolder", appearance, self.ControlSize);
		return (DataTemplate)self.FindResource(resourceKey);
	}, "ContentDataTemplateConverter");

	public ActionsPresenter.ControlSizes ControlSize { get; set; }

	public DelegateCommand MenuButtonOpenedClosedCommand
	{
		get
		{
			return (DelegateCommand)GetValue(MenuButtonOpenedClosedCommandProperty);
		}
		private set
		{
			SetValue(MenuButtonOpenedClosedCommandProperty, value);
		}
	}

	public bool IsOpen
	{
		get
		{
			return __isOpen;
		}
		set
		{
			if (__isOpen != value)
			{
				__isOpen = value;
				this.IsOpenChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public FrameworkElement ActionableElement => this.FindDescendant<Button>();

	public event EventHandler IsOpenChanged;

	public ActionGroupControlHolder()
	{
		InitializeComponent();
		MenuButtonOpenedClosedCommand = new DelegateCommand(MenuButtonOpenedClosedCommandFunction);
	}

	private void MenuButtonOpenedClosedCommandFunction(object args)
	{
		EventsToCommandMappingArgs eventsToCommandMappingArgs = (EventsToCommandMappingArgs)args;
		IsOpen = ((VideoOSMenuButtonBase)eventsToCommandMappingArgs.Sender).IsOpen;
	}

	protected override AutomationPeer OnCreateAutomationPeer()
	{
		return new ActionControlHolderAutomationPeer(this);
	}
}
