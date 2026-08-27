using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class OverflowMenuAlertIndicatorIconExtension : BindingMarkupExtensionBase<OverflowMenuAlertIndicatorIconExtension.State, VideoOSIconSourceBase>
{
	public class State : StateBase
	{
		public VideoOSIcon Icon { get; set; }

		public HeaderItemOverflowMenu HeaderItemOverflowMenu { get; set; }

		public VideoOSTabControlPrimaryBase VideoOsTabControl { get; set; }
	}

	private static VideoOSIconBuiltInSource NormalIcon = new VideoOSIconBuiltInSource
	{
		Icon = VideoOSIconBuiltInSource.Icons.Chevron_Double_Down
	};

	private static VideoOSIconBuiltInSource AlertIcon = new VideoOSIconBuiltInSource
	{
		Icon = VideoOSIconBuiltInSource.Icons.Chevron_Double_Down_With_Badge_Combined
	};

	public override void InitState(State state)
	{
		state.Icon = (VideoOSIcon)state.TargetFrameworkElement;
		state.HeaderItemOverflowMenu = state.TargetFrameworkElement.FindAncestor<HeaderItemOverflowMenu>();
		state.VideoOsTabControl = state.TargetFrameworkElement.FindAncestor<VideoOSTabControlPrimaryBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "Items", state.HeaderItemOverflowMenu, refreshPropertyListenersOnChange: true);
		if (state.HeaderItemOverflowMenu.Items == null)
		{
			return;
		}
		foreach (object item in state.HeaderItemOverflowMenu.Items)
		{
			if (item is VideoOSTabControlPrimaryItem propertyOwner)
			{
				AddPropertyListener(state, "Alert", propertyOwner, refreshPropertyListenersOnChange: false);
			}
			else
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemAlertPropertyName, item, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override VideoOSIconSourceBase GetDefaultValue()
	{
		return NormalIcon;
	}

	public override VideoOSIconSourceBase ResolveValue(State state)
	{
		VideoOSIconSourceBase result = GetDefaultValue();
		if (state.HeaderItemOverflowMenu.Items != null)
		{
			foreach (object item in state.HeaderItemOverflowMenu.Items)
			{
				if (item is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)
				{
					if (videoOSTabControlPrimaryItem.Alert != null)
					{
						result = AlertIcon;
						break;
					}
				}
				else if (item.ResolvePropertyValue<VideoOSAlert>(state.VideoOsTabControl.ItemAlertPropertyName, throwOnError: false) != null)
				{
					result = AlertIcon;
					break;
				}
			}
		}
		return result;
	}
}
