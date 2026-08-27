using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Utilities.Validation;

namespace VideoOS.Common.Integration;

public class ServerFileStorage : IServerFileStorage, IDisposable
{
	private abstract class BaseCleanupProxy : IServerFileStorageCleanupProxy
	{
		protected ServerFileStorage FileStorage { get; }

		public abstract IEnumerable<IServerFileStorageCleanupEntry> Entries { get; }

		protected BaseCleanupProxy(ServerFileStorage fileStorage)
		{
			FileStorage = fileStorage;
		}

		public bool LockEntries()
		{
			bool lockTaken = false;
			Monitor.Enter(FileStorage._storageLock, ref lockTaken);
			return lockTaken;
		}

		public void UnlockEntries()
		{
			Monitor.Exit(FileStorage._storageLock);
		}

		public void Remove(IServerFileStorageCleanupEntry entry)
		{
			entry.NullCheck("", "Remove", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\ServerFileStorage.CleanupProxy.cs", 35);
			FileStorage.RemoveStorageInternal(FileStorage.GetStorageInternal(entry.Id), forceDisposeStream: true);
		}
	}

	private class TemporaryCleanupProxy : BaseCleanupProxy
	{
		public override IEnumerable<IServerFileStorageCleanupEntry> Entries
		{
			get
			{
				lock (base.FileStorage._storageLock)
				{
					return from kv in base.FileStorage._storageMap
						where !kv.Value.Persisted
						select kv.Value;
				}
			}
		}

		public TemporaryCleanupProxy(ServerFileStorage fileStorage)
			: base(fileStorage)
		{
		}
	}

	private class PersistedCleanupProxy : BaseCleanupProxy
	{
		public override IEnumerable<IServerFileStorageCleanupEntry> Entries
		{
			get
			{
				lock (base.FileStorage._storageLock)
				{
					return from kv in base.FileStorage._storageMap
						where kv.Value.Persisted
						select kv.Value;
				}
			}
		}

		public PersistedCleanupProxy(ServerFileStorage fileStorage)
			: base(fileStorage)
		{
		}
	}

	private class StorageEntry : IServerFileStorageEntry, IServerFileStorageCleanupEntry
	{
		private class StorageStream : Stream
		{
			private readonly Stream _innerStream;

			private StorageEntry _storageEntry;

			private bool _disposed;

			private ILogger Logger => _storageEntry.Logger;

			private IServerFileStorageQuota Quota => _storageEntry.Quota;

			public override bool CanRead => _innerStream.CanRead;

			public override bool CanSeek => _innerStream.CanSeek;

			public override bool CanWrite => _innerStream.CanWrite;

			public override long Length => _innerStream.Length;

			public override long Position
			{
				get
				{
					return _innerStream.Position;
				}
				set
				{
					ThrowIfDisposed();
					_storageEntry.Accessed();
					if (value >= _innerStream.Length)
					{
						throw new InvalidOperationException("Can not set the position beyond the end of the stream.");
					}
					_innerStream.Position = value;
				}
			}

			public StorageStream(Stream innerStream, StorageEntry storageInfo)
			{
				_innerStream = innerStream;
				_storageEntry = storageInfo;
			}

			public override void Flush()
			{
				ThrowIfDisposed();
				_storageEntry.Accessed();
				_innerStream.Flush();
			}

			public override int Read(byte[] buffer, int offset, int count)
			{
				ThrowIfDisposed();
				_storageEntry.Accessed();
				return _innerStream.Read(buffer, offset, count);
			}

			public override long Seek(long offset, SeekOrigin origin)
			{
				ThrowIfDisposed();
				_storageEntry.Accessed();
				switch (origin)
				{
				case SeekOrigin.Begin:
					if (offset >= _innerStream.Length)
					{
						throw new InvalidOperationException("Can not set the position beyond the end of the stream.");
					}
					break;
				case SeekOrigin.End:
					if (offset + _innerStream.Length >= _innerStream.Length)
					{
						throw new InvalidOperationException("Can not set the position beyond the end of the stream.");
					}
					break;
				case SeekOrigin.Current:
					if (offset + _innerStream.Position >= _innerStream.Length)
					{
						throw new InvalidOperationException("Can not set the position beyond the end of the stream.");
					}
					break;
				}
				return _innerStream.Seek(offset, origin);
			}

