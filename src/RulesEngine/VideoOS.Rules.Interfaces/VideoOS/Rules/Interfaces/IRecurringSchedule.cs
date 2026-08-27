using System;

namespace VideoOS.Rules.Interfaces;

public interface IRecurringSchedule
{
	DateTime CreationTime { get; }

	bool IsDefined { get; }

	DateTime GetNextScheduleTime(DateTime previousTime);

	DateTime FindFirstScheduleTime();

	void ReadXmlString(string configuration);
}
