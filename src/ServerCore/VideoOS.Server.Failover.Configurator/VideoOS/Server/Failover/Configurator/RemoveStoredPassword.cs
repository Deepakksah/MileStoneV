using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class RemoveStoredPassword : IConfigurationStep
{
	public string ServerConfigPath { get; set; }

	private ILogger Log { get; set; }

	private Runspace Runspace { get; set; }

	public RemoveStoredPassword(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\RemoveStoredCypher.ps1");
		powerShell.AddParameter("-ServerConfigPath", ServerConfigPath);
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
