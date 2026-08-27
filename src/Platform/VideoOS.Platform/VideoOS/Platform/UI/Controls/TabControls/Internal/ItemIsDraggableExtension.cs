using System.Windows.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal;

public class ItemIsDraggableExtension : BindingMarkupExtensionBase<ItemIsDraggableExtension.State, bool>
{
	public class State : StateBase
	{
		public UserControl UserControl { get; set; }

		public VideoOSTabControlBase VideoOsTabControl { get; set; }
	}

	public override void InitState(State state)
	{
		state.UserControl = (UserControl)state.TargetFrameworkElement;
		state.VideoOsTabControl = state.UserControl.FindAncestor<VideoOSTabControlBase>();
	}

	public override void AddPropertyListeners(State state)
	{
		AddPropertyListener(state, "DataContext", state.UserControl, refreshPropertyListenersOnChange: true);
		if (state.UserControl.DataContext is VideoOSTabControlItemBase)
		{
			AddPropertyListener(state, "IsDraggable", state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.UserControl.DataContext != null)
		{
			AddPropertyListener(state, "ItemIsDraggablePropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemIsDraggablePropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemIsDraggablePropertyName, state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override bool GetDefaultValue()
	{
		return true;
	}

	public override bool ResolveValue(State state)
	{
		bool flag = GetDefaultValue();
		if (state.UserControl.DataContext != null)
		{
			flag = ((!(state.UserControl.DataContext is VideoOSTabControlItemBase videoOSTabControlItemBase)) ? state.UserControl.DataContext.ResolvePropertyValue(state.VideoOsTabControl.ItemIsDraggablePropertyName, throwOnError: false, flag) : videoOSTabControlItemBase.IsDraggable);
		}
		return flag;
	}
}
