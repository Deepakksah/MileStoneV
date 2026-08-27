using System;

namespace VideoOS.Platform.Live;

public class LiveStatusEventArgs : EventArgs
{
	public StatusFlags CurrentStatusFlags;

	public StatusFlags ChangedStatusFlags;
}
