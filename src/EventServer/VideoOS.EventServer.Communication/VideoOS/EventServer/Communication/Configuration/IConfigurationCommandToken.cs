using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ServiceModel;

namespace VideoOS.EventServer.Communication.Configuration;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/CentralServerConfigurationCommandToken", ConfigurationName = "VideoOS.EventServer.Communication.Configuration.IConfigurationCommandToken", SessionMode = SessionMode.NotAllowed)]
public interface IConfigurationCommandToken
{
	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool CreateAlarmDefinition(string token, AlarmDefinition alarmDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UpdateAlarmDefinition(string token, AlarmDefinition alarmDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteAlarmDefinition(string token, Guid alarmDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDefinition GetAlarmDefinition(string token, Guid alarmDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinitions", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinitionsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDefinitionsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDefinition[] GetAlarmDefinitions(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/CreateTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool CreateTimeProfileDefinition(string token, TimeProfileDefinition timeprofileDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UpdateTimeProfileDefinition(string token, TimeProfileDefinition timeprofileDefinition);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteTimeProfileDefinition(string token, Guid timeprofileDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinition", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinitionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinitionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	TimeProfileDefinition GetTimeProfileDefinition(string token, Guid timeprofileDefinitionId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinitions", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinitionsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetTimeProfileDefinitionsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	TimeProfileDefinition[] GetTimeProfileDefinitions(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GeneralSettings GetGeneralSettings(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool SetGeneralSettings(string token, GeneralSettings generalSettings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ResetGeneralSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ResetGeneralSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ResetGeneralSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool ResetGeneralSettings(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GetEventRetentionResp GetEventRetention(string token, GetEventRetentionParams getEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDefaultEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDefaultEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDefaultEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	GetDefaultEventRetentionResp GetDefaultEventRetention(string token, GetDefaultEventRetentionParams getEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetEventRetention", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetEventRetentionResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetEventRetentionConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SetEventRetentionResp SetEventRetention(string token, SetEventRetentionParams setEventRetentionParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSoundConfiguration", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSoundConfigurationResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSoundConfigurationConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SoundConfigurationItem[] GetSoundConfiguration(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UploadSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UploadSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UploadSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool UploadSound(string token, SoundDataItem soundData);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	bool DeleteSound(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DownloadSound", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DownloadSoundResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DownloadSoundConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SoundDataItem DownloadSound(string token, Guid id);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypes", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypesResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypesConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventType[] GetEventTypes(string token, string culture);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypeGroups", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypeGroupsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypeGroupsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventTypeGroup[] GetEventTypeGroups(string token, string culture);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypesForUser", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypesForUserResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetEventTypesForUserConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	EventType[] GetEventTypesForUser(string token, Guid sourceId, string languageId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDisableAlarmItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDisableAlarmItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetDisableAlarmItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	DisableAlarmItem[] GetDisableAlarmItems(string token, string languageId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddDisableAlarmItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddDisableAlarmItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddDisableAlarmItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void AddDisableAlarmItems(string token, DisableAlarmItem[] disableAlarmsItems);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifyDisableAlarmItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifyDisableAlarmItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifyDisableAlarmItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void ModifyDisableAlarmItem(string token, DisableAlarmItem disableAlarmsItem);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteDisableAlarmItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteDisableAlarmItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteDisableAlarmItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void DeleteDisableAlarmItem(string token, Guid sourceId, Guid disableAlarmsItemId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSuppressStatusItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSuppressStatusItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetSuppressStatusItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	SuppressStatusItem[] GetSuppressStatusItems(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddSuppressStatusItems", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddSuppressStatusItemsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/AddSuppressStatusItemsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void AddSuppressStatusItems(string token, SuppressStatusItem[] suppressStatusItems);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifySuppressStatusItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifySuppressStatusItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ModifySuppressStatusItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void ModifySuppressStatusItem(string token, SuppressStatusItem suppressStatusItem);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSuppressStatusItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSuppressStatusItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/DeleteSuppressStatusItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void DeleteSuppressStatusItem(string token, Guid sourceId, Guid suppressStatusItemId);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetPrioritySoundMappings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetPrioritySoundMappingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetPrioritySoundMappingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void SetPrioritySoundMappings(string token, Tuple<Guid, Guid>[] mappings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetPrioritySoundMappings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetPrioritySoundMappingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetPrioritySoundMappingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	Tuple<Guid, Guid>[] GetPrioritySoundMappings(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDataSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDataSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetAlarmDataSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	AlarmDataSettings GetAlarmDataSettings(string token);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetActiveProperties", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetActivePropertiesResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/GetActivePropertiesConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	KeyValuePair<string, string>[] GetActiveProperties(string token, string key);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetAlarmDataSettings", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetAlarmDataSettingsResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/SetAlarmDataSettingsConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	void SetAlarmDataSettings(string token, AlarmDataSettings alarmDataSettings);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ClientSessionHeartbeat", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ClientSessionHeartbeatResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/ClientSessionHeartbeatConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	HeartbeatResp ClientSessionHeartbeat(string token, HeartbeatParams heartbeatParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateEventsExpiration", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateEventsExpirationResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/UpdateEventsExpirationConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	UpdateEventsExpirationResp UpdateEventsExpiration(string token, UpdateEventsExpirationParams updateEventsExpirationParams);

	[OperationContract(Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/LookupItem", ReplyAction = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/LookupItemResponse")]
	[FaultContract(typeof(ConfigurationServiceFault), Action = "http://videoos.net/2/CentralServerConfigurationCommandToken/IConfigurationCommandToken/LookupItemConfigurationServiceFaultFault", Name = "ConfigurationServiceFault", Namespace = "http://videoos.net/2/CentralServerConfigurationCommand")]
	LookupItemResp LookupItem(string token, LookupItemParams lookupItemParams);
}
