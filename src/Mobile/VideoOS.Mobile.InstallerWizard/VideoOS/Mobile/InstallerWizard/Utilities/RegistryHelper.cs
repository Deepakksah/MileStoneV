using VideoOS.Mobile.CommonUtilities;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.InstallerWizard.Utilities;

public class RegistryHelper : IRegistryProvider
{
	public IRegistryHelper Instance;

	public IRegistryHelper GetRegistryInstance()
	{
		if (Instance == null)
		{
			Instance = new RegistryHelperBase();
			Instance.SubKeyName = OemResources.Instance.MobileServerLocalMachineRegistryKey;
		}
		return Instance;
	}
}
