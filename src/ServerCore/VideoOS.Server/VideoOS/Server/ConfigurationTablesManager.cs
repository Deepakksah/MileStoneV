using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using VideoOS.Common.Crypto;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

namespace VideoOS.Server;

public sealed class ConfigurationTablesManager
{
	private List<ITableAdapter> tableAdapters;

	private string currentSid;

	public TableAdapterManager TableAdapterManager { get; private set; }

	public ICollection<ITableAdapter> AllAdapters => tableAdapters;

	public ConfigurationTablesManager()
	{
		tableAdapters = new List<ITableAdapter>();
		TableAdapterManager = new TableAdapterManager();
	}

	public void SetUserSid(string sid)
	{
		if (sid == null)
		{
			throw new ArgumentNullException("sid");
		}
		if (string.IsNullOrEmpty(sid))
		{
			throw new ArgumentException("No security identifier was specified.", "sid");
		}
		if (currentSid != null)
		{
			throw new InvalidOperationException("The current sid value has not been cleared after the last use of the table adapter.");
		}
		currentSid = sid;
		foreach (ITableAdapter allAdapter in AllAdapters)
		{
			if (allAdapter is IUserContextAdapter userContextAdapter)
			{
				userContextAdapter.SetUserSid(sid);
			}
		}
	}

	public void ClearUserSid()
	{
		if (currentSid == null)
		{
			throw new InvalidOperationException("There is no current sid.");
		}
		currentSid = null;
		foreach (ITableAdapter allAdapter in AllAdapters)
		{
			if (allAdapter is IUserContextAdapter userContextAdapter)
			{
				userContextAdapter.ClearUserSid();
			}
		}
	}

	public static ConfigurationTablesManager Create(SqlConnection connection)
	{
		ConfigurationTablesManager configurationTablesManager = Create();
		configurationTablesManager.SetConnection(connection);
		return configurationTablesManager;
	}

