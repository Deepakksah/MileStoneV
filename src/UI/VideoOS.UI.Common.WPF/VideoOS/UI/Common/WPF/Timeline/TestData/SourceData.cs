#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Threading;
using C5;
using VideoOS.UI.Common.WPF.Timeline.InterfaceData;
using VideoOS.UI.Common.WPF.Timeline.SequenceRetriever;
using VideoOS.UI.Common.WPF.Utils.Threading;

namespace VideoOS.UI.Common.WPF.Timeline.TestData;

public class SourceData
{
	private class GetSequencesAsyncHandler
	{
		public SourceQuery Query { get; set; }

		public Dispatcher Dispatcher { get; set; }

		public Action<SourceQuery, IEnumerable<Sequence>> CallbackAction { get; set; }

		public Func<SourceQuery, bool, IEnumerable<Sequence>> GetSequencesFunc { get; set; }

		public void GetSequencesCallback(IAsyncResult ar)
		{
			if (Dispatcher != null && Dispatcher != Dispatcher.CurrentDispatcher)
			{
				Dispatcher.BeginInvoke(GetSequencesCallback, ar);
			}
			else
			{
				CallbackAction(Query, GetSequencesFunc.EndInvoke(ar));
			}
		}
	}

	private static Dictionary<ISequenceSource, SourceData> _sourceDataDictionary;

	private static Dictionary<object, int> _groupSeeds;

	private static object lockObject;

	private SortedArray<Sequence> _data = new SortedArray<Sequence>((MemoryType)0);

	private Random _randomDelayGenerator = new Random(12345);

	private ISequenceSource _source;

	private ManualResetEventSlim _dataGeneratedEvent = new ManualResetEventSlim(initialState: false);

	private Func<SourceQuery, bool, IEnumerable<Sequence>> _getSequencesFunc;

	public static DateTime SceneBeginTime { get; set; }

	public static DateTime SceneEndTime { get; set; }

	public TimeSpan AverageDelay { get; set; }

	public TimeSpan MaxDelayDeviation { get; set; }

	static SourceData()
	{
		lockObject = new object();
		Reset();
	}

	public static void Reset()
	{
		_sourceDataDictionary = new Dictionary<ISequenceSource, SourceData>();
		_groupSeeds = new Dictionary<object, int>();
		SceneBeginTime = DateTime.Today.AddDays(-1.0);
		SceneEndTime = DateTime.Today.AddDays(1.0);
	}

	public static SourceData GetSourceData(ISequenceSource source)
	{
		lock (lockObject)
		{
			SourceData sourceData = null;
			if (!_sourceDataDictionary.TryGetValue(source, out sourceData))
			{
				if (!_groupSeeds.TryGetValue(source.SourceGroup, out var seed))
				{
					seed = 12345 * (_groupSeeds.Count + 1);
					_groupSeeds.Add(source.SourceGroup, seed);
				}
				sourceData = new SourceData(source);
				_sourceDataDictionary.Add(source, sourceData);
				Action action = delegate
				{
					foreach (Sequence item in GenerateRandomSequences(seed, source.VisualAppearance, SceneBeginTime, SceneEndTime))
					{
						sourceData._data.Add(item);
					}
					sourceData.SetDataGeneratedEvent();
					Trace.WriteLine($"TestData.SourceData: Generated {((CollectionValueBase<Sequence>)(object)sourceData._data).Count} sequences for {source}, group {source.SourceGroup.GetHashCode()}.");
				};
				Task.Factory.StartNew(action);
			}
			return sourceData;
		}
	}

	private SourceData(ISequenceSource source)
	{
		AverageDelay = TimeSpan.FromMilliseconds(100.0);
		MaxDelayDeviation = TimeSpan.FromMilliseconds(50.0);
		_source = source;
		_getSequencesFunc = GetSequences;
	}

	private void SetDataGeneratedEvent()
	{
		_dataGeneratedEvent.Set();
	}

