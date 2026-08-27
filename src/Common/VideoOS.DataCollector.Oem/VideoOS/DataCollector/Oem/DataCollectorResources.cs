using System;
using System.IO;
using VideoOS.Utilities.OemResource;

namespace VideoOS.DataCollector.Oem;

public class DataCollectorResources : OemResourceReaderBase, IDataCollectorResources
{
	public string DataCollectorServiceName => GetResourceString("DataCollectorServiceName");

	public string ConfigFileName => "appSettings.json";

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyName), DataCollectorServiceInstallDir);

	public string DataCollectorServiceInstallDir => GetResourceString("DataCollectorServiceInstallDir");

	private string CompanyName => GetResourceString("CompanyName");

	public DataCollectorResources(IResourcesFolderLocator resourcesFolderLocator)
		: base(resourcesFolderLocator)
	{
	}
}
