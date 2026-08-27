using System;

namespace VideoOS.Platform.AccessControl.Alarms;

public interface IACAlarm
{
	Guid Id { get; }

	string ExternalEventId { get; }

	string ExternalEventTypeId { get; }

	string ExternalSourceId { get; }

	int StateId { get; }

	string StateName { get; }

	IACAlarmUpdate Update { get; }
}
