using System;

namespace VideoOS.Common.SharedTimeProfile;

public interface ITimeProfileEvaluation
{
	bool EvaluateTimeProfile(Guid id, DateTime utcTime);

	TimeSpan GetAppointmentDuration(Guid id, DateTime utcTime, TimeSpan searchInterval);
}
