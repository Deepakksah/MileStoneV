using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedTimeProfile;

public interface ITimeProfileAppointmentDayCache : IEnumerable<TimeProfileAppointmentDay>, IEnumerable
{
	int MaxCacheEntries { get; }

	TimeProfileAppointmentDay this[int index] { get; }

	void Add(TimeProfileAppointmentDay appointment);

	void Insert(int index, TimeProfileAppointmentDay appointment);

	void RemoveAt(int index);

	void Clear();
}
