using System.IO;
using VideoOS.Common.Integration;

namespace VideoOS.Server;

public sealed class StreamWithStorage
{
	public IServerFileStorageEntry Storage { get; set; }

	public Stream FileStream { get; set; }
}
