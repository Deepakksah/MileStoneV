using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.InstallerWizard.Utilities;

public interface IRegistryProvider
{
	IRegistryHelper GetRegistryInstance();
}
