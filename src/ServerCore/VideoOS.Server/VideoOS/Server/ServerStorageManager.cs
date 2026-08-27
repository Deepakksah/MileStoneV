using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class ServerStorageManager : IServerStorageManager, IDisposable
{
	private object _lock = new object();

	private IDictionary<Guid, StreamWithStorage> transferIdToStorageIdMap;

	public ServerStorageManager()
	{
		transferIdToStorageIdMap = new ConcurrentDictionary<Guid, StreamWithStorage>();
	}

	public StreamWithStorage GetStreamWithId(Guid transferId)
	{
		lock (_lock)
		{
			if (!transferIdToStorageIdMap.TryGetValue(transferId, out var value))
			{
				IServerFileStorageEntry serverFileStorageEntry = Server.Instance.ServerFileStorage.CreateStorage();
				Stream fileStream = serverFileStorageEntry.CreateStream();
				value = new StreamWithStorage
				{
					Storage = serverFileStorageEntry,
					FileStream = fileStream
				};
				transferIdToStorageIdMap[transferId] = value;
			}
			return value;
		}
	}

	public void ReturnStream(Guid transferId)
	{
		if (transferIdToStorageIdMap.TryGetValue(transferId, out var value))
		{
			value.FileStream.Dispose();
			value.FileStream = null;
		}
	}

	public void Dispose()
	{
		foreach (StreamWithStorage value in transferIdToStorageIdMap.Values)
		{
			value.FileStream?.Dispose();
			Server.Instance.ServerFileStorage.ReturnStorage(value.Storage);
		}
		GC.SuppressFinalize(this);
	}
}
