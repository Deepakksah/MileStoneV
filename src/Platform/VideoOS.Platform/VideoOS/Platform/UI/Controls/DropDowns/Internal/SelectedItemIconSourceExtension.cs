using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.DropDowns.Internal;

public class SelectedItemIconSourceExtension : BindingMarkupExtensionBase<SelectedItemIconSourceExtension.State, VideoOSIconSourceBase>
{
	public class State : StateBase
	{
		public VideoOSIcon VideoOsIcon { get; set; }

		public VideoOSDropDownBase VideoOsDropDown { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOsIcon = (VideoOSIcon)state.TargetFrameworkElement;
		state.VideoOsDropDown = state.VideoOsIcon.FindAncestor<VideoOSDropDownBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "SelectedItems", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "PlaceholderText", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "CustomDisplayValue", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "SelectedItem", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
		if (state.VideoOsDropDown.SelectedItem is VideoOSDropDownItem)
		{
			AddPropertyListener(state, "IconSource", state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsDropDown.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemIconSourcePropertyName", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsDropDown.ItemIconSourcePropertyName))
			{
				AddPropertyListener(state, state.VideoOsDropDown.ItemIconSourcePropertyName, state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override VideoOSIconSourceBase GetDefaultValue()
	{
		return null;
	}

	public override VideoOSIconSourceBase ResolveValue(State state)
	{
		VideoOSIconSourceBase result = GetDefaultValue();
		if (state.VideoOsDropDown.CustomDisplayValue == null)
		{
			if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Single)
			{
				result = GetItemIconSource(state.VideoOsDropDown.SelectedItem, state.VideoOsDropDown.ItemIconSourcePropertyName);
			}
			else if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Multiple)
			{
				List<object> list = state.VideoOsDropDown.SelectedItems?.ToList();
				if (list != null && list.Count == 1)
				{
					result = GetItemIconSource(list[0], state.VideoOsDropDown.ItemIconSourcePropertyName);
				}
			}
		}
		return result;
	}

	private static VideoOSIconSourceBase GetItemIconSource(object item, string iconSourceMemberPath)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.IconSource;
		}
		return item.ResolvePropertyValue<VideoOSIconSourceBase>(iconSourceMemberPath, throwOnError: false);
	}
}
