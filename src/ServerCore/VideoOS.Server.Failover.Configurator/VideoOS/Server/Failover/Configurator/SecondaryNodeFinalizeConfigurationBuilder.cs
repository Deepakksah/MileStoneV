using System.Collections.Generic;
using System.IO;
using System.Management.Automation.Runspaces;
using Microsoft.Extensions.Logging;

namespace VideoOS.Server.Failover.Configurator;

public class SecondaryNodeFinalizeConfigurationBuilder : IConfigurationBuilder
{
	public bool EnableHttps { get; set; }

	public string SecondaryNodeAddress { get; set; }

	public string ConfigurationScriptsPath { get; set; }

	public string SafekitPath { get; set; }

	public string ServerConfiguratorPath { get; set; }

	public string ServerConfigPath { get; set; }

	public string IdpAddress { get; set; }

	public string LicenseFilePath { get; set; }

	public string DesktopShortcutIconName { get; set; }

	public string DataCollectorConfigFilePath { get; set; }

	private Runspace Runspace { get; }

	private ILogger Log { get; }

	public string SafekitVirtualAddress { get; public set; }

	public SecondaryNodeFinalizeConfigurationBuilder(Runspace runspace, ILogger log)
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
		yield return new SetClusterMembershipFlag(Runspace, Log)
		{
			ServerConfigPath = ServerConfigPath,
			IsClusterMember = true,
			ClusterLocalWebserverUriString = WebConsoleHelper.GetWebConsoleAddress(SecondaryNodeAddress, EnableHttps),
			ClusterModuleName = (FailoverConfigurationHelper.IsExternalSqlSetup() ? "videoosnosql" : "videoos"),
			SafekitVirtualAddress = SafekitVirtualAddress
		};
		yield return new UpdateDataCollectorAppConfig(SecondaryNodeAddress, SafekitVirtualAddress, DataCollectorConfigFilePath, Log);
		yield return new SetLocation(Runspace, Log)
		{
			Path = ConfigurationScriptsPath
		};
		yield return new RegisterServices(Log)
		{
			ServerConfiguratorPath = ServerConfiguratorPath,
			IdpAddress = IdpAddress
		};
		yield return new ChangeFailoverLicense(Log, LicenseFilePath, SafekitPath);
		yield return new CreateFailoverDesktopShortcut(EnableHttps, SecondaryNodeAddress, Path.Combine(ConfigurationScriptsPath, DesktopShortcutIconName));
	}
}
