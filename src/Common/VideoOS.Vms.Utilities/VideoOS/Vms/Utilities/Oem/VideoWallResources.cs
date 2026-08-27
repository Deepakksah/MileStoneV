using VideoOS.Utilities.OemResource;

namespace VideoOS.Vms.Utilities.Oem;

public class VideoWallResources : ApplicationResources
{
	public string ApplicationName => GetResourceString("VideoWallApplicationName");

	public string ApplicationNameShort => GetResourceString("VideoWallApplicationNameShort");

	public string FullName => GetResourceString("VideoWallFullName").Replace("SMARTWALLVERSION", Version);

	public string FullNameInstaller => GetResourceString("VideoWallInstallerFullName").Replace("SMARTWALLVERSION", Version);

	public override string Version => GetResourceString("VideoWallVersion");

	public string DefaultInstallDirectory => GetResourceString("VideoWallInstallDir");

	public VideoWallResources(OemResourcePackage resourcePackage)
		: base(resourcePackage)
	{
	}
}
