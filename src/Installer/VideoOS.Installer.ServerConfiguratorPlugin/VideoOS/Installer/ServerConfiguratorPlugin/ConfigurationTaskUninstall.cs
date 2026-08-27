using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.Manager;
using VideoOS.Installer.ServerConfiguratorPlugin.Properties;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class ConfigurationTaskUninstall : MSIUninstallerTask, IInstallationAdapter
{
	private ServerConfigurator _configurator;

	public ConfigurationTaskUninstall()
		: base(null)
	{
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
		_configurator = new ServerConfigurator(installParameters, installation: false);
		_parameters = installParameters;
		_parameters?.Add(new PackageParameter("WEBAPIPORT", ServerConfigFileUpdater.GetPortFromConfigFile()));
	}

	protected override void DoRun()
	{
		LogManager.Write("DoRun(): Configuration task uninstallation started");
		LogManager.Write("Calling StopServerServiceTrayController");
		ServerConfiguratorTrayController.StopServerServiceTrayController();
		if (GlobalData.GetValueAsBoolean("IS_UPGRADE"))
		{
			ReadAuthorizationServerAuthority();
		}
		ServerConfigurator.RemoveCertificateRegistrationAndPersistData();
		base.DoRun();
		try
		{
			base.MaximumProgress = _configurator.MaxProgress;
			base.Message = DefaultMessage;
			NotifyObservers();
			_configurator.OnProgressChanged += _configurator_OnProgressChanged;
			_configurator.RunUninstall();
		}
		catch (Exception ex)
		{
			LogManager.Write(ex);
			throw new InstallationAdapterException(Resources.ConfigurationPlugin_ErrorOccured, ex);
		}
		finally
		{
			_configurator.OnProgressChanged -= _configurator_OnProgressChanged;
		}
	}

	private void ReadAuthorizationServerAuthority()
	{
		try
		{
			string s = InstallerManager.Instance.OldInstallInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MAJOR")?.Value;
			string s2 = InstallerManager.Instance.OldInstallInfo?.InstallEnvironment.Parameters.SingleOrDefault((KeyValueParameters<string, string> x) => x.Key == "VERSION_MINOR")?.Value;
			if (int.TryParse(s, NumberStyles.None, CultureInfo.InvariantCulture, out var result) && int.TryParse(s2, NumberStyles.None, CultureInfo.InvariantCulture, out var result2))
			{
				Version version = new Version(result, result2);
				if (version == new Version(13, 2) || version == new Version(13, 3) || version == new Version(20, 1))
				{
					string value = ReadAuthority(Path.Combine(_parameters?.Single((IPackageParameter x) => x.Target == "INSTALLLOCATION").Value, "IIS\\IDP", "appsettings.json"));
					GlobalData.SetValue("AUTHORITY", value, "ServerConfiguratorPlugin.ConfigurationTaskUninstall.DoRun");
				}
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "ReadAuthorizationServerAuthority. Unable to get existing version"));
			}
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Reading authority from existing authorization server config failed. Exception occurred.\r\nException: {0}", arg));
		}
	}

	private static string ReadAuthority(string configPath)
	{
		string result = string.Empty;
		if (File.Exists(configPath))
		{
			try
			{
				dynamic val = JsonConvert.DeserializeObject(File.ReadAllText(configPath));
				result = val["IdentityProviderSettings"]["Authority"];
			}
			catch (Exception inner)
			{
				throw new ServerConfiguratorFailedException("Reading old Authorization Server config failed", inner);
			}
		}
		return result;
	}

	private void _configurator_OnProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		base.CurrentProgress = e.Progress;
		NotifyObservers();
	}
}
