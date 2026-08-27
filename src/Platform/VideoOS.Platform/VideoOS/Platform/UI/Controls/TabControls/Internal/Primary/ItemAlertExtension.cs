using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class ItemAlertExtension : BindingMarkupExtensionBase<ItemAlertExtension.State, VideoOSAlert>
{
	public class State : StateBase
	{
		public VideoOSBadgeBase VideoOSBadge { get; set; }

		public VideoOSTabControlPrimaryBase VideoOsTabControlPrimary { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOSBadge = (VideoOSBadgeBase)state.TargetFrameworkElement;
		state.VideoOsTabControlPrimary = state.VideoOSBadge.FindAncestor<VideoOSTabControlPrimaryBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.VideoOSBadge, refreshPropertyListenersOnChange: true);
		if (state.VideoOSBadge.DataContext is VideoOSTabControlPrimaryItem)
		{
			AddPropertyListener(state, "Alert", state.VideoOSBadge.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOSBadge.DataContext != null)
		{
			AddPropertyListener(state, "ItemAlertPropertyName", state.VideoOsTabControlPrimary, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControlPrimary.ItemAlertPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControlPrimary.ItemAlertPropertyName, state.VideoOSBadge.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override VideoOSAlert GetDefaultValue()
	{
		return null;
	}

	public override VideoOSAlert ResolveValue(State state)
	{
		VideoOSAlert videoOSAlert = GetDefaultValue();
		if (state.VideoOSBadge.DataContext != null)
		{
			videoOSAlert = ((!(state.VideoOSBadge.DataContext is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOSBadge.DataContext.ResolvePropertyValue(state.VideoOsTabControlPrimary.ItemAlertPropertyName, throwOnError: false, videoOSAlert) : videoOSTabControlPrimaryItem.Alert);
		}
		return videoOSAlert;
	}
}
