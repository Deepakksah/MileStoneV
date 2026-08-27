using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

namespace VideoOS.Server.Failover.Configurator;

public class StopCertificateServer : IConfigurationStep
{
	public string SafekitPath { get; set; }

	public async Task ExecuteAsync()
	{
		Process proc = new Process();
		proc.StartInfo.WorkingDirectory = Path.Combine(SafekitPath, "web\\bin");
		proc.StartInfo.FileName = Path.Combine(SafekitPath, "web\\bin", "stopcaserv.cmd");
		proc.StartInfo.UseShellExecute = false;
		proc.StartInfo.CreateNoWindow = true;
		proc.Start();
		await Task.Factory.StartNew(delegate
		{
			proc.WaitForExit();
		});
	}
}
