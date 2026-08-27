using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.EventServer.Communication.Configuration;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ConfigurationCommandTokenClient : ClientBase<IConfigurationCommandToken>, IConfigurationCommandToken
{
	public ConfigurationCommandTokenClient()
	{
	}

	public ConfigurationCommandTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ConfigurationCommandTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationCommandTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ConfigurationCommandTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public bool CreateAlarmDefinition(string token, AlarmDefinition alarmDefinition)
	{
		return base.Channel.CreateAlarmDefinition(token, alarmDefinition);
	}

	public bool UpdateAlarmDefinition(string token, AlarmDefinition alarmDefinition)
	{
		return base.Channel.UpdateAlarmDefinition(token, alarmDefinition);
	}

	public bool DeleteAlarmDefinition(string token, Guid alarmDefinitionId)
	{
		return base.Channel.DeleteAlarmDefinition(token, alarmDefinitionId);
	}

	public AlarmDefinition GetAlarmDefinition(string token, Guid alarmDefinitionId)
	{
		return base.Channel.GetAlarmDefinition(token, alarmDefinitionId);
	}

	public AlarmDefinition[] GetAlarmDefinitions(string token)
	{
		return base.Channel.GetAlarmDefinitions(token);
	}

	public bool CreateTimeProfileDefinition(string token, TimeProfileDefinition timeprofileDefinition)
	{
		return base.Channel.CreateTimeProfileDefinition(token, timeprofileDefinition);
	}

	public bool UpdateTimeProfileDefinition(string token, TimeProfileDefinition timeprofileDefinition)
	{
		return base.Channel.UpdateTimeProfileDefinition(token, timeprofileDefinition);
	}

	public bool DeleteTimeProfileDefinition(string token, Guid timeprofileDefinitionId)
	{
		return base.Channel.DeleteTimeProfileDefinition(token, timeprofileDefinitionId);
	}

	public TimeProfileDefinition GetTimeProfileDefinition(string token, Guid timeprofileDefinitionId)
	{
		return base.Channel.GetTimeProfileDefinition(token, timeprofileDefinitionId);
	}

	public TimeProfileDefinition[] GetTimeProfileDefinitions(string token)
	{
		return base.Channel.GetTimeProfileDefinitions(token);
	}

	public GeneralSettings GetGeneralSettings(string token)
	{
		return base.Channel.GetGeneralSettings(token);
	}

	public bool SetGeneralSettings(string token, GeneralSettings generalSettings)
	{
		return base.Channel.SetGeneralSettings(token, generalSettings);
	}

	public bool ResetGeneralSettings(string token)
	{
		return base.Channel.ResetGeneralSettings(token);
	}

	public GetEventRetentionResp GetEventRetention(string token, GetEventRetentionParams getEventRetentionParams)
	{
		return base.Channel.GetEventRetention(token, getEventRetentionParams);
	}

	public GetDefaultEventRetentionResp GetDefaultEventRetention(string token, GetDefaultEventRetentionParams getEventRetentionParams)
	{
		return base.Channel.GetDefaultEventRetention(token, getEventRetentionParams);
	}

	public SetEventRetentionResp SetEventRetention(string token, SetEventRetentionParams setEventRetentionParams)
	{
		return base.Channel.SetEventRetention(token, setEventRetentionParams);
	}

	public SoundConfigurationItem[] GetSoundConfiguration(string token)
	{
		return base.Channel.GetSoundConfiguration(token);
	}

	public bool UploadSound(string token, SoundDataItem soundData)
	{
		return base.Channel.UploadSound(token, soundData);
	}

	public bool DeleteSound(string token, Guid id)
	{
		return base.Channel.DeleteSound(token, id);
	}

	public SoundDataItem DownloadSound(string token, Guid id)
	{
		return base.Channel.DownloadSound(token, id);
	}

	public EventType[] GetEventTypes(string token, string culture)
	{
		return base.Channel.GetEventTypes(token, culture);
	}

	public EventTypeGroup[] GetEventTypeGroups(string token, string culture)
	{
		return base.Channel.GetEventTypeGroups(token, culture);
	}

	public EventType[] GetEventTypesForUser(string token, Guid sourceId, string languageId)
	{
		return base.Channel.GetEventTypesForUser(token, sourceId, languageId);
	}

	public DisableAlarmItem[] GetDisableAlarmItems(string token, string languageId)
	{
		return base.Channel.GetDisableAlarmItems(token, languageId);
	}

	public void AddDisableAlarmItems(string token, DisableAlarmItem[] disableAlarmsItems)
	{
		base.Channel.AddDisableAlarmItems(token, disableAlarmsItems);
	}

	public void ModifyDisableAlarmItem(string token, DisableAlarmItem disableAlarmsItem)
	{
		base.Channel.ModifyDisableAlarmItem(token, disableAlarmsItem);
	}

	public void DeleteDisableAlarmItem(string token, Guid sourceId, Guid disableAlarmsItemId)
	{
		base.Channel.DeleteDisableAlarmItem(token, sourceId, disableAlarmsItemId);
	}

	public SuppressStatusItem[] GetSuppressStatusItems(string token)
	{
		return base.Channel.GetSuppressStatusItems(token);
	}

	public void AddSuppressStatusItems(string token, SuppressStatusItem[] suppressStatusItems)
	{
		base.Channel.AddSuppressStatusItems(token, suppressStatusItems);
	}

	public void ModifySuppressStatusItem(string token, SuppressStatusItem suppressStatusItem)
	{
		base.Channel.ModifySuppressStatusItem(token, suppressStatusItem);
	}

	public void DeleteSuppressStatusItem(string token, Guid sourceId, Guid suppressStatusItemId)
	{
		base.Channel.DeleteSuppressStatusItem(token, sourceId, suppressStatusItemId);
	}

	public void SetPrioritySoundMappings(string token, Tuple<Guid, Guid>[] mappings)
	{
		base.Channel.SetPrioritySoundMappings(token, mappings);
	}

	public Tuple<Guid, Guid>[] GetPrioritySoundMappings(string token)
	{
		return base.Channel.GetPrioritySoundMappings(token);
	}

	public AlarmDataSettings GetAlarmDataSettings(string token)
	{
		return base.Channel.GetAlarmDataSettings(token);
	}

	public KeyValuePair<string, string>[] GetActiveProperties(string token, string key)
	{
		return base.Channel.GetActiveProperties(token, key);
	}

	public void SetAlarmDataSettings(string token, AlarmDataSettings alarmDataSettings)
	{
		base.Channel.SetAlarmDataSettings(token, alarmDataSettings);
	}

	public HeartbeatResp ClientSessionHeartbeat(string token, HeartbeatParams heartbeatParams)
	{
		return base.Channel.ClientSessionHeartbeat(token, heartbeatParams);
	}

	public UpdateEventsExpirationResp UpdateEventsExpiration(string token, UpdateEventsExpirationParams updateEventsExpirationParams)
	{
		return base.Channel.UpdateEventsExpiration(token, updateEventsExpirationParams);
	}

	public LookupItemResp LookupItem(string token, LookupItemParams lookupItemParams)
	{
		return base.Channel.LookupItem(token, lookupItemParams);
	}
}
