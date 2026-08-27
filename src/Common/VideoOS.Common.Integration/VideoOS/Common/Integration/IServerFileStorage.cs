using System;

namespace VideoOS.Common.Integration;

public interface IServerFileStorage : IDisposable
{
	string RootDirectory { get; }

	IServerFileStorageQuota Quota { get; }

	void PurgeDataFolder();

	IServerFileStorageEntry CreateStorage();

	void ReturnStorage(IServerFileStorageEntry storage);

	IServerFileStorageEntry GetStorage(string id);
}
