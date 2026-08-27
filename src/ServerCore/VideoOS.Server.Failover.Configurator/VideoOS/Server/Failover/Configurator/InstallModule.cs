using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class InstallModule : IConfigurationStep
{
	public string ModuleName { get; set; }

	public string SafekitPath { get; set; }

	public string ConfigurationScriptsPath { get; set; }

	public bool Deconfigure { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public InstallModule(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\InstallModule.ps1");
		powerShell.AddParameter("-ModuleName", ModuleName);
		powerShell.AddParameter("-EnvironmentInstallPath", SafekitPath);
		if (Deconfigure)
		{
			powerShell.AddParameter("-Deconfigure");
		}
		else
		{
			powerShell.AddParameter("-Configure");
			powerShell.AddParameter("-ModulePath", Path.Combine(ConfigurationScriptsPath, ModuleName + ".safe"));
		}
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
