using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Drivers;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Common.SharedComponent.Options;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Threading;
using VideoOS.Vms.Utilities.SystemSettings;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server.GetConfiguration;

public static class ClientConfigurationProvider
{
	public static ConfigurationInfo GetConfiguration(IGeminiRequestContext requestContext, bool isPublicRequest)
	{
		bool includeStreams = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "LiveMultiStreaming");
		bool includeSmartWallLicense = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartWall");
		bool live;
		bool browse;
		bool setup;
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientLogin"))
		{
			live = false;
			browse = false;
			setup = false;
		}
		else
		{
			GetClientProfileSettings(requestContext, out live, out browse, out setup);
		}
		bool isAdministrator = requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext);
		Guid[] memberOfRoles = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles;
		string[] roleIds = ((memberOfRoles == null) ? new string[0] : memberOfRoles.Select((Guid f) => f.ToString()).ToArray());
		bool isEdgeStorageLicensed = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EdgeStorage");
		ConfigurationInfo proxy = new ConfigurationInfo();
		string systemSetting = ServerInformation.GetSystemSetting(requestContext, SettingList.LocalIPRanges);
		if (isPublicRequest && LocalIPRangeXmlFormatter.IsInRange(requestContext.RemoteIPAddress, systemSetting))
		{
			isPublicRequest = false;
		}
		proxy.BookmarkSettings = new BookmarkSettingInfo();
		proxy.BookmarkSettings.DefaultPreTimeSec = Convert.ToInt32(ServerInformation.GetSystemSetting(requestContext, SettingList.BookmarkPreTime), CultureInfo.InvariantCulture);
		proxy.BookmarkSettings.DefaultPostTimeSec = Convert.ToInt32(ServerInformation.GetSystemSetting(requestContext, SettingList.BookmarkPostTime), CultureInfo.InvariantCulture);
		proxy.ServerOptions = GetServerOptions(requestContext);
		string systemSetting2 = ServerInformation.GetSystemSetting(requestContext, SettingList.RetentionOptions);
		ReadRetentionOptions(requestContext, proxy, systemSetting2);
		proxy.BookmarkSettings = new BookmarkSettingInfo();
		proxy.BookmarkSettings.DefaultPreTimeSec = Convert.ToInt32(ServerInformation.GetSystemSetting(requestContext, SettingList.BookmarkPreTime), CultureInfo.InvariantCulture);
		proxy.BookmarkSettings.DefaultPostTimeSec = Convert.ToInt32(ServerInformation.GetSystemSetting(requestContext, SettingList.BookmarkPostTime), CultureInfo.InvariantCulture);
		ThreadPoolTask threadPoolTask = ThreadPoolTask.Create(delegate
		{
			ReadMgtServerInfo(requestContext, proxy);
			ReadApplicationSecurity(requestContext, proxy, live, browse, setup);
			ReadExternalPlugins(requestContext, proxy, includeSmartWallLicense);
			ReadAlertTypes(proxy);
			ReadMatrixMonitors(requestContext, proxy);
			ReadAudioMessages(requestContext, proxy);
		});
		ThreadPoolTask threadPoolTask2 = ThreadPoolTask.Create(delegate
		{
			IEnumerable<EventTypeData> events = EventTypeData.QueryEventTypes(requestContext);
			ReadEventTypes(requestContext, proxy, events);
			ReadSystemEventTypes(proxy, events);
		});
		HashSet<Guid> visibilityCache = new HashSet<Guid>();
		ReadRecorders(requestContext, proxy, null, isPublicRequest, visibilityCache, includeStreams, isAdministrator, roleIds, isEdgeStorageLicensed);
		ReadDeviceMappings(requestContext, proxy, visibilityCache);
		ReadDeviceGroups(requestContext, proxy, visibilityCache);
		try
		{
			threadPoolTask.Wait();
			threadPoolTask2.Wait();
		}
		catch (ThreadPoolTaskExecuteException ex)
		{
			throw ex.InnerException;
		}
		return proxy;
	}

	private static void ReadDeviceGroups(IGeminiRequestContext requestContext, ConfigurationInfo proxy, HashSet<Guid> visibilityCache)
	{
		IEnumerable<DeviceGroupData> enumerable = DeviceGroupData.QueryDeviceGroupsAll(requestContext);
		Dictionary<Guid, List<DeviceGroupData>> dictionary = new Dictionary<Guid, List<DeviceGroupData>>();
		Dictionary<Guid, List<DeviceGroupData>> dictionary2 = new Dictionary<Guid, List<DeviceGroupData>>();
		Dictionary<Guid, List<DeviceGroupData>> dictionary3 = new Dictionary<Guid, List<DeviceGroupData>>();
		Dictionary<Guid, List<DeviceGroupData>> dictionary4 = new Dictionary<Guid, List<DeviceGroupData>>();
		Dictionary<Guid, List<DeviceGroupData>> dictionary5 = new Dictionary<Guid, List<DeviceGroupData>>();
		Dictionary<Guid, List<DeviceGroupData>> dictionary6 = new Dictionary<Guid, List<DeviceGroupData>>();
		foreach (DeviceGroupData item in enumerable)
		{
			switch (item.DeviceType)
			{
			case "Camera":
				AddDeviceGroupInternal(item, dictionary);
				break;
			case "Metadata":
				AddDeviceGroupInternal(item, dictionary4);
				break;
			case "Output":
				AddDeviceGroupInternal(item, dictionary6);
				break;
			case "Input":
				AddDeviceGroupInternal(item, dictionary5);
				break;
			case "AudioOutput":
				AddDeviceGroupInternal(item, dictionary3);
				break;
			case "AudioInput":
				AddDeviceGroupInternal(item, dictionary2);
				break;
			}
		}
		Dictionary<Guid, IEnumerable<Guid>> members = DeviceGroupData.QueryDeviceGroupMembers(requestContext);
		ReadCameraGroups(proxy, visibilityCache, dictionary, members);
		ReadMicrophoneGroups(proxy, visibilityCache, dictionary2, members);
		ReadSpeakerGroups(proxy, visibilityCache, dictionary3, members);
		ReadMetadataDeviceGroups(proxy, visibilityCache, dictionary4, members);
		ReadInputGroups(proxy, visibilityCache, dictionary5, members);
		ReadOutputGroups(proxy, visibilityCache, dictionary6, members);
	}

	private static ServerOption[] GetServerOptions(IGeminiRequestContext requestContext)
	{
		ServerOption serverOption = new ServerOption();
		serverOption.Key = "Bookmark";
		serverOption.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "Bookmarking"))
		{
			serverOption.Value = "False";
		}
		ServerOption serverOption2 = new ServerOption();
		serverOption2.Key = "EvidenceLock";
		serverOption2.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EvidenceLock"))
		{
			serverOption2.Value = "False";
		}
		ServerOption serverOption3 = new ServerOption();
		serverOption3.Key = "PrivacyMask";
		serverOption3.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "ExtendedPrivacyMasking"))
		{
			serverOption3.Value = "False";
		}
		ServerOption serverOption4 = new ServerOption();
		serverOption4.Key = "PrivacyMaskUsed";
		serverOption4.Value = ((GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "ExtendedPrivacyMasking") && ConfigurationDataManager.FindPrivacyMaskUsed(requestContext)) ? "True" : "False");
		ServerOption serverOption5 = new ServerOption();
		serverOption5.Key = "ClientProfile";
		serverOption5.Value = "True";
		ServerOption serverOption6 = new ServerOption();
		serverOption6.Key = "SystemMonitor";
		serverOption6.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SystemMonitor"))
		{
			serverOption6.Value = "False";
		}
		ServerOption serverOption7 = new ServerOption();
		serverOption7.Key = "MetadataDevices";
		serverOption7.Value = "True";
		bool flag = Convert.ToBoolean(ServerInformation.GetSystemSetting(requestContext, SettingList.LogAuditEnableUserAccessLogging), CultureInfo.InvariantCulture);
		ServerOption serverOption8 = new ServerOption();
		serverOption8.Key = "LogAuditUserAccessLogging";
		serverOption8.Value = (flag ? "True" : "False");
		ServerOption serverOption9 = new ServerOption();
		serverOption9.Key = "LogAuditPlaybackWindowSizeInSec";
		serverOption9.Value = ServerInformation.GetSystemSetting(requestContext, SettingList.LogAuditPlaybackWindowSizeInSec);
		ServerOption serverOption10 = new ServerOption();
		serverOption10.Key = "LogAuditPlaybackFramesBeforeLogging";
		serverOption10.Value = ServerInformation.GetSystemSetting(requestContext, SettingList.LogAuditPlaybackFramesBeforeLogging);
		ServerOption serverOption11 = new ServerOption();
		serverOption11.Key = "ManualRecording";
		serverOption11.Value = "True";
		ServerOption serverOption12 = new ServerOption();
		serverOption12.Key = "ExtendedPtz";
		serverOption12.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "ExtendedPtz"))
		{
			serverOption12.Value = "False";
		}
		ServerOption serverOption13 = new ServerOption();
		serverOption13.Key = "ExtendedPtzInfo";
		serverOption13.Value = "True";
		ServerOption serverOption14 = new ServerOption();
		serverOption14.Key = "LicenseInfo";
		serverOption14.Value = "True";
		ServerOption serverOption15 = new ServerOption();
		serverOption15.Key = "SmartMap";
		serverOption15.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartMap"))
		{
			serverOption15.Value = "False";
		}
		ServerOption serverOption16 = new ServerOption();
		serverOption16.Key = "SmartMapOtherProviders";
		serverOption16.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartMapOtherProviders"))
		{
			serverOption16.Value = "False";
		}
		ServerOption serverOption17 = new ServerOption();
		serverOption17.Key = "AdaptiveStreaming";
		serverOption17.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "AdaptiveStreaming"))
		{
			serverOption17.Value = "False";
		}
		ServerOption serverOption18 = new ServerOption();
		serverOption18.Key = "MultistreamRecording";
		serverOption18.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "MultistreamRecording"))
		{
			serverOption18.Value = "False";
		}
		ServerOption serverOption19 = new ServerOption();
		serverOption19.Key = "SaveSearches";
		serverOption19.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SaveSearches"))
		{
			serverOption19.Value = "False";
		}
		ServerOption serverOption20 = new ServerOption();
		serverOption20.Key = "SearchCategories";
		serverOption20.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SearchCategories"))
		{
			serverOption20.Value = "False";
		}
		ServerOption serverOption21 = new ServerOption();
		serverOption21.Key = "RemoveCameraFromSmartMap";
		serverOption21.Value = "True";
		ServerOption serverOption22 = new ServerOption();
		serverOption22.Key = "MultipleSimultaneousSearches";
		serverOption22.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "MultipleSimultaneousSearches"))
		{
			serverOption22.Value = "False";
		}
		ServerOption serverOption23 = new ServerOption();
		serverOption23.Key = "IntegrationRegistrations";
		serverOption23.Value = "True";
		ServerOption serverOption24 = new ServerOption();
		serverOption24.Key = "SmartWall";
		serverOption24.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartWall"))
		{
			serverOption24.Value = "False";
		}
		bool flag2 = Convert.ToBoolean(ServerInformation.GetSystemSetting(requestContext, SettingList.CollectUsageDataEnabled), CultureInfo.InvariantCulture);
		ServerOption serverOption25 = new ServerOption
		{
			Key = "CollectUsageData",
			Value = flag2.ToString()
		};
		bool flag3 = Convert.ToBoolean(ServerInformation.GetSystemSetting(requestContext, SettingList.CollectTelemetryEnabled), CultureInfo.InvariantCulture);
		ServerOption serverOption26 = new ServerOption
		{
			Key = "CollectTelemetryData",
			Value = flag3.ToString()
		};
		ServerOption serverOption27 = new ServerOption();
		serverOption27.Key = "RestrictedMedia";
		serverOption27.Value = "True";
		if (!GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "RestrictedMedia"))
		{
			serverOption27.Value = "False";
		}
		return new ServerOption[27]
		{
			serverOption, serverOption3, serverOption4, serverOption5, serverOption7, serverOption8, serverOption9, serverOption10, serverOption2, serverOption11,
			serverOption6, serverOption12, serverOption13, serverOption14, serverOption15, serverOption16, serverOption17, serverOption18, serverOption19, serverOption20,
			serverOption21, serverOption22, serverOption23, serverOption24, serverOption25, serverOption26, serverOption27
		};
	}

	private static void AddDeviceGroupInternal(DeviceGroupData group, Dictionary<Guid, List<DeviceGroupData>> groups)
	{
		if (groups.TryGetValue(group.ParentDeviceGroupId, out var value))
		{
			value.Add(group);
			return;
		}
		value = new List<DeviceGroupData>();
		value.Add(group);
		groups.Add(group.ParentDeviceGroupId, value);
	}

	public static RecorderInfo[] GetConfigurationRecorders(IGeminiRequestContext requestContext, Guid[] recorderIds, bool isPublicRequest)
	{
		if (recorderIds == null || recorderIds.Count() == 0)
		{
			return new RecorderInfo[0];
		}
		bool includeStreams = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "LiveMultiStreaming");
		ConfigurationInfo configurationInfo = new ConfigurationInfo();
		string systemSetting = ServerInformation.GetSystemSetting(requestContext, SettingList.LocalIPRanges);
		if (isPublicRequest && LocalIPRangeXmlFormatter.IsInRange(requestContext.RemoteIPAddress, systemSetting))
		{
			isPublicRequest = false;
		}
		bool isAdministrator = requestContext.GetService<SecurityManagerService>().IsAdministrator(requestContext);
		Guid[] memberOfRoles = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles;
		string[] roleIds = ((memberOfRoles == null) ? new string[0] : memberOfRoles.Select((Guid f) => f.ToString()).ToArray());
		bool isEdgeStorageLicensed = GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "EdgeStorage");
		HashSet<Guid> visibilityCache = new HashSet<Guid>();
		ReadRecorders(requestContext, configurationInfo, recorderIds, isPublicRequest, visibilityCache, includeStreams, isAdministrator, roleIds, isEdgeStorageLicensed);
		return configurationInfo.Recorders;
	}

	public static HardwareInfo[] GetConfigurationHardware(IGeminiRequestContext requestContext, Guid[] hardwareIds)
	{
		if (hardwareIds == null || hardwareIds.Count() == 0)
		{
			return new HardwareInfo[0];
		}
		return ReadHardware(requestContext, hardwareIds, null);
	}

	private static void ReadRetentionOptions(IGeminiRequestContext requestContext, ConfigurationInfo proxy, string options)
	{
		List<Guid> list = new List<Guid>();
		proxy.RetentionSettings = new RetentionSettingInfo();
		RetentionOptionsXmlFormatter retentionOptionsXmlFormatter = RetentionOptionsXmlManager.Deserialize(options);
		RetentionOptionRoleRelation[] array = RetentionOptionRoleRelation.QueryRetentionOptionRoleRelation(requestContext);
		Guid[] memberOfRoles = IdentityCache.ReadIdentityFromCache(requestContext, requestContext.AuthenticatedUserSid, QueryMembership.Expanded).MemberOfRoles;
		RetentionOptionRoleRelation[] array2 = array;
		foreach (RetentionOptionRoleRelation retentionOptionRoleRelation in array2)
		{
			if (memberOfRoles.Contains(new Guid(retentionOptionRoleRelation.IdRole)))
			{
				list.Add(new Guid(retentionOptionRoleRelation.IdRetention));
			}
		}
		if (list.Count == 0)
		{
			list.Add(retentionOptionsXmlFormatter.RetentionOptions.Single((RetentionTimeOption option) => option.Default).Id);
		}
		proxy.RetentionSettings.RetentionOptions = ReadTimeOptions(retentionOptionsXmlFormatter, list);
	}

	private static RetentionOption[] ReadTimeOptions(RetentionOptionsXmlFormatter formatter, List<Guid> optionIds)
	{
		if (formatter != null)
		{
			List<RetentionTime> list = new List<RetentionTime>();
			foreach (RetentionTimeOption retentionOption in formatter.RetentionOptions)
			{
				if (optionIds.Contains(retentionOption.Id))
				{
					list.AddRange(retentionOption.RetentionTimes);
				}
			}
			return list.Select((RetentionTime i) => new RetentionOption
			{
				RetentionOptionType = (RetentionOptionType)i.Option,
				RetentionUnits = i.Unit
			}).ToArray();
		}
		return null;
	}

	private static void ReadMgtServerInfo(IGeminiRequestContext requestContext, ConfigurationInfo proxy)
	{
		IdNameDescription idNameDescription = ConfigurationDataManager.FindHomeSiteInfo(requestContext);
		proxy.ServerId = idNameDescription.Id;
		proxy.ServerName = idNameDescription.Name;
		proxy.ServerDescription = idNameDescription.Description;
	}

	private static void ReadApplicationSecurity(IGeminiRequestContext requestContext, ConfigurationInfo proxy, bool live, bool browse, bool setup)
	{
		proxy.ApplicationAccess = new ApplicationSecurityInfo();
		proxy.ApplicationAccess.SmartClientSecurity = new SmartClientSecurityInfo();
		proxy.ApplicationAccess.SmartClientSecurity.SmartClientLive = live;
		proxy.ApplicationAccess.SmartClientSecurity.SmartClientBrowse = browse;
		proxy.ApplicationAccess.SmartClientSecurity.SmartClientSetup = setup;
		proxy.ApplicationAccess.SmartClientSecurity.SmartClientReport = requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.SystemMonitor, "GENERIC_READ", "/");
	}

	public static void GetClientProfileSettings(IGeminiRequestContext requestContext, out bool allowLive, out bool allowBrowse, out bool allowSetup)
	{
		allowBrowse = false;
		allowLive = false;
		allowSetup = false;
		if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(requestContext, "SmartClientProfiles"))
		{
			ClientProfile clientProfile = ClientProfile.QueryActiveSmartClientProfile(requestContext);
			if (clientProfile == null)
			{
				return;
			}
			ClientProfileSettings fromXml = ClientProfileSettings.GetFromXml(clientProfile.SettingsXml);
			if (fromXml != null)
			{
				VideoOS.Management.Common.ClientProfileSetting clientProfileSetting = fromXml[OptionId.PanesLiveTab.ToString()];
				if (clientProfileSetting != null)
				{
					allowLive = clientProfileSetting.Value == OptionAvailable.Available.ToString();
				}
				VideoOS.Management.Common.ClientProfileSetting clientProfileSetting2 = fromXml[OptionId.PanesPlaybackTab.ToString()];
				if (clientProfileSetting2 != null)
				{
					allowBrowse = clientProfileSetting2.Value == OptionAvailable.Available.ToString();
				}
				VideoOS.Management.Common.ClientProfileSetting clientProfileSetting3 = fromXml[OptionId.PanesSetupTab.ToString()];
				if (clientProfileSetting3 != null)
				{
					allowSetup = clientProfileSetting3.Value == OptionAvailable.Available.ToString();
				}
			}
			return;
		}
		ClientSettings fromXml2 = ClientSettings.GetFromXml(ConfigurationDataManager.GetSmartClientSettingsXml(requestContext));
		Role[] array = Role.FindRolesByMember(requestContext, requestContext.AuthenticatedUserSid);
		foreach (Role userRole in array)
		{
			ClientSetting clientSetting = fromXml2.FirstOrDefault((ClientSetting p) => p.RoleId == new Guid(userRole.Id));
			if (clientSetting == null)
			{
				allowLive = true;
				allowBrowse = true;
				allowSetup = true;
				break;
			}
			allowLive = allowLive || clientSetting.Live;
			allowBrowse = allowBrowse || clientSetting.Playback;
			allowSetup = allowSetup || clientSetting.Setup;
		}
	}

	private static void ReadExternalPlugins(IGeminiRequestContext requestContext, ConfigurationInfo proxy, bool includeSmartWallLicense)
	{
		Guid guid = new Guid("5540029a-a075-46c5-a81c-48f1eee44ade");
		List<LicenseInfo> list = new List<LicenseInfo>();
		foreach (ExternalPluginsData item in ExternalPluginsData.QueryExternalSmartClientPlugins(requestContext))
		{
			if (item.PluginId != guid)
			{
				LicenseInfo licenseInfo = new LicenseInfo();
				licenseInfo.LicenseId = item.PluginId;
				licenseInfo.Data = item.Data;
				list.Add(licenseInfo);
			}
		}
		if (includeSmartWallLicense)
		{
			LicenseInfo licenseInfo2 = new LicenseInfo();
			licenseInfo2.LicenseId = guid;
			licenseInfo2.Data = string.Format(CultureInfo.InvariantCulture, "<pluginconfiguration id=\"{0}\" plugin=\"VideoOS.RemoteClient.Plugin.VideoWall\\VideoWallViewItem\" />", guid);
			list.Add(licenseInfo2);
		}
		proxy.Licenses = list.ToArray();
	}

	private static void ReadEventTypes(IGeminiRequestContext requestContext, ConfigurationInfo proxy, IEnumerable<EventTypeData> events)
	{
		List<EventTypeInfo> list = new List<EventTypeInfo>();
		foreach (EventTypeData @event in events)
		{
			if (string.Equals(@event.GeneratorType, "External", StringComparison.OrdinalIgnoreCase) && string.Equals(@event.GeneratorSubType, "UserDefined", StringComparison.OrdinalIgnoreCase) && requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_READ", @event.Id.ToString()))
			{
				EventTypeInfo eventTypeInfo = new EventTypeInfo();
				eventTypeInfo.EventTypeId = @event.Id;
				eventTypeInfo.Name = @event.Name;
				eventTypeInfo.Description = @event.Description;
				list.Add(eventTypeInfo);
			}
		}
		proxy.EventTypes = list.ToArray();
		if (list.Count > 0)
		{
			EventTypeGroupInfo eventTypeGroupInfo = new EventTypeGroupInfo();
			eventTypeGroupInfo.GroupId = Guid.Empty;
			eventTypeGroupInfo.Name = "All EventTypes (Corporate)";
			eventTypeGroupInfo.EventTypes = list.Select((EventTypeInfo e) => e.EventTypeId).ToArray();
			eventTypeGroupInfo.EventTypeGroups = new EventTypeGroupInfo[0];
			proxy.EventTypeGroups = new EventTypeGroupInfo[1] { eventTypeGroupInfo };
		}
		else
		{
			proxy.EventTypeGroups = new EventTypeGroupInfo[0];
		}
	}

	private static void ReadSystemEventTypes(ConfigurationInfo proxy, IEnumerable<EventTypeData> events)
	{
		List<SystemEventTypeInfo> list = new List<SystemEventTypeInfo>();
		foreach (EventTypeData @event in events)
		{
			SystemEventTypeInfo systemEventTypeInfo = new SystemEventTypeInfo();
			systemEventTypeInfo.EventTypeId = @event.Id;
			systemEventTypeInfo.Name = @event.Name;
			systemEventTypeInfo.Description = @event.Description;
			switch (@event.GeneratorType)
			{
			case "Device":
			case "RecorderPlugin":
			case "DriverDevice":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Device;
				break;
			case "External":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.External;
				break;
			case "DriverHardware":
			case "Hardware":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Hardware;
				break;
			case "Recorder":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Recorder;
				break;
			case "Server":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Server;
				break;
			case "System":
			case "SystemDashboard":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.System;
				break;
			case "Timer":
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Timer;
				break;
			default:
				systemEventTypeInfo.EventSource = VideoOS.WebService.Common.ServerCommandService.EventSource.Undefined;
				break;
			}
			list.Add(systemEventTypeInfo);
		}
		proxy.SystemEventTypes = list.ToArray();
	}

	private static void ReadAlertTypes(ConfigurationInfo proxy)
	{
		List<AlertTypeInfo> list = new List<AlertTypeInfo>();
		proxy.AlertTypes = list.ToArray();
		proxy.AlertTypeGroups = LoadAlertTypeGroups();
	}

	private static AlertTypeGroupInfo[] LoadAlertTypeGroups()
	{
		AlertTypeGroupInfo alertTypeGroupInfo = new AlertTypeGroupInfo();
		alertTypeGroupInfo.GroupId = Guid.Empty;
		alertTypeGroupInfo.Name = "All AlertTypes (Corporate)";
		List<Guid> list = new List<Guid>();
		alertTypeGroupInfo.AlertTypes = list.ToArray();
		alertTypeGroupInfo.AlertTypeGroups = new AlertTypeGroupInfo[0];
		return new AlertTypeGroupInfo[1] { alertTypeGroupInfo };
	}

	private static void ReadAudioMessages(IGeminiRequestContext requestContext, ConfigurationInfo proxy)
	{
		AudioMessageIdNameDescription[] source = ConfigurationDataManager.FindAudioMessages(requestContext);
		proxy.AudioMessages = source.Select((AudioMessageIdNameDescription f) => new AudioMessageInfo
		{
			Id = f.Id,
			Name = f.Name,
			Description = f.Description
		}).ToArray();
	}

	private static void ReadDeviceMappings(IGeminiRequestContext requestContext, ConfigurationInfo proxy, HashSet<Guid> visibilityCache)
	{
		List<DeviceMappingInfo> list = new List<DeviceMappingInfo>();
		foreach (DeviceLinkData item in DeviceLinkData.QueryDeviceLinks(requestContext))
		{
			if (visibilityCache.Contains(item.IdDevice) && visibilityCache.Contains(item.IdDeviceLink))
			{
				DeviceMappingInfo deviceMappingInfo = new DeviceMappingInfo();
				deviceMappingInfo.DeviceA = item.IdDevice;
				deviceMappingInfo.DeviceB = item.IdDeviceLink;
				list.Add(deviceMappingInfo);
			}
		}
		proxy.DeviceMappings = list.ToArray();
	}

	private static void ReadMatrixMonitors(IGeminiRequestContext requestContext, ConfigurationInfo proxy)
	{
		List<MatrixMonitorInfo> list = new List<MatrixMonitorInfo>();
		foreach (MatrixViewData item in MatrixViewData.QueryMatrixViews(requestContext))
		{
			if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_READ", item.Id))
			{
				MatrixMonitorInfo matrixMonitorInfo = new MatrixMonitorInfo();
				matrixMonitorInfo.MatrixMonitorId = new Guid(item.Id);
				matrixMonitorInfo.DisplayName = item.Name;
				list.Add(matrixMonitorInfo);
			}
		}
		proxy.MatrixMonitors = list.ToArray();
	}

	private static void ReadRecorders(IGeminiRequestContext requestContext, ConfigurationInfo proxy, Guid[] recorderIds, bool isPublicRequest, HashSet<Guid> visibilityCache, bool includeStreams, bool isAdministrator, string[] roleIds, bool isEdgeStorageLicensed)
	{
		try
		{
			bool useDefaultPresetAsPtzHome = Convert.ToBoolean(ServerInformation.GetSystemSetting(requestContext, SettingList.PtzUseDefaultPresetAsPtzHome), CultureInfo.InvariantCulture);
			if (isAdministrator)
			{
				ReadRecordersParallelAdmin(requestContext, proxy, recorderIds, isPublicRequest, visibilityCache, includeStreams, isEdgeStorageLicensed, useDefaultPresetAsPtzHome);
			}
			else
			{
				ReadRecordersParallel(requestContext, proxy, recorderIds, isPublicRequest, visibilityCache, includeStreams, roleIds, isEdgeStorageLicensed, useDefaultPresetAsPtzHome);
			}
		}
		catch (ThreadPoolTaskExecuteException ex)
		{
			throw ex.InnerException;
		}
	}

	private static void ReadRecordersParallelAdmin(IGeminiRequestContext requestContext, ConfigurationInfo proxy, Guid[] recorderIds, bool isPublicRequest, HashSet<Guid> visibilityCache, bool includeStreams, bool isEdgeStorageLicensed, bool useDefaultPresetAsPtzHome)
	{
		List<RecorderInfo> recorderList = new List<RecorderInfo>();
		Dictionary<Guid, DeviceRecorderPlugin> threeSixtyPlugins = Load360LensPlugIns(requestContext);
		Dictionary<Guid, ExternalPluginsData> externalPlugins = LoadExternalPlugIns(requestContext);
		List<ThreadPoolTask> list = new List<ThreadPoolTask>();
		foreach (RecorderData item2 in RecorderData.QueryEnabledRecorders(requestContext, recorderIds))
		{
			ThreadPoolTask item = ThreadPoolTask.Create(delegate(object state)
			{
				RecorderData recorder = state as RecorderData;
				RecorderInfo recorderInfo = new RecorderInfo();
				FillRecorderInfo(recorderInfo, recorder, isPublicRequest);
				IEnumerable<DeviceData> enumerable = DeviceData.QueryEnabledDevicesByRecorderAdmin(requestContext, recorderInfo.RecorderId);
				Dictionary<Guid, IEnumerable<DeviceTrackData>> deviceTrackDictionary = DeviceTrackData.QueryDeviceTracksByDeviceIds(requestContext, enumerable.Select((DeviceData d) => d.Id).ToArray());
				IEnumerable<MetadataTypeData> metadataTypesForMetadataDevices = MetadataTypeData.QueryMetadataInfoForEnabledDevicesByRecorderAdmin(requestContext, recorderInfo.RecorderId);
				FillRecorder(requestContext, recorderInfo, threeSixtyPlugins, externalPlugins, visibilityCache, includeStreams, isAdministrator: true, isEdgeStorageLicensed, enumerable, deviceTrackDictionary, metadataTypesForMetadataDevices, useDefaultPresetAsPtzHome);
				lock (recorderList)
				{
					recorderList.Add(recorderInfo);
				}
			}, item2);
			list.Add(item);
			if (list.Count <= 4)
			{
				continue;
			}
			List<WaitHandle> list2 = new List<WaitHandle>();
			foreach (ThreadPoolTask item3 in list)
			{
				list2.Add(item3.WaitHandle);
			}
			WaitHandle.WaitAny(list2.ToArray());
			ThreadPoolTask[] array = list.ToArray();
			foreach (ThreadPoolTask threadPoolTask in array)
			{
				if (threadPoolTask.Wait(0))
				{
					list.Remove(threadPoolTask);
				}
			}
		}
		foreach (ThreadPoolTask item4 in list)
		{
			item4.Wait();
		}
		proxy.Recorders = recorderList.ToArray();
	}

	private static void ReadRecordersParallel(IGeminiRequestContext requestContext, ConfigurationInfo proxy, Guid[] recorderIds, bool isPublicRequest, HashSet<Guid> visibilityCache, bool includeStreams, string[] roleIds, bool isEdgeStorageLicensed, bool useDefaultPresetAsPtzHome)
	{
		List<RecorderInfo> recorderList = new List<RecorderInfo>();
		Dictionary<Guid, DeviceRecorderPlugin> threeSixtyPlugins = Load360LensPlugIns(requestContext);
		Dictionary<Guid, ExternalPluginsData> externalPlugins = LoadExternalPlugIns(requestContext);
		List<ThreadPoolTask> list = new List<ThreadPoolTask>();
		Dictionary<Guid, IEnumerable<DeviceData>> recorderDeviceDictionary = DeviceData.QueryEnabledDevicesByRoleId(requestContext, roleIds);
		Guid[] deviceIds = (from d in recorderDeviceDictionary.Values.SelectMany((IEnumerable<DeviceData> dd) => dd)
			select d.Id).ToArray();
		Dictionary<Guid, IEnumerable<DeviceTrackData>> deviceTrackDictionary = DeviceTrackData.QueryDeviceTracksByDeviceIds(requestContext, deviceIds);
		Dictionary<Guid, IEnumerable<MetadataTypeData>> recorderMetadataTypeDataDictionary = MetadataTypeData.QueryMetadataInfoForEnabledDevicesByRoleId(requestContext, roleIds);
		foreach (RecorderData currentRecorder in RecorderData.QueryEnabledRecorders(requestContext, recorderIds))
		{
			ThreadPoolTask item = ThreadPoolTask.Create(delegate(object state)
			{
				RecorderData recorder = state as RecorderData;
				if (!recorderDeviceDictionary.TryGetValue(currentRecorder.Id, out var value))
				{
					value = new List<DeviceData>();
				}
				if (!recorderMetadataTypeDataDictionary.TryGetValue(currentRecorder.Id, out var value2))
				{
					value2 = new List<MetadataTypeData>();
				}
				RecorderInfo recorderInfo = new RecorderInfo();
				FillRecorderInfo(recorderInfo, recorder, isPublicRequest);
				FillRecorder(requestContext, recorderInfo, threeSixtyPlugins, externalPlugins, visibilityCache, includeStreams, isAdministrator: false, isEdgeStorageLicensed, value, deviceTrackDictionary, value2, useDefaultPresetAsPtzHome);
				lock (recorderList)
				{
					recorderList.Add(recorderInfo);
				}
			}, currentRecorder);
			list.Add(item);
			if (list.Count <= 4)
			{
				continue;
			}
			List<WaitHandle> list2 = new List<WaitHandle>();
			foreach (ThreadPoolTask item2 in list)
			{
				list2.Add(item2.WaitHandle);
			}
			WaitHandle.WaitAny(list2.ToArray());
			ThreadPoolTask[] array = list.ToArray();
			foreach (ThreadPoolTask threadPoolTask in array)
			{
				if (threadPoolTask.Wait(0))
				{
					list.Remove(threadPoolTask);
				}
			}
		}
		foreach (ThreadPoolTask item3 in list)
		{
			item3.Wait();
		}
		proxy.Recorders = recorderList.ToArray();
	}

	private static void FillRecorderInfo(RecorderInfo recorderInfo, RecorderData recorder, bool isPublicRequest)
	{
		recorderInfo.RecorderId = recorder.Id;
		recorderInfo.ServiceId = recorder.ServiceId;
		recorderInfo.Name = recorder.Name;
		recorderInfo.Description = recorder.Description;
		recorderInfo.HostName = recorder.HostName;
		recorderInfo.WebServerUri = recorder.WebServerUri;
		if (isPublicRequest && !string.IsNullOrEmpty(recorder.WebServerUriConfiguration))
		{
			recorderInfo.WebServerUri = recorder.WebServerUriConfiguration;
		}
		recorderInfo.TimeZoneName = recorder.TimeZoneName;
		recorderInfo.XmlEncoding = "utf-8";
		recorderInfo.LastModified = recorder.LastModified;
	}

	public static Dictionary<Guid, DeviceRecorderPlugin> Load360LensPlugIns(IGeminiRequestContext requestContext)
	{
		Dictionary<Guid, DeviceRecorderPlugin> dictionary = new Dictionary<Guid, DeviceRecorderPlugin>();
		IEnumerable<DeviceRecorderPlugin> enumerable = DeviceRecorderPlugin.QueryEnabledPluginsByType(requestContext, GlobalIDs.ImmerVisionPlugInGuid);
		if (enumerable.Any())
		{
			foreach (DeviceRecorderPlugin item in enumerable)
			{
				dictionary.Add(item.DeviceId, item);
			}
		}
		return dictionary;
	}

	public static Dictionary<Guid, ExternalPluginsData> LoadExternalPlugIns(IGeminiRequestContext requestContext)
	{
		Dictionary<Guid, ExternalPluginsData> dictionary = new Dictionary<Guid, ExternalPluginsData>();
		foreach (ExternalPluginsData item in ExternalPluginsData.QueryExternalServerPlugins(requestContext))
		{
			dictionary.Add(item.PluginId, item);
		}
		return dictionary;
	}

	private static void FillRecorder(IGeminiRequestContext requestContext, RecorderInfo recorderInfo, Dictionary<Guid, DeviceRecorderPlugin> threeSixtyPlugins, Dictionary<Guid, ExternalPluginsData> externalPlugins, HashSet<Guid> visibilityCache, bool includeStreams, bool isAdministrator, bool isEdgeStorageLicensed, IEnumerable<DeviceData> devices, Dictionary<Guid, IEnumerable<DeviceTrackData>> deviceTrackDictionary, IEnumerable<MetadataTypeData> metadataTypesForMetadataDevices, bool useDefaultPresetAsPtzHome)
	{
		List<CameraInfo> list = new List<CameraInfo>();
		List<InputInfo> list2 = new List<InputInfo>();
		List<OutputInfo> list3 = new List<OutputInfo>();
		List<MicrophoneInfo> list4 = new List<MicrophoneInfo>();
		List<SpeakerInfo> list5 = new List<SpeakerInfo>();
		List<MetadataDeviceInfo> list6 = new List<MetadataDeviceInfo>();
		Dictionary<Guid, List<Guid>> dictionary = new Dictionary<Guid, List<Guid>>();
		foreach (DeviceData device in devices)
		{
			lock (visibilityCache)
			{
				visibilityCache.Add(device.Id);
			}
			switch (RegisteredDeviceType.GetDeviceTypeEnum(device.DeviceType))
			{
			case DeviceTypeEnum.Camera:
			{
				CameraInfo cameraInfo = new CameraInfo();
				cameraInfo.RecorderId = recorderInfo.RecorderId;
				cameraInfo.Tracks = FillTracks(device, deviceTrackDictionary);
				DeviceRecorderPlugin value = null;
				threeSixtyPlugins.TryGetValue(device.Id, out value);
				FillCameraDevice(requestContext, cameraInfo, device, value, externalPlugins, includeStreams, isAdministrator, isEdgeStorageLicensed, useDefaultPresetAsPtzHome);
				list.Add(cameraInfo);
				break;
			}
			case DeviceTypeEnum.Input:
			{
				InputInfo inputInfo = new InputInfo();
				inputInfo.RecorderId = recorderInfo.RecorderId;
				FillInputDevice(requestContext, inputInfo, device, isAdministrator);
				list2.Add(inputInfo);
				break;
			}
			case DeviceTypeEnum.Output:
			{
				OutputInfo outputInfo = new OutputInfo();
				outputInfo.RecorderId = recorderInfo.RecorderId;
				FillOutputDevice(requestContext, outputInfo, device, isAdministrator);
				list3.Add(outputInfo);
				break;
			}
			case DeviceTypeEnum.AudioInput:
			{
				MicrophoneInfo microphoneInfo = new MicrophoneInfo();
				microphoneInfo.RecorderId = recorderInfo.RecorderId;
				microphoneInfo.Tracks = FillTracks(device, deviceTrackDictionary);
				FillMicrophoneDevice(requestContext, microphoneInfo, device, isAdministrator, isEdgeStorageLicensed);
				list4.Add(microphoneInfo);
				break;
			}
			case DeviceTypeEnum.AudioOutput:
			{
				SpeakerInfo speakerInfo = new SpeakerInfo();
				speakerInfo.RecorderId = recorderInfo.RecorderId;
				speakerInfo.Tracks = FillTracks(device, deviceTrackDictionary);
				FillSpeakerDevice(requestContext, speakerInfo, device, isAdministrator, isEdgeStorageLicensed);
				list5.Add(speakerInfo);
				break;
			}
			case DeviceTypeEnum.Metadata:
			{
				MetadataDeviceInfo metadataDeviceInfo = new MetadataDeviceInfo();
				metadataDeviceInfo.RecorderId = recorderInfo.RecorderId;
				metadataDeviceInfo.Tracks = FillTracks(device, deviceTrackDictionary);
				MetadataTypeData[] metadataTypes = metadataTypesForMetadataDevices.Where((MetadataTypeData m) => m.DeviceId == device.Id).ToArray();
				FillMetadataDevice(requestContext, metadataDeviceInfo, device, metadataTypes, isAdministrator, isEdgeStorageLicensed);
				list6.Add(metadataDeviceInfo);
				break;
			}
			}
			if (!dictionary.TryGetValue(device.HardwareId, out var value2))
			{
				value2 = new List<Guid>();
				dictionary.Add(device.HardwareId, value2);
			}
			value2.Add(device.Id);
		}
		recorderInfo.Cameras = list.ToArray();
		recorderInfo.Inputs = list2.ToArray();
		recorderInfo.Outputs = list3.ToArray();
		recorderInfo.Microphones = list4.ToArray();
		recorderInfo.Speakers = list5.ToArray();
		recorderInfo.MetadataDevices = list6.ToArray();
		recorderInfo.Hardware = ReadHardware(requestContext, dictionary.Keys, dictionary);
	}

	private static TrackInfo[] FillTracks(DeviceData device, Dictionary<Guid, IEnumerable<DeviceTrackData>> deviceTrackDictionary)
	{
		if (device.EdgestoragePlaybackEnabled)
		{
			return null;
		}
		IEnumerable<DeviceTrackData> source = deviceTrackDictionary[device.Id];
		List<TrackInfo> list = new List<TrackInfo>();
		DeviceTrackData deviceTrackData = source.Single((DeviceTrackData t) => t.IsPrimary);
		if (deviceTrackData.MergeEdge)
		{
			list.Add(new TrackInfo
			{
				TrackId = source.Single((DeviceTrackData t) => t.IsEdge).TrackId,
				Edge = true
			});
		}
		list.Add(new TrackInfo
		{
			TrackId = deviceTrackData.TrackId,
			Edge = false
		});
		return list.ToArray();
	}

	private static void FillDevice(DeviceInfo deviceInfo, DeviceData device)
	{
		deviceInfo.HardwareId = device.HardwareId;
		deviceInfo.DeviceId = device.Id;
		deviceInfo.Name = device.Name;
		deviceInfo.Description = device.Description;
		deviceInfo.Shortcut = device.Shortcut;
		deviceInfo.DeviceIndex = device.DeviceIndex;
		deviceInfo.GisPoint = device.GisPoint;
		deviceInfo.ShortName = device.ShortName;
		deviceInfo.Icon = device.Icon;
		deviceInfo.CoverageDirection = device.CoverageDirection;
		deviceInfo.CoverageDepth = device.CoverageDepth;
		deviceInfo.CoverageFieldOfView = device.CoverageFieldOfView;
	}

	private static void FillCameraDevice(IGeminiRequestContext requestContext, CameraInfo cameraInfo, DeviceData device, DeviceRecorderPlugin threeSixtyPlugin, Dictionary<Guid, ExternalPluginsData> externalPlugins, bool includeStreams, bool isAdministrator, bool isEdgeStorageLicensed, bool useDefaultPresetAsPtzHome)
	{
		DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(device.Settings);
		FillDevice(cameraInfo, device);
		cameraInfo.StopManualRecordingSeconds = (device.ManualTimeoutEnabled ? (device.ManualTimeoutMinutes * 60) : 0);
		cameraInfo.EdgeStorageSupported = deviceSettingUtilities.IsEdgeStorageSupported && !device.EdgestoragePlaybackEnabled && isEdgeStorageLicensed;
		cameraInfo.EdgeStoragePlayback = device.EdgestoragePlaybackEnabled && isEdgeStorageLicensed;
		if (includeStreams)
		{
			cameraInfo.Streams = ReadStreams(device);
		}
		FillCameraSecurity(requestContext, cameraInfo, device.Id.ToString(), isAdministrator);
		FillCameraPtzSettings(cameraInfo, deviceSettingUtilities, useDefaultPresetAsPtzHome);
		cameraInfo.IpixSettings = new IpixInfo();
		cameraInfo.PanoramicLensSettings = new PanoramicLensInfo();
		if (deviceSettingUtilities.IsPtzEnabled)
		{
			ReadPresets(cameraInfo.PtzSettings, device);
		}
		else
		{
			ReadPanoramicLensSettings(cameraInfo.PanoramicLensSettings, threeSixtyPlugin, externalPlugins);
		}
		cameraInfo.MulticastEnabled = device.MulticastEnabled;
	}

	private static void FillCameraSecurity(IGeminiRequestContext requestContext, CameraInfo cameraInfo, string deviceId, bool isAdministrator)
	{
		cameraInfo.CameraSecurity = new CameraSecurityInfo();
		cameraInfo.CameraSecurity.Live = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "VIEW_LIVE", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.Browse = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "PLAYBACK", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ExportAvi = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "EXPORT", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ExportJpeg = cameraInfo.CameraSecurity.ExportAvi;
		cameraInfo.CameraSecurity.SmartSearch = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "SMARTSEARCH", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.GetSequences = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "READ_SEQUENCES", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ExportDatabase = cameraInfo.CameraSecurity.ExportAvi;
		cameraInfo.CameraSecurity.StartRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "START_MANUAL_RECORDING", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.StopRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "STOP_MANUAL_RECORDING", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.BookmarkView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "READ_BOOKMARKS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.BookmarkAdd = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "CREATE_BOOKMARKS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.BookmarkEdit = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "EDIT_BOOKMARKS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.BookmarkDelete = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "DELETE_BOOKMARKS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RetentionView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "READ_EVIDENCE_LOCK", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RetentionCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "CREATE_EVIDENCE_LOCK", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RetentionRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "DELETE_EVIDENCE_LOCK", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RestrictedMediaView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "READ_RESTRICTIONS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RestrictedMediaCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "CREATE_RESTRICTIONS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RestrictedMediaRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "DELETE_RESTRICTIONS", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.PtzSecurity = new PtzSecurityInfo();
		cameraInfo.CameraSecurity.PtzSecurity.ManualControl = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "PTZ_CONTROL", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.PtzSecurity.PresetControl = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "ACTIVATE_PTZ_PRESET", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.PtzSecurity.ReserveControl = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "RESERVE_PTZ_SESSION", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ManagePresets = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ManagePatrollingProfiles = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.ManagePresetLocks = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceId, checkAdministrator: false);
		cameraInfo.CameraSecurity.RetrieveEdgeRecordings = cameraInfo.EdgeStorageSupported && (isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "READ_REMOTE_RECORDINGS", deviceId, checkAdministrator: false));
	}

	private static void FillCameraPtzSettings(CameraInfo cameraInfo, DeviceSettingUtilities deviceSettingUtils, bool useDefaultPresetAsPtzHome)
	{
		List<CapabilityInfo> list = new List<CapabilityInfo>();
		cameraInfo.PtzSettings = new PtzInfo();
		if (deviceSettingUtils.IsPtzEnabled)
		{
			cameraInfo.PtzSettings.PtzEnabled = true;
			DevicePtzSupport ptzSupport = deviceSettingUtils.PtzSupport;
			cameraInfo.PtzSettings.IsCenterOnPositionInViewSupported = ptzSupport.CenterOnPositionInView;
			cameraInfo.PtzSettings.IsPtzCenterAndZoomToRectangleSupported = ptzSupport.CenterAndZoomToRectangle;
			cameraInfo.PtzSettings.IsPtzHomeSupported = useDefaultPresetAsPtzHome || ptzSupport.Home;
			cameraInfo.PtzSettings.IsPtzDiagonalSupported = ptzSupport.Diagonal;
			if (deviceSettingUtils.PresetConfiguration != null)
			{
				cameraInfo.PtzSettings.EditPreset = (deviceSettingUtils.PresetConfiguration.HasAbsolutePresets && deviceSettingUtils.PresetConfiguration.Absolute.CanQueryPosition) || deviceSettingUtils.PresetConfiguration.HasInternalPresets;
			}
			else
			{
				cameraInfo.PtzSettings.EditPreset = false;
			}
			PtzSetting[] ptzControls = deviceSettingUtils.PtzControls;
			foreach (PtzSetting ptzSetting in ptzControls)
			{
				CapabilityInfo capabilityInfo = new CapabilityInfo();
				capabilityInfo.CapabilityId = new Guid(ptzSetting.TypeId);
				capabilityInfo.Name = ptzSetting.SettingName;
				capabilityInfo.Absolute = ptzSetting.Absolute;
				capabilityInfo.Relative = ptzSetting.Relative;
				capabilityInfo.Automatic = ptzSetting.Automatic;
				capabilityInfo.Start = ptzSetting.Start;
				capabilityInfo.Stop = ptzSetting.Stop;
				capabilityInfo.Speed = ptzSetting.Speed;
				list.Add(capabilityInfo);
			}
		}
		cameraInfo.PtzSettings.Capabilities = list.ToArray();
	}

	private static void FillInputDevice(IGeminiRequestContext requestContext, InputInfo inputInfo, DeviceData device, bool isAdministrator)
	{
		FillDevice(inputInfo, device);
		inputInfo.InputSecurity = new InputSecurityInfo();
		inputInfo.InputSecurity.ReadInput = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Input, "GENERIC_READ", device.Id.ToString(), checkAdministrator: false);
	}

	private static void FillOutputDevice(IGeminiRequestContext requestContext, OutputInfo outputInfo, DeviceData device, bool isAdministrator)
	{
		FillDevice(outputInfo, device);
		outputInfo.OutputSecurity = new OutputSecurityInfo();
		outputInfo.OutputSecurity.Activate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Output, "ACTIVATE", device.Id.ToString(), checkAdministrator: false);
	}

	private static void FillMicrophoneDevice(IGeminiRequestContext requestContext, MicrophoneInfo microphoneInfo, DeviceData device, bool isAdministrator, bool isEdgeStorageLicensed)
	{
		DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(device.Settings);
		FillDevice(microphoneInfo, device);
		microphoneInfo.StopManualRecordingSeconds = (device.ManualTimeoutEnabled ? (device.ManualTimeoutMinutes * 60) : 0);
		microphoneInfo.EdgeStorageSupported = isEdgeStorageLicensed && deviceSettingUtilities.IsEdgeStorageSupported && !device.EdgestoragePlaybackEnabled;
		microphoneInfo.EdgeStoragePlayback = isEdgeStorageLicensed && device.EdgestoragePlaybackEnabled;
		microphoneInfo.MulticastEnabled = device.MulticastEnabled;
		string objectId = device.Id.ToString();
		microphoneInfo.MicrophoneSecurity = new MicrophoneSecurityInfo();
		microphoneInfo.MicrophoneSecurity.Live = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "LISTEN", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.Browse = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "PLAYBACK", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.GetSequences = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "READ_SEQUENCES", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.ExportDatabase = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "EXPORT", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.BookmarkView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "READ_BOOKMARKS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.BookmarkAdd = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "CREATE_BOOKMARKS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.BookmarkEdit = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "EDIT_BOOKMARKS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.BookmarkDelete = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "DELETE_BOOKMARKS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RetentionView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "READ_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RetentionCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "CREATE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RetentionRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "DELETE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RestrictedMediaView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "READ_RESTRICTIONS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RestrictedMediaCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "CREATE_RESTRICTIONS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RestrictedMediaRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "DELETE_RESTRICTIONS", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.RetrieveEdgeRecordings = microphoneInfo.EdgeStorageSupported && (isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "READ_REMOTE_RECORDINGS", objectId, checkAdministrator: false));
		microphoneInfo.MicrophoneSecurity.StartRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "START_MANUAL_RECORDING", objectId, checkAdministrator: false);
		microphoneInfo.MicrophoneSecurity.StopRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "STOP_MANUAL_RECORDING", objectId, checkAdministrator: false);
	}

	private static void FillSpeakerDevice(IGeminiRequestContext requestContext, SpeakerInfo speakerInfo, DeviceData device, bool isAdministrator, bool isEdgeStorageLicensed)
	{
		DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(device.Settings);
		FillDevice(speakerInfo, device);
		speakerInfo.StopManualRecordingSeconds = (device.ManualTimeoutEnabled ? (device.ManualTimeoutMinutes * 60) : 0);
		speakerInfo.EdgeStorageSupported = isEdgeStorageLicensed && deviceSettingUtilities.IsEdgeStorageSupported && !device.EdgestoragePlaybackEnabled;
		speakerInfo.EdgeStoragePlayback = isEdgeStorageLicensed && device.EdgestoragePlaybackEnabled;
		speakerInfo.MulticastEnabled = device.MulticastEnabled;
		string objectId = device.Id.ToString();
		speakerInfo.SpeakerSecurity = new SpeakerSecurityInfo();
		speakerInfo.SpeakerSecurity.Live = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "LISTEN", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.Browse = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "PLAYBACK", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.Speak = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "SPEAK", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.GetSequences = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "READ_SEQUENCES", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.ExportDatabase = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "EXPORT", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.BookmarkView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "READ_BOOKMARKS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.BookmarkAdd = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "CREATE_BOOKMARKS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.BookmarkEdit = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "EDIT_BOOKMARKS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.BookmarkDelete = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "DELETE_BOOKMARKS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RetentionView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "READ_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RetentionCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "CREATE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RetentionRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "DELETE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RestrictedMediaView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "READ_RESTRICTIONS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RestrictedMediaCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "CREATE_RESTRICTIONS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RestrictedMediaRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "DELETE_RESTRICTIONS", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.RetrieveEdgeRecordings = speakerInfo.EdgeStorageSupported && (isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "READ_REMOTE_RECORDINGS", objectId, checkAdministrator: false));
		speakerInfo.SpeakerSecurity.StartRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "START_MANUAL_RECORDING", objectId, checkAdministrator: false);
		speakerInfo.SpeakerSecurity.StopRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "STOP_MANUAL_RECORDING", objectId, checkAdministrator: false);
	}

	private static void FillMetadataDevice(IGeminiRequestContext requestContext, MetadataDeviceInfo metadataDeviceInfo, DeviceData device, MetadataTypeData[] metadataTypes, bool isAdministrator, bool isEdgeStorageLicensed)
	{
		DeviceSettingUtilities deviceSettingUtilities = new DeviceSettingUtilities(device.Settings);
		FillDevice(metadataDeviceInfo, device);
		metadataDeviceInfo.StopManualRecordingSeconds = (device.ManualTimeoutEnabled ? (device.ManualTimeoutMinutes * 60) : 0);
		metadataDeviceInfo.EdgeStorageSupported = isEdgeStorageLicensed && deviceSettingUtilities.IsEdgeStorageSupported && !device.EdgestoragePlaybackEnabled;
		metadataDeviceInfo.EdgeStoragePlayback = isEdgeStorageLicensed && device.EdgestoragePlaybackEnabled;
		metadataDeviceInfo.MulticastEnabled = device.MulticastEnabled;
		string objectId = device.Id.ToString();
		metadataDeviceInfo.MetadataDeviceSecurity = new MetadataDeviceSecurityInfo();
		metadataDeviceInfo.MetadataDeviceSecurity.Live = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "LIVE", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.Browse = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "PLAYBACK", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.GetSequences = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "READ_SEQUENCES", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.ExportDatabase = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "EXPORT", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RetentionView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "READ_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RetentionCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "CREATE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RetentionRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "DELETE_EVIDENCE_LOCK", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RestrictedMediaView = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "READ_RESTRICTIONS", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RestrictedMediaCreate = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "CREATE_RESTRICTIONS", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RestrictedMediaRemove = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "DELETE_RESTRICTIONS", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.RetrieveEdgeRecordings = metadataDeviceInfo.EdgeStorageSupported && (isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "READ_REMOTE_RECORDINGS", objectId, checkAdministrator: false));
		metadataDeviceInfo.MetadataDeviceSecurity.StartRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "START_MANUAL_RECORDING", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataDeviceSecurity.StopRecording = isAdministrator || requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "STOP_MANUAL_RECORDING", objectId, checkAdministrator: false);
		metadataDeviceInfo.MetadataTypes = new MetadataTypeInfo[metadataTypes.Length];
		for (int i = 0; i < metadataTypes.Length; i++)
		{
			MetadataTypeData metadataTypeData = metadataTypes[i];
			MetadataTypeInfo metadataTypeInfo = new MetadataTypeInfo();
			metadataTypeInfo.Id = metadataTypeData.Id;
			metadataTypeInfo.Name = metadataTypeData.Name;
			metadataTypeInfo.DisplayId = metadataTypeData.DisplayId;
			metadataTypeInfo.DisplayName = metadataTypeData.DisplayName;
			metadataTypeInfo.ValidTime = deviceSettingUtilities.GetValidTime(device.DeviceIndex, metadataTypeData.ItemIndex);
			metadataDeviceInfo.MetadataTypes[i] = metadataTypeInfo;
		}
	}

	private static void FillHardware(HardwareInfo hardwareInfo, HardwareData hardwareData, Guid[] hardwareDevices)
	{
		hardwareInfo.HardwareId = hardwareData.Id;
		hardwareInfo.DeviceIds = hardwareDevices;
		hardwareInfo.RecorderId = hardwareData.RecorderId;
		hardwareInfo.Name = hardwareData.Name;
		hardwareInfo.Description = hardwareData.Description;
		hardwareInfo.Interconnected = hardwareData.Subtype.HasFlag(Subtypes.InterconnectedProduct);
		hardwareInfo.Uri = hardwareData.Uri;
		hardwareInfo.LastModified = hardwareData.LastModified;
	}

	private static HardwareInfo[] ReadHardware(IGeminiRequestContext requestContext, IEnumerable<Guid> hardwareIds, Dictionary<Guid, List<Guid>> hardwareDeviceIds)
	{
		IEnumerable<HardwareData> enumerable = HardwareData.QueryHardwareByIds(requestContext, hardwareIds.ToArray());
		List<HardwareInfo> list = new List<HardwareInfo>();
		foreach (HardwareData item in enumerable)
		{
			List<Guid> value = null;
			if (hardwareDeviceIds != null)
			{
				hardwareDeviceIds.TryGetValue(item.Id, out value);
			}
			else
			{
				value = new List<Guid>(DeviceData.QueryDeviceIdsByHardware(requestContext, item.Id));
			}
			if (value != null && value.Count() > 0)
			{
				HardwareInfo hardwareInfo = new HardwareInfo();
				FillHardware(hardwareInfo, item, value.ToArray());
				list.Add(hardwareInfo);
			}
		}
		return list.ToArray();
	}

	private static VideoOS.WebService.Common.ServerCommandService.StreamInfo[] ReadStreams(DeviceData device)
	{
		List<VideoOS.WebService.Common.ServerCommandService.StreamInfo> list = new List<VideoOS.WebService.Common.ServerCommandService.StreamInfo>();
		if (!string.IsNullOrEmpty(device.Streams))
		{
			StreamsConfiguration streamsConfiguration = new StreamsConfiguration();
			streamsConfiguration.LoadXml(device.Streams);
			foreach (VideoOS.Common.Integration.StreamInfo item2 in streamsConfiguration)
			{
				if (item2.LiveFeed)
				{
					VideoOS.WebService.Common.ServerCommandService.StreamInfo item = new VideoOS.WebService.Common.ServerCommandService.StreamInfo
					{
						StreamId = item2.StreamReferenceId,
						Name = item2.Name,
						Default = item2.LiveDefault
					};
					list.Add(item);
				}
			}
		}
		return list.ToArray();
	}

	private static void ReadPresets(PtzInfo ptzInfo, DeviceData device)
	{
		List<PresetInfo> list = new List<PresetInfo>();
		if (!string.IsNullOrEmpty(device.Presets))
		{
			foreach (PtzPreset item in PtzPresetCollection.FromXml(device.Presets))
			{
				PresetInfo presetInfo = new PresetInfo();
				if (item.IsNumbered)
				{
					presetInfo.Name = string.Format(CultureInfo.InvariantCulture, "{1} [#{0}]", item.Number, item.Name);
				}
				else
				{
					presetInfo.Name = item.Name;
				}
				presetInfo.Id = item.Id;
				presetInfo.Position = new PositionInfo();
				presetInfo.Position.Pan = item.PtzArgs.Pan;
				presetInfo.Position.Tilt = item.PtzArgs.Tilt;
				presetInfo.Position.Zoom = item.PtzArgs.Zoom;
				presetInfo.Locked = item.Locked;
				list.Add(presetInfo);
			}
		}
		ptzInfo.Presets = list.ToArray();
	}

	private static void ReadPanoramicLensSettings(PanoramicLensInfo panoramicLensInfo, DeviceRecorderPlugin threeSixtyPlugin, Dictionary<Guid, ExternalPluginsData> externalPlugins)
	{
		panoramicLensInfo.PanoramicLensEnabled = false;
		if (threeSixtyPlugin == null)
		{
			return;
		}
		if (threeSixtyPlugin.RecorderPluginId == WellKnownPluginIds.GenericDewarpPlugin)
		{
			GenericDewarpSettings genericDewarpSettings = GenericDewarpSettings.FromXml(threeSixtyPlugin.Settings);
			if (genericDewarpSettings.Enabled)
			{
				panoramicLensInfo.PanoramicLensEnabled = genericDewarpSettings.Enabled;
				panoramicLensInfo.CameraMount = WebApiConvert.ToString(genericDewarpSettings.Position);
				panoramicLensInfo.PanoramicLensType = "generic";
				panoramicLensInfo.ImmerVision = new ImmerVisionInfo();
				panoramicLensInfo.ImmerVision.LensProfileRpl = "";
				panoramicLensInfo.ImmerVision.LensProfileName = "";
				panoramicLensInfo.ImmerVision.LensProfileData = "";
				panoramicLensInfo.ImmerVision.LensProfileFieldOfView = 180.0;
			}
		}
		else
		{
			if (!(threeSixtyPlugin.RecorderPluginId == WellKnownPluginIds.ImmerVisionPlugin))
			{
				return;
			}
			ImmerVisionSettings immerVisionSettings = ImmerVisionSettings.FromXml(threeSixtyPlugin.Settings);
			if (immerVisionSettings.Enabled && externalPlugins.ContainsKey(WellKnownPluginIds.ImmerVisionPlugin))
			{
				ImmerVisionWrapper immerVisionWrapper = new ImmerVisionWrapper(externalPlugins[WellKnownPluginIds.ImmerVisionPlugin].Data);
				LensProfile lensProfile = null;
				if (immerVisionWrapper.TryGetLensProfile(immerVisionSettings.RegisteredPanomorphLensNumber, out lensProfile))
				{
					panoramicLensInfo.PanoramicLensEnabled = immerVisionSettings.Enabled;
					panoramicLensInfo.CameraMount = WebApiConvert.ToString(immerVisionSettings.Position);
					panoramicLensInfo.PanoramicLensType = "immervision";
					panoramicLensInfo.ImmerVision = new ImmerVisionInfo();
					panoramicLensInfo.ImmerVision.LensProfileRpl = lensProfile.RegisteredPanomorphLensNumber;
					panoramicLensInfo.ImmerVision.LensProfileName = lensProfile.Name;
					panoramicLensInfo.ImmerVision.LensProfileData = string.Empty;
					panoramicLensInfo.ImmerVision.LensProfileFieldOfView = immerVisionSettings.FieldOfView;
				}
			}
		}
	}

	private static void ReadCameraGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.CameraGroups = CreateGroupInfoArray<CameraGroupInfo>("Camera", visibilityCache, devices, members);
	}

	private static void ReadInputGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.InputGroups = CreateGroupInfoArray<InputGroupInfo>("Input", visibilityCache, devices, members);
	}

	private static void ReadOutputGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.OutputGroups = CreateGroupInfoArray<OutputGroupInfo>("Output", visibilityCache, devices, members);
	}

	private static void ReadMicrophoneGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.MicrophoneGroups = CreateGroupInfoArray<MicrophoneGroupInfo>("AudioInput", visibilityCache, devices, members);
	}

	private static void ReadSpeakerGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.SpeakerGroups = CreateGroupInfoArray<SpeakerGroupInfo>("AudioOutput", visibilityCache, devices, members);
	}

	private static void ReadMetadataDeviceGroups(ConfigurationInfo proxy, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members)
	{
		proxy.MetadataDeviceGroups = CreateGroupInfoArray<MetadataDeviceGroupInfo>("Metadata", visibilityCache, devices, members);
	}

	private static T[] CreateGroupInfoArray<T>(string deviceType, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members) where T : GroupInfo, new()
	{
		List<T> list = new List<T>();
		if (devices.TryGetValue(Guid.Empty, out var value))
		{
			foreach (DeviceGroupData item in value)
			{
				T val = new T
				{
					GroupId = item.Id,
					Name = item.Name,
					Description = item.Description
				};
				if (LoadDeviceGroups(val, item, deviceType, visibilityCache, devices, members))
				{
					list.Add(val);
				}
			}
		}
		return list.ToArray();
	}

	private static bool LoadDeviceGroups<T>(T groupInfo, DeviceGroupData deviceGroup, string deviceType, HashSet<Guid> visibilityCache, Dictionary<Guid, List<DeviceGroupData>> devices, Dictionary<Guid, IEnumerable<Guid>> members) where T : GroupInfo, new()
	{
		List<T> list = new List<T>();
		List<Guid> list2 = new List<Guid>();
		if (devices.TryGetValue(deviceGroup.Id, out var value))
		{
			foreach (DeviceGroupData item in value)
			{
				T val = new T
				{
					GroupId = item.Id,
					Name = item.Name,
					Description = item.Description
				};
				if (LoadDeviceGroups(val, item, deviceType, visibilityCache, devices, members))
				{
					list.Add(val);
				}
			}
		}
		if (members.TryGetValue(deviceGroup.Id, out var value2))
		{
			foreach (Guid item2 in value2)
			{
				if (visibilityCache.Contains(item2))
				{
					list2.Add(item2);
				}
			}
		}
		if (groupInfo is CameraGroupInfo cameraGroupInfo)
		{
			cameraGroupInfo.Cameras = list2.ToArray();
			cameraGroupInfo.CameraGroups = list.ConvertAll<CameraGroupInfo>(GroupToCameraGroup).ToArray();
			if (cameraGroupInfo.Cameras.Length == 0)
			{
				return cameraGroupInfo.CameraGroups.Length != 0;
			}
			return true;
		}
		if (groupInfo is InputGroupInfo inputGroupInfo)
		{
			inputGroupInfo.Inputs = list2.ToArray();
			inputGroupInfo.InputGroups = list.ConvertAll<InputGroupInfo>(GroupToInputGroup).ToArray();
			if (inputGroupInfo.Inputs.Length == 0)
			{
				return inputGroupInfo.InputGroups.Length != 0;
			}
			return true;
		}
		if (groupInfo is OutputGroupInfo outputGroupInfo)
		{
			outputGroupInfo.Outputs = list2.ToArray();
			outputGroupInfo.OutputGroups = list.ConvertAll<OutputGroupInfo>(GroupToOutputGroup).ToArray();
			if (outputGroupInfo.Outputs.Length == 0)
			{
				return outputGroupInfo.OutputGroups.Length != 0;
			}
			return true;
		}
		if (groupInfo is MicrophoneGroupInfo microphoneGroupInfo)
		{
			microphoneGroupInfo.Microphones = list2.ToArray();
			microphoneGroupInfo.MicrophoneGroups = list.ConvertAll<MicrophoneGroupInfo>(GroupToMicrophoneGroup).ToArray();
			if (microphoneGroupInfo.Microphones.Length == 0)
			{
				return microphoneGroupInfo.MicrophoneGroups.Length != 0;
			}
			return true;
		}
		if (groupInfo is SpeakerGroupInfo speakerGroupInfo)
		{
			speakerGroupInfo.Speakers = list2.ToArray();
			speakerGroupInfo.SpeakerGroups = list.ConvertAll<SpeakerGroupInfo>(GroupToSpeakerGroup).ToArray();
			if (speakerGroupInfo.Speakers.Length == 0)
			{
				return speakerGroupInfo.SpeakerGroups.Length != 0;
			}
			return true;
		}
		if (groupInfo is MetadataDeviceGroupInfo metadataDeviceGroupInfo)
		{
			metadataDeviceGroupInfo.MetadataDevices = list2.ToArray();
			metadataDeviceGroupInfo.MetadataDeviceGroups = list.ConvertAll<MetadataDeviceGroupInfo>(GroupToMetadataDeviceGroup).ToArray();
			if (metadataDeviceGroupInfo.MetadataDevices.Length == 0)
			{
				return metadataDeviceGroupInfo.MetadataDeviceGroups.Length != 0;
			}
			return true;
		}
		return false;
	}

	private static CameraGroupInfo GroupToCameraGroup(GroupInfo groupInfo)
	{
		return groupInfo as CameraGroupInfo;
	}

	private static InputGroupInfo GroupToInputGroup(GroupInfo groupInfo)
	{
		return groupInfo as InputGroupInfo;
	}

	private static OutputGroupInfo GroupToOutputGroup(GroupInfo groupInfo)
	{
		return groupInfo as OutputGroupInfo;
	}

	private static MicrophoneGroupInfo GroupToMicrophoneGroup(GroupInfo groupInfo)
	{
		return groupInfo as MicrophoneGroupInfo;
	}

	private static SpeakerGroupInfo GroupToSpeakerGroup(GroupInfo groupInfo)
	{
		return groupInfo as SpeakerGroupInfo;
	}

	private static MetadataDeviceGroupInfo GroupToMetadataDeviceGroup(GroupInfo groupInfo)
	{
		return groupInfo as MetadataDeviceGroupInfo;
	}

	public static DeviceDisabledInfo[] GetDevicesDisabled(IGeminiRequestContext requestContext)
	{
		if (!requestContext.GetSecurityManagerService().IsAdministrator(requestContext))
		{
			throw new UnauthorizedAccessException(VmoResources.VMO61008);
		}
		DeviceDisabledInfo[] array = (from d in DeviceDisabledData.QueryDisabledDevicesAdmin(requestContext)
			select new DeviceDisabledInfo
			{
				RecorderId = d.RecorderId,
				HardwareId = d.HardwareId,
				DeviceId = d.Id,
				DeviceType = d.Type,
				DeviceName = d.Name
			}).ToArray();
		DeviceDisabledInfo[] array2 = array;
		foreach (DeviceDisabledInfo deviceDisabledInfo in array2)
		{
			if (RegisteredDeviceType.IsAudioInput(deviceDisabledInfo.DeviceType))
			{
				deviceDisabledInfo.DeviceType = DeviceType.Microphone.ToString();
			}
			else if (RegisteredDeviceType.IsAudioOutput(deviceDisabledInfo.DeviceType))
			{
				deviceDisabledInfo.DeviceType = DeviceType.Speaker.ToString();
			}
		}
		return array;
	}

	public static Change[] QueryChanges(IGeminiRequestContext requestContext, DateTime changesFrom)
	{
		changesFrom = changesFrom.ToUniversalTime();
		List<Change> list = new List<Change>();
		foreach (RecorderModifiedData item in RecorderData.QueryRecordersChangedAfter(requestContext, changesFrom))
		{
			Change change = new Change();
			change.ChangeType = new ChangeType();
			change.ChangeType.Recorder = true;
			change.Id = item.Id.ToString();
			change.Time = item.LastModified;
			list.Add(change);
		}
		foreach (HardwareModifiedData item2 in HardwareData.QueryHardwareChangedAfter(requestContext, changesFrom))
		{
			Change change2 = new Change();
			change2.ChangeType = new ChangeType();
			change2.ChangeType.Hardware = true;
			change2.Id = item2.Id.ToString();
			change2.Time = item2.LastModified;
			list.Add(change2);
		}
		return list.OrderBy((Change c) => c.Time).ToArray();
	}

	public static RecorderInfo GetRecorderInfoOnly(IGeminiRequestContext requestContext, Guid recorderId, bool isPublicRequest)
	{
		RecorderData recorder = RecorderData.QueryActiveRecorder(requestContext, recorderId);
		RecorderInfo recorderInfo = new RecorderInfo();
		string systemSetting = ServerInformation.GetSystemSetting(requestContext, SettingList.LocalIPRanges);
		if (isPublicRequest && LocalIPRangeXmlFormatter.IsInRange(requestContext.RemoteIPAddress, systemSetting))
		{
			isPublicRequest = false;
		}
		FillRecorderInfo(recorderInfo, recorder, isPublicRequest);
		return recorderInfo;
	}
}
