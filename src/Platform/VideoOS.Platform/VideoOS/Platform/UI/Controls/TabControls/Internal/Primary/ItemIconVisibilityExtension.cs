using System.Windows;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class ItemIconVisibilityExtension : BindingMarkupExtensionBase<ItemIconVisibilityExtension.State, Visibility>
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

	public override Visibility GetDefaultValue()
	{
		return Visibility.Collapsed;
	}

	public override Visibility ResolveValue(State state)
	{
		Visibility result = GetDefaultValue();
		if (state.VideoOSIcon.DataContext != null)
		{
			VideoOSIconSourceBase videoOSIconSourceBase = ((!(state.VideoOSIcon.DataContext is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOSIcon.DataContext.ResolvePropertyValue<VideoOSIconSourceBase>(state.VideoOsTabControl.ItemIconSourcePropertyName, throwOnError: false) : videoOSTabControlPrimaryItem.IconSource);
			if (videoOSIconSourceBase != null)
			{
				result = Visibility.Visible;
			}
		}
		return result;
	}
}
