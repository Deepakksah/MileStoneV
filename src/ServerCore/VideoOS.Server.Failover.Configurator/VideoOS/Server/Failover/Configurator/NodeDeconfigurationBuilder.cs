using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class NodeDeconfigurationBuilder : IConfigurationBuilder
{
	public string ConfigurationScriptsPath { get; set; }

	public string SafekitPath { get; set; }

	public string ModuleName { get; set; }

	public string ServerConfiguratorPath { get; set; }

	public string ServerConfigPath { get; set; }

	public string IdpAddress { get; set; }

	public string DataCollectorConfigFilePath { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public NodeDeconfigurationBuilder(Runspace runspace, ILogger log)
	{
		Runspace = runspace;
		Log = log;
	}

	public IEnumerable<IConfigurationStep> Build()
	{
		yield return new ShutdownSafekit
		{
			SafekitPath = SafekitPath
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureCluster(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			Deconfigure = true
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new InstallModule(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			ModuleName = ModuleName,
			Deconfigure = true
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureServices(Runspace, Log)
		{
			Deconfigure = true
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureHttps(Runspace, Log)
		{
			SafekitPath = SafekitPath,
			HttpsPassword = "123456",
			Deconfigure = true
		};
		yield return new StopCertificateServer
		{
			SafekitPath = SafekitPath
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new SetClusterMembershipFlag(Runspace, Log)
		{
			ServerConfigPath = ServerConfigPath,
			IsClusterMember = false,
			ClusterLocalWebserverUriString = "",
			ClusterModuleName = "",
			SafekitVirtualAddress = ""
		};
		yield return new UpdateDataCollectorAppConfig(string.Empty, string.Empty, DataCollectorConfigFilePath, Log)
		{
			Deconfigure = true
		};
		yield return new RegisterServices(Log)
		{
			ServerConfiguratorPath = ServerConfiguratorPath,
			IdpAddress = IdpAddress,
			Deconfigure = true
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new ConfigureFirewall(Runspace, Log)
		{
			PrivateSafekitBinaryFolderPath = Path.Combine(SafekitPath, "private\\bin"),
			Deconfigure = true
		};
		yield return new VerifyDeconfiguration(Log)
		{
			ModuleName = ModuleName,
			SafekitPath = SafekitPath
		};
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new StopSafekitServices(Runspace, Log);
		yield return new RemoveStoredPassword(Runspace, Log)
		{
			ServerConfigPath = ServerConfigPath
		};
		yield return new DeleteFailoverDesktopShortcut();
	}
}
