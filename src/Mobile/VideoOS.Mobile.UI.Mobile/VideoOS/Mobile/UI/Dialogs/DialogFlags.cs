using System;

namespace VideoOS.Mobile.UI.Dialogs;

[Flags]
public enum DialogFlags
{
	None = 0,
	Modal = 1,
	CanResize = 2,
	NoTransparency = 4
}
