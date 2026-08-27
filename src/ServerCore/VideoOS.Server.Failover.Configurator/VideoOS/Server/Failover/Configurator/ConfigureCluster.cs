using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureCluster : IConfigurationStep
{
	public string ClusterXmlPath { get; set; }

	public string PrimaryNodeName { get; set; }

	public string PrimaryNodeAddress { get; set; }

	public string SecondaryNodeName { get; set; }

	public string SecondaryNodeAddress { get; set; }

	public string SafekitPath { get; set; }

	public bool Deconfigure { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public ConfigureCluster(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureCluster.ps1");
		powerShell.AddParameter("-EnvironmentInstallPath", SafekitPath);
		if (Deconfigure)
		{
			powerShell.AddParameter("-Deconfigure");
		}
		else
		{
			powerShell.AddParameter("-Configure");
			powerShell.AddParameter("-ClusterFilepath", ClusterXmlPath);
			powerShell.AddParameter("-ClusterFirstNodeName", PrimaryNodeName);
			powerShell.AddParameter("-ClusterFirstNodeAddress", PrimaryNodeAddress);
			powerShell.AddParameter("-ClusterSecondNodeName", SecondaryNodeName);
			powerShell.AddParameter("-ClusterSecondNodeAddress", SecondaryNodeAddress);
		}
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
