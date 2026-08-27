using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Central.Util;

public sealed class CycleTaskMember : IDisposable
{
	private Task _task;

	private CancellationTokenSource _cancelTokenSource;

	private readonly TimeSpan _cycleTime;

	private readonly ManualResetEvent _tmpEvent = new ManualResetEvent(initialState: false);

	private readonly TimeSpan _cycleTimeWhenHasMoreWork = TimeSpan.FromMilliseconds(1.0);

	private readonly Func<TaskFuncResult> _onTaskFunc;

	private readonly Action _onTaskStart;

	private readonly Action _onTaskStop;

	public bool IsCycling { get; private set; }

	public string Name { get; set; }

	public CycleTaskMember(Func<TaskFuncResult> taskFunc)
		: this(taskFunc, null, null, TimeSpan.FromMilliseconds(10.0))
	{
	}

	public CycleTaskMember(Func<TaskFuncResult> taskFunc, Action taskStart, Action taskStop, TimeSpan cycleTime)
	{
		_onTaskFunc = taskFunc;
		_onTaskStart = taskStart;
		_onTaskStop = taskStop;
		_cycleTime = cycleTime;
	}

	public CycleTaskMember(Action taskFunc)
		: this(taskFunc.ToFunOfTaskFuncResult())
	{
	}

	public void StartTask()
	{
		if (!IsCycling)
		{
			if (_task != null)
			{
				WaitTaskToStop(TimeSpan.Zero);
			}
			IsCycling = true;
			_cancelTokenSource = new CancellationTokenSource();
			_task = new Task(ActionFunction, _cancelTokenSource.Token, TaskCreationOptions.LongRunning);
			_task.Start();
		}
	}

	public void SignalTaskToStop()
	{
		IsCycling = false;
	}

	public bool WaitTaskToStop(TimeSpan waitTimeOut, bool cancelTaskOnTimeout = false)
	{
		bool result = true;
		if (_task != null && !_task.Wait(waitTimeOut) && cancelTaskOnTimeout)
		{
			try
			{
				_cancelTokenSource?.Cancel();
				result = _task.Wait(waitTimeOut);
			}
			catch
			{
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
			_onTaskStart?.Invoke();
			while (IsCycling)
			{
				TimeSpan timeout = ((_onTaskFunc() == TaskFuncResult.HasMoreWorkToDo) ? _cycleTimeWhenHasMoreWork : _cycleTime);
				_tmpEvent.WaitOne(timeout);
			}
			_onTaskStop?.Invoke();
		}
		catch
		{
		}
	}

	public void Dispose()
	{
		SignalTaskToStop();
		_tmpEvent.Dispose();
	}
}
