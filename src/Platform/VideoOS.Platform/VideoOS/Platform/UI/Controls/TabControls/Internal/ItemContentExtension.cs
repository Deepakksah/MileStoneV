using System.Windows.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal;

public class ItemContentExtension : BindingMarkupExtensionBase<ItemContentExtension.State, object>
{
	public class State : StateBase
	{
		public ContentControl ContentControl { get; set; }

		public VideoOSTabControlBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.ContentControl = (ContentControl)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.ContentControl.FindAncestor<VideoOSTabControlBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "SelectedItem", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
		if (state.VideoOsTabControl.SelectedItem is VideoOSTabControlItemBase)
		{
			AddPropertyListener(state, "Data", state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsTabControl.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemDataPropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemDataPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemDataPropertyName, state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override object GetDefaultValue()
	{
		return null;
	}

	public override object ResolveValue(State state)
	{
		object obj = GetDefaultValue();
		if (state.VideoOsTabControl.SelectedItem != null)
		{
			obj = ((!(state.VideoOsTabControl.SelectedItem is VideoOSTabControlItemBase videoOSTabControlItemBase)) ? state.VideoOsTabControl.SelectedItem.ResolvePropertyValue(state.VideoOsTabControl.ItemDataPropertyName, throwOnError: false, obj) : videoOSTabControlItemBase.Data);
			if (obj == null)
			{
				obj = state.VideoOsTabControl.SelectedItem;
			}
		}
		return obj;
	}
}
