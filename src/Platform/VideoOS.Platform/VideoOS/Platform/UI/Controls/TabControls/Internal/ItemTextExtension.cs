using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal;

public class ItemTextExtension : BindingMarkupExtensionBase<ItemTextExtension.State, string>
{
	public class State : StateBase
	{
		public VideoOSTextBlockBase VideoOsTextBlock { get; set; }

		public VideoOSTabControlBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOsTextBlock = (VideoOSTextBlockBase)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.VideoOsTextBlock.FindAncestor<VideoOSTabControlBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.VideoOsTextBlock, refreshPropertyListenersOnChange: true);
		if (state.VideoOsTextBlock.DataContext is VideoOSTabControlItemBase)
		{
			AddPropertyListener(state, "Text", state.VideoOsTextBlock.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOsTextBlock.DataContext != null)
		{
			AddPropertyListener(state, "ItemTextPropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemTextPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemTextPropertyName, state.VideoOsTextBlock.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override string GetDefaultValue()
	{
		return null;
	}

	public override string ResolveValue(State state)
	{
		string text = GetDefaultValue();
		if (state.VideoOsTextBlock.DataContext != null)
		{
			text = ((!(state.VideoOsTextBlock.DataContext is VideoOSTabControlItemBase videoOSTabControlItemBase)) ? state.VideoOsTextBlock.DataContext.ResolvePropertyValue(state.VideoOsTabControl.ItemTextPropertyName, throwOnError: false, text) : videoOSTabControlItemBase.Text);
		}
		return text;
	}
}
