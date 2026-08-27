using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Management.Common;
using VideoOS.Server.RecorderCommunication;

namespace VideoOS.Server.CommandHandler;

public sealed class ReplaceHardwareCommandHandler : ICommandHandler
{
	private const string HWIDHardware = "hardware";

	private const int HeartbeatResponseTime = 5000;

	private IServerEnvironment _serverEnvironment;

	private IConfigurationDataSetComponent ConfigurationDataSetComponent => _serverEnvironment.ConfigurationDataSetComponent;

	private IConfigurationDataLoader ConfigurationDataLoader => _serverEnvironment.ConfigurationDataLoader;

	private IServerConfigurationLookup ServerConfigurationLookup => _serverEnvironment.ServerConfigurationLookup;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	private ILicenseManager LicenseManager => _serverEnvironment.LicenseManager;

	private IServer Server => _serverEnvironment.Server;

	private ILogAuditLogger AuditLogger => _serverEnvironment.AuditLogger;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	public ReplaceHardwareCommandHandler(IServerEnvironment serverEnvironment)
	{
		_serverEnvironment = serverEnvironment ?? throw new ArgumentNullException("serverEnvironment");
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		ReplaceHardwareCommand cmd = (command as ReplaceHardwareCommand) ?? throw new ArgumentException("Command/Command handler type mismatch", "command");
		ICallContextInfo callContextInfo = (state as ICallContextInfo) ?? throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		Timer timer = new Timer(SendHeartBeatResponse, responseSink, 5000, 5000);
		ResponseBase response;
		try
		{
			response = ProcessCommand(cmd, callContextInfo);
		}
		finally
		{
			ManualResetEvent manualResetEvent = new ManualResetEvent(initialState: false);
			timer.Dispose(manualResetEvent);
			manualResetEvent.WaitOne(TimeSpan.FromSeconds(10.0));
			manualResetEvent.Dispose();
		}
		responseSink.AddResponse(response);
	}

