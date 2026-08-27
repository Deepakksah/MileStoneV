using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VideoOS.Common.Integration;

public class ServerFileAccess : IServerFileAccess
{
	public void Delete(string path)
	{
		File.Delete(path);
	}

	public bool Exists(string path)
	{
		return File.Exists(path);
	}

	public void Move(string source, string destination)
	{
		File.Move(source, destination);
	}

	public Stream Open(string path, FileMode mode)
	{
		return File.Open(path, mode);
	}

	public void CreateDirectory(string path)
	{
		Directory.CreateDirectory(path);
	}

	public IEnumerable<string> GetFiles(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		if (directoryInfo.Exists)
		{
			return from fi in directoryInfo.GetFiles()
				select fi.FullName;
		}
		return Enumerable.Empty<string>();
	}
}
