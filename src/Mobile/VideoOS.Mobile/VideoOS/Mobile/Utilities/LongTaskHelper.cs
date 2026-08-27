using System;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Utilities;

public class LongTaskHelper
{
	private bool _isRunning;

	private bool _isNextExecutionNeeded;

	private readonly Action _action;

	private readonly object _lockObject = new object();

	private readonly CancellationToken _token;

	public bool IsRunning
	{
		get
		{
			lock (_lockObject)
			{
				return _isRunning;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_isRunning = value;
			}
		}
	}

	private bool IsNextExecutionNeeded
	{
		get
		{
			lock (_lockObject)
			{
				return _isNextExecutionNeeded;
			}
		}
		set
		{
			lock (_lockObject)
			{
				_isNextExecutionNeeded = value;
			}
		}
	}

	public LongTaskHelper(Action action)
	{
		_action = action;
	}

	public void Execute()
	{
		IsNextExecutionNeeded = true;
		if (!IsRunning)
		{
			IsNextExecutionNeeded = false;
			Task.Factory.StartNew(DoAction, _token);
			IsRunning = false;
		}
	}

	private void DoAction()
	{
		do
		{
			IsNextExecutionNeeded = false;
			try
			{
				_action();
			}
			catch (Exception ex)
			{
				Log.Debug(MethodBase.GetCurrentMethod().Name, ex.Message);
			}
		}
		while (IsNextExecutionNeeded);
	}
}
