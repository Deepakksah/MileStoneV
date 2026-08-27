using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureFirewall : IConfigurationStep
{
	public string PrivateSafekitBinaryFolderPath { get; set; }

	public bool Deconfigure { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public ConfigureFirewall(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureFirewall.ps1");
		powerShell.AddParameter("-PrivateBinFolderLocation", PrivateSafekitBinaryFolderPath);
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