	private ResponseBase ProcessCommand(ReplaceHardwareCommand cmd, ICallContextInfo callContextInfo)
	{
		Guid recorderId = cmd.RecorderId;
		List<RHSettingValue> RHInheritedHardwareSettingValues = null;
		List<RHSettingValue> RHDefaultHardwareSettingValues = null;
		List<RHSettingValue> RHInheritedHardwarePtzSettingValues = null;
		List<RHSettingValue> RHDefaultHardwarePtzSettingValues = null;
		ConfigurationDataSet recorderChanges = null;
		List<string> RHInheritedDevices = new List<string>();
		List<string> RHAddedDevices = new List<string>();
		List<string> RHAddedDeviceIds = new List<string>();
		List<string> RHRemovedDevices = new List<string>();
		List<RHSettingsDevice> RHSettingsDevices = new List<RHSettingsDevice>();
		SessionLogger sessionLogger = new SessionLogger();
		LogEntryAuditLogger auditSessionLogger = new LogEntryAuditLogger(_serverEnvironment.AuditAndSystemLogger, sessionLogger, new ServerInfoCollector());
		UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector eventCollector = new UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector(Server.HomeSiteId, Server.EVSEventConsumer);
		try
		{
			ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
			try
			{
				using (ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(ConfigurationDataLoader)))
				{
					IPHardware hardware = configurationManager.FindHardwareByHardwareId(cmd.OldHardwareID);
					var (flag, result) = VerifyNoDeviceEvidenceLocks(hardware);
					if (!flag)
					{
						return result;
					}
					MakeConfigrationChanges(hardware, configurationManager);
				}
				ApplyLicenseChanges();
			}
			finally
			{
				ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
			}
			UpdateConfigurationManagerCommandHandler.LogPendingAuditLogs(sessionLogger, AuditLogger);
			eventCollector.FireConfigChangedEvent();
			Guid[] array = RecorderCommManager.QueryRecorderServiceIds(recorderId);
			foreach (Guid serviceId in array)
			{
				try
				{
					RecorderCommManager.ReplaceHardwareUpdateSystemConfigurationAsync(serviceId, new SystemConfigurationDto(recorderChanges), callContextInfo);
				}
				catch (RecorderNotFoundException)
				{
				}
				catch (NotSupportedByLegacyRecorderException)
				{
				}
			}
			return new ReplaceHardwareResponse(string.Empty, RHInheritedDevices.ToArray(), RHAddedDevices.ToArray(), RHAddedDeviceIds.ToArray(), RHRemovedDevices.ToArray(), RHInheritedHardwareSettingValues.ToArray(), RHDefaultHardwareSettingValues.ToArray(), RHInheritedHardwarePtzSettingValues.ToArray(), RHDefaultHardwarePtzSettingValues.ToArray(), RHSettingsDevices.ToArray());
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			string resourceString = ExceptionHandling.GetResourceString(ex4);
			if (string.IsNullOrEmpty(resourceString))
			{
				Logger.LogError("Replace hardware failed with an unexpected error. Exception:\r\n{Exception}", ex4);
				return new FailResponse();
			}
			return new ErrorResponse(0, resourceString);
		}
		void ApplyLicenseChanges()
		{
			LicenseManager.QueueAutomaticActivation();
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			requiredService.ClearAllCaches();
			byte[] recorderLicense;
			lock (LicenseManager)
			{
				recorderLicense = LicenseManager.GetRecorderLicense(recorderId);
			}
			using ConfigurationManager configurationManager2 = new ConfigurationManager(new ConfigurationDataSetLoader(ConfigurationDataLoader));
			ICallContextInfo callContextInfo2 = Server.CreateServerServiceCallContextUserInfo();
			License license = null;
			try
			{
				license = configurationManager2.Licenses[recorderId];
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager2.Licenses.Add(recorderId, recorderLicense);
			}
			else
			{
				license.DataAsBytes = recorderLicense;
				license.LastModified = DateTime.UtcNow;
			}
			ConfigurationDataSet changes = configurationManager2.Dataset.GetChanges();
			recorderChanges.Merge(changes);
			ConfigurationDataSetComponent.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo2);
		}
		void MakeConfigrationChanges(IPHardware iPHardware, ConfigurationManager configurationManager2)
		{
			MainDefinition oldMainDefinition = iPHardware.MainDefinition.Clone();
			List<string> list = new List<string> { cmd.NewSettingsXml };
			RHInheritedDevice[] inheritedDevices = cmd.InheritedDevices;
			foreach (RHInheritedDevice rHInheritedDevice in inheritedDevices)
			{
				list.Add(rHInheritedDevice.NewSettingsXml);
			}
			MainDefinition mainDefinition = MainDefinition.LoadXml(cmd.NewDefinitionXml, list);
			bool isTheSameHardwareDriver = iPHardware.ProductId == cmd.ProductId;
			Dictionary<string, RHInheritedDevice> dictionary = (from newDeviceDefinition in mainDefinition.HardwareDefinition.DeviceDefinitions
				join inheritedDevice in cmd.InheritedDevices on newDeviceDefinition.Hwid equals new DeviceHwid(inheritedDevice.NewDeviceHwidText)
				select inheritedDevice).ToDictionary((RHInheritedDevice rHInheritedDevice3) => rHInheritedDevice3.NewDeviceHwidText, (RHInheritedDevice result2) => result2);
			iPHardware.SetHardwareDefinition(mainDefinition.SaveAsXml());
			UpdateConfigSettings ucs = new UpdateConfigSettings(Logger);
			List<Device> addedDevices = new List<Device>();
			TransferHardwareSettings(cmd, iPHardware, isTheSameHardwareDriver, mainDefinition, oldMainDefinition, ucs, out RHInheritedHardwareSettingValues, out RHDefaultHardwareSettingValues, out RHInheritedHardwarePtzSettingValues, out RHDefaultHardwarePtzSettingValues, out var RHInheritedEvents, out var RHDeletedEvents);
			HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode = (LicenseManager.IsFeatureLicensed("HardwareAcceleratedVMD") ? HardwareAccelerationMode.HardwareAccelerationModeType.Automatic : HardwareAccelerationMode.HardwareAccelerationModeType.Off);
			foreach (DeviceDefinition deviceDefinition in mainDefinition.HardwareDefinition.DeviceDefinitions)
			{
				RHInheritedDevice rHInheritedDevice2 = dictionary[deviceDefinition.Hwid.Text];
				if (iPHardware.Devices.TryGet(rHInheritedDevice2.OldDeviceID, out var device))
				{
					TransferDevice(device, deviceDefinition, rHInheritedDevice2);
				}
				else
				{
					AddNewDevice(deviceDefinition, rHInheritedDevice2, hardwareAccelerationMode);
				}
			}
			RemoveDevices();
			iPHardware.ConfigureDeviceLinks(addedDevices);
			RemoveEvents();
			auditSessionLogger.AuditHardwareReplaced(iPHardware.ID, cmd.NewAuditHardwareName, iPHardware.Name, cmd.RecorderName, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
			eventCollector.AddEvent(iPHardware.ID.ToString(), iPHardware.Name, ChangeAction.Updated, "Hardware");
			configurationManager2.Dataset.UpdateLastModified();
			recorderChanges = configurationManager2.Dataset.GetChanges();
			ConfigurationDataSetComponent.WriteConfigurationDataSetChangesForReplaceHardware((ConfigurationDataSet)recorderChanges.Copy(), callContextInfo);
			void AddNewDevice(DeviceDefinition newDeviceDefinition, RHInheritedDevice inheritedDevice, HardwareAccelerationMode.HardwareAccelerationModeType hardwareAccelerationMode2)
			{
				Guid id = Guid.NewGuid();
				Guid recordingStorageId = Guid.Empty;
				string name = inheritedDevice?.NewDeviceName ?? "New Device";
				string empty = string.Empty;
				string empty2 = string.Empty;
				string empty3 = string.Empty;
				int index = newDeviceDefinition.Hwid.Index;
				string deviceTypeString = RegisteredDeviceType.GetDeviceTypeString(newDeviceDefinition.Hwid.Type);
				RegisteredDeviceType registeredDeviceType = configurationManager2.RegisteredDeviceTypes[deviceTypeString];
				string text = inheritedDevice?.NewSettingsXml ?? string.Empty;
				string presets = inheritedDevice?.NewPresetsXml ?? PtzPresetCollection.EmptyXml;
				string defaultStreamXml4NewDevice = Device.GetDefaultStreamXml4NewDevice(newDeviceDefinition, text);
				if (registeredDeviceType != null && registeredDeviceType.IsRecordingDevice)
				{
					try
					{
						recordingStorageId = ServerConfigurationLookup.QuerydDefaultStorageIdByRecorder(cmd.RecorderId);
					}
					catch
					{
						throw;
					}
				}
				Device device2 = iPHardware.Devices.AddNew(id, name, empty, empty2, deviceTypeString, text, presets, empty3, multicastEnabled: false, defaultStreamXml4NewDevice, index, recordingStorageId, failoverLiveOnly: false, disableOnFailover: false, -1, -1, -1, hardwareAccelerationMode2);
				addedDevices.Add(device2);
				RHAddedDevices.Add(device2.Name);
				RHAddedDeviceIds.Add(device2.ID.ToString());
				auditSessionLogger.AuditDeviceAdded(device2.ID, device2.Name, device2.DeviceType, cmd.RecorderName, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
				eventCollector.AddEvent(device2.ID.ToString(), device2.Name, ChangeAction.Added, UpdateConfigurationManagerCommandHandler.ConvertDeviceTypeToItemType(device2.DeviceType));
			}
			void RemoveDevices()
			{
				Guid[] removedDevices = cmd.RemovedDevices;
				for (int k = 0; k < removedDevices.Length; k++)
				{
					Guid guid = removedDevices[k];
					if (iPHardware.Devices.TryGet(guid, out var device2))
					{
						string name = device2.Name;
						string deviceType = device2.DeviceType;
						iPHardware.Devices.Remove(device2);
						RHRemovedDevices.Add(name);
						auditSessionLogger.AuditDeviceRemoved(guid, name, deviceType, cmd.RecorderName, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
						eventCollector.AddEvent(guid.ToString(), name, ChangeAction.Deleted, UpdateConfigurationManagerCommandHandler.ConvertDeviceTypeToItemType(deviceType));
					}
				}
			}
			void RemoveEvents()
			{
				Guid[] removedEvents = cmd.RemovedEvents;
				foreach (Guid id in removedEvents)
				{
					configurationManager2.EventTypes.Remove(id);
				}
			}
			void TransferDevice(Device device2, DeviceDefinition newDeviceDefinition, RHInheritedDevice inheritedDevice)
			{
				List<RHSettingValue> list2 = new List<RHSettingValue>();
				List<RHSettingValue> list3 = new List<RHSettingValue>();
				List<RHSettingValue> list4 = new List<RHSettingValue>();
				List<RHSettingValue> list5 = new List<RHSettingValue>();
				List<RHSettingValue> list6 = new List<RHSettingValue>();
				List<RHSettingValue> list7 = new List<RHSettingValue>();
				List<string> list8 = new List<string>();
				List<string> list9 = new List<string>();
				DeviceHwid oldDeviceHwid = device2.Hwid;
				device2.Index = newDeviceDefinition.Hwid.Index;
				TransferDevicePatrollingProfiles(device2, configurationManager2, isTheSameHardwareDriver, list8, list9, inheritedDevice.TransferDeviceInternalPresets);
				DriverSettings currentDeviceSettings = device2.Settings.Clone();
				TransferDeviceSettings(device2, newDeviceDefinition, inheritedDevice?.NewSettingsXml ?? string.Empty, currentDeviceSettings, ucs, isTheSameHardwareDriver, list2, list3, configurationManager2);
				if (newDeviceDefinition.PtzSettings != null)
				{
					TransferDevicePtzSettings(device2, newDeviceDefinition, currentDeviceSettings, ucs, isTheSameHardwareDriver, list2, list3);
				}
				TransferDeviceStreamSettings(device2, newDeviceDefinition, currentDeviceSettings, ucs, isTheSameHardwareDriver, list4, list5);
				TransferDeviceMetadataItemSettings(device2, newDeviceDefinition, currentDeviceSettings, ucs, isTheSameHardwareDriver, list6, list7);
				RHSettingsDevices.Add(new RHSettingsDevice(device2.ID, device2.Name, list2.ToArray(), list3.ToArray(), list4.ToArray(), list5.ToArray(), list6.ToArray(), list7.ToArray(), list8.ToArray(), list9.ToArray(), RHInheritedEvents.Where((RHEventValue e) => StringComparer.Ordinal.Equals(e.DeviceHWId, oldDeviceHwid.Text)).ToArray(), RHDeletedEvents.Where((RHEventValue e) => StringComparer.Ordinal.Equals(e.DeviceHWId, oldDeviceHwid.Text)).ToArray()));
				RHInheritedDevices.Add(device2.Name);
				auditSessionLogger.AuditDeviceReplaced(device2.ID, device2.Name, inheritedDevice?.NewAuditDeviceName ?? device2.Name, device2.DeviceType, cmd.RecorderName, callContextInfo.UserDomainAccountName, callContextInfo.LocationIPAddress);
				eventCollector.AddEvent(device2.ID.ToString(), device2.Name, ChangeAction.Updated, UpdateConfigurationManagerCommandHandler.ConvertDeviceTypeToItemType(device2.DeviceType));
			}
		}
		(bool success, ResponseBase Response) VerifyNoDeviceEvidenceLocks(IPHardware iPHardware)
		{
			List<Guid> list = new List<Guid>();
			Guid[] removedDevices = cmd.RemovedDevices;
			for (int j = 0; j < removedDevices.Length; j++)
			{
				Guid item = removedDevices[j];
				if (ServerConfigurationLookup.DeviceHasEvidenceLock(item.ToString()))
				{
					list.Add(item);
				}
			}
			if (list.Count > 0)
			{
				List<LockedDevices> list2 = new List<LockedDevices>();
				foreach (Device device3 in iPHardware.Devices)
				{
					if (list.Contains(device3.ID))
					{
						list2.Add(new LockedDevices(device3.ID.ToString(), device3.Name, iPHardware.ID.ToString()));
					}
				}
				return (success: false, Response: EvidenceLockFailedRemoveResponse.CreateRemoveDevicesFailedResponse(iPHardware.Name, list2));
			}
			return (success: true, Response: null);
		}
	}

	private static void TransferDeviceMetadataItemSettings(Device device, DeviceDefinition newDeviceDefinition, DriverSettings currentDeviceSettings, UpdateConfigSettings ucs, bool isTheSameHardwareDriver, List<RHSettingValue> RHInheritedDeviceMetadataItemSettingValues, List<RHSettingValue> RHDefaultDeviceMetadataItemSettingValues)
	{
		foreach (MetadataItemDefinition metadataItem in newDeviceDefinition.MetadataItems)
		{
			ucs.UpdateSettings(currentDeviceSettings, device.Settings, metadataItem.Capabilities, isTheSameHardwareDriver, out var inheritedSettings, out var defaultSettings);
			foreach (UpdateConfigSettings.ConfigSettingValue item in inheritedSettings)
			{
				if (item.Visible)
				{
					RHInheritedDeviceMetadataItemSettingValues.Add(new RHSettingValue(item.SectionName, item.SettingName, item.SettingValue));
				}
			}
			foreach (UpdateConfigSettings.ConfigSettingValue item2 in defaultSettings)
			{
				if (item2.Visible)
				{
					RHDefaultDeviceMetadataItemSettingValues.Add(new RHSettingValue(item2.SectionName, item2.SettingName, item2.SettingValue));
				}
			}
		}
	}

	private static void TransferDeviceStreamSettings(Device device, DeviceDefinition newDeviceDefinition, DriverSettings currentDeviceSettings, UpdateConfigSettings ucs, bool isTheSameHardwareDriver, List<RHSettingValue> RHInheritedDeviceStreamSettingValues, List<RHSettingValue> RHDefaultDeviceStreamSettingValues)
	{
		TransferVisibleStreamSettings();
		if (!newDeviceDefinition.StreamCombinations.Supported)
		{
			device.Streams = string.Empty;
			SetDefaultTrackSettings();
			return;
		}
		StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
		if (!string.IsNullOrEmpty(device.Streams))
		{
			StreamsConfiguration streamsConfiguration2 = StreamsConfiguration.FromXml(device.Streams);
			foreach (StreamInfo item in streamsConfiguration2)
			{
				StreamDefinition streamDefinition = newDeviceDefinition.Streams.FindByReferenceId(item.StreamReferenceId);
				if (streamDefinition != null)
				{
					streamsConfiguration.Add(new StreamInfo(item)
					{
						Hwid = streamDefinition.Hwid
					});
				}
			}
		}
		bool newPrimaryRecordingStreamWasSelected = !streamsConfiguration.Any((StreamInfo s) => s.TrackDefinition == WellKnownDeviceTrackDefinition.Primary);
		if (streamsConfiguration.Count == 0)
		{
			streamsConfiguration.LoadXml(Device.GetDefaultStreamXml4NewDevice(newDeviceDefinition, device.SettingsAsXml));
		}
		else
		{
			streamsConfiguration.EnsureLiveAndRecordSelections();
		}
		device.Streams = streamsConfiguration.SaveXml();
		EnsureValidDeviceTracks(streamsConfiguration, newPrimaryRecordingStreamWasSelected);
		void EnsureValidDeviceTracks(StreamsConfiguration newStreamsConfiguration, bool flag2)
		{
			foreach (DeviceTrack item2 in device.Tracks.Where((DeviceTrack t) => t.IDTrackDefinition != WellKnownDeviceTrackDefinition.Edge && !newStreamsConfiguration.Any((StreamInfo s) => s.TrackDefinition == t.IDTrackDefinition)))
			{
				if (item2.DefaultPlayback)
				{
					item2.DefaultPlayback = false;
				}
				if (item2.MergeEdge)
				{
					item2.MergeEdge = false;
				}
				if (item2.InUse)
				{
					item2.InUse = false;
				}
			}
			bool flag = device.Tracks.Any((DeviceTrack t) => t.DefaultPlayback);
			if (flag2 || !flag)
			{
				DeviceTrack deviceTrack = device.Tracks.First((DeviceTrack t) => t.IsPrimary);
				if (!flag)
				{
					deviceTrack.DefaultPlayback = true;
				}
				if (flag2 && !deviceTrack.MergeEdge)
				{
					deviceTrack.MergeEdge = true;
				}
			}
		}
		void SetDefaultTrackSettings()
		{
			foreach (DeviceTrack track in device.Tracks)
			{
				bool flag = track.IDTrackDefinition == WellKnownDeviceTrackDefinition.Primary;
				bool flag2 = track.IDTrackDefinition == WellKnownDeviceTrackDefinition.Primary;
				bool flag3 = track.IDTrackDefinition == WellKnownDeviceTrackDefinition.Primary || track.IDTrackDefinition == WellKnownDeviceTrackDefinition.Edge;
				if (track.DefaultPlayback != flag)
				{
					track.DefaultPlayback = flag;
				}
				if (track.MergeEdge != flag2)
				{
					track.MergeEdge = flag2;
				}
				if (track.InUse != flag3)
				{
					track.InUse = flag3;
				}
			}
		}
		void TransferVisibleStreamSettings()
		{
			foreach (StreamDefinition stream in newDeviceDefinition.Streams)
			{
				ucs.UpdateSettings(currentDeviceSettings, device.Settings, stream.Capabilities, isTheSameHardwareDriver, out var inheritedSettings, out var defaultSettings);
				foreach (UpdateConfigSettings.ConfigSettingValue item3 in inheritedSettings.Where((UpdateConfigSettings.ConfigSettingValue s) => s.Visible))
				{
					RHInheritedDeviceStreamSettingValues.Add(new RHSettingValue(item3.SectionName, item3.SettingName, item3.SettingValue));
				}
				foreach (UpdateConfigSettings.ConfigSettingValue item4 in defaultSettings.Where((UpdateConfigSettings.ConfigSettingValue s) => s.Visible))
				{
					RHDefaultDeviceStreamSettingValues.Add(new RHSettingValue(item4.SectionName, item4.SettingName, item4.SettingValue));
				}
			}
		}
	}

	private static void TransferDevicePtzSettings(Device device, DeviceDefinition newDeviceDefinition, DriverSettings currentDeviceSettings, UpdateConfigSettings ucs, bool isTheSameHardwareDriver, List<RHSettingValue> RHInheritedDeviceSettingValues, List<RHSettingValue> RHDefaultDeviceSettingValues)
	{
		bool useCurrentReadOnlyValues = newDeviceDefinition.MainDefinition.HardwareDefinition.IsPtzEnabledVideoServer();
		ucs.UpdateSettings(currentDeviceSettings, device.Settings, newDeviceDefinition.PtzSettings.Capabilities, isTheSameHardwareDriver, useCurrentReadOnlyValues, out var inheritedSettings, out var defaultSettings);
		if (!newDeviceDefinition.MainDefinition.HardwareDefinition.IsPtzEnabledVideoServer())
		{
			return;
		}
		foreach (UpdateConfigSettings.ConfigSettingValue item in inheritedSettings)
		{
			if (IsVisiblePtzServerSetting(item))
			{
				RHInheritedDeviceSettingValues.Add(new RHSettingValue(item.SectionName, item.SettingName, item.SettingValue));
			}
		}
		foreach (UpdateConfigSettings.ConfigSettingValue item2 in defaultSettings)
		{
			if (IsVisiblePtzServerSetting(item2))
			{
				RHDefaultDeviceSettingValues.Add(new RHSettingValue(item2.SectionName, item2.SettingName, item2.SettingValue));
			}
		}
	}

	private static bool IsVisiblePtzServerSetting(UpdateConfigSettings.ConfigSettingValue sc)
	{
		if (!(sc.TypeReferenceId == TypeReferenceIds.PtzComPort) && !(sc.TypeReferenceId == TypeReferenceIds.PtzDeviceId) && !(sc.TypeReferenceId == TypeReferenceIds.PtzEnabled))
		{
			return sc.TypeReferenceId == TypeReferenceIds.PtzProtocol;
		}
		return true;
	}

	private static void TransferDeviceSettings(Device device, DeviceDefinition newDeviceDefinition, string settingsXml, DriverSettings currentDeviceSettings, UpdateConfigSettings ucs, bool isTheSameHardwareDriver, List<RHSettingValue> RHInheritedDeviceSettingValues, List<RHSettingValue> RHDefaultDeviceSettingValues, ConfigurationManager configurationManager)
	{
		bool flag = device.IsRecordingDevice && device.SettingUtilities.IsEdgeStorageSupported;
		device.Settings.Clear();
		device.Settings.LoadXml(settingsXml, SettingsLoadMethod.Reload);
		ucs.UpdateSettings(currentDeviceSettings, device.Settings, newDeviceDefinition.Capabilities, isTheSameHardwareDriver, out var inheritedSettings, out var defaultSettings);
		foreach (UpdateConfigSettings.ConfigSettingValue item in inheritedSettings)
		{
			if (item.Visible)
			{
				RHInheritedDeviceSettingValues.Add(new RHSettingValue(item.SectionName, item.SettingName, item.SettingValue));
			}
		}
		foreach (UpdateConfigSettings.ConfigSettingValue item2 in defaultSettings)
		{
			if (item2.Visible)
			{
				RHDefaultDeviceSettingValues.Add(new RHSettingValue(item2.SectionName, item2.SettingName, item2.SettingValue));
			}
		}
		if (flag && !device.SettingUtilities.IsEdgeStorageSupported)
		{
			if (configurationManager.Dataset.RecordingStorageDevice.Rows.Count == 0)
			{
				configurationManager.EnsureRecordingStorageDeviceLoadedByRecorderId(device.IPHardware.Recorder.ID);
			}
			ConfigurationDataSet.RecordingStorageDeviceRow recordingStorageDeviceRow = configurationManager.Dataset.RecordingStorageDevice.FindByIDDevice(device.ID);
			if (recordingStorageDeviceRow != null && recordingStorageDeviceRow.edgestorage_enabled)
			{
				recordingStorageDeviceRow.edgestorage_enabled = false;
			}
		}
	}

	private static void TransferDevicePatrollingProfiles(Device device, ConfigurationManager configurationManager, bool isTheSameHardwareDriver, List<string> RHInheritedPatrollingProfiles, List<string> RHDeletedPatrollingProfiles, bool transferDevicePresets)
	{
		bool flag = true;
		if (isTheSameHardwareDriver)
		{
			foreach (PtzPreset preset in device.Presets)
			{
				if (!transferDevicePresets && preset.IsDevicePreset)
				{
					flag = false;
					break;
				}
			}
		}
		else
		{
			flag = false;
		}
		foreach (PatrollingProfile value in configurationManager.PatrollingProfiles.Values)
		{
			if (value.IDDevice == device.ID)
			{
				if (flag)
				{
					RHInheritedPatrollingProfiles.Add(value.Name);
				}
				else
				{
					RHDeletedPatrollingProfiles.Add(value.Name);
				}
			}
		}
		if (!flag)
		{
			configurationManager.PatrollingProfiles.RemoveProfilesForDevice(device.ID);
			device.Presets.Clear();
		}
	}

	private static void TransferHardwareSettings(ReplaceHardwareCommand cmd, IPHardware hardware, bool isTheSameHardwareDriver, MainDefinition newMainDefinition, MainDefinition oldMainDefinition, UpdateConfigSettings ucs, out List<RHSettingValue> RHInheritedHardwareSettingValues, out List<RHSettingValue> RHDefaultHardwareSettingValues, out List<RHSettingValue> RHInheritedHardwarePtzSettingValues, out List<RHSettingValue> RHDefaultHardwarePtzSettingValues, out List<RHEventValue> RHInheritedEvents, out List<RHEventValue> RHDeletedEvents)
	{
		hardware.Uri = cmd.Uri;
		hardware.LoginID = cmd.UserName;
		hardware.Password = cmd.Password;
		hardware.DriverNumber = cmd.DriverNumber;
		hardware.DriverProxyName = cmd.DriverNumber.ToString(CultureInfo.InvariantCulture);
		hardware.ProductId = cmd.ProductId;
		hardware.IDHardwareDriver = cmd.NewHardwareDriverID;
		hardware.HardwareDataLastUpdated = DateTime.UtcNow;
		RHInheritedHardwareSettingValues = new List<RHSettingValue>();
		RHDefaultHardwareSettingValues = new List<RHSettingValue>();
		RHInheritedHardwarePtzSettingValues = new List<RHSettingValue>();
		RHDefaultHardwarePtzSettingValues = new List<RHSettingValue>();
		RHInheritedEvents = new List<RHEventValue>();
		RHDeletedEvents = new List<RHEventValue>();
		DriverSettings previousSettings = hardware.Settings.Clone();
		hardware.Settings.Clear();
		hardware.Settings.LoadXml(cmd.NewSettingsXml, SettingsLoadMethod.Reload);
		ucs.UpdateSettings(previousSettings, hardware.Settings, newMainDefinition.HardwareDefinition.Capabilities, transferSettingIfPossible: true, out var inheritedSettings, out var defaultSettings);
		foreach (UpdateConfigSettings.ConfigSettingValue item2 in inheritedSettings)
		{
			if (item2.Visible)
			{
				RHInheritedHardwareSettingValues.Add(new RHSettingValue(item2.SectionName, item2.SettingName, item2.SettingValue));
			}
		}
		foreach (UpdateConfigSettings.ConfigSettingValue item3 in defaultSettings)
		{
			if (item3.Visible)
			{
				RHDefaultHardwareSettingValues.Add(new RHSettingValue(item3.SectionName, item3.SettingName, item3.SettingValue));
			}
		}
		if (newMainDefinition.HardwareDefinition.PtzSettings != null)
		{
			ucs.UpdateSettings(previousSettings, hardware.Settings, newMainDefinition.HardwareDefinition.PtzSettings.Capabilities, transferSettingIfPossible: true, out inheritedSettings, out defaultSettings);
			foreach (UpdateConfigSettings.ConfigSettingValue item4 in inheritedSettings)
			{
				if (item4.Visible)
				{
					RHInheritedHardwarePtzSettingValues.Add(new RHSettingValue(item4.SectionName, item4.SettingName, item4.SettingValue));
				}
			}
			foreach (UpdateConfigSettings.ConfigSettingValue item5 in defaultSettings)
			{
				if (item5.Visible)
				{
					RHDefaultHardwarePtzSettingValues.Add(new RHSettingValue(item5.SectionName, item5.SettingName, item5.SettingValue));
				}
			}
		}
		DriverSettings driverSettings = hardware.EventSettings.Clone();
		hardware.EventSettings.Clear();
		List<string> inheritedEvents;
		List<string> deletedEvents;
		if (isTheSameHardwareDriver)
		{
			ucs.UpdateEventSettings(driverSettings, hardware.EventSettings, newMainDefinition.Events, out inheritedEvents, out deletedEvents);
		}
		else
		{
			inheritedEvents = new List<string>();
			deletedEvents = new List<string>();
			string[] sectionNames = driverSettings.GetSectionNames();
			foreach (string item in sectionNames)
			{
				deletedEvents.Add(item);
			}
		}
		UpdateRHEventValueList(RHInheritedEvents, inheritedEvents, hardware, oldMainDefinition, driverSettings);
		UpdateRHEventValueList(RHDeletedEvents, deletedEvents, hardware, oldMainDefinition, driverSettings);
	}

	private static void UpdateRHEventValueList(List<RHEventValue> eventValueList, List<string> eventHwidList, IPHardware hardware, MainDefinition oldMainDefinition, DriverSettings previousEventSettings)
	{
		foreach (string eventHwid in eventHwidList)
		{
			if (!previousEventSettings.HasSection(eventHwid))
			{
				continue;
			}
			string valueByTypeId = previousEventSettings.GetValueByTypeId(eventHwid, TypeReferenceIds.EventSource.ToString());
			Device device;
			if (valueByTypeId == "hardware")
			{
				VideoOS.Common.Integration.Definition.EventDefinition eventDefinition = oldMainDefinition.Events.Find(new EventHwid(eventHwid));
				if (eventDefinition != null)
				{
					eventValueList.Add(new RHEventValue(valueByTypeId, hardware.Name, eventHwid, eventDefinition.DisplayNameReferenceId));
				}
			}
			else if (hardware.Devices.TryGet(new DeviceHwid(valueByTypeId), out device))
			{
				VideoOS.Common.Integration.Definition.EventDefinition eventDefinition2 = oldMainDefinition.Events.Find(new EventHwid(eventHwid));
				if (eventDefinition2 != null)
				{
					eventValueList.Add(new RHEventValue(valueByTypeId, device.Name, eventHwid, eventDefinition2.DisplayNameReferenceId));
				}
			}
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	private void SendHeartBeatResponse(object state)
	{
		IResponseSink responseSink = (IResponseSink)state;
		responseSink.AddResponse(new HeartBeatResponse());
	}
}
