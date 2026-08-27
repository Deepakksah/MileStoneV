using System;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;

namespace VideoOS.Server.Failover.Configurator;

public class FailoverConfiguratorRunner
{
	public readonly ILogger Logger;

	public IServerAccess ServerAccess { get; }

	public IProtectionProvider ProtectionProvider { get; }

	public Settings Settings { get; set; }

	public FailoverConfiguratorRunner(IServerAccess serverAccess, IProtectionProvider passwordProtection, ILogger logger)
	{
		ServerAccess = serverAccess;
		ProtectionProvider = passwordProtection;
		Logger = logger;
		Settings = Settings.Load(Logger);
	}

	public void EnterMaintenanceMode(bool isPrimary, bool isHttps, bool isExternalSqlSetup)
	{
		Settings.MaintenanceMode = true;
		Settings.IsPrimary = isPrimary;
		Settings.IsHttps = isHttps;
		Settings.IsExternalSqlSetup = isExternalSqlSetup;
		Settings.Save();
	}

	public void LeaveMaintenanceMode()
	{
		Settings.MaintenanceMode = false;
		Settings.IsPrimary = false;
		Settings.IsHttps = false;
		Settings.IsExternalSqlSetup = false;
		Settings.Save();
	}

	public async Task<(bool, string)> ConfigureSecondaryNodeAsync(bool httpsEnabled, string webConsolePassword, string httpsPassword = "")
	{
		if (string.IsNullOrEmpty(httpsPassword) && httpsEnabled)
		{
			Random random = new Random();
			httpsPassword = random.Next(100000, 1000000).ToString();
		}
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		SecondaryNodePrepareConfigurationBuilder stepsBuilder = new SecondaryNodePrepareConfigurationBuilder(runSpace, Logger)
		{
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"),
			ServerConfigPath = OemResources.Instance.Server.ConfigFilePath,
			EnableHttps = httpsEnabled,
			UserPassword = httpsPassword,
			SecondaryNodeFqdn = FailoverConfigurationHelper.LocalFqdn,
			WebConsolePassword = webConsolePassword,
			ProtectionProvider = new DPAPIProtectionProvider()
		};
		runSpace.Open();
		EnterMaintenanceMode(isPrimary: false, httpsEnabled, isExternalSqlSetup: false);
		return (await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false), httpsPassword);
	}

	public async Task<bool> ConfigurePrimaryNodeAsync(bool httpsEnabled, string httpsPassword, string webserverPassword, string virtualIP, string secondaryMachineAddress, string sqlDataFolder, string sqlLogFolder, bool isExternalSqlSetup = false, string licenseFilename = "")
	{
		Uri idpUri = FailoverConfigurationHelper.IdpUri;
		if (idpUri == null)
		{
			Logger.LogError("Failed to get the IDP URI.");
			return false;
		}
		string idpAddress = FailoverConfigurationHelper.ConstructIdpAddress(idpUri, virtualIP);
		string sqlServerFolderFromDatabase = ProductLocationHelper.GetSqlServerFolderFromDatabase(ServerAccess);
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		PrimaryNodeConfigurationBuilder stepsBuilder = new PrimaryNodeConfigurationBuilder(runSpace, Logger)
		{
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"),
			ServerConfigPath = OemResources.Instance.Server.ConfigFilePath,
			SqlServerPathData = ((!string.IsNullOrEmpty(sqlDataFolder)) ? sqlDataFolder : Path.Combine(sqlServerFolderFromDatabase, "DATA")),
			SqlServerPathLog = ((!string.IsNullOrEmpty(sqlLogFolder)) ? sqlLogFolder : Path.Combine(sqlServerFolderFromDatabase, "Log")),
			EnableHttps = httpsEnabled,
			PrimaryNodeAddress = FailoverConfigurationHelper.LocalFqdn,
			SecondaryNodeAddress = secondaryMachineAddress,
			VirtualIp = virtualIP,
			PrimaryNodeName = "node1",
			SecondaryNodeName = "node2",
			ModuleName = (isExternalSqlSetup ? "videoosnosql" : "videoos"),
			HttpsPassword = httpsPassword,
			LicenseFilePath = licenseFilename,
			DesktopShortcutIconName = "MsFailoverWebConsole.ico",
			ServerConfiguratorPath = ProductLocationHelper.ServerConfiguratorDirectory,
			IdpAddress = idpAddress,
			WebConsolePassword = webserverPassword,
			ProtectionProvider = new DPAPIProtectionProvider(),
			DataCollectorConfigFilePath = Path.Combine(ProductLocationHelper.DataCollectorDirectory, "appsettings.json")
		};
		runSpace.Open();
		EnterMaintenanceMode(isPrimary: true, httpsEnabled, isExternalSqlSetup);
		return await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
	}

	public async Task<bool> FinalizeSecondaryNodeAsync(bool httpsEnabled, bool isExternalSqlSetup, string licenseFilePath = "")
	{
		Uri idpUri;
		string clusterIPAddress;
		try
		{
			idpUri = FailoverConfigurationHelper.IdpUri;
			if (idpUri == null)
			{
				Logger.LogError("Failed to get the IDP URI.");
				return false;
			}
			clusterIPAddress = FailoverConfigurationHelper.ClusterIPAddress;
		}
		catch (IOException ex)
		{
			Logger.LogError("Error while getting cluster address: {ExceptionMessage}", ex.Message);
			return false;
		}
		string idpAddress = FailoverConfigurationHelper.ConstructIdpAddress(idpUri, clusterIPAddress);
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		SecondaryNodeFinalizeConfigurationBuilder stepsBuilder = new SecondaryNodeFinalizeConfigurationBuilder(runSpace, Logger)
		{
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"),
			ServerConfiguratorPath = ProductLocationHelper.ServerConfiguratorDirectory,
			ServerConfigPath = OemResources.Instance.Server.ConfigFilePath,
			EnableHttps = httpsEnabled,
			IdpAddress = idpAddress,
			LicenseFilePath = licenseFilePath,
			SecondaryNodeAddress = FailoverConfigurationHelper.LocalFqdn,
			DesktopShortcutIconName = "MsFailoverWebConsole.ico",
			DataCollectorConfigFilePath = Path.Combine(ProductLocationHelper.DataCollectorDirectory, "appsettings.json"),
			SafekitVirtualAddress = clusterIPAddress
		};
		runSpace.Open();
		EnterMaintenanceMode(isPrimary: false, httpsEnabled, isExternalSqlSetup);
		return await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
	}

	public async Task<bool> DownloadServerCertificateAsync(string downloadFolder)
	{
		if (!Directory.Exists(downloadFolder))
		{
			Logger.LogError("The download folder doesn't exist.");
			return false;
		}
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		UserCertificatesCreationConfigurationBuilder stepsBuilder = new UserCertificatesCreationConfigurationBuilder(runSpace, Logger)
		{
			SaveToFolder = downloadFolder,
			LocalMachineFqdn = FailoverConfigurationHelper.LocalFqdn,
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit")
		};
		runSpace.Open();
		return await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
	}

	public async Task<bool> DeconfigureAsync(bool isExternalSqlSetup = false)
	{
		Uri idpUri = FailoverConfigurationHelper.IdpUri;
		if (idpUri == null)
		{
			Logger.LogError("Failed to get the IDP URI.");
			return false;
		}
		string localFqdn = FailoverConfigurationHelper.LocalFqdn;
		string idpAddress = FailoverConfigurationHelper.ConstructIdpAddress(idpUri, localFqdn);
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		NodeDeconfigurationBuilder stepsBuilder = new NodeDeconfigurationBuilder(runSpace, Logger)
		{
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"),
			ServerConfiguratorPath = ProductLocationHelper.ServerConfiguratorDirectory,
			ServerConfigPath = OemResources.Instance.Server.ConfigFilePath,
			IdpAddress = idpAddress,
			ModuleName = (isExternalSqlSetup ? "videoosnosql" : "videoos"),
			DataCollectorConfigFilePath = Path.Combine(ProductLocationHelper.DataCollectorDirectory, "appsettings.json")
		};
		runSpace.Open();
		bool flag;
		try
		{
			flag = await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
		}
		catch (VerifyDeconfigurationException)
		{
			Logger.LogError("Deconfiguration failed, staying in maintenance mode.");
			flag = false;
		}
		if (flag)
		{
			LeaveMaintenanceMode();
		}
		return flag;
	}

	public async Task<bool> CreateWebServerPasswordAsync(string webConsolePassword)
	{
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		WebServerPasswordCreationConfigurationBuilder stepsBuilder = new WebServerPasswordCreationConfigurationBuilder(runSpace, Logger)
		{
			ConfigurationScriptsPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "scripts"),
			ServerConfigPath = OemResources.Instance.Server.ConfigFilePath,
			SafekitPath = Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"),
			WebServerPassword = webConsolePassword,
			ProtectionProvider = ProtectionProvider
		};
		runSpace.Open();
		return await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
	}

	public async Task<bool> ChangeFailoverLicenseAsync(string failoverLicensePath)
	{
		using Runspace runSpace = RunspaceFactory.CreateRunspace();
		ChangeFailoverLicenseConfigurationBuilder stepsBuilder = new ChangeFailoverLicenseConfigurationBuilder(Path.Combine(ProductLocationHelper.FailoverServerDirectory, "safekit"), failoverLicensePath, Logger);
		runSpace.Open();
		return await RunConfigurationStepsAsync(stepsBuilder, Logger, ignoreErrors: false);
	}

	private static async Task<bool> RunConfigurationStepsAsync(IConfigurationBuilder stepsBuilder, ILogger logger, bool ignoreErrors)
	{
		foreach (IConfigurationStep step in stepsBuilder.Build())
		{
			try
			{
				logger.LogInformation("Running step {TypeName}...", step.GetType().Name);
				await step.ExecuteAsync();
			}
			catch (ScriptErrorException ex)
			{
				string text = string.Empty;
				foreach (ErrorRecord error in ex.Errors)
				{
					string text2 = error?.Exception?.Message;
					text = text + text2 + "\n";
				}
				logger.LogError("{ErrorsDescription}", text);
				if (!ignoreErrors)
				{
					return false;
				}
			}
			catch (Exception ex2)
			{
				logger.LogError("{ExceptionTypeName} occured during RunConfigurationStepsAsync: {ExceptionMessage}", ex2.GetType().Name, ex2.Message);
				if (!ignoreErrors)
				{
					return false;
				}
			}
			finally
			{
				logger.LogInformation("Finished step {StepTypeName}.", step.GetType().Name);
			}
		}
		return true;
	}
}
