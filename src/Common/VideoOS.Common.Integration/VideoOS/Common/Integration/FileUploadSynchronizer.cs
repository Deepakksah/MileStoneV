using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Validation;

namespace VideoOS.Common.Integration;

public class FileUploadSynchronizer : IFileUploadSynchronizer, IDisposable
{
	private class UploadEntryInfo
	{
		public AutoResetEvent UploadEvent { get; } = new AutoResetEvent(initialState: true);

		public DateTime LastChecked { get; set; } = DateTime.MinValue;

		public bool FileExists { get; set; }
	}

	private readonly object _entriesLock = new object();

	private readonly Dictionary<string, UploadEntryInfo> _uploadLocks = new Dictionary<string, UploadEntryInfo>(StringComparer.OrdinalIgnoreCase);

	private readonly ManualResetEvent _stopEvent = new ManualResetEvent(initialState: false);

	private readonly CountdownEvent _stopCountdown = new CountdownEvent(1);

	private bool _disposed;

	private ILogger Logger { get; }

	private TimeSpan CacheDuration { get; }

	public FileUploadSynchronizer(ILogger logger, TimeSpan cacheDuration)
	{
		Logger = logger ?? throw new ArgumentNullException("logger");
		CacheDuration = cacheDuration;
	}

	public bool Upload(Guid recorderId, string fileId, TimeSpan timeout, FileExistsFunction existsFunc, FileUploadFunction uploadFunc)
	{
		fileId.NullCheck("", "Upload", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\FileUploadSynchronizer.cs", 36);
		existsFunc.NullCheck("", "Upload", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\FileUploadSynchronizer.cs", 37);
		uploadFunc.NullCheck("", "Upload", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\FileUploadSynchronizer.cs", 38);
		ThrowIfDisposed();
		if (!_stopCountdown.TryAddCount())
		{
			return false;
		}
		try
		{
			if (_stopEvent.WaitOne(0))
			{
				return false;
			}
			UploadEntryInfo orCreateLock = GetOrCreateLock(recorderId, fileId);
			if (WaitHandle.WaitAny(new WaitHandle[2] { _stopEvent, orCreateLock.UploadEvent }, timeout) != 1)
			{
				LoggerExtensions.LogWarning(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecorderId}, Failed to wait for the upload event.", new object[2] { fileId, recorderId });
				return false;
			}
			try
			{
				if (orCreateLock.FileExists && orCreateLock.LastChecked > DateTime.UtcNow - CacheDuration)
				{
					LoggerExtensions.LogInformation(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecorderId}, File exists, read from cache.", new object[2] { fileId, recorderId });
					return true;
				}
				if (ExecuteExistsFuncAndCache(existsFunc, orCreateLock))
				{
					LoggerExtensions.LogInformation(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecordId}, File exists, read from server and cached.", new object[2] { fileId, recorderId });
					return true;
				}
				LoggerExtensions.LogInformation(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecordId}, File does not exist on server.", new object[2] { fileId, recorderId });
				if (_stopEvent.WaitOne(0))
				{
					return false;
				}
				if (!uploadFunc())
				{
					LoggerExtensions.LogError(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecordId}, File upload failed.", new object[2] { fileId, recorderId });
					return false;
				}
				LoggerExtensions.LogInformation(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecorderId}, File exists, read from cache.", new object[2] { fileId, recorderId });
				bool flag = ExecuteExistsFuncAndCache(existsFunc, orCreateLock);
				if (flag)
				{
					LoggerExtensions.LogInformation(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecordId}, File upload verified, read from server and cached.", new object[2] { fileId, recorderId });
				}
				else
				{
					LoggerExtensions.LogError(Logger, "(FileUploadSynchronizer) FileID: {FileId}, RecorderID: {RecordId}, File upload verification failed, file does not exist on server.", new object[2] { fileId, recorderId });
				}
				return flag;
			}
			finally
			{
				orCreateLock.UploadEvent.Set();
			}
		}
		finally
		{
			_stopCountdown.Signal();
		}
	}

	private static bool ExecuteExistsFuncAndCache(FileExistsFunction existsFunc, UploadEntryInfo uploadInfo)
	{
		bool flag = existsFunc();
		uploadInfo.LastChecked = DateTime.UtcNow;
		uploadInfo.FileExists = flag;
		return flag;
	}

	private void ThrowIfDisposed()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("FileUploadSynchronizer");
		}
	}

	private UploadEntryInfo GetOrCreateLock(Guid recorderId, string fileId)
	{
		string key = string.Format(CultureInfo.InvariantCulture, "{0}:{1}", recorderId.ToString(), fileId);
		UploadEntryInfo value;
		lock (_entriesLock)
		{
			if (!_uploadLocks.TryGetValue(key, out value))
			{
				value = new UploadEntryInfo();
				_uploadLocks[key] = value;
			}
		}
		return value;
	}

	protected virtual void Dispose(bool disposing)
	{
		if (_disposed)
		{
			return;
		}
		if (disposing)
		{
			_stopEvent.Set();
			_stopCountdown.Signal();
			_stopCountdown.Wait();
			lock (_entriesLock)
			{
				foreach (KeyValuePair<string, UploadEntryInfo> uploadLock in _uploadLocks)
				{
					uploadLock.Value.UploadEvent.Dispose();
				}
				_uploadLocks.Clear();
			}
			_stopEvent.Dispose();
		}
		_disposed = true;
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}
}
