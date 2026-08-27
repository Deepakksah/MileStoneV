using System;
using VideoOS.Platform.AccessControl.Alarms;

namespace VideoOS.Platform.AccessControl.Plugin;

public abstract class InternalAccessControlUtil
{
	public static InternalAccessControlUtil Instance { get; set; }

	public abstract Version EventServerVersion { get; }

	public abstract void Log(bool error, string where, string text);

	public virtual IACAlarmRepository CreateACAlarmRepository()
	{
		return null;
	}
}
