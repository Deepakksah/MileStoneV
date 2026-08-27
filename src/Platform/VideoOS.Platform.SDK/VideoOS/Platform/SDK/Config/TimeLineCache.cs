#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Platform.Data;

namespace VideoOS.Platform.SDK.Config;

public class TimeLineCache
{
	private Dictionary<DateTime, Sequence> _cacheSequences = new Dictionary<DateTime, Sequence>();

	private DateTime _cacheBegin = DateTime.MaxValue;

	private DateTime _cacheEnd = DateTime.MinValue;

	private IRecorder _recorder;

	private Guid _deviceGuid;

	private Guid[] _timeLineTypes;

	private int _mask;

	public TimeLineCache(IRecorder recorder, Guid deviceId, Guid type)
	{
		_recorder = recorder;
		_deviceGuid = deviceId;
		_timeLineTypes = new Guid[1] { type };
		if (type == DataType.TimeLineInformationTypeGuids.ActivitiesDetected)
		{
			_mask = 1;
		}
		if (type == DataType.TimeLineInformationTypeGuids.RecordedDataAvailable)
		{
			_mask = 1;
		}
	}

	public SequenceCollection GetSequenceAtTimeMinutes(DateTime timeReq, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter)
	{
		SequenceCollection sequenceCollection = new SequenceCollection();
		bool flag = false;
		DateTime dateTime = new DateTime(timeReq.Ticks, DateTimeKind.Utc) - TimeSpan.FromSeconds(timeReq.Second);
		_cacheSequences.Clear();
		_cacheBegin = dateTime;
		_cacheEnd = dateTime;
		if (_cacheBegin == DateTime.MaxValue)
		{
			_cacheBegin = dateTime;
			_cacheEnd = dateTime;
		}
		while (!flag)
		{
			flag = true;
			if (GetCacheBefore(dateTime, minutesBefore, maxCountBefore) || dateTime < _cacheBegin)
			{
				DateTime dateTime2 = _cacheBegin - TimeSpan.FromHours(4.0);
				TimeLineInformationCollection timeLineInformation = _recorder.GetTimeLineInformation(_deviceGuid, _timeLineTypes, dateTime2, 60, 240);
				if (timeLineInformation != null)
				{
					MergeInfo(timeLineInformation, dateTime2, before: true);
					flag = false;
				}
			}
			if (!GetCacheAfter(dateTime - TimeSpan.FromMinutes(1.0), minutesAfter, maxCountAfter) && !(dateTime > _cacheEnd))
			{
				continue;
			}
			DateTime targetTime = _cacheEnd + TimeSpan.FromHours(4.0);
			TimeLineInformationCollection timeLineInformation2 = _recorder.GetTimeLineInformation(_deviceGuid, _timeLineTypes, _cacheEnd, 60, 240);
			if (timeLineInformation2 != null)
			{
				MergeInfo(timeLineInformation2, targetTime, before: false);
				if (timeLineInformation2.Count > 0 && timeLineInformation2[0].Data != "")
				{
					flag = false;
				}
			}
		}
		sequenceCollection.AddRange(GetList(dateTime, minutesBefore, maxCountBefore, takeFirst: false));
		sequenceCollection.AddRange(GetList(dateTime, minutesAfter, maxCountAfter, takeFirst: true));
		return sequenceCollection;
	}

