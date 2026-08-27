using VideoOS.Utilities.OemResource;

namespace VideoOS.Common.Integration.Oem;

public class ServiceChannelResources : ApplicationResources
{
	public string ApplicationName => GetResourceString("ServiceChannelApplicationName");

	public string FullName => GetResourceString("ServiceChannelFullName").Replace("SERVICECHANNELVERSION", Version);

	public string DefaultInstallDirectory => GetResourceString("ServiceChannelInstallDir");

	public override string Version => GetResourceString("ServiceChannelVersion");

	public string ProductVersion => GetResourceString("ServiceChannelProductVersion");

	public ServiceChannelResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
