using System;
using System.Threading;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

public class MarkedDataSynchronizationManager : IDisposable
{
	private Timer syncTimer;

	public MarkedDataSynchronizationManager()
	{
		if (!GeminiApplicationCore.IsExecutingModuleTest)
		{
			syncTimer = new Timer(SyncTimerCallback, null, (int)TimeSpan.FromMinutes(1.0).TotalMilliseconds, -1);
		}
	}

	private void SyncTimerCallback(object state)
	{
		using GeminiRequestContext requestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			MarkedData.CleanExpiredMarkedData(requestContext);
		}
		catch (DatabaseException ex)
		{
			GeminiTrace.Info(TraceType.Security, "An SQL error occurred during cleanup of marked data. Exception={0}", ex.ToString());
		}
		catch (Exception ex2)
		{
			GeminiTrace.Error(TraceType.Security, "A system error occurred in MarkedDataSynchronizationManager clearing expired marked data. Exception message: {0}", ex2.Message);
		}
		finally
		{
			syncTimer.Change((int)TimeSpan.FromMinutes(15.0).TotalMilliseconds, -1);
		}
	}

	public void Stop()
	{
		if (syncTimer != null)
		{
			syncTimer.Dispose();
			syncTimer = null;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Stop();
		}
	}
}
