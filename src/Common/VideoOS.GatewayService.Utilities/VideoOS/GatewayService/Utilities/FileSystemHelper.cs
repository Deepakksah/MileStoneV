using System.IO;

namespace VideoOS.GatewayService.Utilities;

public class FileSystemHelper : IFileSystemHelper
{
	public bool DirectoryExists(string path)
	{
		return Directory.Exists(path);
	}
}
