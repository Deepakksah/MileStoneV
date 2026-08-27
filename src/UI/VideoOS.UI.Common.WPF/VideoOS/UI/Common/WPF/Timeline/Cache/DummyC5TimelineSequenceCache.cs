using System;
using System.Collections.Generic;
using System.Linq;
using C5;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Timeline.TestData;

namespace VideoOS.UI.Common.WPF.Timeline.Cache;

public class DummyC5TimelineSequenceCache : ITimelineSequenceCache
{
	private Dictionary<ISequenceSource, SortedArray<Sequence>> _unusedSequenceSourceSequenceDictionary = new Dictionary<ISequenceSource, SortedArray<Sequence>>();

	private Dictionary<ISequenceSource, SortedArray<Sequence>> _sequenceSourceSequenceDictionary = new Dictionary<ISequenceSource, SortedArray<Sequence>>();

	public bool SupportsMultithreading => false;

	public string Name => "DummyC5TimelineSequenceCache";

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
		foreach (KeyValuePair<ISequenceSource, SortedArray<Sequence>> item in _sequenceSourceSequenceDictionary)
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
			SortedArray<Sequence> val = new SortedArray<Sequence>((MemoryType)0);
			val.AddSorted((IEnumerable<Sequence>)SourceData.GetSourceData(item2).GetSequences(DateTime.Today.Subtract(TimeSpan.FromDays(1.0)), DateTime.Today.Add(TimeSpan.FromDays(1.0))));
			_sequenceSourceSequenceDictionary.Add(item2, val);
		}
	}

	public void InvalidateSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime fromTime, DateTime toTime)
	{
	}

	public void SetCurrentTimeInterval(TimeInterval interval)
	{
	}

	public ISequenceSource[] GetTimelineData(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, TimeSpan interval, int count)
	{
		ISequenceSource[] array = new ISequenceSource[count];
		Sequence sequence = default(Sequence);
		Sequence sequence2 = default(Sequence);
		foreach (SequenceSource sequenceSource in sequenceSources)
		{
			if (!_sequenceSourceSequenceDictionary.ContainsKey(sequenceSource))
			{
				continue;
			}
			SortedArray<Sequence> val = _sequenceSourceSequenceDictionary[sequenceSource];
			if (val == null)
			{
				continue;
			}
			DateTime dateTime = beginTime;
			int num = 0;
			int num2 = 0;
			for (int i = 0; i < count; i++)
			{
				DateTime dateTime2 = dateTime;
				dateTime = dateTime2.Add(interval);
				num = ((num2 > 0) ? (num2 - 1) : (val.TryWeakPredecessor(new Sequence
				{
					BeginTime = dateTime2
				}, ref sequence) ? val.IndexOf(sequence) : 0));
				num2 = ((!val.TryWeakSuccessor(new Sequence
				{
					BeginTime = dateTime
				}, ref sequence2)) ? ((CollectionValueBase<Sequence>)(object)val).Count : val.IndexOf(sequence2));
				for (int j = num; j < num2; j++)
				{
					if (val[j].EndTime >= dateTime2)
					{
						array[i] = sequenceSource;
						break;
					}
				}
			}
		}
		return array;
	}

	public IEnumerable<Sequence> GetSequences(IEnumerable<ISequenceSource> sequenceSources, DateTime beginTime, DateTime endTime)
	{
		List<Sequence> list = new List<Sequence>();
		Sequence sequence = default(Sequence);
		Sequence sequence2 = default(Sequence);
		foreach (SequenceSource sequenceSource in sequenceSources)
		{
			if (!_sequenceSourceSequenceDictionary.ContainsKey(sequenceSource))
			{
				continue;
			}
			SortedArray<Sequence> val = _sequenceSourceSequenceDictionary[sequenceSource];
			if (val == null || ((CollectionValueBase<Sequence>)(object)val).Count <= 0)
			{
				continue;
			}
			val.TryWeakPredecessor(new Sequence
			{
				BeginTime = beginTime
			}, ref sequence);
			if (sequence == null)
			{
				sequence = ((IEnumerable<Sequence>)val).First();
			}
			val.TryWeakSuccessor(new Sequence
			{
				BeginTime = endTime
			}, ref sequence2);
			IDirectedCollectionValue<Sequence> val2 = ((sequence2 == null) ? val.RangeFrom(sequence) : val.RangeFromTo(sequence, sequence2));
			foreach (Sequence item in (IEnumerable<Sequence>)val2)
			{
				if (item.EndTime >= beginTime)
				{
					list.Add(item);
				}
			}
		}
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
