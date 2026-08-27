using VideoOS.Utilities.OemResource;

namespace VideoOS.DataCollector.Oem;

public class FailoverServerResources : OemResourceReaderBase, IFailoverServerResources
{
	public string FailoverServiceName => GetResourceString("FailoverServiceName");

	public FailoverServerResources(IResourcesFolderLocator resourcesFolderLocator)
		: base(resourcesFolderLocator)
	{
	}
}
