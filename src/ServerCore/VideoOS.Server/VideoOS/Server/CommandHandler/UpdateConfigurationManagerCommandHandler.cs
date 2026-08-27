using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using VideoOS.Administration.AddIn.Util;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Common.Services;
using VideoOS.EventServer.Communication.Rest;
using VideoOS.EventServer.Events.RestApi.Client;
using VideoOS.Management.Common;
using VideoOS.Management.VmoClient;
using VideoOS.Management.VmoClient.Proxy;
using VideoOS.Platform.EventsData;
using VideoOS.Platform.Proxy.RestApi;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Server.ServiceChannel;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateConfigurationManagerCommandHandler : ICommandHandler
{
	private class RecorderRequiringLicense
	{
		public Guid Id { get; }

		public bool RecorderDeleted { get; set; }

		public bool CreateNewLicense { get; set; } = true;

		public bool NewLicenseCreated { get; set; }

		public RecorderRequiringLicense(Guid recorderId)
		{
			Id = recorderId;
		}
	}

	private class RecordersRequiringLicense : Dictionary<Guid, RecorderRequiringLicense>
	{
		public RecorderRequiringLicense GetOrAdd(Guid key)
		{
			if (!TryGetValue(key, out var value))
			{
				value = new RecorderRequiringLicense(key);
				Add(key, value);
			}
			return value;
		}
	}

	public class ConfigurationChangedEventCollector
	{
		private Dictionary<string, ConfigurationChangedEventInfo> _events = new Dictionary<string, ConfigurationChangedEventInfo>();

		private Guid _homeSiteId;

		private IEventConsumer _eventConsumer;

		public ConfigurationChangedEventInfo[] EventInfos => _events.Values.ToArray();

		public ConfigurationChangedEventCollector(Guid homeSiteId, IEventConsumer eventConsumer)
		{
			_homeSiteId = homeSiteId;
			_eventConsumer = eventConsumer;
		}

		public bool EventExists(string id, string resourceType, ChangeAction changeAction)
		{
			string actionName = GetActionName(changeAction);
			string key = id + resourceType + actionName;
			return _events.ContainsKey(key);
		}

		public void AddEvent(string id, ChangeAction changeAction, string itemType)
		{
			AddEvent(id, string.Empty, changeAction, itemType);
		}

		public void AddEvent(string id, string resourceName, ChangeAction changeAction, string itemType)
		{
			if (!Guid.TryParse(id, out var result))
			{
				result = Guid.Empty;
				Server.Instance.Logger.LogWarning("Failed to convert resource id {ResourceId} to Guid.", id);
			}
			NameConvert nameConvert = new NameConvert();
			string restResource = new RestResource
			{
				Id = result,
				Name = nameConvert.ItemTypeToResourcePlural(itemType)
			}.ToString();
			string actionName = GetActionName(changeAction);
			ConfigurationChangedEventInfo value = new ConfigurationChangedEventInfo
			{
				HomeSiteId = _homeSiteId,
				EventConsumer = _eventConsumer,
				Action = actionName,
				RestName = resourceName,
				RestResource = restResource
			};
			string key = id + itemType + actionName;
			if (!_events.ContainsKey(key))
			{
				_events.Add(key, value);
			}
		}

		public void FireConfigChangedEvent()
		{
			foreach (ConfigurationChangedEventInfo value in _events.Values)
			{
				value.EventConsumer.SentEventToConsumer(new PostFreeFormEventBody
				{
					Data = new SystemConfigurationChangedEventData
					{
						Action = value.Action,
						RelatedItem = new SystemConfigurationChangedEventDataRelatedItem
						{
							RelatedItemRestResource = value.RestResource,
							Name = value.RestName
						}
					},
					Source = value.HomeSiteId.ToString(),
					Type = BuiltInEventTypes.EventIDs.SystemConfigurationChanged,
					Time = DateTimeOffset.UtcNow
				});
			}
		}

		private static string GetActionName(ChangeAction changeAction)
		{
			return changeAction switch
			{
				ChangeAction.Deleted => "deleted", 
				ChangeAction.Added => "created", 
				_ => "updated", 
			};
		}
	}

	public class ConfigurationChangedEventInfo
	{
		public Guid HomeSiteId { get; set; }

		public IEventConsumer EventConsumer { get; set; }

		public string Action { get; set; }

		public string RestResource { get; set; }

		public string RestName { get; set; }
	}

	private readonly Server _server;

	private readonly IServerConfigurationLookup _serverConfigurationLookup;

	private readonly IDataComponentFactory _dataComponentFactory;

	private readonly IConfigurationDataLoader _configurationDataLoader;

	private readonly IConfigurationDataSetComponent _configurationDataSetComponent;

	private readonly IRecorderCommManager _recorderCommManager;

	private readonly ILogAuditLogger _auditLogger;

	private readonly IEventConsumer _eventConsumer;

	public UpdateConfigurationManagerCommandHandler(Server server)
		: this(server, server.EVSEventConsumer, server.ConfigurationLookup, server.DataComponentFactory, server.Data, server.Data, server.RecorderCommManager, ServerLogManager.Instance)
	{
	}

	public UpdateConfigurationManagerCommandHandler(Server server, IEventConsumer eventConsumer, IServerConfigurationLookup serverConfigurationLookup, IDataComponentFactory dataComponentFactory, IConfigurationDataLoader configurationDataLoader, IConfigurationDataSetComponent configurationDataSetComponent, IRecorderCommManager recorderCommManager, ILogAuditLogger auditLogger)
	{
		_server = server;
		_serverConfigurationLookup = serverConfigurationLookup;
		_dataComponentFactory = dataComponentFactory;
		_configurationDataLoader = configurationDataLoader;
		_configurationDataSetComponent = configurationDataSetComponent;
		_recorderCommManager = recorderCommManager;
		_auditLogger = auditLogger;
		_eventConsumer = eventConsumer;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateConfigurationManagerCommand cmd))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		ProcessCommand(cmd, callContextInfo, responseSink);
	}

	private void ProcessCommand(UpdateConfigurationManagerCommand cmd, ICallContextInfo callContextInfo, IResponseSink responseSink)
	{
		if (cmd.ConfigurationManagerDto.DataSet == null)
		{
			responseSink.AddResponse(new SuccessResponse());
			return;
		}
		SessionLogger sessionLogger = new SessionLogger();
		LogEntryAuditLogger auditSessionLogger = new LogEntryAuditLogger(_server.AuditAndSystemLogger, sessionLogger, new ServerInfoCollector());
		ConfigurationChangedEventCollector configurationChangedEventCollector = new ConfigurationChangedEventCollector(_server.HomeSiteId, _eventConsumer);
		_configurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			Dictionary<Guid, ConfigurationDataSet> recorderChanges = new Dictionary<Guid, ConfigurationDataSet>();
			List<Guid> deleteRecorderIds = new List<Guid>();
			RecordersRequiringLicense recordersRequiringLicense = new RecordersRequiringLicense();
			Dictionary<Guid, string> recorderNames = new Dictionary<Guid, string>();
			List<Guid> deletedDeviceIds = new List<Guid>();
			Dictionary<Guid, string> deviceIdNames = new Dictionary<Guid, string>();
			Dictionary<Guid, string> deviceIdTypes = new Dictionary<Guid, string>();
			ConfigurationDataSet dataSet = cmd.ConfigurationManagerDto.DataSet;
			ConfigurationDataSet changes = dataSet.GetChanges(DataRowState.Added | DataRowState.Modified);
			ConfigurationDataSet changes2 = dataSet.GetChanges(DataRowState.Deleted);
			bool queueAutomaticActivation = CheckForAutomaticActivationTrigger(dataSet);
			try
			{
				ResponseBase responseBase = null;
				using (new SendHeartbeatResponseTimer(cmd.SendHeartbeat, responseSink))
				{
					bool persistChanges = cmd.PersistChanges;
					responseBase = AnalyzeChangesByRecorder(changes, changes2, recorderChanges, recordersRequiringLicense, deleteRecorderIds, recorderNames, deletedDeviceIds, auditSessionLogger, callContextInfo, persistChanges, deviceIdNames, deviceIdTypes);
					if (persistChanges)
					{
						AnalyzeChangesForConfigurationChangedEvents(changes, changes2, configurationChangedEventCollector, deviceIdNames, deviceIdTypes);
					}
					if (responseBase == null)
					{
						AnalyzeChangesForAllRecorders(dataSet, recorderChanges);
					}
				}
				if (responseBase != null)
				{
					responseSink.AddResponse(responseBase);
					return;
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				Server.Instance.Logger.LogError("Failed to analyze configuration update diffgram. Exception: {Exception}", ex2);
				responseSink.AddResponse(new ExceptionResponse(ex2));
				return;
			}
			ResponseBase responseBase2;
			using (new SendHeartbeatResponseTimer(cmd.SendHeartbeat, responseSink))
			{
				responseBase2 = CheckFeaturesAndWriteChanges(dataSet, cmd, callContextInfo);
			}
			if (!(responseBase2 is SuccessResponse))
			{
				responseSink.AddResponse(responseBase2);
				return;
			}
			using (new SendHeartbeatResponseTimer(cmd.SendHeartbeat, responseSink))
			{
				ApplyLogManagerConfigurationChanges(changes);
				ApplyRuleEngineConfigurationChanges(dataSet, callContextInfo);
				HandleLicenseChanges(dataSet, deleteRecorderIds, recorderChanges, recordersRequiringLicense, queueAutomaticActivation);
				RemoveTasksForDeletedDevices(deletedDeviceIds);
				SendUpdatesToRecorders(recorderChanges, callContextInfo);
				NotifyRecordersProcessPendingTasks(deleteRecorderIds);
			}
			responseSink.AddResponse(responseBase2);
		}
		finally
		{
			_configurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
		LogPendingAuditLogs(sessionLogger, _auditLogger);
		configurationChangedEventCollector.FireConfigChangedEvent();
	}

	private void NotifyRecordersProcessPendingTasks(List<Guid> deleteRecorderIds)
	{
		if (deleteRecorderIds.Count > 0)
		{
			_recorderCommManager.TryAllRecordersProcessPendingTasksAsync();
		}
	}

	private void HandleLicenseChanges(ConfigurationDataSet allDataChanges, List<Guid> deleteRecorderIds, Dictionary<Guid, ConfigurationDataSet> recorderChanges, RecordersRequiringLicense recordersRequiringLicense, bool queueAutomaticActivation)
	{
		try
		{
			if (queueAutomaticActivation)
			{
				_server.LicenseManager.QueueAutomaticActivation();
			}
			ICallContextInfo callContextInfo = Server.Instance.CreateServerServiceCallContextUserInfo();
			DeleteIdsFromLicenseTable(deleteRecorderIds, callContextInfo);
			HandleServerLicenseChanged(allDataChanges, recordersRequiringLicense);
			UpdateRecorderLicenses(recordersRequiringLicense, recorderChanges, callContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogError("License handling failed. Exception: {Exception}", ex2);
		}
	}

	private static void ApplyLogManagerConfigurationChanges(ConfigurationDataSet dataChanged)
	{
		if (dataChanged != null && dataChanged.SystemSettings.Rows.Count > 0)
		{
			ServerLogManager.Instance.UpdateSystemSettings(dataChanged.SystemSettings);
		}
	}

	private ResponseBase CheckFeaturesAndWriteChanges(ConfigurationDataSet allDataChanges, UpdateConfigurationManagerCommand cmd, ICallContextInfo callContextInfo)
	{
		ConfigurationDataSet configurationDataSet = (ConfigurationDataSet)allDataChanges.Copy();
		ResponseBase responseBase = null;
		try
		{
			if (cmd.PersistChanges)
			{
				bool flag = HasDeletedHardware(configurationDataSet);
				_server.UpdateSystemConfiguration(configurationDataSet, callContextInfo);
				if (flag)
				{
					ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
					CommandProcessor.ProcessAsync(new CleanupHardwareDefinitionsCommand(), state, new NullResponseSink());
				}
				if (LogSettingsUpdated(configurationDataSet))
				{
					LogDataAccessor.Initialize();
				}
				RegisteredServicesUpdated(configurationDataSet);
			}
			responseBase = new SuccessResponse();
			ServerLogManager.Instance.AddLogEntry(LogEntryId.DatabaseConnectionRestored);
		}
		catch (ServerServiceException exception)
		{
			responseBase = new ExceptionResponse(exception);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception exception2)
		{
			responseBase = new ExceptionResponse(exception2);
		}
		return responseBase;
	}

	private void ApplyRuleEngineConfigurationChanges(ConfigurationDataSet allDataChanges, ICallContextInfo callContextInfo)
	{
		_server.UpdateRuleConfigurationFromDiffGram(allDataChanges, new NullResponseSink(), callContextInfo);
	}

	public static void LogPendingAuditLogs(SessionLogger sessionLogger, ILogAuditLogger auditLogger)
	{
		foreach (LogEntryAudit sessionLog in sessionLogger.SessionLogs)
		{
			auditLogger.AddAuditLogEntry(sessionLog);
		}
	}

	public static string ConvertDeviceTypeToItemType(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => "Camera", 
			"AudioInput" => "Microphone", 
			"AudioOutput" => "Speaker", 
			"Metadata" => "Metadata", 
			"Input" => "InputEvent", 
			"Output" => "Output", 
			_ => throw new InvalidOperationException("Unknown device type: " + deviceType), 
		};
	}

	public static string ConvertDeviceTypeToItemGroupType(string deviceType)
	{
		return deviceType switch
		{
			"Camera" => "CameraGroup", 
			"AudioInput" => "MicrophoneGroup", 
			"AudioOutput" => "SpeakerGroup", 
			"Metadata" => "MetadataGroup", 
			"Input" => "InputEventGroup", 
			"Output" => "OutputGroup", 
			_ => throw new InvalidOperationException("Unknown device type: " + deviceType), 
		};
	}

	private static void RemoveTasksForDeletedDevices(IEnumerable<Guid> deletedDeviceIds)
	{
		try
		{
			if (deletedDeviceIds.Count() > 0)
			{
				using (TaskComponent taskComponent = new TaskComponent(Server.Instance.CorporateEncryptionSymmetric))
				{
					taskComponent.RemoveTasksByDeviceIds(deletedDeviceIds.ToArray());
					return;
				}
			}
		}
		catch (DatabaseException ex)
		{
			Server.Instance.Logger.LogError("Remove device tasks handling failed. Exception: {Exception}", ex);
		}
	}

	public static bool CheckForAutomaticActivationTrigger(ConfigurationDataSet allDataChanges)
	{
		bool flag = false;
		DataTable changes = allDataChanges.Hardware.GetChanges(DataRowState.Added | DataRowState.Deleted);
		DataTable changes2 = allDataChanges.Devices.GetChanges(DataRowState.Added | DataRowState.Deleted);
		if (changes != null || changes2 != null)
		{
			flag = true;
		}
		else
		{
			changes = allDataChanges.Hardware.GetChanges(DataRowState.Modified);
			changes2 = allDataChanges.Devices.GetChanges(DataRowState.Modified);
			if (changes != null)
			{
				foreach (ConfigurationDataSet.HardwareRow row in changes.Rows)
				{
					if (row.Enabled != row.GetEnabled(DataRowVersion.Original))
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag && changes2 != null)
			{
				foreach (ConfigurationDataSet.DevicesRow row2 in changes2.Rows)
				{
					if (row2.Enabled != row2.GetEnabled(DataRowVersion.Original))
					{
						flag = true;
					}
				}
			}
		}
		return flag;
	}

	private static bool LogSettingsUpdated(ConfigurationDataSet changes)
	{
		foreach (ConfigurationDataSet.SystemSettingsRow row in changes.SystemSettings.Rows)
		{
			if (row.Name.StartsWith("log", StringComparison.OrdinalIgnoreCase))
			{
				return true;
			}
		}
		foreach (ConfigurationDataSet.RegisteredServersRow row2 in changes.RegisteredServers.Rows)
		{
			if (string.Compare(row2.Type, ServiceTypes.ServiceType_LogService.ToString(), StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(row2.Type, ServiceTypes.ServiceType_LegacyLogService.ToString(), StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
		}
		return false;
	}

	private static void RegisteredServicesUpdated(ConfigurationDataSet changes)
	{
		if (changes.RegisteredServers.Count == 0)
		{
			return;
		}
		List<string> list = new List<string>();
		foreach (ConfigurationDataSet.RegisteredServersRow row in changes.RegisteredServers.Rows)
		{
			list.Add(row.IDServer);
		}
		try
		{
			using (ServerConnection serverConnection = new ServerConnection(ServerConfig.Instance.VmoCommunicationConfiguration.ServerName))
			{
				serverConnection.ServerPort = ServerConfig.Instance.VmoCommunicationConfiguration.Port;
				using VmoClient vmoClient = new VmoClient(serverConnection);
				vmoClient.ServerConnection.Connect();
				vmoClient.ServerConnection.GetClientProxy<ManagementServerProxy>().ResolveDataCollectorIdOnRegisteredServices(list.ToArray());
			}
			Server.Instance.ServerRegistrationChanged();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			Server.Instance.Logger.LogWarning("Failed to signal registered service resolver after registered service changes. Error: {ExceptionMessage}", ex2.Message);
		}
	}

	private static bool HasDeletedHardware(ConfigurationDataSet changes)
	{
		if (changes == null)
		{
			return false;
		}
		if (changes.Hardware == null)
		{
			return false;
		}
		return changes.Hardware.GetChanges(DataRowState.Deleted) != null;
	}

	private void AnalyzeChangesForConfigurationChangedEvents(ConfigurationDataSet dataChanged, ConfigurationDataSet dataDeleted, ConfigurationChangedEventCollector eventCollector, Dictionary<Guid, string> deviceIdNames, Dictionary<Guid, string> deviceIdTypes)
	{
		if (dataChanged != null)
		{
			foreach (ConfigurationDataSet.RecordersRow row in dataChanged.Recorders.Rows)
			{
				if (row.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row.IDRecorder, row.Name, ChangeAction.Added, "RecordingServer");
				}
				if (row.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row.IDRecorder, row.Name, ChangeAction.Updated, "RecordingServer");
				}
			}
			foreach (ConfigurationDataSet.HardwareRow row2 in dataChanged.Hardware.Rows)
			{
				if (row2.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row2.IDHardware, row2.Name, ChangeAction.Added, "Hardware");
				}
				if (row2.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row2.IDHardware, row2.Name, ChangeAction.Updated, "Hardware");
				}
			}
			foreach (ConfigurationDataSet.HardwareBlockRow row3 in dataChanged.HardwareBlock.Rows)
			{
				if (row3.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row3.hardware_id, ChangeAction.Updated, "Hardware");
				}
			}
			foreach (ConfigurationDataSet.DevicesRow row4 in dataChanged.Devices.Rows)
			{
				if (row4.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row4.IDDevice, row4.Name, ChangeAction.Added, ConvertDeviceTypeToItemType(row4.DeviceType));
				}
				if (row4.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row4.IDDevice, row4.Name, ChangeAction.Updated, ConvertDeviceTypeToItemType(row4.DeviceType));
				}
			}
			foreach (ConfigurationDataSet.DeviceDataDistributionRow row5 in dataChanged.DeviceDataDistribution.Rows)
			{
				string deviceType = FindDeviceTypeByDeviceId(new Guid(row5.device_id), deviceIdTypes);
				string text = ConvertDeviceTypeToItemType(deviceType);
				if (!eventCollector.EventExists(row5.device_id, text, ChangeAction.Added))
				{
					eventCollector.AddEvent(row5.device_id, ChangeAction.Updated, text);
				}
			}
			foreach (ConfigurationDataSet.DeviceTrackRow row6 in dataChanged.DeviceTrack.Rows)
			{
				string deviceType2 = FindDeviceTypeByDeviceId(new Guid(row6.IDDevice), deviceIdTypes);
				string text2 = ConvertDeviceTypeToItemType(deviceType2);
				if (!eventCollector.EventExists(row6.IDDevice, text2, ChangeAction.Added))
				{
					eventCollector.AddEvent(row6.IDDevice, ChangeAction.Updated, text2);
				}
			}
			foreach (ConfigurationDataSet.RecorderPluginDeviceRow row7 in dataChanged.RecorderPluginDevice.Rows)
			{
				string deviceType3 = FindDeviceTypeByDeviceId(new Guid(row7.IDDevice), deviceIdTypes);
				string text3 = ConvertDeviceTypeToItemType(deviceType3);
				if (!eventCollector.EventExists(row7.IDDevice, text3, ChangeAction.Added))
				{
					eventCollector.AddEvent(row7.IDDevice, ChangeAction.Updated, text3);
				}
			}
			foreach (ConfigurationDataSet.DeviceGroupsRow row8 in dataChanged.DeviceGroups.Rows)
			{
				string itemType = ConvertDeviceTypeToItemGroupType(row8.DeviceType);
				if (row8.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row8.IDDeviceGroup, ChangeAction.Added, itemType);
				}
				if (row8.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row8.IDDeviceGroup, ChangeAction.Updated, itemType);
				}
			}
			foreach (ConfigurationDataSet.DeviceGroupMembersRow row9 in dataChanged.DeviceGroupMembers.Rows)
			{
				string deviceType4 = FindDeviceTypeByDeviceId(new Guid(row9.IDDevice), deviceIdTypes);
				string text4 = ConvertDeviceTypeToItemGroupType(deviceType4);
				if (!eventCollector.EventExists(row9.IDDeviceGroup, text4, ChangeAction.Added))
				{
					eventCollector.AddEvent(row9.IDDeviceGroup, ChangeAction.Updated, text4);
				}
			}
			foreach (ConfigurationDataSet.DevicePrivacyMaskRow row10 in dataChanged.DevicePrivacyMask.Rows)
			{
				if (row10.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row10.IDDevice, ChangeAction.Added, "PrivacyProtection");
				}
				if (row10.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row10.IDDevice, ChangeAction.Updated, "PrivacyProtection");
				}
			}
			foreach (ConfigurationDataSet.PatrollingProfilesRow row11 in dataChanged.PatrollingProfiles.Rows)
			{
				if (row11.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row11.IDPatrollingProfile, ChangeAction.Added, "PatrollingProfile");
				}
				if (row11.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row11.IDPatrollingProfile, ChangeAction.Updated, "PatrollingProfile");
				}
			}
			foreach (ConfigurationDataSet.RecordingStorageRow row12 in dataChanged.RecordingStorage.Rows)
			{
				if (row12.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row12.pkid, row12.name, ChangeAction.Added, "Storage");
				}
				if (row12.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row12.pkid, row12.name, ChangeAction.Updated, "Storage");
				}
			}
			foreach (ConfigurationDataSet.ArchiveStorageRow row13 in dataChanged.ArchiveStorage.Rows)
			{
				if (row13.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row13.pkid, row13.name, ChangeAction.Added, "ArchiveStorage");
				}
				if (row13.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row13.pkid, row13.name, ChangeAction.Updated, "ArchiveStorage");
				}
			}
			foreach (ConfigurationDataSet.RulesRow row14 in dataChanged.Rules.Rows)
			{
				if (row14.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row14.IDRule, row14.Name, ChangeAction.Added, "Rule");
				}
				if (row14.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row14.IDRule, row14.Name, ChangeAction.Updated, "Rule");
				}
			}
			foreach (ConfigurationDataSet.TimeProfileRow row15 in dataChanged.TimeProfile.Rows)
			{
				if (row15.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row15.pkid, row15.name, ChangeAction.Added, "TimeProfile");
				}
				if (row15.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row15.pkid, row15.name, ChangeAction.Updated, "TimeProfile");
				}
			}
			foreach (ConfigurationDataSet.EventTypesRow row16 in dataChanged.EventTypes.Rows)
			{
				if (row16.RowState == DataRowState.Added)
				{
					eventCollector.AddEvent(row16.IDEventType, row16.Name, ChangeAction.Added, "UserDefinedEvent");
				}
				if (row16.RowState == DataRowState.Modified)
				{
					eventCollector.AddEvent(row16.IDEventType, row16.Name, ChangeAction.Updated, "UserDefinedEvent");
				}
			}
		}
		if (dataDeleted == null)
		{
			return;
		}
		foreach (ConfigurationDataSet.RecordersRow row17 in dataDeleted.Recorders.Rows)
		{
			eventCollector.AddEvent(row17.IDRecorder, row17.Name, ChangeAction.Deleted, "RecordingServer");
		}
		foreach (ConfigurationDataSet.HardwareRow row18 in dataDeleted.Hardware.Rows)
		{
			eventCollector.AddEvent(row18.IDHardware, row18.Name, ChangeAction.Deleted, "Hardware");
		}
		foreach (ConfigurationDataSet.HardwareBlockRow row19 in dataDeleted.HardwareBlock.Rows)
		{
			if (!eventCollector.EventExists(row19.hardware_id, "Hardware", ChangeAction.Deleted))
			{
				eventCollector.AddEvent(row19.hardware_id, ChangeAction.Updated, "Hardware");
			}
		}
		foreach (ConfigurationDataSet.DevicesRow row20 in dataDeleted.Devices.Rows)
		{
			eventCollector.AddEvent(row20.IDDevice, row20.Name, ChangeAction.Deleted, ConvertDeviceTypeToItemType(row20.DeviceType));
		}
		foreach (ConfigurationDataSet.DeviceDataDistributionRow row21 in dataDeleted.DeviceDataDistribution.Rows)
		{
			string deviceType5 = FindDeviceTypeByDeviceId(new Guid(row21.device_id), deviceIdTypes);
			string text5 = ConvertDeviceTypeToItemType(deviceType5);
			if (!eventCollector.EventExists(row21.device_id, text5, ChangeAction.Deleted))
			{
				eventCollector.AddEvent(row21.device_id, ChangeAction.Updated, text5);
			}
		}
		foreach (ConfigurationDataSet.DevicePrivacyMaskRow row22 in dataDeleted.DevicePrivacyMask.Rows)
		{
			eventCollector.AddEvent(row22.IDDevice, ChangeAction.Deleted, "PrivacyProtection");
		}
		foreach (ConfigurationDataSet.DeviceTrackRow row23 in dataDeleted.DeviceTrack.Rows)
		{
			string deviceType6 = FindDeviceTypeByDeviceId(new Guid(row23.IDDevice), deviceIdTypes);
			string text6 = ConvertDeviceTypeToItemType(deviceType6);
			if (!eventCollector.EventExists(row23.IDDevice, text6, ChangeAction.Deleted))
			{
				eventCollector.AddEvent(row23.IDDevice, ChangeAction.Updated, text6);
			}
		}
		foreach (ConfigurationDataSet.RecorderPluginDeviceRow row24 in dataDeleted.RecorderPluginDevice.Rows)
		{
			string deviceType7 = FindDeviceTypeByDeviceId(new Guid(row24.IDDevice), deviceIdTypes);
			string text7 = ConvertDeviceTypeToItemType(deviceType7);
			if (!eventCollector.EventExists(row24.IDDevice, text7, ChangeAction.Deleted))
			{
				eventCollector.AddEvent(row24.IDDevice, ChangeAction.Updated, text7);
			}
		}
		foreach (ConfigurationDataSet.PatrollingProfilesRow row25 in dataDeleted.PatrollingProfiles.Rows)
		{
			eventCollector.AddEvent(row25.IDDevice, ChangeAction.Deleted, "PatrollingProfile");
		}
		foreach (ConfigurationDataSet.DeviceGroupsRow row26 in dataDeleted.DeviceGroups.Rows)
		{
			string itemType2 = ConvertDeviceTypeToItemGroupType(row26.DeviceType);
			eventCollector.AddEvent(row26.IDDeviceGroup, ChangeAction.Deleted, itemType2);
		}
		foreach (ConfigurationDataSet.DeviceGroupMembersRow row27 in dataDeleted.DeviceGroupMembers.Rows)
		{
			string deviceType8 = FindDeviceTypeByDeviceId(new Guid(row27.IDDevice), deviceIdTypes);
			string text8 = ConvertDeviceTypeToItemGroupType(deviceType8);
			if (!eventCollector.EventExists(row27.IDDeviceGroup, text8, ChangeAction.Deleted))
			{
				eventCollector.AddEvent(row27.IDDeviceGroup, ChangeAction.Updated, text8);
			}
		}
		foreach (ConfigurationDataSet.RecordingStorageRow row28 in dataDeleted.RecordingStorage.Rows)
		{
			eventCollector.AddEvent(row28.pkid, row28.name, ChangeAction.Deleted, "Storage");
		}
		foreach (ConfigurationDataSet.ArchiveStorageRow row29 in dataDeleted.ArchiveStorage.Rows)
		{
			eventCollector.AddEvent(row29.pkid, row29.name, ChangeAction.Deleted, "ArchiveStorage");
		}
		foreach (ConfigurationDataSet.RulesRow row30 in dataDeleted.Rules.Rows)
		{
			eventCollector.AddEvent(row30.IDRule, row30.Name, ChangeAction.Deleted, "Rule");
		}
		foreach (ConfigurationDataSet.TimeProfileRow row31 in dataDeleted.TimeProfile.Rows)
		{
			eventCollector.AddEvent(row31.pkid, row31.name, ChangeAction.Deleted, "TimeProfile");
		}
		foreach (ConfigurationDataSet.EventTypesRow row32 in dataDeleted.EventTypes.Rows)
		{
			eventCollector.AddEvent(row32.IDEventType, row32.Name, ChangeAction.Deleted, "UserDefinedEvent");
		}
	}

	private ResponseBase AnalyzeChangesByRecorder(ConfigurationDataSet dataChanged, ConfigurationDataSet dataDeleted, Dictionary<Guid, ConfigurationDataSet> recorderChanges, RecordersRequiringLicense recordersRequiringLicense, List<Guid> deleteRecorderIds, Dictionary<Guid, string> recorderNames, List<Guid> deletedDeviceIds, LogEntryAuditLogger auditSessionLogger, ICallContextInfo callContextInfo, bool persistChanges, Dictionary<Guid, string> deviceIdNames, Dictionary<Guid, string> deviceIdTypes)
	{
		Dictionary<Guid, Guid> dictionary = new Dictionary<Guid, Guid>();
		Dictionary<Guid, Guid> dictionary2 = new Dictionary<Guid, Guid>();
		Dictionary<Guid, Guid> dictionary3 = new Dictionary<Guid, Guid>();
		HashSet<Guid> hashSet = new HashSet<Guid>();
		string userDomainAccountName = callContextInfo.UserDomainAccountName;
		string locationIPAddress = callContextInfo.LocationIPAddress;
		if (dataChanged != null)
		{
			foreach (ConfigurationDataSet.HardwareRow row in dataChanged.Hardware.Rows)
			{
				GetDiffgramDatasetForRecorder(recorderChanges, row.IDRecorder).Hardware.ImportRow(row);
				dictionary.Add(new Guid(row.IDHardware), new Guid(row.IDRecorder));
				string recordingServerName = FindRecorderNameByRecorderId(new Guid(row.IDRecorder), recorderNames);
				if (row.RowState == DataRowState.Added || row.Enabled != row.GetEnabled(DataRowVersion.Original))
				{
					recordersRequiringLicense.GetOrAdd(new Guid(row.IDRecorder)).CreateNewLicense = true;
				}
				if (row.RowState == DataRowState.Added)
				{
					ConfigurationDataSet diffgramDatasetForRecorder = GetDiffgramDatasetForRecorder(recorderChanges, row.IDRecorder);
					if (!row.IsHostedHardware_idNull())
					{
						using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_configurationDataLoader));
						VideoOS.Common.Integration.Configuration.HostedHardware hostedHardware = configurationManager.FindHostedHardwareById(row.HostedHardware_id);
						VideoOS.Common.Integration.Configuration.HostingServer hostingServer = hostedHardware.HostingServer;
						if (!diffgramDatasetForRecorder.HostingServer.Any((ConfigurationDataSet.HostingServerRow hs) => hs.pkid == hostingServer.Id))
						{
							DataRow dataRow = diffgramDatasetForRecorder.HostingServer.NewRow();
							dataRow.ItemArray = hostingServer.DataRow.ItemArray;
							diffgramDatasetForRecorder.HostingServer.Rows.Add(dataRow);
						}
						DataRow dataRow2 = diffgramDatasetForRecorder.HostedHardware.NewRow();
						dataRow2.ItemArray = hostedHardware.DataRow.ItemArray;
						diffgramDatasetForRecorder.HostedHardware.Rows.Add(dataRow2);
					}
					if (!diffgramDatasetForRecorder.HardwareDefinition.Any((ConfigurationDataSet.HardwareDefinitionRow d) => string.Equals(d.C18NDigest, row.C18NDigest, StringComparison.Ordinal)))
					{
						ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow = dataChanged.HardwareDefinition.FirstOrDefault((ConfigurationDataSet.HardwareDefinitionRow d) => string.Equals(d.C18NDigest, row.C18NDigest, StringComparison.Ordinal));
						if (hardwareDefinitionRow != null)
						{
							diffgramDatasetForRecorder.HardwareDefinition.ImportRow(hardwareDefinitionRow);
						}
						else
						{
							using ConfigurationManager configurationManager2 = new ConfigurationManager(new ConfigurationDataSetLoader(_configurationDataLoader));
							IPHardwareDefinition iPHardwareDefinition = configurationManager2.FindDefinitionByDigest(row.C18NDigest);
							DataRow dataRow3 = diffgramDatasetForRecorder.HardwareDefinition.NewRow();
							dataRow3.ItemArray = iPHardwareDefinition.DataRow.ItemArray;
							diffgramDatasetForRecorder.HardwareDefinition.Rows.Add(dataRow3);
						}
					}
					if (persistChanges)
					{
						auditSessionLogger.AuditHardwareAdded(new Guid(row.IDHardware), row.Name, recordingServerName, userDomainAccountName, locationIPAddress);
					}
				}
				if (row.RowState != DataRowState.Modified)
				{
					continue;
				}
				try
				{
					if (!persistChanges)
					{
						continue;
					}
					if (!string.Equals(row.GetSettings(DataRowVersion.Original), row.Settings) || !string.Equals(row.GetLoginID(DataRowVersion.Original), row.LoginID) || !string.Equals(row.GetURI(DataRowVersion.Original), row.URI) || !string.Equals(row.GetEncryptedPassword(DataRowVersion.Original), row.EncryptedPassword))
					{
						auditSessionLogger.AuditHardwareSettingsChanged(new Guid(row.IDHardware), row.Name, recordingServerName, userDomainAccountName, locationIPAddress);
					}
					string name = row.GetName(DataRowVersion.Original);
					if (!string.Equals(name, row.Name))
					{
						auditSessionLogger.AuditHardwareRenamed(new Guid(row.IDHardware), row.Name, name, recordingServerName, userDomainAccountName, locationIPAddress);
					}
					if (row.GetEnabled(DataRowVersion.Original) != row.Enabled)
					{
						if (row.Enabled)
						{
							auditSessionLogger.AuditHardwareEnabled(new Guid(row.IDHardware), row.Name, recordingServerName, userDomainAccountName, locationIPAddress);
						}
						else
						{
							auditSessionLogger.AuditHardwareDisabled(new Guid(row.IDHardware), row.Name, recordingServerName, userDomainAccountName, locationIPAddress);
						}
					}
				}
				catch (ConfigurationItemNotFoundException)
				{
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (Exception)
				{
				}
			}
			foreach (ConfigurationDataSet.HardwareBlockRow item in dataChanged.HardwareBlock)
			{
				string text = FindRecorderIDFromHardware(dictionary, item.hardware_id);
				if (text.Length > 0)
				{
					ConfigurationDataSet diffgramDatasetForRecorder2 = GetDiffgramDatasetForRecorder(recorderChanges, text);
					diffgramDatasetForRecorder2.HardwareBlock.ImportRow(item);
				}
			}
			foreach (ConfigurationDataSet.DevicesRow row2 in dataChanged.Devices.Rows)
			{
				dictionary2.Add(new Guid(row2.IDDevice), new Guid(row2.IDHardware));
				deviceIdNames[new Guid(row2.IDDevice)] = row2.Name;
				deviceIdTypes[new Guid(row2.IDDevice)] = row2.DeviceType;
				string text2 = FindRecorderIDFromHardware(dictionary, row2.IDHardware);
				string recordingServerName2 = FindRecorderNameByRecorderId(new Guid(text2), recorderNames);
				if (row2.RowState == DataRowState.Added)
				{
					auditSessionLogger.AuditDeviceAdded(new Guid(row2.IDDevice), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
					hashSet.Add(new Guid(row2.IDDevice));
				}
				if (row2.RowState == DataRowState.Modified)
				{
					try
					{
						if (persistChanges)
						{
							if (!string.Equals(row2.GetSettings(DataRowVersion.Original), row2.Settings))
							{
								auditSessionLogger.AuditDeviceSettingsChanged(new Guid(row2.IDDevice), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (row2.GetEnabled(DataRowVersion.Original) != row2.Enabled)
							{
								if (row2.Enabled)
								{
									auditSessionLogger.AuditDeviceEnabled(new Guid(row2.IDDevice), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
								}
								else
								{
									auditSessionLogger.AuditDeviceDisabled(new Guid(row2.IDDevice), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
								}
							}
							if (!string.Equals(row2.GetName(DataRowVersion.Original), row2.Name))
							{
								auditSessionLogger.AuditDeviceRenamed(new Guid(row2.IDDevice), row2.GetName(DataRowVersion.Original), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (!string.Equals(row2.GetGisPointString(DataRowVersion.Original), row2.GisPointString))
							{
								auditSessionLogger.AuditDeviceGisPointChanged(new Guid(row2.IDDevice), row2.Name, GisPointHelper.GisPointToDisplayText(row2.GisPointString), row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (!string.Equals(row2.GetShortName(DataRowVersion.Original), row2.ShortName))
							{
								auditSessionLogger.AuditDeviceShortNameChanged(new Guid(row2.IDDevice), row2.Name, row2.ShortName, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (row2.GetIcon(DataRowVersion.Original) != row2.Icon)
							{
								auditSessionLogger.AuditDeviceIconChanged(new Guid(row2.IDDevice), row2.Name, row2.Icon.ToString(NumberFormatInfo.InvariantInfo), row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (row2.GetCoverageDirection(DataRowVersion.Original) != row2.CoverageDirection)
							{
								auditSessionLogger.AuditDeviceCoverageDirectionChanged(new Guid(row2.IDDevice), row2.Name, DeviceCoverageHelper.CoverageDirectionToDisplayText(row2.CoverageDirection, NumberFormatInfo.InvariantInfo), row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (row2.GetCoverageDepth(DataRowVersion.Original) != row2.CoverageDepth)
							{
								auditSessionLogger.AuditDeviceCoverageDepthChanged(new Guid(row2.IDDevice), row2.Name, DeviceCoverageHelper.CoverageDepthToDisplayText(row2.CoverageDepth, toFeet: false, NumberFormatInfo.InvariantInfo), row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (row2.GetCoverageFieldOfView(DataRowVersion.Original) != row2.CoverageFieldOfView)
							{
								auditSessionLogger.AuditDeviceCoverageFieldOfViewChanged(new Guid(row2.IDDevice), row2.Name, DeviceCoverageHelper.CoverageFieldOfViewToDisplayText(row2.CoverageFieldOfView, NumberFormatInfo.InvariantInfo), row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							if (!string.Equals(row2.GetStreamSettings(DataRowVersion.Original), row2.Streams))
							{
								auditSessionLogger.AuditDeviceStreamSettingsChanged(new Guid(row2.IDDevice), row2.Name, row2.DeviceType, recordingServerName2, userDomainAccountName, locationIPAddress);
							}
							PtzPresetCollection ptzPresetCollection = PtzPresetCollection.FromXml(row2.GetPresetSettings(DataRowVersion.Original));
							PtzPresetCollection ptzPresetCollection2 = PtzPresetCollection.FromXml(row2.GetPresetSettings(DataRowVersion.Current));
							foreach (PtzPreset preset in ptzPresetCollection2)
							{
								PtzPreset ptzPreset = ptzPresetCollection.FirstOrDefault((PtzPreset f) => f.Id == preset.Id);
								if (ptzPreset == null)
								{
									auditSessionLogger.AuditPtzPresetCreated(new Guid(row2.IDDevice), row2.Name, preset.Name, recordingServerName2, userDomainAccountName, locationIPAddress);
								}
								else if (!preset.EqualsTo(ptzPreset))
								{
									auditSessionLogger.AuditPtzPresetChanged(new Guid(row2.IDDevice), row2.Name, preset.Name, recordingServerName2, userDomainAccountName, locationIPAddress);
								}
							}
							foreach (PtzPreset preset2 in ptzPresetCollection)
							{
								PtzPreset ptzPreset2 = ptzPresetCollection2.FirstOrDefault((PtzPreset f) => f.Id == preset2.Id);
								if (ptzPreset2 == null)
								{
									auditSessionLogger.AuditPtzPresetDeleted(new Guid(row2.IDDevice), row2.Name, preset2.Name, recordingServerName2, userDomainAccountName, locationIPAddress);
								}
							}
						}
					}
					catch (ConfigurationItemNotFoundException)
					{
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch (Exception)
					{
					}
				}
				if (text2.Length <= 0)
				{
					continue;
				}
				GetDiffgramDatasetForRecorder(recorderChanges, text2).Devices.ImportRow(row2);
				string text3 = null;
				if (row2.RowState == DataRowState.Added || row2.Enabled != row2.GetEnabled(DataRowVersion.Original))
				{
					recordersRequiringLicense.GetOrAdd(new Guid(text2)).CreateNewLicense = true;
					ConfigurationDataSet.HardwareRow hardwareRow = dataChanged.Hardware.FindByIDHardware(row2.IDHardware);
					if (hardwareRow != null)
					{
						HardwareSettingUtilities hardwareSettingUtilities = new HardwareSettingUtilities(hardwareRow.Settings);
						text3 = hardwareSettingUtilities.MacAddress;
					}
					if (string.IsNullOrEmpty(text3))
					{
						text3 = ConfigurationDataManager.FindHardwareMacAddressByDeviceId(new Guid(row2.IDDevice));
					}
				}
				else if (row2.RowState == DataRowState.Modified)
				{
					if (string.IsNullOrEmpty(text3))
					{
						text3 = ConfigurationDataManager.FindHardwareMacAddressByDeviceId(new Guid(row2.IDDevice));
					}
				}
				else if (row2.RowState == DataRowState.Deleted)
				{
					recordersRequiringLicense.GetOrAdd(new Guid(text2)).CreateNewLicense = true;
					ConfigurationDataSet.HardwareRow hardwareRow2 = dataDeleted.Hardware.Rows.Cast<ConfigurationDataSet.HardwareRow>().FirstOrDefault((ConfigurationDataSet.HardwareRow r) => string.Equals(r.GetIDHardware(DataRowVersion.Original), row2.IDHardware, StringComparison.OrdinalIgnoreCase));
					if (hardwareRow2 != null)
					{
						HardwareSettingUtilities hardwareSettingUtilities2 = new HardwareSettingUtilities(hardwareRow2.GetSettings(DataRowVersion.Original));
						text3 = hardwareSettingUtilities2.MacAddress;
					}
				}
				if (string.IsNullOrEmpty(text3))
				{
					throw new InvalidOperationException("Could not find hardware mac address for device being modified, added or deleted.");
				}
				IEnumerable<Guid> relatedRecordingServers = _server.LicenseManager.CountManager.GetRelatedRecordingServers(text3);
				foreach (Guid item2 in relatedRecordingServers)
				{
					if (new Guid(text2) != item2)
					{
						recordersRequiringLicense.GetOrAdd(item2).CreateNewLicense = true;
					}
				}
			}
			foreach (ConfigurationDataSet.DeviceLinksRow row3 in dataChanged.DeviceLinks.Rows)
			{
				string text4 = FindRecorderIDFromDevice(dictionary, dictionary2, row3.IDDevice);
				string text5 = FindRecorderIDFromDevice(dictionary, dictionary2, row3.IDDeviceLinked);
				if (text4.Length > 0 && text5.Length > 0 && string.Equals(text4, text5, StringComparison.OrdinalIgnoreCase))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, text4).DeviceLinks.ImportRow(row3);
				}
			}
			foreach (ConfigurationDataSet.DeviceDataDistributionRow item3 in dataChanged.DeviceDataDistribution)
			{
				string text6 = FindRecorderIDFromDevice(dictionary, dictionary2, item3.device_id);
				if (text6.Length > 0)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, text6).DeviceDataDistribution.ImportRow(item3);
				}
			}
			foreach (ConfigurationDataSet.DevicePrivacyMaskRow row4 in dataChanged.DevicePrivacyMask.Rows)
			{
				string text7 = FindRecorderIDFromDevice(dictionary, dictionary2, row4.IDDevice);
				if (!string.IsNullOrEmpty(text7))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, text7).DevicePrivacyMask.ImportRow(row4);
				}
			}
			foreach (ConfigurationDataSet.DeviceTrackRow row5 in dataChanged.DeviceTrack.Rows)
			{
				string text8 = FindRecorderIDFromDevice(dictionary, dictionary2, row5.IDDevice);
				if (!string.IsNullOrEmpty(text8))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, text8).DeviceTrack.ImportRow(row5);
				}
			}
			foreach (IGrouping<string, ConfigurationDataSet.RecorderPluginDeviceRow> item4 in from r in dataChanged.RecorderPluginDevice
				group r by r.GetIDDevice())
			{
				string key = item4.Key;
				string text9 = FindRecorderIDFromDevice(dictionary, dictionary2, key);
				if (string.IsNullOrEmpty(text9))
				{
					continue;
				}
				foreach (ConfigurationDataSet.RecorderPluginDeviceRow item5 in item4)
				{
					GetDiffgramDatasetForRecorder(recorderChanges, text9).RecorderPluginDevice.ImportRow(item5);
				}
				string recordingServerName3 = FindRecorderNameByRecorderId(new Guid(text9), recorderNames);
				string deviceName = FindDeviceNameByDeviceId(new Guid(key), deviceIdNames);
				string deviceType = FindDeviceTypeByDeviceId(new Guid(key), deviceIdTypes);
				IEnumerable<ConfigurationDataSet.RecorderPluginDeviceRow> source = item4.Where((ConfigurationDataSet.RecorderPluginDeviceRow r) => GlobalIDs.ImmerVisionPlugInGuid.Equals(new Guid(r.IDRecorderPlugin)));
				if (source.Any())
				{
					bool flag = source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Modified && r.GetEnabled(DataRowVersion.Original));
					bool flag2 = source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.Enabled);
					if (flag && !flag2)
					{
						auditSessionLogger.AuditDevice360LensSettingsDisabled(new Guid(key), deviceName, deviceType, recordingServerName3, userDomainAccountName, locationIPAddress);
					}
					else if (!flag && flag2)
					{
						auditSessionLogger.AuditDevice360LensSettingsEnabled(new Guid(key), deviceName, deviceType, recordingServerName3, userDomainAccountName, locationIPAddress);
					}
					else
					{
						bool flag3 = source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Modified && r.GetEnabled(DataRowVersion.Current) && !r.GetEnabled(DataRowVersion.Original)) && source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Modified && !r.GetEnabled(DataRowVersion.Current) && r.GetEnabled(DataRowVersion.Original));
						bool flag4 = source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Added && r.Enabled) && source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Modified && !r.GetEnabled(DataRowVersion.Current) && r.GetEnabled(DataRowVersion.Original));
						bool flag5 = source.Any((ConfigurationDataSet.RecorderPluginDeviceRow r) => r.RowState == DataRowState.Modified && r.IsSettingsChanged());
						if (flag3 || flag4 || flag5)
						{
							auditSessionLogger.AuditDevice360LensSettingsChanged(new Guid(key), deviceName, deviceType, recordingServerName3, userDomainAccountName, locationIPAddress);
						}
					}
				}
				ConfigurationDataSet.RecorderPluginDeviceRow recorderPluginDeviceRow = item4.FirstOrDefault((ConfigurationDataSet.RecorderPluginDeviceRow r) => WellKnownPluginIds.MotionPluginId == new Guid(r.IDRecorderPlugin));
				if (recorderPluginDeviceRow != null && !hashSet.Contains(new Guid(key)))
				{
					auditSessionLogger.AuditDeviceMotionDetectionSettingsChanged(new Guid(key), deviceName, deviceType, recordingServerName3, userDomainAccountName, locationIPAddress);
				}
			}
			foreach (ConfigurationDataSet.PatrollingProfilesRow row6 in dataChanged.PatrollingProfiles.Rows)
			{
				string text10 = FindRecorderIDFromDevice(dictionary, dictionary2, row6.IDDevice);
				if (text10.Length <= 0)
				{
					continue;
				}
				GetDiffgramDatasetForRecorder(recorderChanges, text10).PatrollingProfiles.ImportRow(row6);
				string cameraName = ConfigurationDataManager.FindDeviceNameByDeviceId(new Guid(row6.IDDevice));
				string recordingServerName4 = ConfigurationDataManager.FindRecorderNameByDeviceId(new Guid(row6.IDDevice));
				if (persistChanges)
				{
					if (row6.RowState == DataRowState.Added)
					{
						auditSessionLogger.AuditPtzPatrollingProfileCreated(new Guid(row6.IDDevice), cameraName, row6.Name, recordingServerName4, userDomainAccountName, locationIPAddress);
					}
					if (row6.RowState == DataRowState.Modified)
					{
						auditSessionLogger.AuditPtzPatrollingProfileChanged(new Guid(row6.IDDevice), cameraName, row6.Name, recordingServerName4, userDomainAccountName, locationIPAddress);
					}
				}
			}
			foreach (ConfigurationDataSet.RecordingStorageRow row7 in dataChanged.RecordingStorage.Rows)
			{
				dictionary3[new Guid(row7.pkid)] = new Guid(row7.recordingserver_id);
				GetDiffgramDatasetForRecorder(recorderChanges, row7.recordingserver_id).RecordingStorage.ImportRow(row7);
			}
			foreach (ConfigurationDataSet.ArchiveStorageRow row8 in dataChanged.ArchiveStorage.Rows)
			{
				Guid guid = FindRecorderIdFromRecordingStorage(dictionary3, new Guid(row8.recordingstorage_id));
				if (!(guid == Guid.Empty))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, guid).ArchiveStorage.ImportRow(row8);
				}
			}
			foreach (ConfigurationDataSet.RecordingStorageDeviceRow row9 in dataChanged.RecordingStorageDevice.Rows)
			{
				if (persistChanges && row9.RowState == DataRowState.Modified)
				{
					if (row9.IsRecordingStorageIdChanged())
					{
						Guid guid2 = new Guid(row9.device_id);
						string databaseBankName = string.Empty;
						try
						{
							databaseBankName = ConfigurationDataManager.FindRecordingStorageNameByRecordingStorageId(new Guid(row9.recordingstorage_id));
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						string recordingServerName5 = string.Empty;
						try
						{
							recordingServerName5 = ConfigurationDataManager.FindRecorderNameByDeviceId(guid2);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						string deviceName2 = string.Empty;
						try
						{
							deviceName2 = ConfigurationDataManager.FindDeviceNameByDeviceId(guid2);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						string deviceType2 = string.Empty;
						try
						{
							deviceType2 = ConfigurationDataManager.FindDeviceTypeByDeviceId(guid2);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						auditSessionLogger.AuditDeviceDatabaseMove(guid2, deviceName2, deviceType2, databaseBankName, recordingServerName5, userDomainAccountName, locationIPAddress);
					}
					if (row9.IsRecordingEnabledChanged() || row9.IsRecordingFramerateChanged() || row9.IsPrebufferEnabledChanged() || row9.IsPrebufferSecondsChanged() || row9.IsEdgeStorageEnabledChanged())
					{
						Guid guid3 = new Guid(row9.device_id);
						string recordingServerName6 = string.Empty;
						try
						{
							recordingServerName6 = ConfigurationDataManager.FindRecorderNameByDeviceId(guid3);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						string deviceName3 = string.Empty;
						try
						{
							deviceName3 = ConfigurationDataManager.FindDeviceNameByDeviceId(guid3);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						string deviceType3 = string.Empty;
						try
						{
							deviceType3 = ConfigurationDataManager.FindDeviceTypeByDeviceId(guid3);
						}
						catch (OutOfMemoryException)
						{
							throw;
						}
						catch
						{
						}
						auditSessionLogger.AuditDeviceRecordingSettingsChanged(guid3, deviceName3, deviceType3, recordingServerName6, userDomainAccountName, locationIPAddress);
					}
				}
				Guid guid4 = FindRecorderIdFromRecordingStorage(dictionary3, new Guid(row9.recordingstorage_id));
				if (!(guid4 == Guid.Empty))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, guid4).RecordingStorageDevice.ImportRow(row9);
				}
			}
			foreach (ConfigurationDataSet.RecordersRow row10 in dataChanged.Recorders.Rows)
			{
				GetDiffgramDatasetForRecorder(recorderChanges, row10.IDRecorder).Recorders.ImportRow(row10);
				if (persistChanges && row10.RowState == DataRowState.Modified && !string.Equals(row10.GetName(DataRowVersion.Original), row10.Name))
				{
					auditSessionLogger.AuditRecordingServerRename(row10.GetName(DataRowVersion.Original), row10.Name, new Guid(row10.IDRecorder), userDomainAccountName, locationIPAddress);
				}
			}
			foreach (ConfigurationDataSet.RecordingServerServiceRow row11 in dataChanged.RecordingServerService.Rows)
			{
				Guid guid5 = FindRecorderIdFromServiceId(new Guid(row11.pkid));
				if (!(guid5 == Guid.Empty))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, guid5).RecordingServerService.ImportRow(row11);
				}
			}
			foreach (ConfigurationDataSet.RecordingServerServiceConfigurableRow row12 in dataChanged.RecordingServerServiceConfigurable.Rows)
			{
				Guid guid6 = FindRecorderIdFromServiceId(new Guid(row12.PKID));
				if (!(guid6 == Guid.Empty))
				{
					GetDiffgramDatasetForRecorder(recorderChanges, guid6).RecordingServerServiceConfigurable.ImportRow(row12);
				}
			}
			if (persistChanges)
			{
				foreach (ConfigurationDataSet.RulesRow row13 in dataChanged.Rules.Rows)
				{
					if (row13.RowState == DataRowState.Added)
					{
						auditSessionLogger.AuditRuleCreated(new Guid(row13.IDRule), row13.Name, userDomainAccountName, locationIPAddress);
					}
					if (row13.RowState != DataRowState.Modified)
					{
						continue;
					}
					if (!string.Equals(row13.GetName(DataRowVersion.Original), row13.Name))
					{
						auditSessionLogger.AuditRuleRenamed(row13.GetName(DataRowVersion.Original), row13.Name, new Guid(row13.IDRule), userDomainAccountName, locationIPAddress);
					}
					if (!string.Equals(row13.GetDefinition(DataRowVersion.Original), row13.Definition))
					{
						auditSessionLogger.AuditRuleEdited(new Guid(row13.IDRule), row13.Name, userDomainAccountName, locationIPAddress);
					}
					if (row13.GetEnabled(DataRowVersion.Original) != row13.Enabled)
					{
						if (row13.Enabled)
						{
							auditSessionLogger.AuditRuleEnabled(new Guid(row13.IDRule), row13.Name, userDomainAccountName, locationIPAddress);
						}
						else
						{
							auditSessionLogger.AuditRuleDisabled(new Guid(row13.IDRule), row13.Name, userDomainAccountName, locationIPAddress);
						}
					}
				}
			}
			if (persistChanges)
			{
				foreach (ConfigurationDataSet.TimeProfileRow row14 in dataChanged.TimeProfile.Rows)
				{
					if (row14.RowState == DataRowState.Added)
					{
						auditSessionLogger.AuditTimeProfileCreated(new Guid(row14.pkid), row14.name, userDomainAccountName, locationIPAddress);
					}
					if (row14.RowState == DataRowState.Modified)
					{
						if (!string.Equals(row14.GetName(DataRowVersion.Original), row14.name))
						{
							auditSessionLogger.AuditTimeProfileRenamed(row14.GetName(DataRowVersion.Original), row14.name, new Guid(row14.pkid), userDomainAccountName, locationIPAddress);
						}
						if (!string.Equals(row14.GetDefinition(DataRowVersion.Original), row14.xml_data))
						{
							auditSessionLogger.AuditTimeProfileEdited(new Guid(row14.pkid), row14.name, userDomainAccountName, locationIPAddress);
						}
					}
				}
			}
		}
		if (dataDeleted != null)
		{
			dataDeleted.RejectChanges();
			List<Guid> list = new List<Guid>();
			List<LockedDevices> list2 = new List<LockedDevices>();
			foreach (ConfigurationDataSet.RecordersRow row15 in dataDeleted.Recorders.Rows)
			{
				list.Add(new Guid(row15.IDRecorder));
			}
			foreach (ConfigurationDataSet.HardwareRow row16 in dataDeleted.Hardware.Rows)
			{
				dictionary.Add(new Guid(row16.IDHardware), new Guid(row16.IDRecorder));
				DataRow dataRow4 = GetDiffgramDatasetForRecorder(recorderChanges, row16.IDRecorder).Hardware.LoadDataRow(row16.ItemArray, fAcceptChanges: true);
				dataRow4.Delete();
				recordersRequiringLicense.GetOrAdd(new Guid(row16.IDRecorder)).CreateNewLicense = true;
				string recordingServerName7 = FindRecorderNameByRecorderId(new Guid(row16.IDRecorder), recorderNames);
				auditSessionLogger.AuditHardwareRemoved(new Guid(row16.IDHardware), row16.Name, recordingServerName7, userDomainAccountName, locationIPAddress);
			}
			foreach (ConfigurationDataSet.HardwareBlockRow item6 in dataDeleted.HardwareBlock)
			{
				string text11 = FindRecorderIDFromHardware(dictionary, item6.hardware_id);
				if (text11.Length > 0)
				{
					DataRow dataRow5 = GetDiffgramDatasetForRecorder(recorderChanges, text11).HardwareBlock.LoadDataRow(item6.ItemArray, fAcceptChanges: true);
					dataRow5.Delete();
				}
			}
			foreach (ConfigurationDataSet.DevicesRow row17 in dataDeleted.Devices.Rows)
			{
				list2.Add(new LockedDevices(row17.IDDevice, row17.Name, row17.IDHardware));
				dictionary2.Add(new Guid(row17.IDDevice), new Guid(row17.IDHardware));
				deletedDeviceIds.Add(new Guid(row17.IDDevice));
				string text12 = FindRecorderIDFromDevice(dictionary, dictionary2, row17.IDDevice);
				if (text12.Length > 0)
				{
					DataRow dataRow6 = GetDiffgramDatasetForRecorder(recorderChanges, text12).Devices.LoadDataRow(row17.ItemArray, fAcceptChanges: true);
					dataRow6.Delete();
					recordersRequiringLicense.GetOrAdd(new Guid(text12)).CreateNewLicense = true;
					string recordingServerName8 = FindRecorderNameByRecorderId(new Guid(text12), recorderNames);
					auditSessionLogger.AuditDeviceRemoved(new Guid(row17.IDDevice), row17.Name, row17.DeviceType, recordingServerName8, userDomainAccountName, locationIPAddress);
				}
			}
			foreach (ConfigurationDataSet.DeviceLinksRow row18 in dataDeleted.DeviceLinks.Rows)
			{
				string text13 = FindRecorderIDFromDevice(dictionary, dictionary2, row18.IDDevice);
				string text14 = FindRecorderIDFromDevice(dictionary, dictionary2, row18.IDDeviceLinked);
				if (text13.Length > 0 && text14.Length > 0 && string.Equals(text13, text14, StringComparison.OrdinalIgnoreCase))
				{
					DataRow dataRow7 = GetDiffgramDatasetForRecorder(recorderChanges, text13).DeviceLinks.LoadDataRow(row18.ItemArray, fAcceptChanges: true);
					dataRow7.Delete();
				}
			}
			foreach (ConfigurationDataSet.DeviceDataDistributionRow item7 in dataDeleted.DeviceDataDistribution)
			{
				string text15 = FindRecorderIDFromDevice(dictionary, dictionary2, item7.device_id);
				if (text15.Length > 0)
				{
					DataRow dataRow8 = GetDiffgramDatasetForRecorder(recorderChanges, text15).DeviceDataDistribution.LoadDataRow(item7.ItemArray, fAcceptChanges: true);
					dataRow8.Delete();
				}
			}
			foreach (ConfigurationDataSet.DevicePrivacyMaskRow row19 in dataDeleted.DevicePrivacyMask.Rows)
			{
				string text16 = FindRecorderIDFromDevice(dictionary, dictionary2, row19.IDDevice);
				if (!string.IsNullOrEmpty(text16))
				{
					DataRow dataRow9 = GetDiffgramDatasetForRecorder(recorderChanges, text16).DevicePrivacyMask.LoadDataRow(row19.ItemArray, fAcceptChanges: true);
					dataRow9.Delete();
				}
			}
			foreach (ConfigurationDataSet.DeviceTrackRow row20 in dataDeleted.DeviceTrack.Rows)
			{
				string text17 = FindRecorderIDFromDevice(dictionary, dictionary2, row20.IDDevice);
				if (!string.IsNullOrEmpty(text17))
				{
					DataRow dataRow10 = GetDiffgramDatasetForRecorder(recorderChanges, text17).DeviceTrack.LoadDataRow(row20.ItemArray, fAcceptChanges: true);
					dataRow10.Delete();
				}
			}
			List<Guid> list3 = new List<Guid>();
			foreach (Guid item8 in list)
			{
				if (!IsRecorderOnline(item8))
				{
					list3.Add(item8);
				}
			}
			List<Guid> list4 = new List<Guid>();
			Guid guid7 = Guid.Empty;
			foreach (Guid key2 in dictionary2.Keys)
			{
				if (!dictionary2.TryGetValue(key2, out var value) || !dictionary.TryGetValue(value, out var value2) || !list3.Contains(value2))
				{
					list4.Add(key2);
					if (HasDeviceEvidenceLock(key2))
					{
						guid7 = key2;
						break;
					}
				}
			}
			if (guid7 != Guid.Empty)
			{
				string hardwareName = string.Empty;
				Guid hardwareId = Guid.Empty;
				dictionary2.TryGetValue(guid7, out hardwareId);
				List<Guid> deviceIdsFailedOnSameHardware = new List<Guid> { guid7 };
				foreach (Guid item9 in from dh in dictionary2
					where dh.Value.Equals(hardwareId)
					select dh.Key)
				{
					if (!list4.Contains(item9) && HasDeviceEvidenceLock(item9))
					{
						deviceIdsFailedOnSameHardware.Add(item9);
					}
				}
				ConfigurationDataSet configurationDataSet = _configurationDataLoader.LoadHardwareByHardwareId(hardwareId);
				foreach (ConfigurationDataSet.HardwareRow item10 in configurationDataSet.Hardware)
				{
					hardwareName = item10.Name;
				}
				List<LockedDevices> devices = list2.Where((LockedDevices f) => deviceIdsFailedOnSameHardware.Contains(new Guid(f.DeviceId))).ToList();
				return EvidenceLockFailedRemoveResponse.CreateRemoveDevicesFailedResponse(hardwareName, devices);
			}
			foreach (ConfigurationDataSet.RecorderPluginDeviceRow row21 in dataDeleted.RecorderPluginDevice.Rows)
			{
				string text18 = FindRecorderIDFromDevice(dictionary, dictionary2, row21.IDDevice);
				if (text18.Length > 0)
				{
					DataRow dataRow11 = GetDiffgramDatasetForRecorder(recorderChanges, text18).RecorderPluginDevice.LoadDataRow(row21.ItemArray, fAcceptChanges: true);
					dataRow11.Delete();
				}
			}
			foreach (ConfigurationDataSet.PatrollingProfilesRow row22 in dataDeleted.PatrollingProfiles.Rows)
			{
				string text19 = FindRecorderIDFromDevice(dictionary, dictionary2, row22.IDDevice);
				if (text19.Length > 0)
				{
					DataRow dataRow12 = GetDiffgramDatasetForRecorder(recorderChanges, text19).PatrollingProfiles.LoadDataRow(row22.ItemArray, fAcceptChanges: true);
					dataRow12.Delete();
					if (persistChanges)
					{
						string cameraName2 = ConfigurationDataManager.FindDeviceNameByDeviceId(new Guid(row22.IDDevice));
						string recordingServerName9 = ConfigurationDataManager.FindRecorderNameByDeviceId(new Guid(row22.IDDevice));
						auditSessionLogger.AuditPtzPatrollingProfileDeleted(new Guid(row22.IDDevice), cameraName2, row22.Name, recordingServerName9, userDomainAccountName, locationIPAddress);
					}
				}
			}
			foreach (ConfigurationDataSet.RecordingStorageRow row23 in dataDeleted.RecordingStorage.Rows)
			{
				dictionary3[new Guid(row23.pkid)] = new Guid(row23.recordingserver_id);
				DataRow dataRow13 = GetDiffgramDatasetForRecorder(recorderChanges, row23.recordingserver_id).RecordingStorage.LoadDataRow(row23.ItemArray, fAcceptChanges: true);
				dataRow13.Delete();
			}
			foreach (ConfigurationDataSet.ArchiveStorageRow row24 in dataDeleted.ArchiveStorage.Rows)
			{
				Guid guid8 = FindRecorderIdFromRecordingStorage(dictionary3, new Guid(row24.recordingstorage_id));
				if (!(guid8 == Guid.Empty))
				{
					DataRow dataRow14 = GetDiffgramDatasetForRecorder(recorderChanges, guid8).ArchiveStorage.LoadDataRow(row24.ItemArray, fAcceptChanges: true);
					dataRow14.Delete();
				}
			}
			foreach (ConfigurationDataSet.RecordingStorageDeviceRow row25 in dataDeleted.RecordingStorageDevice.Rows)
			{
				Guid guid9 = FindRecorderIdFromRecordingStorage(dictionary3, new Guid(row25.recordingstorage_id));
				if (!(guid9 == Guid.Empty))
				{
					DataRow dataRow15 = GetDiffgramDatasetForRecorder(recorderChanges, guid9).RecordingStorageDevice.LoadDataRow(row25.ItemArray, fAcceptChanges: true);
					dataRow15.Delete();
				}
			}
			foreach (ConfigurationDataSet.RecordersRow row26 in dataDeleted.Recorders.Rows)
			{
				RecorderRequiringLicense orAdd = recordersRequiringLicense.GetOrAdd(new Guid(row26.IDRecorder));
				orAdd.CreateNewLicense = false;
				orAdd.RecorderDeleted = true;
				deleteRecorderIds.Add(new Guid(row26.IDRecorder));
				DataRow dataRow16 = GetDiffgramDatasetForRecorder(recorderChanges, row26.IDRecorder).Recorders.LoadDataRow(row26.ItemArray, fAcceptChanges: true);
				dataRow16.Delete();
				auditSessionLogger.AuditRecordingServerRemove(new Guid(row26.IDRecorder), row26.Name, userDomainAccountName, locationIPAddress);
			}
			foreach (ConfigurationDataSet.RecordingServerServiceRow row27 in dataDeleted.RecordingServerService.Rows)
			{
				Guid guid10 = FindRecorderIdFromServiceId(new Guid(row27.pkid));
				if (!(guid10 == Guid.Empty))
				{
					DataRow dataRow17 = GetDiffgramDatasetForRecorder(recorderChanges, guid10).RecordingServerService.LoadDataRow(row27.ItemArray, fAcceptChanges: true);
					dataRow17.Delete();
				}
			}
			foreach (ConfigurationDataSet.RecordingServerServiceConfigurableRow row28 in dataDeleted.RecordingServerServiceConfigurable.Rows)
			{
				Guid guid11 = FindRecorderIdFromServiceId(new Guid(row28.PKID));
				if (!(guid11 == Guid.Empty))
				{
					DataRow dataRow18 = GetDiffgramDatasetForRecorder(recorderChanges, guid11).RecordingServerServiceConfigurable.LoadDataRow(row28.ItemArray, fAcceptChanges: true);
					dataRow18.Delete();
				}
			}
			if (persistChanges)
			{
				foreach (ConfigurationDataSet.RulesRow row29 in dataDeleted.Rules.Rows)
				{
					auditSessionLogger.AuditRuleDelete(new Guid(row29.IDRule), row29.Name, userDomainAccountName, locationIPAddress);
				}
			}
			foreach (ConfigurationDataSet.TimeProfileRow row30 in dataDeleted.TimeProfile.Rows)
			{
				auditSessionLogger.AuditTimeProfileDelete(new Guid(row30.pkid), row30.name, userDomainAccountName, locationIPAddress);
			}
		}
		return null;
	}

	private bool IsRecorderOnline(Guid recorderId)
	{
		return _recorderCommManager.IsRecorderAttachedAndConnectedNotInFailover(recorderId);
	}

	private bool HasDeviceEvidenceLock(Guid idDevice)
	{
		return _server.HasDeviceEvidenceLock(idDevice.ToString());
	}

	private void AnalyzeChangesForAllRecorders(ConfigurationDataSet allChanges, Dictionary<Guid, ConfigurationDataSet> recorderChanges)
	{
		bool flag = false;
		DataTable[] array = new DataTable[15]
		{
			allChanges.DeviceGroupMembers.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.DeviceGroups.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.EventTypes.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.HostingServer.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.RecorderInfo.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.Rules.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.RuleVariables.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.ServerGroup.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.ServerGroupMembership.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.SystemSettings.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.TimeProfile.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.AudioMessage.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.RecorderInfo.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.FailoverInfo.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified),
			allChanges.DriverNumbers.GetChanges(DataRowState.Added | DataRowState.Deleted | DataRowState.Modified)
		};
		DataTable[] array2 = array;
		foreach (DataTable dataTable in array2)
		{
			if (dataTable != null)
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		RecorderConfigData[] array3 = RecorderConfigData.QueryRecorders(_dataComponentFactory);
		RecorderConfigData[] array4 = array3;
		foreach (RecorderConfigData recorderConfigData in array4)
		{
			ConfigurationDataSet diffgramDatasetForRecorder = GetDiffgramDatasetForRecorder(recorderChanges, recorderConfigData.Id);
			DataTable[] array5 = array;
			foreach (DataTable dataTable2 in array5)
			{
				if (dataTable2 == null)
				{
					continue;
				}
				DataTable dataTable3 = diffgramDatasetForRecorder.Tables[dataTable2.TableName];
				foreach (DataRow row in dataTable2.Rows)
				{
					dataTable3.ImportRow(row);
				}
			}
		}
	}

	private void UpdateRecorderLicenses(RecordersRequiringLicense recordersRequiringLicense, Dictionary<Guid, ConfigurationDataSet> recorderChanges, ICallContextInfo callContextInfo)
	{
		if (!recordersRequiringLicense.Values.Any((RecorderRequiringLicense r) => r.CreateNewLicense || r.NewLicenseCreated || r.RecorderDeleted))
		{
			return;
		}
		_configurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			Dictionary<Guid, byte[]> dictionary = new Dictionary<Guid, byte[]>();
			lock (_server.LicenseManager)
			{
				if (recordersRequiringLicense.Values.Any((RecorderRequiringLicense r) => r.CreateNewLicense || r.RecorderDeleted))
				{
					ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
					requiredService.ClearAllCaches();
				}
				foreach (RecorderRequiringLicense item in recordersRequiringLicense.Values.Where((RecorderRequiringLicense r) => r.CreateNewLicense || r.RecorderDeleted))
				{
					_server.LicenseManager.CountManager.ClearByRecorder(item.Id);
				}
				foreach (RecorderRequiringLicense item2 in recordersRequiringLicense.Values.Where((RecorderRequiringLicense r) => r.CreateNewLicense || r.NewLicenseCreated))
				{
					byte[] value = new byte[0];
					try
					{
						value = _server.RequestRecorderLicense(item2.Id);
					}
					catch (OutOfMemoryException)
					{
						throw;
					}
					catch
					{
					}
					if (!item2.RecorderDeleted)
					{
						dictionary[item2.Id] = value;
					}
				}
				using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_configurationDataLoader));
				configurationManager.Licenses.TryPreloadLicenses(dictionary.Select((KeyValuePair<Guid, byte[]> r) => r.Key));
				foreach (KeyValuePair<Guid, byte[]> item3 in dictionary)
				{
					Guid key = item3.Key;
					byte[] value2 = item3.Value;
					ConfigurationDataSet diffgramDatasetForRecorder = GetDiffgramDatasetForRecorder(recorderChanges, key);
					License license = null;
					try
					{
						license = configurationManager.Licenses[key];
					}
					catch (ConfigurationItemNotFoundException)
					{
					}
					if (license == null)
					{
						license = configurationManager.Licenses.Add(key, value2);
					}
					else
					{
						license.DataAsBytes = value2;
					}
					diffgramDatasetForRecorder.License.ImportRow(license.DataRow);
				}
				ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
				_configurationDataSetComponent.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
			}
		}
		finally
		{
			_configurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private void HandleServerLicenseChanged(ConfigurationDataSet allDataChanges, RecordersRequiringLicense recordersRequiringLicense)
	{
		if (allDataChanges == null)
		{
			return;
		}
		ConfigurationDataSet.LicenseRow licenseRow = allDataChanges.License.FindByIDLicense(License.WellKnownId.ServerLicense);
		if (licenseRow != null)
		{
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			requiredService.ClearAllCaches();
			lock (_server.LicenseManager)
			{
				try
				{
					_server.UpdateLicense(licenseRow.Data, LicenseUpdateOption.Update);
					ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseValid);
					Server.Instance.ResourceItemChanged("licenseInformations", licenseRow.IDLicense.ToString(), string.Empty, EventAction.Updated);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
					if (_server.LicenseManager.IsLicenseExpired)
					{
						ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseExpired);
					}
					else
					{
						ServerLogManager.Instance.AddLogEntry(LogEntryId.LicenseInvalid);
					}
				}
			}
			RecorderConfigData[] array = RecorderConfigData.QueryRecorders(_dataComponentFactory);
			RecorderConfigData[] array2 = array;
			foreach (RecorderConfigData recorderConfigData in array2)
			{
				if (recorderConfigData.Enabled)
				{
					RecorderRequiringLicense orAdd = recordersRequiringLicense.GetOrAdd(recorderConfigData.Id);
					orAdd.NewLicenseCreated = true;
					orAdd.CreateNewLicense = false;
					orAdd.RecorderDeleted = false;
				}
			}
		}
		ConfigurationDataSet.LicenseRow licenseRow2 = allDataChanges.License.FindByIDLicense(License.WellKnownId.LicenseOwner);
		if (licenseRow2 != null)
		{
			string ownerLicense = Encoding.UTF8.GetString(licenseRow2.Data);
			_server.UpdateLicenseOwner(ownerLicense);
		}
		ConfigurationDataSet.LicenseRow licenseRow3 = allDataChanges.License.FindByIDLicense(License.WellKnownId.SiteInformation);
		if (licenseRow3 != null)
		{
			string sitesInfo = Encoding.UTF8.GetString(licenseRow3.Data);
			_server.UpdateSitesInformation(sitesInfo);
		}
	}

	private static ConfigurationDataSet GetDiffgramDatasetForRecorder(Dictionary<Guid, ConfigurationDataSet> recordersAffected, Guid recorderId)
	{
		if (!recordersAffected.TryGetValue(recorderId, out var value))
		{
			value = new ConfigurationDataSet();
			recordersAffected.Add(recorderId, value);
		}
		return value;
	}

	private static ConfigurationDataSet GetDiffgramDatasetForRecorder(Dictionary<Guid, ConfigurationDataSet> recordersAffected, string recorderId)
	{
		return GetDiffgramDatasetForRecorder(recordersAffected, new Guid(recorderId));
	}

	private void SendUpdatesToRecorders(Dictionary<Guid, ConfigurationDataSet> recorderChanges, ICallContextInfo callContextInfo)
	{
		foreach (KeyValuePair<Guid, ConfigurationDataSet> recorderChange in recorderChanges)
		{
			Guid key = recorderChange.Key;
			if (!_serverConfigurationLookup.IsRecorderEnabled(key) && _serverConfigurationLookup.IsRecorderRegistered(key))
			{
				continue;
			}
			Guid[] array = _recorderCommManager.QueryRecorderServiceIds(key);
			foreach (Guid serviceId in array)
			{
				try
				{
					_recorderCommManager.UpdateSystemConfigurationAsync(serviceId, new SystemConfigurationDto(recorderChange.Value), callContextInfo);
				}
				catch (RecorderNotFoundException)
				{
				}
				catch (NotSupportedByLegacyRecorderException)
				{
					string text = _serverConfigurationLookup.FindRecorderDisplayName(key);
					ServerLogManager.Instance.AddLogEntry(LogEntryId.LegacyRecorderNoConfigurationUpdates, text);
				}
			}
		}
	}

	private void DeleteIdsFromLicenseTable(List<Guid> deleteLicenseIds, ICallContextInfo callContextInfo)
	{
		if (deleteLicenseIds.Count <= 0)
		{
			return;
		}
		_configurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(_configurationDataLoader));
			foreach (Guid deleteLicenseId in deleteLicenseIds)
			{
				try
				{
					configurationManager.Licenses.Remove(deleteLicenseId);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
			}
			ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
			_configurationDataSetComponent.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
		}
		finally
		{
			_configurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private static Guid FindRecorderIdFromServiceId(Guid serviceId)
	{
		return ConfigurationDataManager.FindRecorderIdFromServiceId(serviceId);
	}

	private static Guid FindRecorderIdFromRecordingStorage(Dictionary<Guid, Guid> recordingStorageRecorderIds, Guid recordingStorageId)
	{
		Guid value = Guid.Empty;
		if (!recordingStorageRecorderIds.TryGetValue(recordingStorageId, out value))
		{
			try
			{
				value = ConfigurationDataManager.FindRecorderIdByRecordingStorageId(recordingStorageId);
				recordingStorageRecorderIds.Add(recordingStorageId, value);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return value;
	}

	private static string FindRecorderIDFromHardware(IDictionary<Guid, Guid> hardwareRecorderIds, string hardwareId)
	{
		Guid guid = new Guid(hardwareId);
		Guid value = Guid.Empty;
		if (!hardwareRecorderIds.TryGetValue(guid, out value))
		{
			try
			{
				value = ConfigurationDataManager.FindRecorderIdByHardwareId(guid);
				hardwareRecorderIds.Add(guid, value);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		if (!(value != Guid.Empty))
		{
			return string.Empty;
		}
		return value.ToString();
	}

	private static string FindRecorderNameByRecorderId(Guid recorderID, Dictionary<Guid, string> recorderNames)
	{
		string value = string.Empty;
		if (!recorderNames.TryGetValue(recorderID, out value))
		{
			try
			{
				value = ConfigurationDataManager.FindRecorderNameByRecorderId(recorderID);
				recorderNames.Add(recorderID, value);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return value;
	}

	private static string FindRecorderIDFromDevice(IDictionary<Guid, Guid> hardwareRecorderIds, IDictionary<Guid, Guid> deviceHardwareIds, string deviceId)
	{
		string result = string.Empty;
		Guid guid = new Guid(deviceId);
		Guid value = Guid.Empty;
		if (deviceHardwareIds.TryGetValue(guid, out value))
		{
			result = FindRecorderIDFromHardware(hardwareRecorderIds, value.ToString());
		}
		else
		{
			try
			{
				Guid guid2 = ConfigurationDataManager.QueryRecorderIdByDeviceId(guid);
				if (guid2 != Guid.Empty)
				{
					result = guid2.ToString();
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return result;
	}

	private static string FindDeviceNameByDeviceId(Guid deviceId, Dictionary<Guid, string> deviceIdNames)
	{
		string value = string.Empty;
		if (!deviceIdNames.TryGetValue(deviceId, out value))
		{
			try
			{
				value = (deviceIdNames[deviceId] = ConfigurationDataManager.FindDeviceNameByDeviceId(deviceId));
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return value;
	}

	private static string FindDeviceTypeByDeviceId(Guid deviceId, Dictionary<Guid, string> deviceIdTypes)
	{
		string value = string.Empty;
		if (!deviceIdTypes.TryGetValue(deviceId, out value))
		{
			try
			{
				value = (deviceIdTypes[deviceId] = ConfigurationDataManager.FindDeviceTypeByDeviceId(deviceId));
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch
			{
			}
		}
		return value;
	}
}
