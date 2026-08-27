using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace VideoOS.Server.Failover.Configurator;

public class ShutdownSafekit : IConfigurationStep
{
	public string SafekitPath { get; set; }

	public async Task ExecuteAsync()
	{
		Process proc = new Process();
		proc.StartInfo.WorkingDirectory = SafekitPath;
		proc.StartInfo.FileName = Path.Combine(SafekitPath, "safekit");
		proc.StartInfo.Arguments = "shutdown";
		proc.StartInfo.CreateNoWindow = true;
		proc.StartInfo.UseShellExecute = false;
		proc.Start();
		await Task.Factory.StartNew(delegate
		{
			proc.WaitForExit();
		});
	}
}
