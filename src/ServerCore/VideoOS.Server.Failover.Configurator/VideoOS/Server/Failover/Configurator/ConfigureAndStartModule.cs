using System;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureAndStartModule : IConfigurationStep
{
	public string SecondaryNodeName { get; set; }

	public string ModuleName { get; set; }

	public string SafekitPath { get; set; }

	public string VirtualIp { get; set; }

	public string SqlServerPathData { get; set; }

	public string SqlServerPathLog { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public ConfigureAndStartModule(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		if (string.Equals(SqlServerPathData, SqlServerPathLog, StringComparison.InvariantCultureIgnoreCase))
		{
			SqlServerPathLog = string.Empty;
		}
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureAndStartModule.ps1");
		powerShell.AddParameter("-ModuleName", ModuleName);
		powerShell.AddParameter("-SecondNodeName", SecondaryNodeName);
		powerShell.AddParameter("-EnvironmentInstallPath", SafekitPath);
		powerShell.AddParameter("-ReplicatedDirectory1", SqlServerPathData);
		powerShell.AddParameter("-ReplicatedDirectory2", SqlServerPathLog);
		powerShell.AddParameter("-VirtualIpAddress", VirtualIp);
		try
		{
			await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
		}
		catch (ScriptErrorException)
		{
			Log.LogDebug("ConfigureAndStartModule ScriptErrorException - swallowing script error");
		}
	}
}