			public override void SetLength(long value)
			{
				ThrowIfDisposed();
				_storageEntry.Accessed();
				if (!_storageEntry.Quota.Update(_storageEntry.QuotaId, (ulong)value))
				{
					LoggerExtensions.LogError(Logger, "(StorageStream) The quota limit was hit while writing to storage with ID {StorageEntryId}", new object[1] { _storageEntry.Id });
					throw new StorageQuotaExceededException(string.Format(CultureInfo.InvariantCulture, "Extending the size of storage {0} failed as the quota {1} was exceeded.", _storageEntry.Id, _storageEntry.FileStorage.Quota));
				}
				_innerStream.SetLength(value);
			}

			public override void Write(byte[] buffer, int offset, int count)
			{
				ThrowIfDisposed();
				_storageEntry.Accessed();
				long consumerUsage = Math.Max(_innerStream.Length, _innerStream.Position + count);
				if (!Quota.Update(_storageEntry.QuotaId, (ulong)consumerUsage))
				{
					LoggerExtensions.LogError(Logger, "(StorageStream) The quota limit was hit while writing to storage with ID {StorageEntryId}", new object[1] { _storageEntry.Id });
					throw new StorageQuotaExceededException(string.Format(CultureInfo.InvariantCulture, "Writing to the storage {0} failed as the quota {1} was exceeded.", _storageEntry.Id, Quota));
				}
				_innerStream.Write(buffer, offset, count);
			}

			private void ThrowIfDisposed()
			{
				if (_disposed)
				{
					throw new ObjectDisposedException("The stream is disposed.");
				}
			}

			protected override void Dispose(bool disposing)
			{
				if (!_disposed)
				{
					if (disposing)
					{
						_innerStream?.Dispose();
						_storageEntry.Stream = null;
						_storageEntry = null;
					}
					_disposed = true;
					base.Dispose(disposing);
				}
			}
		}

		private readonly object _entryLock = new object();

		private readonly object _expirationLock = new object();

		private DateTime _expiration;

		private StorageStream Stream { get; set; }

		private Guid QuotaId { get; }

		private ServerFileStorage FileStorage { get; }

		private ILogger Logger => FileStorage.Logger;

		private IServerFileAccess FileAccess => FileStorage.FileAccess;

		private IServerFileStorageQuota Quota => FileStorage.Quota;

		public string Id { get; private set; }

		public string FilePath { get; private set; }

		public bool Persisted { get; private set; }

		public DateTime UtcExpiration
		{
			get
			{
				lock (_expirationLock)
				{
					return _expiration;
				}
			}
			set
			{
				lock (_expirationLock)
				{
					_expiration = value;
				}
			}
		}

		public StorageEntry(ServerFileStorage fileStorage)
		{
			fileStorage.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\ServerFileStorage.StorageEntry.cs", 51);
			FileStorage = fileStorage;
			Id = Guid.NewGuid().ToString("N");
			FilePath = GenerateStoragePath(Id, Persisted);
			QuotaId = Guid.NewGuid();
			Accessed();
		}

		public Stream CreateStream()
		{
			lock (_entryLock)
			{
				Accessed();
				if (Stream != null)
				{
					LoggerExtensions.LogError(Logger, "(ServerFileStorage) An un-disposed stream already exists for storage with ID {Id}. Storage will not be persisted.", new object[1] { Id });
					throw new StorageStreamAlreadyExistsException("A storage stream has already been created for this storage. Dispose the stream first.");
				}
				FileAccess.CreateDirectory(Path.GetDirectoryName(FilePath));
				Stream innerStream = FileAccess.Open(FilePath, FileMode.OpenOrCreate);
				Stream = new StorageStream(innerStream, this);
				return System.IO.Stream.Synchronized(Stream);
			}
		}

