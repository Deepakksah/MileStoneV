using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace VideoOS.Central.Util;

public sealed class BatchProcessor<T> : IDisposable
{
	private CycleTaskMember _cycleTaskMember;

	private readonly ConcurrentQueue<T> _queue = new ConcurrentQueue<T>();

	private readonly Func<IEnumerable<T>, bool> _batchProcessor;

	private readonly int _maxNumberOfProcessedItems;

	public BatchProcessor(Func<IEnumerable<T>, bool> batchProcessor, TimeSpan batchTime, int maxNumberOfProcessedItems = 15)
	{
		_cycleTaskMember = new CycleTaskMember(OnCycle, null, null, batchTime);
		_batchProcessor = batchProcessor ?? throw new ArgumentNullException("batchProcessor");
		_maxNumberOfProcessedItems = maxNumberOfProcessedItems;
	}

	private TaskFuncResult OnCycle()
	{
		List<T> list = new List<T>();
		T result;
		while (list.Count < _maxNumberOfProcessedItems && _queue.TryDequeue(out result))
		{
			list.Add(result);
		}
		if (list.Count == 0)
		{
			return TaskFuncResult.SleepForaWhile;
		}
		if (_batchProcessor(list))
		{
			return TaskFuncResult.HasMoreWorkToDo;
		}
		list.ForEach(delegate(T itm)
		{
			_queue.Enqueue(itm);
		});
		return TaskFuncResult.SleepForaWhile;
	}

	public void Start()
	{
		_cycleTaskMember.StartTask();
	}

	public void Stop()
	{
		_cycleTaskMember.SignalTaskToStop();
	}

	public void Close()
	{
		_cycleTaskMember?.Dispose();
		_cycleTaskMember = null;
	}

	public void Enqueue(T item)
	{
		_queue.Enqueue(item);
	}

	public void Dispose()
	{
		Close();
	}
}
