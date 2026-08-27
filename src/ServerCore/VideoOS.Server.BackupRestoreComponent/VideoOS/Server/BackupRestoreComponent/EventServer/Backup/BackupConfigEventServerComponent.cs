using System;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Services;
using VideoOS.Vms.BackupRestore;

namespace VideoOS.Server.BackupRestoreComponent.EventServer.Backup;

public class BackupConfigEventServerComponent : IConfigBackup
{
	private IConfigurationDataSetLoader _configurationDataSetLoader;

	private RegisteredServer EventServerRegistration
	{
		get
		{
			RegisteredServer result = null;
			using (ConfigurationManager configurationManager = new ConfigurationManager(_configurationDataSetLoader))
			{
				foreach (RegisteredServer value in configurationManager.RegisteredServers.Values)
				{
					if (value.Type.Equals(ServiceTypes.ServiceType_EventServerService))
					{
						result = value;
						break;
					}
				}
			}
			return result;
		}
	}

	public string ComponentName => "EventServer";

	public BackupConfigEventServerComponent(IConfigurationDataSetLoader configurationDataSetLoader)
	{
		if (configurationDataSetLoader == null)
		{
			throw new ArgumentNullException("configurationDataSetLoader");
		}
		_configurationDataSetLoader = configurationDataSetLoader;
	}

	public BackupValidationResult ValidateForBackup(ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 100);
		return BackupValidationResult.CreateSuccessResult();
	}

	public BackupComponentResult Backup(string folderPath, ReportComponentStatus reportStatus, AbortRequested abortRequested)
	{
		if (reportStatus == null)
		{
			throw new ArgumentNullException("reportStatus");
		}
		reportStatus(ComponentName, 100);
		return BackupComponentResult.CreateSuccessResult(new string[0]);
	}

	public string ConfigData()
	{
		RegisteredServer eventServerRegistration = EventServerRegistration;
		if (eventServerRegistration != null)
		{
			return eventServerRegistration.CustomData;
		}
		return string.Empty;
	}
}
