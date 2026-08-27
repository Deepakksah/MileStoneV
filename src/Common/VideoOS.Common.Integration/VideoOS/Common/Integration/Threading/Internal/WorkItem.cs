using System;
using System.Diagnostics;
using System.Threading;

namespace VideoOS.Common.Integration.Threading.Internal;

public class WorkItem : IHasWorkItemPriority
{
	private enum WorkItemState
	{
		InQueue,
		InProgress,
		Completed,
		Canceled
	}

	private class WorkItemResult : IWorkItemResult, IWorkItemResult<object>, IWaitableResult, IInternalWorkItemResult, IInternalWaitableResult
	{
		private readonly WorkItem _workItem;

		public bool IsCompleted => _workItem.IsCompleted;

		public bool IsCanceled => _workItem.IsCanceled;

		public object State => _workItem._state;

		public WorkItemPriority WorkItemPriority => _workItem._workItemInfo.WorkItemPriority;

		public object Result => GetResult();

		public object Exception => _workItem._exception;

		public event WorkItemStateCallback OnWorkItemStarted
		{
			add
			{
				_workItem.OnWorkItemStarted += value;
			}
			remove
			{
				_workItem.OnWorkItemStarted -= value;
			}
		}

		public event WorkItemStateCallback OnWorkItemCompleted
		{
			add
			{
				_workItem.OnWorkItemCompleted += value;
			}
			remove
			{
				_workItem.OnWorkItemCompleted -= value;
			}
		}

		public WorkItemResult(WorkItem workItem)
		{
			_workItem = workItem;
		}

		public WorkItem GetWorkItem()
		{
			return _workItem;
		}

		public object GetResult()
		{
			return _workItem.GetResult(-1, exitContext: true, null);
		}

		public object GetResult(int millisecondsTimeout, bool exitContext)
		{
			return _workItem.GetResult(millisecondsTimeout, exitContext, null);
		}

		public object GetResult(TimeSpan timeout, bool exitContext)
		{
			return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, null);
		}

