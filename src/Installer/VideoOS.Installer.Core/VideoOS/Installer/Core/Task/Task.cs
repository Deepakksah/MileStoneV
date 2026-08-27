using System;
using System.Threading;
using VideoOS.Installer.Core.Observer;

namespace VideoOS.Installer.Core.Task;

public abstract class Task : Observable
{
	private object _dateTimeLock = new object();

	private DateTime _endingTime;

	protected ManualResetEvent _taskCompletedEvent;

	protected volatile bool _pausePending;

	protected volatile TaskResult _result;

	protected volatile TaskStatus _status;

	private DateTime _startingTime;

	private Exception _lastError;

	private Exception _lastWarning;

	private Guid _id = Guid.NewGuid();

	private bool _continueOnError;

	private bool _showErrorWithWarningColor;

	public virtual DateTime EndingTime
	{
		get
		{
			lock (_dateTimeLock)
			{
				return _endingTime;
			}
		}
	}

	public Guid Id => _id;

	public virtual Exception LastError
	{
		get
		{
			return _lastError;
		}
		protected set
		{
			Interlocked.Exchange(ref _lastError, value);
		}
	}

	public virtual Exception LastWarning
	{
		get
		{
			return _lastWarning;
		}
		protected set
		{
			Interlocked.Exchange(ref _lastWarning, value);
		}
	}

	protected virtual bool Paused => Status == TaskStatus.Paused;

	public virtual TaskResult Result => _result;

	public virtual TaskStatus Status
	{
		get
		{
			return _status;
		}
		protected set
		{
			if (_status != value)
			{
				_status = value;
				NotifyObservers();
			}
		}
	}

	public virtual DateTime StartingTime
	{
		get
		{
			lock (_dateTimeLock)
			{
				return _startingTime;
			}
		}
	}

	protected virtual bool Terminate => Status == TaskStatus.Terminating;

	public bool ContinueOnError
	{
		get
		{
			return _continueOnError;
		}
		set
		{
			_continueOnError = value;
		}
	}

	public bool ShowErrorWithWarningColor
	{
		get
		{
			return _showErrorWithWarningColor;
		}
		set
		{
			_showErrorWithWarningColor = value;
		}
	}

	public Task()
	{
		_status = TaskStatus.NotStarted;
		_result = TaskResult.Unknown;
		_startingTime = DateTime.MinValue;
		_endingTime = DateTime.MinValue;
		_pausePending = false;
		_taskCompletedEvent = new ManualResetEvent(initialState: false);
	}

	protected abstract void DoRun();

	public virtual void Pause()
	{
		if (Status == TaskStatus.NotStarted || Status == TaskStatus.Started)
		{
			_pausePending = !_pausePending;
		}
		else if (Status == TaskStatus.Paused)
		{
			if (_pausePending)
			{
				_pausePending = !_pausePending;
			}
			Status = TaskStatus.Running;
		}
		else if (_status == TaskStatus.Running)
		{
			Status = TaskStatus.Paused;
		}
	}

	protected abstract void PostRun();

	protected abstract void PreRun();

	public virtual TaskResult Start()
	{
		if (Status == TaskStatus.NotStarted || Status == TaskStatus.Terminating)
		{
			Status = TaskStatus.Started;
			lock (_dateTimeLock)
			{
				_startingTime = DateTime.Now;
			}
			try
			{
				if (_result != TaskResult.TerminatedByUser)
				{
					try
					{
						PreRun();
						if (_pausePending)
						{
							Status = TaskStatus.Paused;
						}
						else
						{
							Status = TaskStatus.Running;
						}
					}
					catch (Exception lastError)
					{
						_result = TaskResult.Error;
						LastError = lastError;
						return _result;
					}
					try
					{
						DoRun();
						if (Status == TaskStatus.Paused)
						{
							Status = TaskStatus.Running;
						}
					}
					catch (Exception lastError2)
					{
						_result = TaskResult.Error;
						LastError = lastError2;
						return _result;
					}
					try
					{
						PostRun();
						if (Status == TaskStatus.Terminating)
						{
							_result = TaskResult.TerminatedByUser;
						}
						if (_result == TaskResult.Unknown)
						{
							_result = TaskResult.Success;
						}
					}
					catch (Exception lastError3)
					{
						_result = TaskResult.Error;
						LastError = lastError3;
						return _result;
					}
				}
			}
			finally
			{
				TaskCompleted();
			}
		}
		return _result;
	}

	public virtual TaskResult Stop()
	{
		if (Status != TaskStatus.Completed)
		{
			TaskStatus status = Status;
			Status = TaskStatus.Terminating;
			if (status != TaskStatus.NotStarted)
			{
				_taskCompletedEvent.WaitOne();
			}
			if (_result != TaskResult.Error)
			{
				_result = TaskResult.TerminatedByUser;
			}
		}
		return _result;
	}

	protected void TaskCompleted()
	{
		lock (_dateTimeLock)
		{
			_endingTime = DateTime.Now;
		}
		Status = TaskStatus.Completed;
		_taskCompletedEvent.Set();
	}
}
