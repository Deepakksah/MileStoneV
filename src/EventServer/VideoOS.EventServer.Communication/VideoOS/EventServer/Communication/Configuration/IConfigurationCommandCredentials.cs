using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerConfigurationCommandCredentials", ConfigurationName = "VideoOS.EventServer.Communication.Configuration.IConfigurationCommandCredentials", SessionMode = SessionMode.NotAllowed)]
public interface IConfigurationCommandCredentials
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool CreateAlarmDefinition(AlarmDefinition alarmDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UpdateAlarmDefinition(AlarmDefinition alarmDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteAlarmDefinition(Guid alarmDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDefinition GetAlarmDefinition(Guid alarmDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinitions", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinitionsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDefinitionsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDefinition[] GetAlarmDefinitions();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/CreateTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool CreateTimeProfileDefinition(TimeProfileDefinition timeprofileDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UpdateTimeProfileDefinition(TimeProfileDefinition timeprofileDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteTimeProfileDefinition(Guid timeprofileDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	TimeProfileDefinition GetTimeProfileDefinition(Guid timeprofileDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinitions", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinitionsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetTimeProfileDefinitionsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	TimeProfileDefinition[] GetTimeProfileDefinitions();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GeneralSettings GetGeneralSettings();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool SetGeneralSettings(GeneralSettings generalSettings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ResetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ResetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ResetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool ResetGeneralSettings();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GetEventRetentionResp GetEventRetention(GetEventRetentionParams getEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDefaultEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDefaultEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDefaultEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GetDefaultEventRetentionResp GetDefaultEventRetention(GetDefaultEventRetentionParams getEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SetEventRetentionResp SetEventRetention(SetEventRetentionParams setEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSoundConfiguration", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSoundConfigurationResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSoundConfigurationConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SoundConfigurationItem[] GetSoundConfiguration();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UploadSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UploadSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UploadSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UploadSound(SoundDataItem soundData);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteSound(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DownloadSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DownloadSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DownloadSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SoundDataItem DownloadSound(Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypes", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypesResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypesConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventType[] GetEventTypes(string culture);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypeGroups", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypeGroupsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypeGroupsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventTypeGroup[] GetEventTypeGroups(string culture);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypesForUser", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypesForUserResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetEventTypesForUserConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventType[] GetEventTypesForUser(Guid sourceId, string languageId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDisableAlarmItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDisableAlarmItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetDisableAlarmItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	DisableAlarmItem[] GetDisableAlarmItems(string languageId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddDisableAlarmItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddDisableAlarmItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddDisableAlarmItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void AddDisableAlarmItems(DisableAlarmItem[] disableAlarmsItems);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifyDisableAlarmItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifyDisableAlarmItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifyDisableAlarmItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void ModifyDisableAlarmItem(DisableAlarmItem disableAlarmsItem);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteDisableAlarmItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteDisableAlarmItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteDisableAlarmItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void DeleteDisableAlarmItem(Guid sourceId, Guid disableAlarmsItemId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSuppressStatusItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSuppressStatusItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetSuppressStatusItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SuppressStatusItem[] GetSuppressStatusItems();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddSuppressStatusItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddSuppressStatusItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/AddSuppressStatusItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void AddSuppressStatusItems(SuppressStatusItem[] suppressStatusItems);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifySuppressStatusItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifySuppressStatusItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ModifySuppressStatusItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void ModifySuppressStatusItem(SuppressStatusItem suppressStatusItem);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSuppressStatusItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSuppressStatusItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/DeleteSuppressStatusItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void DeleteSuppressStatusItem(Guid sourceId, Guid suppressStatusItemId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetPrioritySoundMappings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetPrioritySoundMappingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetPrioritySoundMappingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void SetPrioritySoundMappings(Tuple<Guid, Guid>[] mappings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetPrioritySoundMappings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetPrioritySoundMappingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetPrioritySoundMappingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	Tuple<Guid, Guid>[] GetPrioritySoundMappings();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDataSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDataSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetAlarmDataSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDataSettings GetAlarmDataSettings();

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetAlarmDataSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetAlarmDataSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/SetAlarmDataSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void SetAlarmDataSettings(AlarmDataSettings alarmDataSettings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetActiveProperties", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetActivePropertiesResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/GetActivePropertiesConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	KeyValuePair<string, string>[] GetActiveProperties(string key);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ClientSessionHeartbeat", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ClientSessionHeartbeatResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/ClientSessionHeartbeatConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	HeartbeatResp ClientSessionHeartbeat(HeartbeatParams heartbeatParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateEventsExpiration", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateEventsExpirationResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/UpdateEventsExpirationConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	UpdateEventsExpirationResp UpdateEventsExpiration(UpdateEventsExpirationParams updateEventsExpirationParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/LookupItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/LookupItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandCredentials/IConfigurationCommandCredentials/LookupItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	LookupItemResp LookupItem(LookupItemParams lookupItemParams);
}
