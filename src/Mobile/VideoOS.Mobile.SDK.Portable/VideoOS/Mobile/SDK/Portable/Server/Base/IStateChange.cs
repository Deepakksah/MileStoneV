using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IStateChange
{
	event EventHandler OnStateChange;

	EventArgs GetState();
}
