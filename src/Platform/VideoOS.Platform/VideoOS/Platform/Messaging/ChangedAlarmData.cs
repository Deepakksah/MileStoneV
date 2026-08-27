using System;

namespace VideoOS.Platform.Messaging;

[Serializable]
public class ChangedAlarmData
{
	public Guid AlarmId;

	public Guid SourceId;

	public DateTime AlarmsDeletedBefore;

	public DateTime AlarmsDeletedBeforeClosed;

	public string ChangeHint;

	public const string ChangeHintSingleAlarm = "SingleAlarm";

	public const string ChangeHintMultipleAlarmsDeleted = "MultipleAlarmsDeleted";

	public const string ChangeHintMassUpdateForSource = "MassUpdateForSource";
}
