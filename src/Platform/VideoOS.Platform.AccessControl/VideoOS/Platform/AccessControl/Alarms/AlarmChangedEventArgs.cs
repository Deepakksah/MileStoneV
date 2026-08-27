using System;

namespace VideoOS.Platform.AccessControl.Alarms;

public sealed class AlarmChangedEventArgs : EventArgs
{
	public IACAlarm Alarm { get; private set; }

	public AlarmChangedEventArgs(IACAlarm alarm)
	{
		Alarm = alarm;
	}
}
