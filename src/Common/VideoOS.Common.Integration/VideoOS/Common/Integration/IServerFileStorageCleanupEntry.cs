using System;

namespace VideoOS.Common.Integration;

public interface IServerFileStorageCleanupEntry
{
	string Id { get; }

	DateTime UtcExpiration { get; }
}
