using System.Windows;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class ItemTextVisibilityExtension : BindingMarkupExtensionBase<ItemTextVisibilityExtension.State, Visibility>
{
	public class State : StateBase
	{
		public VideoOSTextBlockBase VideoOsTextBlock { get; set; }

		public VideoOSTabControlPrimaryBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOsTextBlock = (VideoOSTextBlockBase)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.VideoOsTextBlock.FindAncestor<VideoOSTabControlPrimaryBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.VideoOsTextBlock, refreshPropertyListenersOnChange: true);
		if (state.VideoOsTextBlock.DataContext is VideoOSTabControlPrimaryItem)
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

	public override Visibility GetDefaultValue()
	{
		return Visibility.Collapsed;
	}

	public override Visibility ResolveValue(State state)
	{
		Visibility result = GetDefaultValue();
		if (state.VideoOsTextBlock.DataContext != null)
		{
			string value = ((!(state.VideoOsTextBlock.DataContext is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOsTextBlock.DataContext.ResolvePropertyValue<string>(state.VideoOsTabControl.ItemTextPropertyName, throwOnError: false) : videoOSTabControlPrimaryItem.Text);
			if (!string.IsNullOrWhiteSpace(value))
			{
				result = Visibility.Visible;
			}
		}
		return result;
	}
}
