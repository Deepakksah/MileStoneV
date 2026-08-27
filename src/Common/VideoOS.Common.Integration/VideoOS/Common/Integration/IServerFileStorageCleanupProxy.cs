using System.Collections.Generic;

namespace VideoOS.Common.Integration;

public interface IServerFileStorageCleanupProxy
{
	IEnumerable<IServerFileStorageCleanupEntry> Entries { get; }

	void Remove(IServerFileStorageCleanupEntry entry);

	bool LockEntries();

	void UnlockEntries();
}
