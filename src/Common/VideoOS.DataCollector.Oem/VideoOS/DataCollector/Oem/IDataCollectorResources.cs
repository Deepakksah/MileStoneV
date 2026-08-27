namespace VideoOS.DataCollector.Oem;

public interface IDataCollectorResources
{
	string CommonApplicationDataFolder { get; }

	string ConfigFileName { get; }

	string DataCollectorServiceInstallDir { get; }

	string DataCollectorServiceName { get; }
}
