using System.IO;
using System.Security.AccessControl;

namespace VideoOS.Common.EnvironmentServices;

public interface IDirectoryService
{
	void CreateDirectory(string directoryName);

	bool Exists(string directoryName);

	string[] GetFiles(string path, string searchPattern, SearchOption searchOption);

	string[] GetDirectories(string path, string searchPattern);

	DirectorySecurity GetAccessControl(string path);

	void SetAccessControl(string path, DirectorySecurity directorySecurity);
}
