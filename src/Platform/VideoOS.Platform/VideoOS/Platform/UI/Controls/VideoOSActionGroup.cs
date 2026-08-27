using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionGroup : VideoOSActionBase
{
	public enum Appearances
	{
		MenuButtonText,
		MenuButtonPrimary,
		MenuButtonSecondary,
		MenuButtonTertiary,
		MenuButtonTransparent
	}

	private Appearances __appearance = Appearances.MenuButtonTransparent;

	private ICollection<VideoOSActionBase> __actions;

	private static IValueConverter ContextMenuItemItemsSourceConverter { get; } = new LambdaConverter1<ICollection<VideoOSActionBase>>(delegate(ICollection<VideoOSActionBase> actions)
	{
		List<object> list = new List<object>();
		if (actions != null)
		{
			foreach (VideoOSActionBase action in actions)
			{
				list.Add(action.AsContextMenuItem());
			}
		}
		return list;
	}, "ContextMenuItemItemsSourceConverter");

	private static IMultiValueConverter ContextMenuItemIsEnabledConverter { get; } = new LambdaConverter2<bool, ICollection<VideoOSActionBase>>((bool isEnabled, ICollection<VideoOSActionBase> actions) => actions != null && actions.Any() && isEnabled, "ContextMenuItemIsEnabledConverter");

	public Appearances Appearance
	{
		get
		{
			return __appearance;
		}
		set
		{
			if (__appearance != value)
			{
				__appearance = value;
				OnPropertyChanged("Appearance");
			}
		}
	}

	public ICollection<VideoOSActionBase> Actions
	{
		get
		{
			return __actions;
		}
		set
		{
			if (value != __actions)
			{
				__actions = value;
				OnPropertyChanged("Actions");
			}
		}
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override object AsContextMenuItem()
	{
		VideoOSContextMenuItem obj = (VideoOSContextMenuItem)base.AsContextMenuItem();
		obj.SetBinding(ItemsControl.ItemsSourceProperty, new Binding("Actions")
		{
			Mode = BindingMode.OneWay,
			Source = this,
			Converter = ContextMenuItemItemsSourceConverter
		});
		obj.SetBinding(binding: new MultiBinding
		{
			Bindings = 
			{
				(BindingBase)new Binding("IsEnabled")
				{
					Mode = BindingMode.OneWay,
					Source = this
				},
				(BindingBase)new Binding("Actions")
				{
					Mode = BindingMode.OneWay,
					Source = this
				}
			},
			Converter = ContextMenuItemIsEnabledConverter
		}, dp: UIElement.IsEnabledProperty);
		return obj;
	}
}
