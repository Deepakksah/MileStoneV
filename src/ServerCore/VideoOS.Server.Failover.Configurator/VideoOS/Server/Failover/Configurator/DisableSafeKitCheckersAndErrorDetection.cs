using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class DisableSafeKitCheckersAndErrorDetection : IConfigurationStep
{
	public string ModuleName { get; set; }

	public string SafekitPath { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public DisableSafeKitCheckersAndErrorDetection(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\DisableSafeKitCheckersAndErrorDetection.ps1");
		powerShell.AddParameter("-ModuleName", ModuleName);
		powerShell.AddParameter("-EnvironmentInstallPath", SafekitPath);
		try
		{
			await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
		}
		catch (ScriptErrorException)
		{
			Log.LogDebug("DisableSafeKitCheckersAndErrorDetection ScriptErrorException - swallowing script error");
		}
	}
}
