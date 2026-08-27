using System.Collections.Generic;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal.Primary;

public class ItemActionsExtension : BindingMarkupExtensionBase<ItemActionsExtension.State, ICollection<VideoOSActionBase>>
{
	public class State : StateBase
	{
		public VideoOSActionListHorizontalSmall VideoOSActionListHorizontalSmall { get; set; }

		public VideoOSTabControlPrimaryBase VideoOsTabControlPrimary { get; set; }
	}

	public override void InitState(State state)
	{
		state.VideoOSActionListHorizontalSmall = (VideoOSActionListHorizontalSmall)state.TargetFrameworkElement;
		state.VideoOsTabControlPrimary = state.VideoOSActionListHorizontalSmall.FindAncestor<VideoOSTabControlPrimaryBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.VideoOSActionListHorizontalSmall, refreshPropertyListenersOnChange: true);
		if (state.VideoOSActionListHorizontalSmall.DataContext is VideoOSTabControlPrimaryItem)
		{
			AddPropertyListener(state, "Actions", state.VideoOSActionListHorizontalSmall.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.VideoOSActionListHorizontalSmall.DataContext != null)
		{
			AddPropertyListener(state, "ItemActionsPropertyName", state.VideoOsTabControlPrimary, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControlPrimary.ItemActionsPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControlPrimary.ItemActionsPropertyName, state.VideoOSActionListHorizontalSmall.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override ICollection<VideoOSActionBase> GetDefaultValue()
	{
		return null;
	}

	public override ICollection<VideoOSActionBase> ResolveValue(State state)
	{
		ICollection<VideoOSActionBase> collection = GetDefaultValue();
		if (state.VideoOSActionListHorizontalSmall.DataContext != null)
		{
			collection = ((!(state.VideoOSActionListHorizontalSmall.DataContext is VideoOSTabControlPrimaryItem videoOSTabControlPrimaryItem)) ? state.VideoOSActionListHorizontalSmall.DataContext.ResolvePropertyValue(state.VideoOsTabControlPrimary.ItemActionsPropertyName, throwOnError: false, collection) : videoOSTabControlPrimaryItem.Actions);
		}
		return collection;
	}
}
