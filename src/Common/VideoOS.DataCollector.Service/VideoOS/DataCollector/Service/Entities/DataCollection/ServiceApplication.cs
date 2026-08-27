namespace VideoOS.DataCollector.Service.Entities.DataCollection;

public class ServiceApplication
{
	public string ApplicationName { get; }

	public string UpgradeCode { get; }

	public bool IsInstalled { get; set; }

	public ServiceApplication(string applicationName, string upgradeCode)
	{
		ApplicationName = applicationName;
		UpgradeCode = upgradeCode;
	}
}
