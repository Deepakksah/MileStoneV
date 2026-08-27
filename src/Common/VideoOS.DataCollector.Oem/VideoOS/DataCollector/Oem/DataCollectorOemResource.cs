using VideoOS.Utilities.OemResource;

namespace VideoOS.DataCollector.Oem;

public class DataCollectorOemResource
{
	private static object _instanceSync = new object();

	private static volatile DataCollectorResources _instance;

	public static DataCollectorResources Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}
			lock (_instanceSync)
			{
				if (_instance == null)
				{
					_instance = new DataCollectorResources(new ResourcesFolderLocator());
				}
			}
			return _instance;
		}
	}
}
