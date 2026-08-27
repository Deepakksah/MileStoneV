using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ConfigurationCommandCredentialsClient : ClientBase<IConfigurationCommandCredentials>, IConfigurationCommandCredentials
{
	public ConfigurationCommandCredentialsClient()
	{
	}

	public ConfigurationCommandCredentialsClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ConfigurationCommandCredentialsClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationCommandCredentialsClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationCommandCredentialsClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool CreateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return base.Channel.CreateAlarmDefinition(alarmDefinition);
	}

	public bool UpdateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return base.Channel.UpdateAlarmDefinition(alarmDefinition);
	}

	public bool DeleteAlarmDefinition(Guid alarmDefinitionId)
	{
		return base.Channel.DeleteAlarmDefinition(alarmDefinitionId);
	}

	public AlarmDefinition GetAlarmDefinition(Guid alarmDefinitionId)
	{
		return base.Channel.GetAlarmDefinition(alarmDefinitionId);
	}

	public AlarmDefinition[] GetAlarmDefinitions()
	{
		return base.Channel.GetAlarmDefinitions();
	}

	public bool CreateTimeProfileDefinition(TimeProfileDefinition timeprofileDefinition)
	{
		return base.Channel.CreateTimeProfileDefinition(timeprofileDefinition);
	}

	public bool UpdateTimeProfileDefinition(TimeProfileDefinition timeprofileDefinition)
	{
		return base.Channel.UpdateTimeProfileDefinition(timeprofileDefinition);
	}

	public bool DeleteTimeProfileDefinition(Guid timeprofileDefinitionId)
	{
		return base.Channel.DeleteTimeProfileDefinition(timeprofileDefinitionId);
	}

	public TimeProfileDefinition GetTimeProfileDefinition(Guid timeprofileDefinitionId)
	{
		return base.Channel.GetTimeProfileDefinition(timeprofileDefinitionId);
	}

	public TimeProfileDefinition[] GetTimeProfileDefinitions()
	{
		return base.Channel.GetTimeProfileDefinitions();
	}

	public GeneralSettings GetGeneralSettings()
	{
		return base.Channel.GetGeneralSettings();
	}

	public bool SetGeneralSettings(GeneralSettings generalSettings)
	{
		return base.Channel.SetGeneralSettings(generalSettings);
	}

	public bool ResetGeneralSettings()
	{
		return base.Channel.ResetGeneralSettings();
	}

	public GetEventRetentionResp GetEventRetention(GetEventRetentionParams getEventRetentionParams)
	{
		return base.Channel.GetEventRetention(getEventRetentionParams);
	}

	public GetDefaultEventRetentionResp GetDefaultEventRetention(GetDefaultEventRetentionParams getEventRetentionParams)
	{
		return base.Channel.GetDefaultEventRetention(getEventRetentionParams);
	}

	public SetEventRetentionResp SetEventRetention(SetEventRetentionParams setEventRetentionParams)
	{
		return base.Channel.SetEventRetention(setEventRetentionParams);
	}

	public SoundConfigurationItem[] GetSoundConfiguration()
	{
		return base.Channel.GetSoundConfiguration();
	}

	public bool UploadSound(SoundDataItem soundData)
	{
		return base.Channel.UploadSound(soundData);
	}

	public bool DeleteSound(Guid id)
	{
		return base.Channel.DeleteSound(id);
	}

	public SoundDataItem DownloadSound(Guid id)
	{
		return base.Channel.DownloadSound(id);
	}

	public EventType[] GetEventTypes(string culture)
	{
		return base.Channel.GetEventTypes(culture);
	}

	public EventTypeGroup[] GetEventTypeGroups(string culture)
	{
		return base.Channel.GetEventTypeGroups(culture);
	}

	public EventType[] GetEventTypesForUser(Guid sourceId, string languageId)
	{
		return base.Channel.GetEventTypesForUser(sourceId, languageId);
	}

	public DisableAlarmItem[] GetDisableAlarmItems(string languageId)
	{
		return base.Channel.GetDisableAlarmItems(languageId);
	}

	public void AddDisableAlarmItems(DisableAlarmItem[] disableAlarmsItems)
	{
		base.Channel.AddDisableAlarmItems(disableAlarmsItems);
	}

	public void ModifyDisableAlarmItem(DisableAlarmItem disableAlarmsItem)
	{
		base.Channel.ModifyDisableAlarmItem(disableAlarmsItem);
	}

	public void DeleteDisableAlarmItem(Guid sourceId, Guid disableAlarmsItemId)
	{
		base.Channel.DeleteDisableAlarmItem(sourceId, disableAlarmsItemId);
	}

	public SuppressStatusItem[] GetSuppressStatusItems()
	{
		return base.Channel.GetSuppressStatusItems();
	}

	public void AddSuppressStatusItems(SuppressStatusItem[] suppressStatusItems)
	{
		base.Channel.AddSuppressStatusItems(suppressStatusItems);
	}

	public void ModifySuppressStatusItem(SuppressStatusItem suppressStatusItem)
	{
		base.Channel.ModifySuppressStatusItem(suppressStatusItem);
	}

	public void DeleteSuppressStatusItem(Guid sourceId, Guid suppressStatusItemId)
	{
		base.Channel.DeleteSuppressStatusItem(sourceId, suppressStatusItemId);
	}

	public void SetPrioritySoundMappings(Tuple<Guid, Guid>[] mappings)
	{
		base.Channel.SetPrioritySoundMappings(mappings);
	}

	public Tuple<Guid, Guid>[] GetPrioritySoundMappings()
	{
		return base.Channel.GetPrioritySoundMappings();
	}

	public AlarmDataSettings GetAlarmDataSettings()
	{
		return base.Channel.GetAlarmDataSettings();
	}

	public void SetAlarmDataSettings(AlarmDataSettings alarmDataSettings)
	{
		base.Channel.SetAlarmDataSettings(alarmDataSettings);
	}

	public KeyValuePair<string, string>[] GetActiveProperties(string key)
	{
		return base.Channel.GetActiveProperties(key);
	}

	public HeartbeatResp ClientSessionHeartbeat(HeartbeatParams heartbeatParams)
	{
		return base.Channel.ClientSessionHeartbeat(heartbeatParams);
	}

	public UpdateEventsExpirationResp UpdateEventsExpiration(UpdateEventsExpirationParams updateEventsExpirationParams)
	{
		return base.Channel.UpdateEventsExpiration(updateEventsExpirationParams);
	}

	public LookupItemResp LookupItem(LookupItemParams lookupItemParams)
	{
		return base.Channel.LookupItem(lookupItemParams);
	}
}
