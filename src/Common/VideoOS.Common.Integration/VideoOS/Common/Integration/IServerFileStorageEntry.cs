using System.IO;

namespace VideoOS.Common.Integration;

public interface IServerFileStorageEntry
{
	string Id { get; }

	string FilePath { get; }

	bool Persisted { get; }

	void Persist(string newId);

	Stream CreateStream();
}
