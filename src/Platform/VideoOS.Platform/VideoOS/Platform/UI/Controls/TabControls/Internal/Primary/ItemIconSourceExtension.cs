using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class ItemIconSourceExtension : BindingMarkupExtensionBase<ItemIconSourceExtension.State, VideoOSIconSourceBase>
{
	public class State : StateBase
	{
		public VideoOSIcon VideoOSIcon { get; set; }

		public VideoOSTabControlPrimaryBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOSIcon = (VideoOSIcon)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.VideoOSIcon.FindAncestor<VideoOSTabControlPrimaryBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.VideoOSIcon, refreshPropertyListenersOnChange: true);
		if (state.VideoOSIcon.DataContext is VideoOSTabControlPrimaryItem)
		{
			AddPropertyListener(state, "IconSource", state.VideoOSIcon.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOSIcon.DataContext != null)
		{
			AddPropertyListener(state, "ItemIconSourcePropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemIconSourcePropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemIconSourcePropertyName, state.VideoOSIcon.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override VideoOSIconSourceBase GetDefaultValue()
	{
		return null;
	}

	public override VideoOSIconSourceBase ResolveValue(State state)
	{
		VideoOSIconSourceBase videoOSIconSourceBase = GetDefaultValue();
		if (state.VideoOSIcon.DataContext != null)
		{
			videoOSIconSourceBase = ((!(state.VideoOSIcon.DataContext is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOSIcon.DataContext.ResolvePropertyValue(state.VideoOsTabControl.ItemIconSourcePropertyName, throwOnError: false, videoOSIconSourceBase) : videoOSTabControlPrimaryItem.IconSource);
		}
		return videoOSIconSourceBase;
	}
}