	public static ConfigurationTablesManager Create()
	{
		ConfigurationTablesManager configurationTablesManager = new ConfigurationTablesManager();
		using (new ConfigurationDataSet())
		{
			configurationTablesManager.TableAdapterManager.CompressorsTableAdapter = new CompressorsTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.CompressorsTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceDataDistributionTableAdapter = new DeviceDataDistributionTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceDataDistributionTableAdapter);
			configurationTablesManager.TableAdapterManager.DevicePrivacyMaskTableAdapter = new DevicePrivacyMaskTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DevicePrivacyMaskTableAdapter);
			configurationTablesManager.TableAdapterManager.DevicesTableAdapter = new DevicesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DevicesTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceTrackTableAdapter = new DeviceTrackTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceTrackTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceTrackDefinitionTableAdapter = new DeviceTrackDefinitionTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceTrackDefinitionTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceGroupsTableAdapter = new DeviceGroupsTableAdapterWithSortedUpdate();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceGroupsTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceGroupMembersTableAdapter = new DeviceGroupMembersTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceGroupMembersTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceLinksTableAdapter = new DeviceLinksTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceLinksTableAdapter);
			configurationTablesManager.TableAdapterManager.RegisteredServersTableAdapter = new RegisteredServersTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RegisteredServersTableAdapter);
			configurationTablesManager.TableAdapterManager.EventTypesTableAdapter = new EventTypesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.EventTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.ExternalPluginsTableAdapter = new ExternalPluginsTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ExternalPluginsTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareDriverRecorderTableAdapter = new HardwareDriverRecorderTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareDriverRecorderTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareTableAdapter = new HardwareTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareBlockTableAdapter = new HardwareBlockTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareBlockTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareDefinitionTableAdapter = new HardwareDefinitionTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareDefinitionTableAdapter);
			configurationTablesManager.TableAdapterManager.LicenseTableAdapter = new LicenseTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.LicenseTableAdapter);
			configurationTablesManager.TableAdapterManager.MatrixTableAdapter = new MatrixTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.MatrixTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationProfilesTableAdapter = new NotificationProfilesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationProfilesTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationSettingsTableAdapter = new NotificationSettingsTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationSettingsTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationTypesTableAdapter = new NotificationTypesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.PatrollingProfilesTableAdapter = new PatrollingProfilesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.PatrollingProfilesTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordersTableAdapter = new RecordersTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordersTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingServerServiceRecorderTableAdapter = new RecordingServerServiceRecorderTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingServerServiceRecorderTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingServerServiceTableAdapter = new RecordingServerServiceTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingServerServiceTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingServerServiceConfigurableTableAdapter = new RecordingServerServiceConfigurableTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingServerServiceConfigurableTableAdapter);
			configurationTablesManager.TableAdapterManager.RecorderPluginDeviceTableAdapter = new RecorderPluginDeviceTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecorderPluginDeviceTableAdapter);
			configurationTablesManager.TableAdapterManager.RecorderPluginsTableAdapter = new RecorderPluginsTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecorderPluginsTableAdapter);
			configurationTablesManager.TableAdapterManager.RegisteredDeviceTypesTableAdapter = new RegisteredDeviceTypesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RegisteredDeviceTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.RulesTableAdapter = new RulesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RulesTableAdapter);
			configurationTablesManager.TableAdapterManager.RuleVariablesTableAdapter = new RuleVariablesTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RuleVariablesTableAdapter);
			configurationTablesManager.TableAdapterManager.SystemSettingsTableAdapter = new SystemSettingsTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.SystemSettingsTableAdapter);
			configurationTablesManager.TableAdapterManager.TimeProfileTableAdapter = new TimeProfileTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.TimeProfileTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingStorageDeviceTableAdapter = new RecordingStorageDeviceTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingStorageDeviceTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingStorageTableAdapter = new RecordingStorageTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingStorageTableAdapter);
			configurationTablesManager.TableAdapterManager.ArchiveStorageTableAdapter = new ArchiveStorageTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ArchiveStorageTableAdapter);
			configurationTablesManager.TableAdapterManager.HostingServerTableAdapter = new HostingServerTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HostingServerTableAdapter);
			configurationTablesManager.TableAdapterManager.HostedHardwareTableAdapter = new HostedHardwareTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HostedHardwareTableAdapter);
			configurationTablesManager.TableAdapterManager.MIPObjectTableAdapter = new MIPObjectTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.MIPObjectTableAdapter);
			configurationTablesManager.TableAdapterManager.ServerGroupTableAdapter = new ServerGroupTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ServerGroupTableAdapter);
			configurationTablesManager.TableAdapterManager.ServerGroupMembershipTableAdapter = new ServerGroupMembershipTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ServerGroupMembershipTableAdapter);
			configurationTablesManager.TableAdapterManager.AudioMessageTableAdapter = new AudioMessageTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.AudioMessageTableAdapter);
			RecorderInfoTableAdapter item = new RecorderInfoTableAdapter();
			configurationTablesManager.tableAdapters.Add(item);
			FailoverInfoTableAdapter item2 = new FailoverInfoTableAdapter();
			configurationTablesManager.tableAdapters.Add(item2);
			DeviceTrackEvidenceLockTableAdapter item3 = new DeviceTrackEvidenceLockTableAdapter();
			configurationTablesManager.tableAdapters.Add(item3);
			DeviceRestrictedMediaTableAdapter item4 = new DeviceRestrictedMediaTableAdapter();
			configurationTablesManager.tableAdapters.Add(item4);
			DeviceTrackInfoTableAdapter item5 = new DeviceTrackInfoTableAdapter();
			configurationTablesManager.tableAdapters.Add(item5);
			return configurationTablesManager;
		}
	}

	public void SetConnection(SqlConnection connection)
	{
		foreach (ITableAdapter tableAdapter in tableAdapters)
		{
			tableAdapter.Connection = connection;
		}
	}

	public void Encrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption)
	{
		foreach (ITableAdapter tableAdapter in tableAdapters)
		{
			if (tableAdapter is ITableAdapterEncryption tableAdapterEncryption)
			{
				tableAdapterEncryption.Encrypt(ds, symmetricEncryption);
			}
		}
	}

	public void Decrypt(ConfigurationDataSet ds, ISymmetricEncryption symmetricEncryption)
	{
		foreach (ITableAdapter tableAdapter in tableAdapters)
		{
			if (tableAdapter is ITableAdapterEncryption tableAdapterEncryption)
			{
				tableAdapterEncryption.Decrypt(ds, symmetricEncryption);
			}
		}
		ds.AcceptChanges();
	}

	public static ConfigurationTablesManager CreateForcedUpdater()
	{
		ConfigurationTablesManager configurationTablesManager = new ConfigurationTablesManager();
		using (new ConfigurationDataSet())
		{
			configurationTablesManager.TableAdapterManager.CompressorsTableAdapter = new CompressorsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.CompressorsTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceDataDistributionTableAdapter = new DeviceDataDistributionTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceDataDistributionTableAdapter);
			configurationTablesManager.TableAdapterManager.DevicePrivacyMaskTableAdapter = new DevicePrivacyMaskTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DevicePrivacyMaskTableAdapter);
			configurationTablesManager.TableAdapterManager.DevicesTableAdapter = new DevicesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DevicesTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceTrackTableAdapter = new DeviceTrackTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceTrackTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceTrackDefinitionTableAdapter = new DeviceTrackDefinitionTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceTrackDefinitionTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceGroupsTableAdapter = new DeviceGroupsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceGroupsTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceGroupMembersTableAdapter = new DeviceGroupMembersTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceGroupMembersTableAdapter);
			configurationTablesManager.TableAdapterManager.DeviceLinksTableAdapter = new DeviceLinksTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.DeviceLinksTableAdapter);
			configurationTablesManager.TableAdapterManager.RegisteredServersTableAdapter = new RegisteredServersTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RegisteredServersTableAdapter);
			configurationTablesManager.TableAdapterManager.EventTypesTableAdapter = new EventTypesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.EventTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.ExternalPluginsTableAdapter = new ExternalPluginsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ExternalPluginsTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareDriverRecorderTableAdapter = new HardwareDriverRecorderTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareDriverRecorderTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareTableAdapter = new HardwareTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareBlockTableAdapter = new HardwareBlockTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareBlockTableAdapter);
			configurationTablesManager.TableAdapterManager.HardwareDefinitionTableAdapter = new HardwareDefinitionTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HardwareDefinitionTableAdapter);
			configurationTablesManager.TableAdapterManager.LicenseTableAdapter = new LicenseTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.LicenseTableAdapter);
			configurationTablesManager.TableAdapterManager.MatrixTableAdapter = new MatrixTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.MatrixTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationProfilesTableAdapter = new NotificationProfilesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationProfilesTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationSettingsTableAdapter = new NotificationSettingsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationSettingsTableAdapter);
			configurationTablesManager.TableAdapterManager.NotificationTypesTableAdapter = new NotificationTypesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.NotificationTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.PatrollingProfilesTableAdapter = new PatrollingProfilesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.PatrollingProfilesTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordersTableAdapter = new RecordersTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordersTableAdapter);
			configurationTablesManager.TableAdapterManager.RecorderPluginDeviceTableAdapter = new RecorderPluginDeviceTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecorderPluginDeviceTableAdapter);
			configurationTablesManager.TableAdapterManager.RecorderPluginsTableAdapter = new RecorderPluginsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecorderPluginsTableAdapter);
			configurationTablesManager.TableAdapterManager.RegisteredDeviceTypesTableAdapter = new RegisteredDeviceTypesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RegisteredDeviceTypesTableAdapter);
			configurationTablesManager.TableAdapterManager.RulesTableAdapter = new RulesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RulesTableAdapter);
			configurationTablesManager.TableAdapterManager.RuleVariablesTableAdapter = new RuleVariablesTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RuleVariablesTableAdapter);
			configurationTablesManager.TableAdapterManager.SystemSettingsTableAdapter = new SystemSettingsTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.SystemSettingsTableAdapter);
			configurationTablesManager.TableAdapterManager.TimeProfileTableAdapter = new TimeProfileTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.TimeProfileTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingStorageDeviceTableAdapter = new RecordingStorageDeviceTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingStorageDeviceTableAdapter);
			configurationTablesManager.TableAdapterManager.RecordingStorageTableAdapter = new RecordingStorageTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.RecordingStorageTableAdapter);
			configurationTablesManager.TableAdapterManager.ArchiveStorageTableAdapter = new ArchiveStorageTableAdapterForce();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ArchiveStorageTableAdapter);
			configurationTablesManager.TableAdapterManager.HostingServerTableAdapter = new HostingServerTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HostingServerTableAdapter);
			configurationTablesManager.TableAdapterManager.HostedHardwareTableAdapter = new HostedHardwareTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.HostedHardwareTableAdapter);
			configurationTablesManager.TableAdapterManager.MIPObjectTableAdapter = new MIPObjectTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.MIPObjectTableAdapter);
			configurationTablesManager.TableAdapterManager.ServerGroupTableAdapter = new ServerGroupTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ServerGroupTableAdapter);
			configurationTablesManager.TableAdapterManager.ServerGroupMembershipTableAdapter = new ServerGroupMembershipTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.ServerGroupMembershipTableAdapter);
			configurationTablesManager.TableAdapterManager.AudioMessageTableAdapter = new AudioMessageTableAdapter();
			configurationTablesManager.tableAdapters.Add(configurationTablesManager.TableAdapterManager.AudioMessageTableAdapter);
			return configurationTablesManager;
		}
	}
}