		public void Persist(string newId)
		{
			newId.NullCheck("", "Persist", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\ServerFileStorage.StorageEntry.cs", 86);
			lock (_entryLock)
			{
				if (Persisted)
				{
					LoggerExtensions.LogError(Logger, "(ServerFileStorage) Storage with ID {Id} is already persisted.", new object[1] { Id });
					throw new InvalidOperationException("Can not persist an already persisted storage.");
				}
				if (Stream != null)
				{
					LoggerExtensions.LogError(Logger, "(ServerFileStorage) An un-disposed storage stream exists for storage with ID {Id}", new object[1] { Id });
					throw new StorageStreamNotDisposedException("An un-disposed storage stream exists for this storage. Dispose the stream before persisting the storage.");
				}
				string text = GenerateStoragePath(newId, persisted: true);
				FileStorage.RenameStorage(this, newId);
				if (FileAccess.Exists(FilePath))
				{
					FileAccess.Move(FilePath, text);
				}
				Id = newId;
				FilePath = text;
				Persisted = true;
				Accessed();
			}
		}

		public void Remove(bool forceDisposeStream)
		{
			if (forceDisposeStream && Stream != null)
			{
				Stream.Dispose();
				Stream = null;
			}
			if (Stream != null)
			{
				LoggerExtensions.LogError(Logger, "(ServerFileStorage) An un-disposed storage stream exists for storage with ID {Id}", new object[1] { Id });
				throw new StorageStreamNotDisposedException("An un-disposed storage stream exists for this storage. Dispose the stream first.");
			}
			if (FileAccess.Exists(FilePath))
			{
				FileAccess.Delete(FilePath);
			}
			Quota.Free(QuotaId);
		}

		private string GenerateStoragePath(string id, bool persisted)
		{
			if (!persisted)
			{
				return Path.Combine(FileStorage.RootDirectory, FileStorage.TemporarySubdirectory, id);
			}
			return Path.Combine(FileStorage.RootDirectory, id);
		}

		public void Accessed()
		{
			TimeSpan timeSpan = (Persisted ? FileStorage.PersistedEntryDuration : FileStorage.TemporaryEntryDuration);
			UtcExpiration = DateTime.UtcNow + timeSpan;
		}
	}

	private const string TemporarySubdirectoryPath = "Temp";

	private readonly object _storageLock = new object();

	private readonly Dictionary<string, StorageEntry> _storageMap;

	private bool _disposed;

	private IServerFileAccess FileAccess { get; }

	private ILogger Logger { get; }

	private IServerFileStorageCleanupService PersistedCleanupService { get; }

	private IServerFileStorageCleanupService TemporaryCleanupService { get; }

	public string RootDirectory { get; }

	public string TemporarySubdirectory { get; }

	public IServerFileStorageQuota Quota { get; }

	private TimeSpan TemporaryEntryDuration { get; }

	private TimeSpan PersistedEntryDuration { get; }

	public ServerFileStorage(IServerFileAccess fileAccess, ILogger logger, string rootDirectory, IServerFileStorageQuota quota, IServerFileStorageCleanupService persistedCleanupService, IServerFileStorageCleanupService temporaryCleanupService, TimeSpan persistedEntryDuration, TimeSpan temporaryEntryDuration)
	{
		FileAccess = fileAccess ?? throw new ArgumentNullException("fileAccess");
		Logger = logger ?? throw new ArgumentNullException("logger");
		RootDirectory = rootDirectory ?? throw new ArgumentNullException("rootDirectory");
		Quota = quota ?? throw new ArgumentNullException("quota");
		TemporarySubdirectory = "Temp";
		_storageMap = new Dictionary<string, StorageEntry>(StringComparer.OrdinalIgnoreCase);
		PersistedEntryDuration = persistedEntryDuration;
		TemporaryEntryDuration = temporaryEntryDuration;
		PersistedCleanupService = persistedCleanupService ?? throw new ArgumentNullException("persistedCleanupService");
		TemporaryCleanupService = temporaryCleanupService ?? throw new ArgumentNullException("temporaryCleanupService");
		PersistedCleanupService.Start(new PersistedCleanupProxy(this));
		TemporaryCleanupService.Start(new TemporaryCleanupProxy(this));
	}

