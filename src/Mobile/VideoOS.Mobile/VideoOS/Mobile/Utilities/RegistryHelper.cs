using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Utilities;

public class RegistryHelper
{
	public static RegistryHelperBase Instance;

	public static void Initialize(string key)
	{
		if (Instance == null)
		{
			Instance = new RegistryHelperBase
			{
				SubKeyName = key
			};
		}
	}
}
