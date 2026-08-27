using System;
using System.Runtime.CompilerServices;
using System.Windows;

namespace VideoOS.Platform.UI.Controls;

public sealed class VideoOSActionHold : VideoOSActionBase
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

	private bool __isHeld;

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

	public bool IsHeld
	{
		get
		{
			return __isHeld;
		}
		public set
		{
			if (__isHeld != value)
			{
				__isHeld = value;
				OnPropertyChanged("IsHeld");
				this.IsHeldChanged?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public event EventHandler IsHeldChanged;

	[MethodImpl(MethodImplOptions.Synchronized)]
	public override object AsContextMenuItem()
	{
		VideoOSContextMenuItem obj = (VideoOSContextMenuItem)base.AsContextMenuItem();
		obj.StaysOpenOnClick = true;
		obj.IsPressedByMouseOrKeyboardChanged += ContextMenuItemOnIsPressedByMouseOrKeyboardChanged;
		return obj;
	}

	private void ContextMenuItemOnIsPressedByMouseOrKeyboardChanged(object sender, RoutedEventArgs e)
	{
		VideoOSContextMenuItem videoOSContextMenuItem = (VideoOSContextMenuItem)sender;
		IsHeld = videoOSContextMenuItem.IsPressedByMouseOrKeyboard;
	}
}
