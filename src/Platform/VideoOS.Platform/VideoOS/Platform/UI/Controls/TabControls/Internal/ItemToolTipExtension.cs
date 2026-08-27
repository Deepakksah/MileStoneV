using System.Windows.Controls;
using VideoOS.Platform.UI.Extensions;
using VideoOS.Platform.UI.Utils;

namespace VideoOS.Platform.UI.Controls.TabControls.Internal;

public class ItemToolTipExtension : BindingMarkupExtensionBase<ItemToolTipExtension.State, string>
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
			AddPropertyListener(state, "Text", state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
			AddPropertyListener(state, "ToolTip", state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
		}
		else if (state.UserControl.DataContext != null)
		{
			AddPropertyListener(state, "ItemTextPropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemTextPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemTextPropertyName, state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
			}
			AddPropertyListener(state, "ItemToolTipPropertyName", state.VideoOsTabControl, refreshPropertyListenersOnChange: true);
			if (!string.IsNullOrWhiteSpace(state.VideoOsTabControl.ItemToolTipPropertyName))
			{
				AddPropertyListener(state, state.VideoOsTabControl.ItemToolTipPropertyName, state.UserControl.DataContext, refreshPropertyListenersOnChange: false);
			}
		}
	}

	public override string GetDefaultValue()
	{
		return null;
	}

	public override string ResolveValue(State state)
	{
		string result = GetDefaultValue();
		if (state.UserControl.DataContext != null)
		{
			string objA;
			string text;
			if (state.UserControl.DataContext is VideoOSTabControlItemBase videoOSTabControlItemBase)
			{
				objA = videoOSTabControlItemBase.Text;
				text = videoOSTabControlItemBase.ToolTip;
			}
			else
			{
				objA = state.UserControl.DataContext.ResolvePropertyValue<string>(state.VideoOsTabControl.ItemTextPropertyName, throwOnError: false);
				text = state.UserControl.DataContext.ResolvePropertyValue<string>(state.VideoOsTabControl.ItemToolTipPropertyName, throwOnError: false);
			}
			result = (object.Equals(objA, text) ? null : text);
		}
		return result;
	}
}
