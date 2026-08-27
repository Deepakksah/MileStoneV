using System;
using System.Runtime.CompilerServices;
using System.Threading;

namespace VideoOS.Common.Integration.Threading.Internal;

public class WorkItemsGroup : WorkItemsGroupBase
{
	private readonly object _lock = new object();

	private readonly SmartThreadPool _smartThreadPool;

	private bool _isSuspended;

	private int _concurrency;

	private readonly PriorityQueue _workItemsQueue;

	private int _workItemsInStpQueue;

	private int _workItemsExecutingInStp;

	private readonly WIGStartInfo _workItemsGroupStartInfo;

	private readonly ManualResetEvent _isIdleWaitHandle = EventWaitHandleFactory.CreateManualResetEvent(initialState: true);

	private CanceledWorkItemsGroup _canceledWorkItemsGroup = new CanceledWorkItemsGroup();

	public override int Concurrency
	{
		get
		{
			return _concurrency;
		}
		set
		{
			int num = value - _concurrency;
			_concurrency = value;
			if (num > 0)
			{
				EnqueueToSTPNextNWorkItem(num);
			}
		}
	}

	public override int WaitingCallbacks => _workItemsQueue.Count;

	public override WIGStartInfo WIGStartInfo => _workItemsGroupStartInfo;

	private event WorkItemsGroupIdleHandler _onIdle;

	public override event WorkItemsGroupIdleHandler OnIdle
	{
		add
		{
			_onIdle += value;
		}
		remove
		{
			_onIdle -= value;
		}
	}

	public WorkItemsGroup(SmartThreadPool smartThreadPool, int concurrency, WIGStartInfo wigStartInfo)
	{
		if (concurrency <= 0)
		{
			throw new ArgumentOutOfRangeException("concurrency", concurrency, "concurrency must be greater than zero");
		}
		_smartThreadPool = smartThreadPool;
		_concurrency = concurrency;
		_workItemsGroupStartInfo = new WIGStartInfo(wigStartInfo).AsReadOnly();
		_workItemsQueue = new PriorityQueue();
		base.Name = "WorkItemsGroup";
		_workItemsInStpQueue = _workItemsExecutingInStp;
		_isSuspended = _workItemsGroupStartInfo.StartSuspended;
	}

	public override object[] GetStates()
	{
		lock (_lock)
		{
			object[] array = new object[_workItemsQueue.Count];
			int num = 0;
			foreach (WorkItem item in _workItemsQueue)
			{
				array[num] = item.GetWorkItemResult().State;
				num++;
			}
			return array;
		}
	}

	public override void Start()
	{
		if (_isSuspended)
		{
			_isSuspended = false;
			EnqueueToSTPNextNWorkItem(Math.Min(_workItemsQueue.Count, _concurrency));
		}
	}

	public override void Cancel(bool abortExecution)
	{
		lock (_lock)
		{
			_canceledWorkItemsGroup.IsCanceled = true;
			_workItemsQueue.Clear();
			_workItemsInStpQueue = 0;
			_canceledWorkItemsGroup = new CanceledWorkItemsGroup();
		}
		if (abortExecution)
		{
			_smartThreadPool.CancelAbortWorkItemsGroup(this);
		}
	}

	public override bool WaitForIdle(int millisecondsTimeout)
	{
		SmartThreadPool.ValidateWorkItemsGroupWaitForIdle(this);
		return STPEventWaitHandle.WaitOne(_isIdleWaitHandle, millisecondsTimeout, exitContext: false);
	}

	private void RegisterToWorkItemCompletion(IWorkItemResult wir)
	{
		IInternalWorkItemResult obj = (IInternalWorkItemResult)wir;
		obj.OnWorkItemStarted += OnWorkItemStartedCallback;
		obj.OnWorkItemCompleted += OnWorkItemCompletedCallback;
	}

	public void OnSTPIsStarting()
	{
		if (!_isSuspended)
		{
			EnqueueToSTPNextNWorkItem(_concurrency);
		}
	}

	public void EnqueueToSTPNextNWorkItem(int count)
	{
		for (int i = 0; i < count; i++)
		{
			EnqueueToSTPNextWorkItem(null, decrementWorkItemsInStpQueue: false);
		}
	}

	private object FireOnIdle(object state)
	{
		FireOnIdleImpl(this._onIdle);
		return null;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void FireOnIdleImpl(WorkItemsGroupIdleHandler onIdle)
	{
		if (onIdle == null)
		{
			return;
		}
		Delegate[] invocationList = onIdle.GetInvocationList();
		for (int i = 0; i < invocationList.Length; i++)
		{
			WorkItemsGroupIdleHandler workItemsGroupIdleHandler = (WorkItemsGroupIdleHandler)invocationList[i];
			try
			{
				workItemsGroupIdleHandler(this);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
	}

	private void OnWorkItemStartedCallback(WorkItem workItem)
	{
		lock (_lock)
		{
			_workItemsExecutingInStp++;
		}
	}

	private void OnWorkItemCompletedCallback(WorkItem workItem)
	{
		EnqueueToSTPNextWorkItem(null, decrementWorkItemsInStpQueue: true);
	}

	public override void Enqueue(WorkItem workItem)
	{
		EnqueueToSTPNextWorkItem(workItem);
	}

	private void EnqueueToSTPNextWorkItem(WorkItem workItem)
	{
		EnqueueToSTPNextWorkItem(workItem, decrementWorkItemsInStpQueue: false);
	}

	private void EnqueueToSTPNextWorkItem(WorkItem workItem, bool decrementWorkItemsInStpQueue)
	{
		lock (_lock)
		{
			if (decrementWorkItemsInStpQueue)
			{
				_workItemsInStpQueue--;
				if (_workItemsInStpQueue < 0)
				{
					_workItemsInStpQueue = 0;
				}
				_workItemsExecutingInStp--;
				if (_workItemsExecutingInStp < 0)
				{
					_workItemsExecutingInStp = 0;
				}
			}
			if (workItem != null)
			{
				workItem.CanceledWorkItemsGroup = _canceledWorkItemsGroup;
				RegisterToWorkItemCompletion(workItem.GetWorkItemResult());
				_workItemsQueue.Enqueue(workItem);
				if (1 == _workItemsQueue.Count && _workItemsInStpQueue == 0)
				{
					_smartThreadPool.RegisterWorkItemsGroup(this);
					base.IsIdle = false;
					_isIdleWaitHandle.Reset();
				}
			}
			if (_workItemsQueue.Count == 0)
			{
				if (_workItemsInStpQueue == 0)
				{
					_smartThreadPool.UnregisterWorkItemsGroup(this);
					base.IsIdle = true;
					_isIdleWaitHandle.Set();
					if (decrementWorkItemsInStpQueue && this._onIdle != null && this._onIdle.GetInvocationList().Length != 0)
					{
						_smartThreadPool.QueueWorkItem(FireOnIdle);
					}
				}
			}
			else if (!_isSuspended && _workItemsInStpQueue < _concurrency)
			{
				WorkItem workItem2 = _workItemsQueue.Dequeue() as WorkItem;
				try
				{
					_smartThreadPool.Enqueue(workItem2);
				}
				catch (ObjectDisposedException ex)
				{
					ex.GetHashCode();
				}
				_workItemsInStpQueue++;
			}
		}
	}
}
