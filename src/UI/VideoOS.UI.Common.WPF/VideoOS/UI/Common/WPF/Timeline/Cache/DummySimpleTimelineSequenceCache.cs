using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Timeline.TestData;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public class DummySimpleTimelineSequenceCache : ITimelineSequenceCache
{
	private Dictionary<ISequenceSource, List<Sequence>> _unusedSequenceSourceSequenceDictionary = new Dictionary<ISequenceSource, List<Sequence>>();

	private Dictionary<ISequenceSource, List<Sequence>> _sequenceSourceSequenceDictionary = new Dictionary<ISequenceSource, List<Sequence>>();

	private TimeInterval _interval;

	public bool SupportsMultithreading => false;

	public string Name => "DummySimpleTimelineSequenceCache";

	public event EventHandler<SequenceSourcesEventArgs> NewSequencesAvailable
	{
		add
		{
		}
		remove
		{
		}
	}

	public void SetSequenceSources(IEnumerable<ISequenceSource> sequenceSources)
	{
		foreach (KeyValuePair<ISequenceSource, List<Sequence>> item in _sequenceSourceSequenceDictionary)
		{
			_unusedSequenceSourceSequenceDictionary.Add(item.Key, item.Value);
		}
		_sequenceSourceSequenceDictionary.Clear();
		List<ISequenceSource> list = new List<ISequenceSource>();
		foreach (ISequenceSource sequenceSource in sequenceSources)
		{
			if (_unusedSequenceSourceSequenceDictionary.ContainsKey(sequenceSource))
			{
				_sequenceSourceSequenceDictionary.Add(sequenceSource, _unusedSequenceSourceSequenceDictionary[sequenceSource]);
				_unusedSequenceSourceSequenceDictionary.Remove(sequenceSource);
			}
			else
			{
				list.Add(sequenceSource);
			}
		}
		foreach (ISequenceSource item2 in list)
		{
			_sequenceSourceSequenceDictionary.Add(item2, SourceData.GetSourceData(item2).GetAllGenerated().ToList());
		}
	}

	public void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
	{
	}

	public void SetCurrentTimeInterval(TimeInterval interval)
	{
		if (_interval != interval)
		{
			_interval = interval;
			IEnumerable<ISequenceSource> sequenceSources = new List<ISequenceSource>(_sequenceSourceSequenceDictionary.Keys);
			_sequenceSourceSequenceDictionary.Clear();
			SetSequenceSources(sequenceSources);
		}
	}

	public ISequenceSource[] GetTimelineData(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, TimeSpan interval, int count)
	{
		ISequenceSource[] array = new ISequenceSource[count];
		DateTime beginTime2 = beginTime;
		for (int i = 0; i < count; i++)
		{
			if (GetSequences(sequenceSources, beginTime2, beginTime2.Add(interval)).Count() > 0)
			{
				array[i] = sequenceSources.First();
			}
			beginTime2 = beginTime2.Add(interval);
		}
		return array;
	}

	public IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, DateTime endTime)
	{
		List<Sequence> list = new List<Sequence>();
		foreach (SequenceSource sequenceSource in sequenceSources)
		{
			if (!_sequenceSourceSequenceDictionary.ContainsKey(sequenceSource))
			{
				continue;
			}
			foreach (Sequence item in _sequenceSourceSequenceDictionary[sequenceSource])
			{
				if (item.IntersectsWith(beginTime, endTime))
				{
					list.Add(item);
				}
			}
		}
		list.Sort();
		return list;
	}

	public void InitConfig(TimelineCacheSettings timelineCacheSettings)
	{
	}

	public void InvalidateSequences(IEnumerable<Guid> sourceIds, DateTime fromTime, DateTime toTime)
	{
		throw new NotImplementedException();
	}

	public void InvalidateSequences(DateTime fromTime, DateTime toTime)
	{
		throw new NotImplementedException();
	}
}
