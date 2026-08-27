using System;
using System.IO;
using System.Reflection;

namespace VideoOS.EventServer.Util.Resources;

public class ResourcesFolderLocator : IResourcesFolderLocator
{
	private readonly string _resourceFilename;

	public string GetResourceFilePath
	{
		get
		{
			string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			if (!File.Exists(Path.Combine(path, _resourceFilename)))
			{
				path = AppDomain.CurrentDomain.BaseDirectory;
				if (AppDomain.CurrentDomain.RelativeSearchPath != null && AppDomain.CurrentDomain.RelativeSearchPath.Length > 0)
				{
					path = ((!Path.IsPathRooted(AppDomain.CurrentDomain.RelativeSearchPath)) ? new DirectoryInfo(Path.Combine(path, AppDomain.CurrentDomain.RelativeSearchPath)).FullName : AppDomain.CurrentDomain.RelativeSearchPath);
				}
			}
			return Path.Combine(path, _resourceFilename);
		}
	}

	public ResourcesFolderLocator(string resourceFileName)
	{
		_resourceFilename = resourceFileName;
	}
}