	public void PurgeDataFolder()
	{
		string path = Path.Combine(RootDirectory, TemporarySubdirectory);
		foreach (string item in FileAccess.GetFiles(RootDirectory).Concat(FileAccess.GetFiles(path)))
		{
			FileAccess.Delete(item);
		}
	}

	public IServerFileStorageEntry CreateStorage()
	{
		if (_disposed)
		{
			throw new ObjectDisposedException("ServerFileStorage");
		}
		lock (_storageLock)
		{
			StorageEntry storageEntry = new StorageEntry(this);
			AddStorageInternal(storageEntry);
			LoggerExtensions.LogInformation(Logger, "(ServerFileStorage) Created new storage with ID {StorageEntryId}", new object[1] { storageEntry.Id });
			return storageEntry;
		}
	}

	public void ReturnStorage(IServerFileStorageEntry storage)
	{
		storage.NullCheck("", "ReturnStorage", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\ServerFileStorage.cs", 143);
		if (_disposed)
		{
			throw new ObjectDisposedException("ServerFileStorage");
		}
		lock (_storageLock)
		{
			LoggerExtensions.LogInformation(Logger, "(ServerFileStorage) Returning storage with ID {StorageId}", new object[1] { storage.Id });
			StorageEntry storageInternal = GetStorageInternal(storage.Id);
			RemoveStorageInternal(storageInternal, forceDisposeStream: false);
		}
	}

	public IServerFileStorageEntry GetStorage(string id)
	{
		id.NullCheck("", "GetStorage", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\Shared\\Common.Integration\\prod\\VideoOS.Common.Integration\\ServerStorage\\ServerFileStorage.cs", 159);
		if (_disposed)
		{
			throw new ObjectDisposedException("ServerFileStorage");
		}
		lock (_storageLock)
		{
			if (!StorageExists(id))
			{
				return null;
			}
			return GetStorageInternal(id);
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				Cleanup();
			}
			_disposed = true;
		}
	}

	private StorageEntry GetStorageInternal(string storageId)
	{
		StorageEntry storageEntry = _storageMap[storageId];
		storageEntry.Accessed();
		return storageEntry;
	}

	private void AddStorageInternal(StorageEntry storage)
	{
		_storageMap.Add(storage.Id, storage);
	}

	private void RemoveStorageInternal(StorageEntry storage, bool forceDisposeStream)
	{
		storage.Remove(forceDisposeStream);
		_storageMap.Remove(storage.Id);
	}

	private bool StorageExists(string storageId)
	{
		return _storageMap.ContainsKey(storageId);
	}

	private void RenameStorage(StorageEntry storageEntry, string newId)
	{
		lock (_storageLock)
		{
			if (StorageExists(newId))
			{
				LoggerExtensions.LogError(Logger, "(ServerFileStorage) A storage with ID {NewId} already exists. Can not rename storage {StorageEntryId}", new object[2] { newId, storageEntry.Id });
				throw new InvalidOperationException("A storage with the same ID already exists.");
			}
			_storageMap.Remove(storageEntry.Id);
			_storageMap[newId] = storageEntry;
		}
	}

	private void Cleanup()
	{
		lock (_storageLock)
		{
			StorageEntry[] array = _storageMap.Select((KeyValuePair<string, StorageEntry> kv) => kv.Value).ToArray();
			foreach (StorageEntry storage in array)
			{
				RemoveStorageInternal(storage, forceDisposeStream: true);
			}
		}
	}
}
