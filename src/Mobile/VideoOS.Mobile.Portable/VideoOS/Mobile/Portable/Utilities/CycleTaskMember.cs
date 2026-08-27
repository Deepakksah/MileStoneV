using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities;

public class CycleTaskMember : IDisposable
{
	private Task _task;

	private CancellationTokenSource _cancelTokenSource;

	private bool _bCycling;

	private readonly int _nCycleTimeInMs = 1;

	private readonly EventWaitHandle _tmpEvent = new ManualResetEvent(initialState: false);

	public Func<TaskFuncResult> OnTaskFuncD { get; private set; }

	public Action OnTaskStartD { get; private set; }

	public Action OnTaskStopD { get; private set; }

	public bool IsCycling => _bCycling;

	public string Name { get; set; }

	public CycleTaskMember(Func<TaskFuncResult> taskFunc, Action taskStart = null, Action taskStop = null, int cycleTimeMs = 10)
	{
		OnTaskFuncD = taskFunc;
		OnTaskStartD = taskStart;
		OnTaskStopD = taskStop;
		_nCycleTimeInMs = cycleTimeMs;
	}

	public CycleTaskMember(Action taskFunc, Action taskStart = null, Action taskStop = null, int cycleTimeMs = 10)
		: this(taskFunc.ToFunOfTaskFuncResult(), taskStart, taskStop, cycleTimeMs)
	{
	}

	~CycleTaskMember()
	{
		Dispose(bDisposing: true);
	}

	public void StartTask()
	{
		if (!_bCycling)
		{
			if (_task != null)
			{
				WaitTaskToStop(0);
			}
			_bCycling = true;
			_cancelTokenSource = new CancellationTokenSource();
			_task = new Task(ActionFunction, _cancelTokenSource.Token, TaskCreationOptions.LongRunning);
			_task.Start();
		}
	}

	public void SignalTaskToStop()
	{
		_bCycling = false;
	}

	public bool WaitTaskToStop(TimeSpan timeOut, bool cancelTaskOnTimeout = false)
	{
		return WaitTaskToStop((int)timeOut.TotalMilliseconds, cancelTaskOnTimeout);
	}

	public bool WaitTaskToStop(int nTimeOutMs, bool cancelTaskOnTimeout = false)
	{
		bool result = true;
		if (_task != null && !_task.Wait(nTimeOutMs) && cancelTaskOnTimeout)
		{
			try
			{
				_cancelTokenSource?.Cancel();
				result = _task.Wait(nTimeOutMs);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "CycleTask.WaitTaskToStop", ex.Message);
			}
		}
		_cancelTokenSource?.Dispose();
		_cancelTokenSource = null;
		return result;
	}

	private void ActionFunction()
	{
		try
		{
			Thread.CurrentThread.Name = Name;
			OnTaskStartD?.Invoke();
			while (_bCycling)
			{
				OnTaskFuncD();
				_tmpEvent.WaitOne(_nCycleTimeInMs);
			}
			OnTaskStopD?.Invoke();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CycleTask.ActionFunction()", ex.Message + Environment.NewLine + " stack trace: " + ex.StackTrace);
		}
	}

	public void Dispose()
	{
		Dispose(bDisposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool bDisposing)
	{
		SignalTaskToStop();
		_tmpEvent.Dispose();
	}
}
