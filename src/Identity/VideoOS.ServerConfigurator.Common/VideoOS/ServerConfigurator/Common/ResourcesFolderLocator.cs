using System.IO;
using System.Reflection;

namespace VideoOS.ServerConfigurator.Common;

public class ResourcesFolderLocator : IResourcesFolderLocator
{
	private readonly string _resourceFilename;

	public string GetResourceFilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), _resourceFilename);

	public ResourcesFolderLocator(string resourceFileName)
	{
		_resourceFilename = resourceFileName;
	}
}
