using System;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionActivate : VideoOSActionBase
{
	public enum Appearances
	{
		ButtonPrimary,
		ButtonSecondary,
		ButtonTertiary,
		ButtonTransparent,
		ButtonText,
		ButtonHyperlink,
		ButtonAccent
	}

	private Appearances __appearance = Appearances.ButtonTransparent;

	private bool _inMenuStaysOpenOnClick;

	private bool __isDestructive;

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

	public bool IsDestructive
	{
		get
		{
			return __isDestructive;
		}
		set
		{
			if (__isDestructive != value)
			{
				__isDestructive = value;
				OnPropertyChanged("IsDestructive");
			}
		}
	}

	public event EventHandler Activated;

	public void RaiseActivatedEvent()
	{
		this.Activated?.Invoke(this, EventArgs.Empty);
	}

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override object AsContextMenuItem()
	{
		VideoOSContextMenuItem obj = (VideoOSContextMenuItem)base.AsContextMenuItem();
		obj.SetBinding(VideoOSContextMenuItem.IsDestructiveProperty, new Binding("IsDestructive")
		{
			Mode = BindingMode.OneWay,
			Source = this
		});
		obj.SetBinding(MenuItem.StaysOpenOnClickProperty, new Binding("InMenuStaysOpenOnClick")
		{
			Mode = BindingMode.OneWay,
			Source = this
		});
		obj.Click += ContextMenuItemOnClick;
		return obj;
	}

	private void ContextMenuItemOnClick(object sender, RoutedEventArgs e)
	{
		RaiseActivatedEvent();
	}
}
