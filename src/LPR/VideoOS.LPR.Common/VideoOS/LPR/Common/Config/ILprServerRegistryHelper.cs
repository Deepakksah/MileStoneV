using System;

namespace VideoOS.LPR.Common.Config;

public interface ILprServerRegistryHelper
{
	string ServerInstanceId { get; set; }

	Uri ServerHostUri { get; }

	Uri ServerUri { get; set; }

	string Language { get; }

	string InstallDir { get; }

	bool SetServerUriInRegistry(Uri uri);

	void DeleteDefaultRegistryKey();

	void CleanCache();
}