	private static IEnumerable<Sequence> GenerateRandomSequences(int seed, VisualAppearance visualAppearance, DateTime sceneBeginTime, DateTime sceneEndTime)
	{
		Random random = new Random(seed);
		DateTime position = sceneBeginTime;
		while (true)
		{
			Sequence recordingSequence = new Sequence
			{
				BeginTime = position.AddSeconds(Math.Pow(random.Next(1, 10), 3.0))
			};
			if (recordingSequence.BeginTime >= sceneEndTime)
			{
				break;
			}
			recordingSequence.EndTime = recordingSequence.BeginTime.AddSeconds(Math.Pow(random.Next(1, 10), 3.0));
			if (recordingSequence.EndTime > sceneEndTime)
			{
				recordingSequence.EndTime = sceneEndTime;
			}
			position = recordingSequence.EndTime;
			if (visualAppearance == VisualAppearance.FillTopLineLowZOrder)
			{
				yield return recordingSequence;
			}
			if (random.Next(100) < 70)
			{
				Sequence sequence = new Sequence();
				sequence.BeginTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.2), DateTimeKind.Utc);
				sequence.EndTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.8), DateTimeKind.Utc);
				if (visualAppearance == VisualAppearance.FillTopLineHighZOrder)
				{
					yield return sequence;
				}
			}
			if (random.Next(100) < 40)
			{
				Sequence sequence2 = new Sequence();
				sequence2.BeginTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.3), DateTimeKind.Utc);
				sequence2.EndTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.6), DateTimeKind.Utc);
				if (visualAppearance == VisualAppearance.FillBottomLineLowZOrder)
				{
					yield return sequence2;
				}
			}
			if (random.Next(100) < 40)
			{
				Sequence sequence3 = new Sequence();
				sequence3.BeginTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.4), DateTimeKind.Utc);
				sequence3.EndTime = new DateTime(recordingSequence.BeginTime.Ticks + (long)((double)(recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) * 0.7), DateTimeKind.Utc);
				if (visualAppearance == VisualAppearance.FillBottomLineHighZOrder)
				{
					yield return sequence3;
				}
			}
			if (random.Next(100) < 10)
			{
				Sequence sequence4 = new Sequence();
				sequence4.BeginTime = new DateTime(recordingSequence.BeginTime.Ticks + (recordingSequence.EndTime.Ticks - recordingSequence.BeginTime.Ticks) / 100 * random.Next(1, 99), DateTimeKind.Utc);
				sequence4.EndTime = sequence4.BeginTime;
				if (visualAppearance == VisualAppearance.Marker)
				{
					yield return sequence4;
				}
			}
		}
	}

	public TimeSpan GetNextDelay()
	{
		return AverageDelay.Add(TimeSpan.FromMilliseconds(_randomDelayGenerator.Next(2 * (int)MaxDelayDeviation.TotalMilliseconds)) - MaxDelayDeviation);
	}

	public ISortedEnumerable<Sequence> GetSequences(SourceQuery query)
	{
		return GetSequences(query, delay: false);
	}

	public ISortedEnumerable<Sequence> GetAllGenerated()
	{
		_dataGeneratedEvent.Wait();
		return ((ISorted<Sequence>)(object)_data).ToSortedEnumerable<Sequence>();
	}

	public ISortedEnumerable<Sequence> GetSequences(DateTime from, DateTime to)
	{
		_dataGeneratedEvent.Wait();
		return ((IEnumerable<Sequence>)_data).GetAllIntersecting(from, to);
	}

	public ISortedEnumerable<Sequence> GetSequences(SourceQuery query, bool delay)
	{
		if (query.Priority != SourceQuery.PriorityIgnore)
		{
			if (delay)
			{
				Thread.Sleep((int)GetNextDelay().TotalMilliseconds);
			}
			_dataGeneratedEvent.Wait();
			if (query.Priority != SourceQuery.PriorityIgnore)
			{
				return ((IEnumerable<Sequence>)_data).GetAllIntersecting(query.BeginTime, query.EndTime);
			}
		}
		return new Sequence[0].ToSortedEnumerable();
	}

	public IAsyncResult BeginGetSequences(SourceQuery query, Action<SourceQuery, IEnumerable<Sequence>> callbackAction, Dispatcher dispatcher = null)
	{
		GetSequencesAsyncHandler getSequencesAsyncHandler = new GetSequencesAsyncHandler
		{
			Query = query,
			CallbackAction = callbackAction,
			Dispatcher = dispatcher,
			GetSequencesFunc = _getSequencesFunc
		};
		return _getSequencesFunc.BeginInvoke(query, arg2: true, getSequencesAsyncHandler.GetSequencesCallback, getSequencesAsyncHandler);
	}
}
