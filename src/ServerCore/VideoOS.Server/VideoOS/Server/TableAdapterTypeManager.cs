using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Configuration.ConfigurationDataSetTableAdapters;

namespace VideoOS.Server;

public sealed class TableAdapterTypeManager
{
	private static Dictionary<string, Type> _tableAdapterTypes = InitializeTableAdapterTypes();

	private static Dictionary<string, Type> InitializeTableAdapterTypes()
	{
		using ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
		dictionary.Add(configurationDataSet.Compressors.TableName, typeof(CompressorsTableAdapter));
		dictionary.Add(configurationDataSet.DeviceDataDistribution.TableName, typeof(DeviceDataDistributionTableAdapter));
		dictionary.Add(configurationDataSet.DeviceTrackEvidenceLock.TableName, typeof(DeviceTrackEvidenceLockTableAdapter));
		dictionary.Add(configurationDataSet.DeviceRestrictedMedia.TableName, typeof(DeviceRestrictedMediaTableAdapter));
		dictionary.Add(configurationDataSet.DeviceTrackInfo.TableName, typeof(DeviceTrackInfoTableAdapter));
		dictionary.Add(configurationDataSet.DeviceTrack.TableName, typeof(DeviceTrackTableAdapter));
		dictionary.Add(configurationDataSet.DeviceTrackDefinition.TableName, typeof(DeviceTrackDefinitionTableAdapter));
		dictionary.Add(configurationDataSet.Devices.TableName, typeof(DevicesTableAdapter));
		dictionary.Add(configurationDataSet.DeviceGroups.TableName, typeof(DeviceGroupsTableAdapterWithSortedUpdate));
		dictionary.Add(configurationDataSet.DeviceGroupMembers.TableName, typeof(DeviceGroupMembersTableAdapter));
		dictionary.Add(configurationDataSet.DeviceLinks.TableName, typeof(DeviceLinksTableAdapter));
		dictionary.Add(configurationDataSet.DevicePrivacyMask.TableName, typeof(DevicePrivacyMaskTableAdapter));
		dictionary.Add(configurationDataSet.RegisteredServers.TableName, typeof(RegisteredServersTableAdapter));
		dictionary.Add(configurationDataSet.EventTypes.TableName, typeof(EventTypesTableAdapter));
		dictionary.Add(configurationDataSet.ExternalPlugins.TableName, typeof(ExternalPluginsTableAdapter));
		dictionary.Add(configurationDataSet.HardwareDriverRecorder.TableName, typeof(HardwareDriverRecorderTableAdapter));
		dictionary.Add(configurationDataSet.Hardware.TableName, typeof(HardwareTableAdapter));
		dictionary.Add(configurationDataSet.HardwareBlock.TableName, typeof(HardwareBlockTableAdapter));
		dictionary.Add(configurationDataSet.HardwareDefinition.TableName, typeof(HardwareDefinitionTableAdapter));
		dictionary.Add(configurationDataSet.License.TableName, typeof(LicenseTableAdapter));
		dictionary.Add(configurationDataSet.Matrix.TableName, typeof(MatrixTableAdapter));
		dictionary.Add(configurationDataSet.NotificationProfiles.TableName, typeof(NotificationProfilesTableAdapter));
		dictionary.Add(configurationDataSet.NotificationSettings.TableName, typeof(NotificationSettingsTableAdapter));
		dictionary.Add(configurationDataSet.NotificationTypes.TableName, typeof(NotificationTypesTableAdapter));
		dictionary.Add(configurationDataSet.PatrollingProfiles.TableName, typeof(PatrollingProfilesTableAdapter));
		dictionary.Add(configurationDataSet.Recorders.TableName, typeof(RecordersTableAdapter));
		dictionary.Add(configurationDataSet.RecordingServerServiceRecorder.TableName, typeof(RecordingServerServiceRecorderTableAdapter));
		dictionary.Add(configurationDataSet.RecordingServerService.TableName, typeof(RecordingServerServiceTableAdapter));
		dictionary.Add(configurationDataSet.RecordingServerServiceConfigurable.TableName, typeof(RecordingServerServiceConfigurableTableAdapter));
		dictionary.Add(configurationDataSet.RecorderPluginDevice.TableName, typeof(RecorderPluginDeviceTableAdapter));
		dictionary.Add(configurationDataSet.RecorderPlugins.TableName, typeof(RecorderPluginsTableAdapter));
		dictionary.Add(configurationDataSet.RegisteredDeviceTypes.TableName, typeof(RegisteredDeviceTypesTableAdapter));
		dictionary.Add(configurationDataSet.Rules.TableName, typeof(RulesTableAdapter));
		dictionary.Add(configurationDataSet.RuleVariables.TableName, typeof(RuleVariablesTableAdapter));
		dictionary.Add(configurationDataSet.SystemSettings.TableName, typeof(SystemSettingsTableAdapter));
		dictionary.Add(configurationDataSet.TimeProfile.TableName, typeof(TimeProfileTableAdapter));
		dictionary.Add(configurationDataSet.RecordingStorageDevice.TableName, typeof(RecordingStorageDeviceTableAdapter));
		dictionary.Add(configurationDataSet.RecordingStorage.TableName, typeof(RecordingStorageTableAdapter));
		dictionary.Add(configurationDataSet.ArchiveStorage.TableName, typeof(ArchiveStorageTableAdapter));
		dictionary.Add(configurationDataSet.HostingServer.TableName, typeof(HostingServerTableAdapter));
		dictionary.Add(configurationDataSet.HostedHardware.TableName, typeof(HostedHardwareTableAdapter));
		dictionary.Add(configurationDataSet.MIPObject.TableName, typeof(MIPObjectTableAdapter));
		dictionary.Add(configurationDataSet.ServerGroup.TableName, typeof(ServerGroupTableAdapter));
		dictionary.Add(configurationDataSet.ServerGroupMembership.TableName, typeof(ServerGroupMembershipTableAdapter));
		dictionary.Add(configurationDataSet.AudioMessage.TableName, typeof(AudioMessageTableAdapter));
		dictionary.Add(configurationDataSet.RecorderInfo.TableName, typeof(RecorderInfoTableAdapter));
		dictionary.Add(configurationDataSet.FailoverInfo.TableName, typeof(FailoverInfoTableAdapter));
		dictionary.Add(configurationDataSet.DriverNumbers.TableName, typeof(DriverNumbersTableAdapter));
		return dictionary;
	}

	public static T[] CreateTableAdapters<T>() where T : class
	{
		return CreateTableAdapters<T>(_tableAdapterTypes.Keys.ToArray());
	}

	public static T[] CreateTableAdapters<T>(string[] tableNames) where T : class
	{
		List<T> list = new List<T>();
		if (tableNames != null && tableNames.Length != 0)
		{
			foreach (string key in tableNames)
			{
				if (_tableAdapterTypes.TryGetValue(key, out var value) && value.GetInterfaces().Contains(typeof(T)) && Activator.CreateInstance(value) is T item)
				{
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}
}
