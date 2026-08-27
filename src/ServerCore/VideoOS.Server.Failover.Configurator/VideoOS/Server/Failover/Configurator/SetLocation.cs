using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class SetLocation : IConfigurationStep
{
	public string Path { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public SetLocation(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand("Set-Location");
		powerShell.AddParameter("-Path", Path);
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
