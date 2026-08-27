using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class StopSafekitServices : IConfigurationStep
{
	private readonly Runspace _runSpace;

	private readonly ILogger _log;

	public StopSafekitServices(Runspace runSpace, ILogger log)
	{
		_runSpace = runSpace;
		_log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = _runSpace;
		powerShell.AddCommand(".\\ConfigureNativeFailoverServices.ps1");
		powerShell.AddParameter("-Stop");
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, _log);
	}
}
