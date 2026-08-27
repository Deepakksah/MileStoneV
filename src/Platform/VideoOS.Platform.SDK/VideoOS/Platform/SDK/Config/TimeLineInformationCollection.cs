using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class TimeLineInformationCollection : IList<TimeLineInformation>, ICollection<TimeLineInformation>, IEnumerable<TimeLineInformation>, IEnumerable
{
	private List<TimeLineInformation> _list = new List<TimeLineInformation>();

	public TimeLineInformation this[int index]
	{
		get
		{
			return _list[index];
		}
		set
		{
			throw new NotSupportedException();
		}
	}

	public int Count => _list.Count;

	bool ICollection<TimeLineInformation>.IsReadOnly => true;

	public void CreateTimeLineInformationCollection(TimeLineInformationData[] TimeLineInformations)
	{
		foreach (TimeLineInformationData timeLineInformationData in TimeLineInformations)
		{
			TimeLineInformation item = new TimeLineInformation(timeLineInformationData.DeviceId, timeLineInformationData.Type, timeLineInformationData.BeginTime, timeLineInformationData.Interval.MicroSeconds, timeLineInformationData.Count, timeLineInformationData.Data);
			_list.Add(item);
		}
	}

	public int IndexOf(TimeLineInformation item)
	{
		return _list.IndexOf(item);
	}

	void IList<TimeLineInformation>.Insert(int index, TimeLineInformation item)
	{
		throw new NotSupportedException();
	}

	void IList<TimeLineInformation>.RemoveAt(int index)
	{
		throw new NotSupportedException();
	}

	void ICollection<TimeLineInformation>.Add(TimeLineInformation item)
	{
		throw new NotSupportedException();
	}

	void ICollection<TimeLineInformation>.Clear()
	{
		throw new NotSupportedException();
	}

	public bool Contains(TimeLineInformation item)
	{
		return _list.Contains(item);
	}

	void ICollection<TimeLineInformation>.CopyTo(TimeLineInformation[] array, int arrayIndex)
	{
		_list.CopyTo(array, arrayIndex);
	}

	bool ICollection<TimeLineInformation>.Remove(TimeLineInformation item)
	{
		throw new NotSupportedException();
	}

	public IEnumerator<TimeLineInformation> GetEnumerator()
	{
		return _list.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return _list.GetEnumerator();
	}
}
