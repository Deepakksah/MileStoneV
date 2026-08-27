using System;

namespace VideoOS.Mobile.Portable.Common;

public class MobileSdkNotInitializedException : Exception
{
	public MobileSdkNotInitializedException()
		: base("Mobile SDK is not initialized! " + Environment.NewLine + "Try calling VideoOS.Mobile.SDK.Environment.Instance.Initialize() first!")
	{
	}
}
