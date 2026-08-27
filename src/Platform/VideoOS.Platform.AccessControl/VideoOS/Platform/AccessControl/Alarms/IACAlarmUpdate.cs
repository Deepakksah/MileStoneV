using System;

namespace VideoOS.Platform.AccessControl.Alarms;

public interface IACAlarmUpdate
{
	DateTime Timestamp { get; }

	string Operator { get; }

	int? StateId { get; }

	string StateName { get; }

	string Comment { get; }
}
