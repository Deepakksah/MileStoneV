using System;
using System.Collections.Generic;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class UserCertificatesCreationConfigurationBuilder : IConfigurationBuilder
{
	public string ConfigurationScriptsPath { get; set; }

	public string SafekitPath { get; set; }

	public string SaveToFolder { get; set; }

	public string LocalMachineFqdn { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public UserCertificatesCreationConfigurationBuilder(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public IEnumerable<IConfigurationStep> Build()
	{
		Random random = new Random();
		string randomPassword = random.Next(100000, 1000000).ToString();
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new StartCertificateServer(Log)
		{
			SafekitPath = SafekitPath,
			UserPassword = randomPassword,
			LocalMachineFqdn = LocalMachineFqdn
		};
		yield return new GenerateCertificates(Log)
		{
			CaPassword = randomPassword,
			SaveToFolder = SaveToFolder
		};
		yield return new StopCertificateServer
		{
			SafekitPath = SafekitPath
		};
	}
}
