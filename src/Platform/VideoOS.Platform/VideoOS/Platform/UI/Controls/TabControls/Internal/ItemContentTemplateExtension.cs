using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal;

public class ItemContentTemplateExtension : BindingMarkupExtensionBase<ItemContentTemplateExtension.State, DataTemplate>
{
	public class State : StateBase
	{
		public ContentControl ContentControl { get; set; }

		public VideoOSTabControlBase VideoOsTabControl { get; set; }
	}

	public DataTemplate FallbackTemplate { get; set; }

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
			AddPropertyListener(state, "ContentTemplate", state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
			AddPropertyListener(state, "ContentTemplate", state.VideoOsTabControl, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsTabControl.SelectedItem != null)
		{
			AddPropertyListener(state, "ItemContentTemplatePropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemContentTemplatePropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemContentTemplatePropertyName, state.VideoOsTabControl.SelectedItem, refreshPropertyListenersOnChange: false);
			}
			AddPropertyListener(state, "ContentTemplate", state.VideoOsTabControl, refreshPropertyListenersOnChange: false);
		}
	}

	public override DataTemplate GetDefaultValue()
	{
		return null;
	}

	public override DataTemplate ResolveValue(State state)
	{
		DataTemplate dataTemplate = GetDefaultValue();
		if (state.VideoOsTabControl.SelectedItem != null)
		{
			dataTemplate = ((!(state.VideoOsTabControl.SelectedItem is VideoOSTabControlItemBase videoOSTabControlItemBase)) ? state.VideoOsTabControl.SelectedItem.ResolvePropertyValue(state.VideoOsTabControl.ItemContentTemplatePropertyName, throwOnError: false, dataTemplate) : videoOSTabControlItemBase.ContentTemplate);
			if (dataTemplate == null)
			{
				dataTemplate = state.VideoOsTabControl.ContentTemplate;
			}
			if (dataTemplate == null)
			{
				dataTemplate = FallbackTemplate;
			}
		}
		return dataTemplate;
	}
}