	private void MergeInfo(TimeLineInformationCollection timelines, DateTime targetTime, bool before)
	{
		foreach (TimeLineInformation timeline in timelines)
		{
			if (timeline.DeviceId != _deviceGuid || timeline.Type != _timeLineTypes[0])
			{
				EnvironmentManager.Instance.Log(error: true, "GetTimeLine", "Server returned data from another source!");
				continue;
			}
			DateTime beginTime = timeline.BeginTime;
			Sequence sequence = null;
			for (int i = 0; i < timeline.Count; i++)
			{
				if (((timeline.Data[i] - 48) & _mask) != 0)
				{
					if (sequence == null)
					{
						sequence = new Sequence(beginTime, beginTime, beginTime);
					}
					else
					{
						sequence.EndTime = beginTime;
					}
				}
				else if (sequence != null)
				{
					sequence.EndTime = beginTime - TimeSpan.FromSeconds(1.0);
					AddSequence(sequence);
					sequence = null;
				}
				beginTime += TimeSpan.FromMinutes(1.0);
			}
			if (sequence != null)
			{
				sequence.EndTime = beginTime - TimeSpan.FromSeconds(1.0);
				AddSequence(sequence);
			}
			if (before)
			{
				_cacheBegin = targetTime;
			}
			if (!before)
			{
				DateTime cacheEnd = timeline.BeginTime + TimeSpan.FromSeconds(timeline.Count * (timeline.Interval / 1000000));
				_cacheEnd = cacheEnd;
			}
		}
	}

	private void AddSequence(Sequence sequence)
	{
		DateTime dateTime = sequence.BeginTime - TimeSpan.FromSeconds(1.0);
		DateTime dateTime2 = sequence.EndTime + TimeSpan.FromSeconds(1.0);
		foreach (Sequence value in _cacheSequences.Values)
		{
			if (value.BeginTime <= sequence.BeginTime && dateTime <= value.EndTime)
			{
				if (value.EndTime < sequence.EndTime)
				{
					value.EndTime = sequence.EndTime;
				}
				return;
			}
			if (value.BeginTime <= dateTime2 && sequence.EndTime <= value.EndTime)
			{
				if (sequence.BeginTime < value.BeginTime)
				{
					_cacheSequences.Remove(value.BeginTime);
					value.BeginTime = sequence.BeginTime;
					_cacheSequences.Add(value.BeginTime, value);
				}
				return;
			}
		}
		if (_cacheSequences.ContainsKey(sequence.BeginTime))
		{
			Trace.WriteLine("Dup begin time:" + sequence.BeginTime.ToString());
		}
		else
		{
			_cacheSequences.Add(sequence.BeginTime, sequence);
		}
	}

	private bool GetCacheBefore(DateTime time, TimeSpan minutes, int count)
	{
		DateTime dateTime = time - minutes;
		if (dateTime > _cacheBegin)
		{
			return false;
		}
		int num = 0;
		foreach (Sequence value in _cacheSequences.Values)
		{
			if ((value.BeginTime >= dateTime && value.BeginTime <= time) || (value.EndTime >= dateTime && value.EndTime <= time))
			{
				num++;
			}
			if (num >= count)
			{
				break;
			}
		}
		return num < count;
	}

	private bool GetCacheAfter(DateTime time, TimeSpan minutes, int count)
	{
		DateTime dateTime = time + minutes;
		if (dateTime < _cacheEnd)
		{
			return false;
		}
		int num = 0;
		foreach (Sequence value in _cacheSequences.Values)
		{
			if ((value.BeginTime >= time && value.BeginTime <= dateTime) || (value.EndTime >= time && value.EndTime <= dateTime))
			{
				num++;
			}
			if (num >= count)
			{
				break;
			}
		}
		return num < count;
	}

	private List<Sequence> GetList(DateTime time, TimeSpan minutes, int count, bool takeFirst)
	{
		DateTime dateTime = ((!takeFirst) ? (time - minutes) : time);
		DateTime dateTime2 = dateTime + minutes;
		SortedList<DateTime, Sequence> sortedList = new SortedList<DateTime, Sequence>();
		foreach (Sequence value in _cacheSequences.Values)
		{
			if ((value.BeginTime >= dateTime && value.BeginTime <= dateTime2) || (value.EndTime >= dateTime && value.EndTime <= dateTime2))
			{
				sortedList.Add(value.BeginTime, value);
			}
		}
		if (sortedList.Count > count)
		{
			Sequence[] array = new Sequence[count];
			int num = ((!takeFirst) ? (sortedList.Count - count) : 0);
			int num2 = 0;
			for (int i = num; i < num + count; i++)
			{
				array[num2++] = sortedList.Values[i];
			}
			return new List<Sequence>(array);
		}
		return new List<Sequence>(sortedList.Values);
	}
}
