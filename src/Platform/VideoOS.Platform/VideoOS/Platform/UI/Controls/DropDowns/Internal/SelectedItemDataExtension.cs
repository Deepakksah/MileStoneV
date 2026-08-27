using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Resources;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.DropDowns.Internal;

public class SelectedItemDataExtension : BindingMarkupExtensionBase<SelectedItemDataExtension.State, string>
{
	public class State : StateBase
	{
		public VideoOSTextBlockBase VideoOsTextBlock { get; set; }

		public VideoOSDropDownBase VideoOsDropDown { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOsTextBlock = (VideoOSTextBlockBase)state.TargetFrameworkElement;
		state.VideoOsDropDown = state.VideoOsTextBlock.FindAncestor<VideoOSDropDownBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "SelectedItems", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "PlaceholderText", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "CustomDisplayValue", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
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

	public override string GetDefaultValue()
	{
		return string.Empty;
	}

	public override string ResolveValue(State state)
	{
		string result = state.VideoOsDropDown.PlaceholderText ?? GetDefaultValue();
		if (state.VideoOsDropDown.CustomDisplayValue != null)
		{
			result = state.VideoOsDropDown.CustomDisplayValue;
		}
		else if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Single)
		{
			result = GetItemDisplayValue(state.VideoOsDropDown.SelectedItem, state.VideoOsDropDown.ItemDataPropertyName, state.VideoOsDropDown.PlaceholderText);
		}
		else if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Multiple)
		{
			List<object> list = state.VideoOsDropDown.SelectedItems?.ToList();
			if (list != null && list.Count != 0)
			{
				result = ((list.Count != 1) ? string.Format(CultureInfo.CurrentCulture, Strings.VideoOSDropDown_SelectedItemsValue, list.Count) : GetItemDisplayValue(list[0], state.VideoOsDropDown.ItemDataPropertyName, state.VideoOsDropDown.PlaceholderText));
			}
		}
		return result;
	}

	private static string GetItemDisplayValue(object item, string displayTextMemberPath, string placeHolderText)
	{
		if (item == null)
		{
			return placeHolderText;
		}
		object obj2;
		if (item is VideoOSDropDownItem { Data: var obj } videoOSDropDownItem)
		{
			if (obj == null)
			{
				obj = videoOSDropDownItem;
			}
			obj2 = obj;
		}
		else
		{
			obj2 = item.ResolvePropertyValue(displayTextMemberPath, throwOnError: false, item);
		}
		if (obj2 == null)
		{
			return string.Empty;
		}
		if (obj2 is string result)
		{
			return result;
		}
		return obj2.ToString() ?? string.Empty;
	}
}
