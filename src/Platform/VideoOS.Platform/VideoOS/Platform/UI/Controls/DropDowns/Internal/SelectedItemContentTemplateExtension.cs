using System.Collections.Generic;
using System.Linq;
using System.Windows;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.DropDowns.Internal;

public class SelectedItemContentTemplateExtension : BindingMarkupExtensionBase<SelectedItemContentTemplateExtension.State, DataTemplate>
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
		AddPropertyListener(state, "SelectedItems", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "ContentTemplate", state.VideoOsDropDown, refreshPropertyListenersOnChange: false);
		AddPropertyListener(state, "SelectedItem", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
		if (state.VideoOsDropDown.SelectedItem is VideoOSDropDownItem)
		{
			AddPropertyListener(state, "ContentTemplate", state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsDropDown.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemContentTemplatePropertyName", state.VideoOsDropDown, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsDropDown.ItemContentTemplatePropertyName))
			{
				AddPropertyListener(state, state.VideoOsDropDown.ItemContentTemplatePropertyName, state.VideoOsDropDown.SelectedItem, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override DataTemplate GetDefaultValue()
	{
		return null;
	}

	public override DataTemplate ResolveValue(State state)
	{
		if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Single)
		{
			return GetItemContentTemplate(state.VideoOsDropDown.SelectedItem, state.VideoOsDropDown.ItemContentTemplatePropertyName) ?? state.VideoOsDropDown.ContentTemplate;
		}
		if (state.VideoOsDropDown.SelectionMode == VideoOSDropDownBase.SelectionModes.Multiple)
		{
			List<object> list = state.VideoOsDropDown.SelectedItems?.ToList();
			if (list != null && list.Count == 1)
			{
				return GetItemContentTemplate(list[0], state.VideoOsDropDown.ItemContentTemplatePropertyName) ?? state.VideoOsDropDown.ContentTemplate;
			}
		}
		return null;
	}

	private static DataTemplate GetItemContentTemplate(object item, string itemContentTemplatePropertyName)
	{
		if (item is VideoOSDropDownItem videoOSDropDownItem)
		{
			return videoOSDropDownItem.ContentTemplate;
		}
		return item.ResolvePropertyValue<DataTemplate>(itemContentTemplatePropertyName, throwOnError: false);
	}
}
