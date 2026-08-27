using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class VerifyDeconfiguration : IConfigurationStep
{
	public string SafekitPath { get; set; }

	public string ModuleName { get; set; }

	public ILogger Log { get; }

	public VerifyDeconfiguration(ILogger log)
	{
		Log = log;
	}

	public async Task ExecuteAsync()
	{
		Process proc = new Process();
		proc.StartInfo.WorkingDirectory = SafekitPath;
		proc.StartInfo.FileName = Path.Combine(SafekitPath, "safekit");
		proc.StartInfo.Arguments = "module listid";
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
		if (commandOutput.IndexOf(ModuleName, StringComparison.OrdinalIgnoreCase) > 0)
		{
			Log.LogError("Deconfiguration verification failed, {ModuleName} module found in cluster configuration: {CommandOutput}", ModuleName, commandOutput);
			throw new VerifyDeconfigurationException();
		}
		Log.LogInformation("Deconfiguration verification success.");
	}
}
