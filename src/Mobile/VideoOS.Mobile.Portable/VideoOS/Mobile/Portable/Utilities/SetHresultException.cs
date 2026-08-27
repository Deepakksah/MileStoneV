using System;

namespace VideoOS.Mobile.Portable.Utilities;

public class SetHresultException : Exception
{
	public SetHresultException(int hresult)
	{
		base.HResult = hresult;
	}
}
