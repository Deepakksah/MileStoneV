using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureServices : IConfigurationStep
{
	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public bool Deconfigure { get; set; }

	public ConfigureServices(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureServices.ps1");
		powerShell.AddParameter("-ManagementServerServiceName", OemResources.Instance.Server.ManagementServerServiceName);
		powerShell.AddParameter("-LogServerServiceName", OemResources.Instance.Server.LogServerServiceName);
		powerShell.AddParameter("-EventServerServiceName", OemResources.Instance.Server.EventServerServiceName);
		powerShell.AddParameter("-DataCollectorServiceName", OemResources.Instance.Server.DataCollectorServiceName);
		if (Deconfigure)
		{
			powerShell.AddParameter("-Deconfigure");
		}
		else
		{
			powerShell.AddParameter("-Configure");
		}
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
