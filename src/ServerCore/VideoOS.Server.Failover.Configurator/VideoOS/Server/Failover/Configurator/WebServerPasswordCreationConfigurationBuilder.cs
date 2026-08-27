using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class WebServerPasswordCreationConfigurationBuilder : IConfigurationBuilder
{
	public string ConfigurationScriptsPath { get; set; }

	public string ServerConfigPath { get; set; }

	public string SafekitPath { get; set; }

	public string WebServerPassword { get; set; }

	public IProtectionProvider ProtectionProvider { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public WebServerPasswordCreationConfigurationBuilder(Runspace runspace, ILogger log)
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
		yield return new ConfigureWebServer(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin"),
			WebConsolePassword = WebServerPassword,
			ProtectionProvider = ProtectionProvider,
			ServerConfigPath = ServerConfigPath
		};
	}
}
