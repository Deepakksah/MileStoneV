using System;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Validation;

namespace VideoOS.Common.Integration;

public class ServerFileStorageCleanupService : IServerFileStorageCleanupService, IDisposable
{
	private readonly TimeSpan _cleanupInterval;

	private readonly ILogger _log;

	private readonly object _timerLock = new object();

	private Timer _cleanupTimer;

	private IServerFileStorageCleanupProxy _cleanupAccess;

	private bool _disposed;

	public ServerFileStorageCleanupService(TimeSpan cleanupInterval, ILogger logger)
	{
		_cleanupInterval = cleanupInterval;
		_log = logger ?? throw new ArgumentNullException("logger");
	}

	public void Start(IServerFileStorageCleanupProxy cleanupProxy)
	{
		cleanupProxy.NullCheck("", "Start", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\Cleanup\\ServerFileStorageCleanupService.cs", 26);
		if (_disposed)
		{
			throw new ObjectDisposedException("ServerFileStorageCleanupService");
		}
		if (_cleanupTimer != null)
		{
			throw new InvalidOperationException("Cleanup service is already started.");
		}
		LoggerExtensions.LogDebug(_log, "(ServerFileStorageCleanupService) Starting storage cleanup service", Array.Empty<object>());
		_cleanupAccess = cleanupProxy;
		_cleanupTimer = new Timer(CleanupFunction, null, TimeSpan.Zero, Timeout.InfiniteTimeSpan);
	}

	private void CleanupFunction(object state)
	{
		try
		{
			_cleanupAccess.LockEntries();
			DateTime now = DateTime.UtcNow;
			IServerFileStorageCleanupEntry[] array = _cleanupAccess.Entries.Where((IServerFileStorageCleanupEntry entry) => entry.UtcExpiration < now).ToArray();
			foreach (IServerFileStorageCleanupEntry serverFileStorageCleanupEntry in array)
			{
				LoggerExtensions.LogInformation(_log, "(ServerFileStorageCleanupService) Storage cleanup service is removing entry '{Id}'", new object[1] { serverFileStorageCleanupEntry.Id });
				_cleanupAccess.Remove(serverFileStorageCleanupEntry);
			}
		}
		catch (IOException ex)
		{
			LoggerExtensions.LogError(_log, "(ServerFileStorageCleanupService) An IO exception occurred during cleanup: {Exception}", new object[1] { ex.ToString() });
		}
		finally
		{
			_cleanupAccess.UnlockEntries();
			lock (_timerLock)
			{
				_cleanupTimer?.Change(_cleanupInterval, Timeout.InfiniteTimeSpan);
			}
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			LoggerExtensions.LogInformation(_log, "Stopping storage cleanup service", Array.Empty<object>());
			lock (_timerLock)
			{
				_cleanupTimer?.Change(-1, -1);
				if (_cleanupTimer != null)
				{
					_cleanupTimer.Dispose();
					_cleanupTimer = null;
				}
			}
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
