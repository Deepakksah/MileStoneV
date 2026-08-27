using System;

namespace VideoOS.Common.Integration;

public interface IServerFileStorageCleanupService : IDisposable
{
	void Start(IServerFileStorageCleanupProxy cleanupProxy);
}
