using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class RegisterServices : IConfigurationStep
{
	public string ServerConfiguratorPath { get; set; }

	public string IdpAddress { get; set; }

	private ILogger Log { get; }

	public bool Deconfigure { get; set; }

	public RegisterServices(ILogger log)
	{
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		Process proc = new Process();
		proc.StartInfo.WorkingDirectory = ServerConfiguratorPath;
		proc.StartInfo.FileName = Path.Combine(ServerConfiguratorPath, "ServerConfigurator.exe");
		if (!Deconfigure)
		{
			proc.StartInfo.Arguments = "/register /managementserveraddress=" + IdpAddress + " /overridelocalmanagementserver /quiet";
		}
		else
		{
			proc.StartInfo.Arguments = "/register /managementserveraddress=" + IdpAddress + " /quiet";
		}
		proc.StartInfo.RedirectStandardOutput = true;
		proc.StartInfo.UseShellExecute = false;
		proc.StartInfo.CreateNoWindow = true;
		proc.Start();
		string commandOutput = string.Empty;
		await Task.Factory.StartNew(delegate
		{
			commandOutput = proc.StandardOutput.ReadToEnd();
			proc.WaitForExit();
		});
		Log.LogInformation("Server configurator output: {CommandOutput}", commandOutput);
		Log.LogInformation("Server configurator exit code: {ExitCode}", proc.ExitCode);
		if (proc.ExitCode != 0)
		{
			Log.LogError("Unsuccessful exit code detected.");
			throw new RegisterServicesException();
		}
	}
}
