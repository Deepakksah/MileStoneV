using System;

namespace VideoOS.Installer.Common.Utilities;

public class ErrorOccuredEventArgs : EventArgs
{
	public Exception Exception { get; set; }

	public ErrorOccuredEventArgs(Exception ex)
	{
		Exception = ex;
	}
}
