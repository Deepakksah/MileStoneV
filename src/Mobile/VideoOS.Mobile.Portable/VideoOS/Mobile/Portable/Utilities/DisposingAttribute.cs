using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class DisposingAttribute : Attribute
{
	public bool ToBeDisposed { get; set; }

	public DisposingAttribute(bool toBeDisposed)
	{
		ToBeDisposed = toBeDisposed;
	}
}
