using System;

namespace VideoOS.Server.Admin.Utilities;

public class SslStatusEventArgs : EventArgs
{
	public SslStatusStep Step { get; }

	public SslStatusEventArgs(SslStatusStep step)
	{
		Step = step;
	}
}
