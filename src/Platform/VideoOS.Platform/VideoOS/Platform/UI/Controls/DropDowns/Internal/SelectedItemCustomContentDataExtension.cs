using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.DropDowns.Internal;

public class SelectedItemCustomContentDataExtension : BindingMarkupExtensionBase<SelectedItemCustomContentDataExtension.State, object>
{
	public class State : StateBase
	{
		public VideoOSDropDownBase VideoOsDropDown { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOsDropDown = state.TargetFrameworkElement.FindAncestor<VideoOSDropDownBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "SelectedItem", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
		if (state.VideoOsDropDown.SelectedItem is VideoOSDropDownItem)
		{
			AddPropertyListener(state, "Data", state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsDropDown.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemDataPropertyName", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsDropDown.ItemDataPropertyName))
			{
				AddPropertyListener(state, state.VideoOsDropDown.ItemDataPropertyName, state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override object GetDefaultValue()
	{
		return null;
	}

	public override object ResolveValue(State state)
	{
		object result = GetDefaultValue();
		if (state.VideoOsDropDown.SelectedItem != null)
		{
			result = GetItemData(state.VideoOsDropDown.SelectedItem, state.VideoOsDropDown.ItemDataPropertyName);
		}
		return result;
	}

	private static object GetItemData(object item, string itemDataPropertyName)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.Data;
		}
		return item.ResolvePropertyValue(itemDataPropertyName, throwOnError: false, item);
	}
}
