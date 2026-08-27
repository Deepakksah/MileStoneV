using VideoOS.Platform;

namespace VideoOS.Mobile.LoginThread;

public interface IMipSdkWraper
{
	Item GetCurrentServerConfiguration();

	void SaveUpdatedItemConfiguration(Item item, bool updateConfigurationId);
}
