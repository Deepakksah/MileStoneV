using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Resources;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class OverflowMenuTooltipExtension : BindingMarkupExtensionBase<OverflowMenuTooltipExtension.State, string>
{
	public class State : StateBase
	{
		public HeaderItemOverflowMenu HeaderItemOverflowMenu { get; set; }

		public VideoOSTabControlBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.HeaderItemOverflowMenu = (HeaderItemOverflowMenu)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.HeaderItemOverflowMenu.FindAncestor<VideoOSTabControlBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "Items", state.HeaderItemOverflowMenu, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "SelectedItem", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
		if (state.VideoOsTabControl.SelectedItem is VideoOSTabControlPrimaryItem)
		{
			AddPropertyListener(state, "Text", state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsTabControl.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemTextPropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemTextPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemTextPropertyName, state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override string GetDefaultValue()
	{
		return Strings.VideoOSTabControl_OverflowButtonTooltip;
	}

	public override string ResolveValue(State state)
	{
		string text = GetDefaultValue();
		if (state.HeaderItemOverflowMenu.Items != null && state.HeaderItemOverflowMenu.Items.Contains(state.VideoOsTabControl.SelectedItem))
		{
			text = ((!(state.VideoOsTabControl.SelectedItem is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOsTabControl.SelectedItem.ResolvePropertyValue(state.VideoOsTabControl.ItemTextPropertyName, throwOnError: false, text) : videoOSTabControlPrimaryItem.Text);
		}
		return text;
	}
}
