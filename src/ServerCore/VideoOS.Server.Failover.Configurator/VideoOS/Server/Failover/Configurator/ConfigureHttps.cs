using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureHttps : IConfigurationStep
{
	public string SafekitPath { get; set; }

	public string PrimaryNodeAddress { get; set; }

	public string SecondaryNodeAddress { get; set; }

	public string HttpsPassword { get; set; }

	public bool Deconfigure { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public ConfigureHttps(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureHTTPS.ps1");
		powerShell.AddParameter("-userpwd", HttpsPassword);
		powerShell.AddParameter("-skbase", SafekitPath);
		if (!Deconfigure)
		{
			powerShell.AddParameter("-Configure");
			powerShell.AddParameter("-machine1hostname", PrimaryNodeAddress);
			powerShell.AddParameter("-machine2hostname", SecondaryNodeAddress);
		}
		else
		{
			powerShell.AddParameter("-Deconfigure");
		}
		try
		{
			await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
		}
		catch (ScriptErrorException)
		{
			Log.LogDebug("ConfigureHTTPS ScriptErrorException - swallowing script error.");
		}
	}
}
