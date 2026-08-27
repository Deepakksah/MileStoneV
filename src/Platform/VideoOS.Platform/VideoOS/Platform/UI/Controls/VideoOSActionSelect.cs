using System;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Data;
using VideoOS.Platform.UI.Converters;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionSelect : VideoOSActionBase
{
	public enum Appearances
	{
		CheckBox,
		ToggleButtonIcon,
		ToggleButtonSwitch
	}

	public enum InMenuSelectedAppearances
	{
		None,
		Marked,
		Highlighted
	}

	private Appearances __appearance = Appearances.ToggleButtonIcon;

	private bool _inMenuStaysOpenOnClick;

	private InMenuSelectedAppearances _menuSelectedAppearance = InMenuSelectedAppearances.Marked;

	private bool __isSelected;

	private static IValueConverter ContextMenuItemCheckedAppearanceConverter { get; } = new LambdaConverter1<InMenuSelectedAppearances>((InMenuSelectedAppearances inMenuSelectedAppearance) => inMenuSelectedAppearance switch
	{
		InMenuSelectedAppearances.None => VideoOSContextMenuItem.CheckedAppearances.None, 
		InMenuSelectedAppearances.Marked => VideoOSContextMenuItem.CheckedAppearances.Marked, 
		InMenuSelectedAppearances.Highlighted => VideoOSContextMenuItem.CheckedAppearances.Highlighted, 
		_ => throw new ArgumentOutOfRangeException("inMenuSelectedAppearance", inMenuSelectedAppearance, null), 
	}, "ContextMenuItemCheckedAppearanceConverter");

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

	public bool InMenuStaysOpenOnClick
	{
		get
		{
			return _inMenuStaysOpenOnClick;
		}
		set
		{
			if (_inMenuStaysOpenOnClick != value)
			{
				_inMenuStaysOpenOnClick = value;
				OnPropertyChanged("InMenuStaysOpenOnClick");
			}
		}
	}

	public InMenuSelectedAppearances InMenuSelectedAppearance
	{
		get
		{
			return _menuSelectedAppearance;
		}
		set
		{
			if (_menuSelectedAppearance != value)
			{
				_menuSelectedAppearance = value;
				OnPropertyChanged("InMenuSelectedAppearance");
			}
		}
	}

	public bool IsSelected
	{
		get
		{
			return __isSelected;
		}
		set
		{
			if (__isSelected != value)
			{
				__isSelected = value;
				OnPropertyChanged("IsSelected");
				this.IsSelectedChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler IsSelectedChanged;

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override object AsContextMenuItem()
	{
		VideoOSContextMenuItem obj = (VideoOSContextMenuItem)base.AsContextMenuItem();
		obj.IsCheckable = true;
		obj.SetBinding(MenuItem.StaysOpenOnClickProperty, new Binding("InMenuStaysOpenOnClick")
		{
			Mode = BindingMode.OneWay,
			Source = this
		});
		obj.SetBinding(VideoOSContextMenuItem.CheckedAppearanceProperty, new Binding("InMenuSelectedAppearance")
		{
			Mode = BindingMode.OneWay,
			Source = this,
			Converter = ContextMenuItemCheckedAppearanceConverter
		});
		obj.SetBinding(MenuItem.IsCheckedProperty, new Binding("IsSelected")
		{
			Mode = BindingMode.TwoWay,
			Source = this
		});
		return obj;
	}
}
