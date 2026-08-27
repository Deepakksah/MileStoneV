using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class EnableSafeKitCheckersAndErrorDetection : IConfigurationStep
{
	public string ModuleName { get; set; }

	public string SafekitPath { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public EnableSafeKitCheckersAndErrorDetection(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\EnableSafeKitCheckersAndErrorDetection.ps1");
		powerShell.AddParameter("-ModuleName", ModuleName);
		powerShell.AddParameter("-EnvironmentInstallPath", SafekitPath);
		try
		{
			await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
		}
		catch (ScriptErrorException)
		{
			Log.LogDebug("EnableSafeKitCheckersAndErrorDetection ScriptErrorException - swallowing script error");
		}
	}
}
