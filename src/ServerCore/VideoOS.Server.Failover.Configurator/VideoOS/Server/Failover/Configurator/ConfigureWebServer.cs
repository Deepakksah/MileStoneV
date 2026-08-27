using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class ConfigureWebServer : IConfigurationStep
{
	public string PrivateSafekitBinaryFolderPath { get; set; }

	public string WebConsolePassword { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public IProtectionProvider ProtectionProvider { get; set; }

	public string ServerConfigPath { get; set; }

	public ConfigureWebServer(Runspace runspace, ILogger log)
	{
		Log = log;
		Runspace = runspace;
	}

	public async Task ExecuteAsync()
	{
		PowerShell powerShell = PowerShell.Create();
		powerShell.Runspace = Runspace;
		powerShell.AddCommand(".\\ConfigureWebServer.ps1");
		powerShell.AddParameter("-PrivateBinFolderLocation", PrivateSafekitBinaryFolderPath);
		powerShell.AddParameter("-WebServerPassword", WebConsolePassword);
		string value = ProtectionProvider.Encode(WebConsolePassword);
		powerShell.AddParameter("-EncodedPassword", value);
		powerShell.AddParameter("-ServerConfigPath", ServerConfigPath);
		await PowershellHelper.InvokeAndThrowIfErrorAsync(powerShell, Log);
	}
}
