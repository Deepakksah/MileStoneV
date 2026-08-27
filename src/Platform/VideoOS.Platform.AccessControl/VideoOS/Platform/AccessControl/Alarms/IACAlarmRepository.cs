using System;
using System.Collections.Generic;

namespace VideoOS.Platform.AccessControl.Alarms;

public interface IACAlarmRepository
{
	event EventHandler<AlarmChangedEventArgs> AlarmChanged;

	void UpdateAlarm(Guid alarmId, ACAlarmUpdateRequest updateRequest);

	IEnumerable<IACAlarmData> GetAlarmsForSource(string sourceId, string eventTypeId, bool includeClosedAlarms);

	IEnumerable<IACAlarmData> GetAlarmsForEvent(string eventId, bool includeClosedAlarms);
}
