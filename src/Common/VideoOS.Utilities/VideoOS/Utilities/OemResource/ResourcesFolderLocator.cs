using System.IO;
using System.Reflection;

namespace VideoOS.Utilities.OemResource;

public class ResourcesFolderLocator : IResourcesFolderLocator
{
	private const string ResourceFilename = "resources.xml";

	public string ResourceFilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources.xml");
}
