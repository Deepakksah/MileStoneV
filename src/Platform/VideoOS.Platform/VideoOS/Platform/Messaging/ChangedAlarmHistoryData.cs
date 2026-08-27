using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class ChangedAlarmHistoryData
{
	public Guid AlarmId;

	public string Author;

	public DateTime Time;

	public string AlarmUpdateKey;

	public string AlarmUpdateValue;
}
