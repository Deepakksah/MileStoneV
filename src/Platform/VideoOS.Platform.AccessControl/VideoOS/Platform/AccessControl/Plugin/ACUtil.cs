using System;

namespace VideoOS.Platform.AccessControl.Plugin;

public static class ACUtil
{
	public static Version EventServerVersion => InternalAccessControlUtil.Instance.EventServerVersion;

	public static void Log(bool error, string where, string text)
	{
		InternalAccessControlUtil.Instance.Log(error, where, text);
	}
}