		public object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
		{
			return _workItem.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle);
		}

		public object GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle)
		{
			return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, cancelWaitHandle);
		}

		public object GetResult(out Exception e)
		{
			return _workItem.GetResult(-1, exitContext: true, null, out e);
		}

		public object GetResult(int millisecondsTimeout, bool exitContext, out Exception e)
		{
			return _workItem.GetResult(millisecondsTimeout, exitContext, null, out e);
		}

		public object GetResult(TimeSpan timeout, bool exitContext, out Exception e)
		{
			return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, null, out e);
		}

		public object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
		{
			return _workItem.GetResult(millisecondsTimeout, exitContext, cancelWaitHandle, out e);
		}

		public object GetResult(TimeSpan timeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
		{
			return _workItem.GetResult((int)timeout.TotalMilliseconds, exitContext, cancelWaitHandle, out e);
		}

		public bool Cancel()
		{
			return Cancel(abortExecution: false);
		}

		public bool Cancel(bool abortExecution)
		{
			return _workItem.Cancel(abortExecution);
		}

		public IWorkItemResult GetWorkItemResult()
		{
			return this;
		}

		public IWorkItemResult<TResult> GetWorkItemResultT<TResult>()
		{
			return new WorkItemResultTWrapper<TResult>(this);
		}
	}

	private readonly WorkItemCallback _callback;

	private object _state;

	private readonly CallerThreadContext _callerContext;

	private object _result;

	private Exception _exception;

	private WorkItemState _workItemState;

	private ManualResetEvent _workItemCompleted;

	private int _workItemCompletedRefCount;

	private readonly WorkItemResult _workItemResult;

	private readonly WorkItemInfo _workItemInfo;

	private CanceledWorkItemsGroup _canceledWorkItemsGroup = CanceledWorkItemsGroup.NotCanceledWorkItemsGroup;

	private CanceledWorkItemsGroup _canceledSmartThreadPool = CanceledWorkItemsGroup.NotCanceledWorkItemsGroup;

	private readonly IWorkItemsGroup _workItemsGroup;

	private Thread _executingThread;

	private long _expirationTime;

	private Stopwatch _waitingOnQueueStopwatch;

	private Stopwatch _processingStopwatch;

	public TimeSpan WaitingTime => _waitingOnQueueStopwatch.Elapsed;

	public TimeSpan ProcessTime => _processingStopwatch.Elapsed;

	public WorkItemInfo WorkItemInfo => _workItemInfo;

	public CanceledWorkItemsGroup CanceledWorkItemsGroup{
		get
		{
			return _canceledWorkItemsGroup;
		}
		set
		{
			_canceledWorkItemsGroup = value;
		}
	}

	public CanceledWorkItemsGroup CanceledSmartThreadPool{
		get
		{
			return _canceledSmartThreadPool;
		}
		set
		{
			_canceledSmartThreadPool = value;
		}
	}

	private bool IsCompleted
	{
		get
		{
			lock (this)
			{
				WorkItemState workItemState = GetWorkItemState();
				return workItemState == WorkItemState.Completed || workItemState == WorkItemState.Canceled;
			}
		}
	}

	public bool IsCanceled
	{
		get
		{
			lock (this)
			{
				return GetWorkItemState() == WorkItemState.Canceled;
			}
		}
	}

	public WorkItemPriority WorkItemPriority => _workItemInfo.WorkItemPriority;

	private event WorkItemStateCallback _workItemStartedEvent;

	private event WorkItemStateCallback _workItemCompletedEvent;

	public event WorkItemStateCallback OnWorkItemStarted
	{
		add
		{
			_workItemStartedEvent += value;
		}
		remove
		{
			_workItemStartedEvent -= value;
		}
	}

	public event WorkItemStateCallback OnWorkItemCompleted
	{
		add
		{
			_workItemCompletedEvent += value;
		}
		remove
		{
			_workItemCompletedEvent -= value;
		}
	}

	private static bool IsValidStatesTransition(WorkItemState currentState, WorkItemState nextState)
	{
		bool result = false;
		switch (currentState)
		{
		case WorkItemState.InQueue:
			result = WorkItemState.InProgress == nextState || WorkItemState.Canceled == nextState;
			break;
		case WorkItemState.InProgress:
			result = WorkItemState.Completed == nextState || WorkItemState.Canceled == nextState;
			break;
		}
		return result;
	}

	public WorkItem(IWorkItemsGroup workItemsGroup, WorkItemInfo workItemInfo, WorkItemCallback callback, object state)
	{
		_workItemsGroup = workItemsGroup;
		_workItemInfo = workItemInfo;
		if (workItemInfo == null)
		{
			throw new ArgumentNullException("workItemInfo");
		}
		if (_workItemInfo.UseCallerCallContext || _workItemInfo.UseCallerHttpContext)
		{
			_callerContext = CallerThreadContext.Capture(_workItemInfo.UseCallerCallContext, _workItemInfo.UseCallerHttpContext);
		}
		_callback = callback;
		_state = state;
		_workItemResult = new WorkItemResult(this);
		Initialize();
	}

	public void Initialize()
	{
		_workItemState = WorkItemState.InQueue;
		_workItemCompleted = null;
		_workItemCompletedRefCount = 0;
		_waitingOnQueueStopwatch = new Stopwatch();
		_processingStopwatch = new Stopwatch();
		_expirationTime = ((_workItemInfo.Timeout > 0) ? (DateTime.UtcNow.Ticks + _workItemInfo.Timeout * 10000) : long.MaxValue);
	}

	public bool WasQueuedBy(IWorkItemsGroup workItemsGroup)
	{
		return workItemsGroup == _workItemsGroup;
	}

	public bool StartingWorkItem()
	{
		_waitingOnQueueStopwatch.Stop();
		_processingStopwatch.Start();
		lock (this)
		{
			if (IsCanceled)
			{
				bool result = false;
				if (_workItemInfo.PostExecuteWorkItemCallback != null && (_workItemInfo.CallToPostExecute & CallToPostExecute.WhenWorkItemCanceled) == CallToPostExecute.WhenWorkItemCanceled)
				{
					result = true;
				}
				return result;
			}
			_executingThread = Thread.CurrentThread;
			SetWorkItemState(WorkItemState.InProgress);
		}
		return true;
	}

	public void Execute()
	{
		CallToPostExecute callToPostExecute = CallToPostExecute.Never;
		switch (GetWorkItemState())
		{
		case WorkItemState.InProgress:
			callToPostExecute |= CallToPostExecute.WhenWorkItemNotCanceled;
			ExecuteWorkItem();
			break;
		case WorkItemState.Canceled:
			callToPostExecute |= CallToPostExecute.WhenWorkItemCanceled;
			break;
		default:
			throw new NotSupportedException();
		}
		if ((callToPostExecute & _workItemInfo.CallToPostExecute) != CallToPostExecute.Never)
		{
			PostExecute();
		}
		_processingStopwatch.Stop();
	}

	public void FireWorkItemCompleted()
	{
		try
		{
			if (this._workItemCompletedEvent != null)
			{
				this._workItemCompletedEvent(this);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	public void FireWorkItemStarted()
	{
		try
		{
			if (this._workItemStartedEvent != null)
			{
				this._workItemStartedEvent(this);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
	}

	private void ExecuteWorkItem()
	{
		CallerThreadContext callerThreadContext = null;
		if (_callerContext != null)
		{
			callerThreadContext = CallerThreadContext.Capture(_callerContext.CapturedCallContext, _callerContext.CapturedHttpContext);
			CallerThreadContext.Apply(_callerContext);
		}
		Exception exception = null;
		object result = null;
		try
		{
			try
			{
				result = _callback(_state);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				exception = ex2;
			}
			Thread thread = Interlocked.CompareExchange(ref _executingThread, null, _executingThread);
			if (thread == null)
			{
				Thread.Sleep(60000);
			}
		}
		catch (ThreadAbortException)
		{
			if (!SmartThreadPool.CurrentThreadEntry.AssociatedSmartThreadPool.IsShuttingDown)
			{
				Thread.ResetAbort();
			}
		}
		if (_callerContext != null)
		{
			CallerThreadContext.Apply(callerThreadContext);
		}
		if (!SmartThreadPool.IsWorkItemCanceled)
		{
			SetResult(result, exception);
		}
	}

	private void PostExecute()
	{
		if (_workItemInfo.PostExecuteWorkItemCallback != null)
		{
			try
			{
				_workItemInfo.PostExecuteWorkItemCallback(_workItemResult);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception)
			{
			}
		}
	}

	public void SetResult(object result, Exception exception)
	{
		_result = result;
		_exception = exception;
		SignalComplete(canceled: false);
	}

	public IWorkItemResult GetWorkItemResult()
	{
		return _workItemResult;
	}

	public static bool WaitAll(IWaitableResult[] waitableResults, int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
	{
		if (waitableResults.Length == 0)
		{
			return true;
		}
		WaitHandle[] array = new WaitHandle[waitableResults.Length];
		GetWaitHandles(waitableResults, array);
		bool result;
		if (cancelWaitHandle == null && array.Length <= 64)
		{
			result = STPEventWaitHandle.WaitAll(array, millisecondsTimeout, exitContext);
		}
		else
		{
			result = true;
			int num = millisecondsTimeout;
			Stopwatch stopwatch = Stopwatch.StartNew();
			WaitHandle[] array2 = ((cancelWaitHandle == null) ? new WaitHandle[1] : new WaitHandle[2] { null, cancelWaitHandle });
			bool flag = -1 == millisecondsTimeout;
			for (int i = 0; i < waitableResults.Length; i++)
			{
				if (!flag && num < 0)
				{
					result = false;
					break;
				}
				array2[0] = array[i];
				int num2 = STPEventWaitHandle.WaitAny(array2, num, exitContext);
				if (num2 > 0 || -1 == num2)
				{
					result = false;
					break;
				}
				if (!flag)
				{
					num = millisecondsTimeout - (int)stopwatch.ElapsedMilliseconds;
				}
			}
		}
		ReleaseWaitHandles(waitableResults);
		return result;
	}

	public static int WaitAny(IWaitableResult[] waitableResults, int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
	{
		WaitHandle[] array;
		if (cancelWaitHandle != null)
		{
			array = new WaitHandle[waitableResults.Length + 1];
			GetWaitHandles(waitableResults, array);
			array[waitableResults.Length] = cancelWaitHandle;
		}
		else
		{
			array = new WaitHandle[waitableResults.Length];
			GetWaitHandles(waitableResults, array);
		}
		int num = STPEventWaitHandle.WaitAny(array, millisecondsTimeout, exitContext);
		if (cancelWaitHandle != null && num == waitableResults.Length)
		{
			num = -1;
		}
		ReleaseWaitHandles(waitableResults);
		return num;
	}

	private static void GetWaitHandles(IWaitableResult[] waitableResults, WaitHandle[] waitHandles)
	{
		for (int i = 0; i < waitableResults.Length; i++)
		{
			WorkItemResult workItemResult = waitableResults[i].GetWorkItemResult() as WorkItemResult;
			waitHandles[i] = workItemResult.GetWorkItem().GetWaitHandle();
		}
	}

	private static void ReleaseWaitHandles(IWaitableResult[] waitableResults)
	{
		for (int i = 0; i < waitableResults.Length; i++)
		{
			((WorkItemResult)waitableResults[i].GetWorkItemResult()).GetWorkItem().ReleaseWaitHandle();
		}
	}

	private WorkItemState GetWorkItemState()
	{
		lock (this)
		{
			if (WorkItemState.Completed == _workItemState)
			{
				return _workItemState;
			}
			long ticks = DateTime.UtcNow.Ticks;
			if (WorkItemState.Canceled != _workItemState && ticks > _expirationTime)
			{
				_workItemState = WorkItemState.Canceled;
			}
			if (WorkItemState.InProgress == _workItemState)
			{
				return _workItemState;
			}
			if (CanceledSmartThreadPool.IsCanceled || CanceledWorkItemsGroup.IsCanceled)
			{
				return WorkItemState.Canceled;
			}
			return _workItemState;
		}
	}

	private void SetWorkItemState(WorkItemState workItemState)
	{
		lock (this)
		{
			if (IsValidStatesTransition(_workItemState, workItemState))
			{
				_workItemState = workItemState;
			}
		}
	}

	private void SignalComplete(bool canceled)
	{
		SetWorkItemState(canceled ? WorkItemState.Canceled : WorkItemState.Completed);
		lock (this)
		{
			if (_workItemCompleted != null)
			{
				_workItemCompleted.Set();
			}
		}
	}

	public void WorkItemIsQueued()
	{
		_waitingOnQueueStopwatch.Start();
	}

	private bool Cancel(bool abortExecution)
	{
		bool result = false;
		bool flag = false;
		lock (this)
		{
			switch (GetWorkItemState())
			{
			case WorkItemState.Canceled:
				result = true;
				break;
			case WorkItemState.InProgress:
				if (abortExecution)
				{
					Thread thread = Interlocked.CompareExchange(ref _executingThread, null, _executingThread);
					if (thread != null)
					{
						thread.Abort();
						result = true;
						flag = true;
					}
				}
				else
				{
					result = true;
					flag = true;
				}
				break;
			case WorkItemState.InQueue:
				flag = true;
				result = true;
				break;
			}
			if (flag)
			{
				SignalComplete(canceled: true);
			}
		}
		return result;
	}

	private object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle)
	{
		Exception e;
		object result = GetResult(millisecondsTimeout, exitContext, cancelWaitHandle, out e);
		if (e != null)
		{
			throw new WorkItemResultException("The work item caused an excpetion, see the inner exception for details", e);
		}
		return result;
	}

	private object GetResult(int millisecondsTimeout, bool exitContext, WaitHandle cancelWaitHandle, out Exception e)
	{
		e = null;
		if (WorkItemState.Canceled == GetWorkItemState())
		{
			throw new WorkItemCancelException("Work item canceled");
		}
		if (IsCompleted)
		{
			e = _exception;
			return _result;
		}
		if (cancelWaitHandle == null)
		{
			bool num = !STPEventWaitHandle.WaitOne(GetWaitHandle(), millisecondsTimeout, exitContext);
			ReleaseWaitHandle();
			if (num)
			{
				throw new WorkItemTimeoutException("Work item timeout");
			}
		}
		else
		{
			WaitHandle waitHandle = GetWaitHandle();
			int num2 = STPEventWaitHandle.WaitAny(new WaitHandle[2] { waitHandle, cancelWaitHandle });
			ReleaseWaitHandle();
			switch (num2)
			{
			case -1:
			case 1:
				throw new WorkItemTimeoutException("Work item timeout");
			}
		}
		if (WorkItemState.Canceled == GetWorkItemState())
		{
			throw new WorkItemCancelException("Work item canceled");
		}
		e = _exception;
		return _result;
	}

	private WaitHandle GetWaitHandle()
	{
		lock (this)
		{
			if (_workItemCompleted == null)
			{
				_workItemCompleted = EventWaitHandleFactory.CreateManualResetEvent(IsCompleted);
			}
			_workItemCompletedRefCount++;
		}
		return _workItemCompleted;
	}

	private void ReleaseWaitHandle()
	{
		lock (this)
		{
			if (_workItemCompleted != null)
			{
				_workItemCompletedRefCount--;
				if (_workItemCompletedRefCount == 0)
				{
					_workItemCompleted.Close();
					_workItemCompleted = null;
				}
			}
		}
	}

	public void DisposeOfState()
	{
		if (_workItemInfo.DisposeOfStateObjects && _state is IDisposable disposable)
		{
			disposable.Dispose();
			_state = null;
		}
	}
}
