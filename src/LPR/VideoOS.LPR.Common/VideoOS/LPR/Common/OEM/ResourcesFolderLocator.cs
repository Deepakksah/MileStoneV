using System;
using System.IO;
using System.Reflection;

namespace VideoOS.LPR.Common.OEM;

public class ResourcesFolderLocator : IResourcesFolderLocator
{
	private readonly string _resourceFilename;

	private readonly bool _useGlobalResourceFile;

	public string ResourceFilePath
	{
		get
		{
			Assembly assembly = (_useGlobalResourceFile ? Assembly.GetEntryAssembly() : Assembly.GetCallingAssembly());
			if (assembly == null)
			{
				return _resourceFilename;
			}
			string path = Path.GetDirectoryName(assembly.Location);
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

	public ResourcesFolderLocator(string resourceFileName, bool useGlobalResourceFile)
	{
		_resourceFilename = resourceFileName;
		_useGlobalResourceFile = useGlobalResourceFile;
	}
}
