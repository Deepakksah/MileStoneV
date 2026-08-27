using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedTimeProfile;

public class TimeProfileAppointmentDayCache : ITimeProfileAppointmentDayCache, IEnumerable<TimeProfileAppointmentDay>, IEnumerable
{
	private List<TimeProfileAppointmentDay> list;

	public int MaxCacheEntries { get; private set; }

	public TimeProfileAppointmentDay this[int index] => list[index];

	public TimeProfileAppointmentDayCache(int maxCacheEntries)
	{
		MaxCacheEntries = maxCacheEntries;
		list = new List<TimeProfileAppointmentDay>(maxCacheEntries);
	}

	public void Add(TimeProfileAppointmentDay appointment)
	{
		list.Add(appointment);
	}

	public void Insert(int index, TimeProfileAppointmentDay appointment)
	{
		list.Insert(index, appointment);
	}

	public void RemoveAt(int index)
	{
		list.RemoveAt(index);
	}

	public void Clear()
	{
		list.Clear();
	}

	public IEnumerator<TimeProfileAppointmentDay> GetEnumerator()
	{
		return list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return list.GetEnumerator();
	}
}
