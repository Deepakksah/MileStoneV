using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.AccessControl;

namespace VideoOS.Common.EnvironmentServices;

[SupportedOSPlatform("Windows")]
[ExcludeFromCodeCoverage]
public class DirectoryService : IDirectoryService
{
	public bool Exists(string directoryName)
	{
		return Directory.Exists(directoryName);
	}

	public void CreateDirectory(string directoryName)
	{
		Directory.CreateDirectory(directoryName);
	}

	public string[] GetFiles(string path, string searchPattern, SearchOption searchOption)
	{
		return Directory.GetFiles(path, searchPattern, searchOption);
	}

	public string[] GetDirectories(string path, string searchPattern)
	{
		return Directory.GetDirectories(path, searchPattern);
	}

	public DirectorySecurity GetAccessControl(string path)
	{
		return Directory.GetAccessControl(path);
	}

	public void SetAccessControl(string path, DirectorySecurity directorySecurity)
	{
		Directory.SetAccessControl(path, directorySecurity);
	}
}
