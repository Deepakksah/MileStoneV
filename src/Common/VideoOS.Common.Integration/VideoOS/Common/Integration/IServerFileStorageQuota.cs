using System;

namespace VideoOS.Common.Integration;

public interface IServerFileStorageQuota
{
	bool Update(Guid consumerId, ulong consumerUsage);

	void Free(Guid consumerId);
}
