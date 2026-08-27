using System;

namespace VideoOS.Server;

public interface IServerStorageManager
{
	StreamWithStorage GetStreamWithId(Guid transferId);

	void ReturnStream(Guid transferId);
}
