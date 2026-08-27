using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class StartCertificateServer : IConfigurationStep
{
	private ILogger Logger;

	public string SafekitPath { get; set; }

	public string UserPassword { get; set; }

	public string LocalMachineFqdn { get; set; }

	public StartCertificateServer(ILogger log)
	{
		Logger = log;
	}

	public async Task ExecuteAsync()
	{
		Process proc = new Process();
		proc.StartInfo.WorkingDirectory = Path.Combine(SafekitPath, "web\\bin");
		proc.StartInfo.FileName = Path.Combine(SafekitPath, "web\\bin", "startcaserv.cmd");
		proc.StartInfo.Arguments = UserPassword + " " + UserPassword;
		proc.StartInfo.CreateNoWindow = true;
		proc.StartInfo.UseShellExecute = false;
		proc.Start();
		await Task.Factory.StartNew(delegate
		{
			proc.WaitForExit();
		});
		string text = await FailoverConfigurationHelper.ValidateCertificateAuthorityConnection(Logger, LocalMachineFqdn, UserPassword);
		if (!string.IsNullOrEmpty(text))
		{
			throw new StartCertificateServerException(text);
		}
	}
}
