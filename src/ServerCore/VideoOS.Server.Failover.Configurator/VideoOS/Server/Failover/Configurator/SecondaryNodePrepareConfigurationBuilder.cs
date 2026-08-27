using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class SecondaryNodePrepareConfigurationBuilder : IConfigurationBuilder
{
	public bool EnableHttps { get; set; }

	public string ConfigurationScriptsPath { get; set; }

	public string SafekitPath { get; set; }

	public string UserPassword { get; set; }

	public string SecondaryNodeFqdn { get; set; }

	public string WebConsolePassword { get; set; }

	public IProtectionProvider ProtectionProvider { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public string ServerConfigPath { get; set; }

	public SecondaryNodePrepareConfigurationBuilder(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public IEnumerable<IConfigurationStep> Build()
	{
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new StartSafekitServices(Runspace, Log);
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureFirewall(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin")
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureWebServer(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin"),
			WebConsolePassword = WebConsolePassword,
			ProtectionProvider = ProtectionProvider,
			ServerConfigPath = ServerConfigPath
		};
		if (EnableHttps)
		{
			yield return new StartCertificateServer(Log)
			{
				SafekitPath = SafekitPath,
				UserPassword = UserPassword,
				LocalMachineFqdn = SecondaryNodeFqdn
			};
		}
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureServices(Runspace, Log);
	}
}
