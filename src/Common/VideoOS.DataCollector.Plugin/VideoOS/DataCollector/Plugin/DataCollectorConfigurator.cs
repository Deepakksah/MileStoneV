using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using Microsoft.Win32;
using Newtonsoft.Json.Linq;
using VideoOS.Installer.Common;
using VideoOS.Installer.Logging;
using VideoOS.Installer.ServerConfigurator;
using VideoOS.ServerConfigurator.Interfaces;
using VideoOS.Utilities.Security;

namespace VideoOS.DataCollector.Plugin;

public class DataCollectorConfigurator
{
	private Dictionary<string, string> _installParameters;

	public DataCollectorConfigurator(Dictionary<string, string> installParameters)
	{
		_installParameters = installParameters;
	}

	private void CreateSecureFolder()
	{
		string commonApplicationDataFolder = new OemResourcesHandler().CommonApplicationDataFolder;
		new SecureFolder().CreateSecureFolder(commonApplicationDataFolder, _installParameters["ACCOUNTNAME"]);
	}

	public void RunInstall(bool isHWMM, JObject oldFile)
	{
		if (!isHWMM)
		{
			CreateSecureFolder();
			CheckDataCollectorAppIdRegistryEntry();
			if (oldFile != null)
			{
				DataCollectorUpdater.EnsureOldDataCollectorAppSettingsValues(_installParameters["INSTALLDIR"], oldFile);
			}
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(DataCollectorData.DataCollectorServerId, Path.Combine(_installParameters["INSTALLDIR"], "Registrar"));
			ApplyStatus applyStatus = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ApplyCertificatesForEndpointsToBeInstalled(new Guid[1] { DataCollectorData.DataCollectorServerId });
			if (applyStatus.ResultType() != RegistrationResultType.Success)
			{
				throw new Exception($"Failed to apply certificate for DataCollector. The status was {applyStatus}.");
			}
			bool offlineRegistration = GlobalData.HasParameter("CONNECTEDTOSERVER") && !GlobalData.GetValueAsBoolean("CONNECTEDTOSERVER");
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.SetLocationOnEndpointToBeInstalled(DataCollectorData.DataCollectorClientId, Path.Combine(_installParameters["INSTALLDIR"], "Registrar"));
			RegisterClientStatus? result = ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.ConfigureClientEndpointsToBeInstalled(new List<Guid> { DataCollectorData.DataCollectorClientId }, offlineRegistration).GetResult<RegisterClientStatus>();
			if (result == RegisterClientStatus.CouldNotStartServer || result == RegisterClientStatus.CouldNotStopServer)
			{
				throw new Exception($"Failed to register client for DataCollector. The status was {applyStatus}.");
			}
		}
	}

	private static void CheckDataCollectorAppIdRegistryEntry()
	{
		try
		{
			using RegistryKey registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);
			using RegistryKey registryKey2 = registryKey.OpenSubKey("SOFTWARE\\VideoOS\\Installation\\DataCollector", writable: true);
			if (registryKey2 != null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CheckDataCollectorAppIdRegistryEntry() Found {0}...", registryKey2.Name));
				if (!(registryKey2.GetValue("DataCollectorAppId") is string))
				{
					Guid guid = Guid.NewGuid();
					registryKey2.SetValue("DataCollectorAppId", guid.ToString("B"));
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CheckDataCollectorAppIdRegistryEntry() inserted DataCollectorAppId {0}", guid.ToString("B")));
				}
				else
				{
					LogManager.Write("CheckDataCollectorAppIdRegistryEntry() DataCollectorAppId allready exists");
				}
				return;
			}
			LogManager.Write("CheckDataCollectorAppIdRegistryEntry() Could not find entry point, creating new...");
			using RegistryKey registryKey3 = registryKey.CreateSubKey("SOFTWARE\\VideoOS\\Installation\\DataCollector");
			LogManager.Write("CheckDataCollectorAppIdRegistryEntry() Created new entry");
			Guid guid2 = Guid.NewGuid();
			registryKey3.SetValue("DataCollectorAppId", guid2.ToString("B"));
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "CheckDataCollectorAppIdRegistryEntry() Created new DataCollectorAppId {0}", guid2.ToString("B")));
		}
		catch (Exception ex)
		{
			throw new DataCollectorConfiguratorFailedException(string.Format(CultureInfo.InvariantCulture, "CheckDataCollectorAppIdRegistryEntry() Error occured: {0}, {1}", ex.GetType().ToString(), ex.Message), ex);
		}
	}

	public void RunUninstall()
	{
		RemoveCertificateRegistrationAndPersistData();
	}

	[SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Installer should not crash during configuration or uninstall")]
	public static void RemoveCertificateRegistrationAndPersistData()
	{
		try
		{
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveClientEndpointInstalled(DataCollectorData.DataCollectorClientId);
			ServerConfigurationDataModelProvider.Instance.CertificateCmdModel.RemoveServerEndpointInstalled(DataCollectorData.DataCollectorServerId);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "RemoveCertificateRegistration(). Error occurred. {0}{1}", Environment.NewLine, arg));
		}
	}
}
