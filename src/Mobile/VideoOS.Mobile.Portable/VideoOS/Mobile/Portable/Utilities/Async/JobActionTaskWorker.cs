using System;
using VideoOS.Mobile.Portable.Log;

namespace VideoOS.Mobile.Portable.Utilities.Async;

public class JobActionTaskWorker
{
	private readonly Action _jobAction;

	private readonly Action _jobCompleted;

	public JobActionTaskWorker(Action jobAction, Action jobCompleted)
	{
		_jobAction = jobAction;
		_jobCompleted = jobCompleted;
	}

	public void DoTheJob()
	{
		try
		{
			_jobAction();
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "JobActionTaskWorker.DoTheJob()._jobAction()", ex.Message);
		}
		try
		{
			_jobCompleted();
		}
		catch (Exception ex2)
		{
			Logger.Instance.Log(LogContent.Error, "JobActionTaskWorker.DoTheJob()._jobCompleted()", ex2.Message);
		}
	}
}
