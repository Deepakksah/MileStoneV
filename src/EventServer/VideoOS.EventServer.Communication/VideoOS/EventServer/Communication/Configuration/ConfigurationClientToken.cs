using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.Threading;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Configuration;

public class ConfigurationClientToken : ConfigurationClient
{
	private readonly ClientPool<ConfigurationCommandTokenClient> _clientPool;

	public ConfigurationClientToken(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/ConfigurationServiceToken"))
	{
		_clientPool = new ClientPool<ConfigurationCommandTokenClient>(CreateClient, 2);
	}

	private ConfigurationCommandTokenClient CreateClient()
	{
		ConfigurationCommandTokenClient configurationCommandTokenClient = new ConfigurationCommandTokenClient(BindingFactory.CreateBasicHttpBindingToken(_uri.IsHttps()), new EndpointAddress(_uri));
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(configurationCommandTokenClient);
		configurationCommandTokenClient.EnsureLargeObjectGraphsAllowed<ConfigurationCommandTokenClient, IConfigurationCommandToken>();
		return configurationCommandTokenClient;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	public override bool CreateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.CreateAlarmDefinition(GetToken(), alarmDefinition));
	}

	public override bool UpdateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.UpdateAlarmDefinition(GetToken(), alarmDefinition));
	}

	public override bool DeleteAlarmDefinition(Guid alarmDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.DeleteAlarmDefinition(GetToken(), alarmDefinitionId));
	}

	public override AlarmDefinition GetAlarmDefinition(Guid alarmDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetAlarmDefinition(GetToken(), alarmDefinitionId));
	}

	public override AlarmDefinition[] GetAlarmDefinitions()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetAlarmDefinitions(GetToken()));
	}

	public override bool CreateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.CreateTimeProfileDefinition(GetToken(), timeProfileDefinition));
	}

	public override bool UpdateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.UpdateTimeProfileDefinition(GetToken(), timeProfileDefinition));
	}

	public override bool DeleteTimeProfileDefinition(Guid timeprofileDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.DeleteTimeProfileDefinition(GetToken(), timeprofileDefinitionId));
	}

	public override TimeProfileDefinition GetTimeProfileDefinition(Guid timeprofileDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetTimeProfileDefinition(GetToken(), timeprofileDefinitionId));
	}

	public override TimeProfileDefinition[] GetTimeProfileDefinitions()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetTimeProfileDefinitions(GetToken()));
	}

	public override GeneralSettings GetGeneralSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetGeneralSettings(GetToken()));
	}

	public override bool SetGeneralSettings(GeneralSettings generalSettings)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.SetGeneralSettings(GetToken(), generalSettings));
	}

	public override bool ResetGeneralSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.ResetGeneralSettings(GetToken()));
	}

	public override GetEventRetentionResp GetEventRetention(GetEventRetentionParams getEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetEventRetention(GetToken(), getEventRetentionParams));
	}

	public override GetDefaultEventRetentionResp GetDefaultEventRetention(GetDefaultEventRetentionParams getDefaultEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetDefaultEventRetention(GetToken(), getDefaultEventRetentionParams));
	}

	public override SetEventRetentionResp SetEventRetention(SetEventRetentionParams setEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.SetEventRetention(GetToken(), setEventRetentionParams));
	}

	public override SoundConfigurationItem[] GetSoundConfiguration()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetSoundConfiguration(GetToken()));
	}

	public override bool UploadSound(SoundDataItem soundData)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.UploadSound(GetToken(), soundData));
	}

	public override bool DeleteSound(Guid id)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.DeleteSound(GetToken(), id));
	}

	public override SoundDataItem DownloadSound(Guid id)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.DownloadSound(GetToken(), id));
	}

	public override EventType[] GetEventTypes()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetEventTypes(GetToken(), Thread.CurrentThread.CurrentUICulture.Name));
	}

	public override EventType[] GetEventTypesTranslated(CultureInfo culture)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetEventTypes(GetToken(), culture.Name));
	}

	public override EventTypeGroup[] GetEventTypeGroups()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetEventTypeGroups(GetToken(), Thread.CurrentThread.CurrentUICulture.Name));
	}

	public override EventType[] GetEventTypesForUser(Guid sourceId, string languageId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetEventTypesForUser(GetToken(), sourceId, languageId));
	}

	public override DisableAlarmItem[] GetDisableAlarmItems(string languageId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetDisableAlarmItems(GetToken(), languageId));
	}

	public override void AddDisableAlarmItems(DisableAlarmItem[] disableAlarmItems)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.AddDisableAlarmItems(GetToken(), disableAlarmItems);
		});
	}

	public override void ModifyDisableAlarmItem(DisableAlarmItem disableAlarmItem)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.ModifyDisableAlarmItem(GetToken(), disableAlarmItem);
		});
	}

	public override void DeleteDisableAlarmItem(Guid sourceId, Guid disableAlarmItemId)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.DeleteDisableAlarmItem(GetToken(), sourceId, disableAlarmItemId);
		});
	}

	public override SuppressStatusItem[] GetSuppressStatusItems()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetSuppressStatusItems(GetToken()));
	}

	public override void AddSuppressStatusItems(SuppressStatusItem[] suppressStatusItems)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.AddSuppressStatusItems(GetToken(), suppressStatusItems);
		});
	}

	public override void ModifySuppressStatusItem(SuppressStatusItem suppressStatusItem)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.ModifySuppressStatusItem(GetToken(), suppressStatusItem);
		});
	}

	public override void DeleteSuppressStatusItem(Guid sourceId, Guid suppressStatusItemId)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.DeleteSuppressStatusItem(GetToken(), sourceId, suppressStatusItemId);
		});
	}

	public override Tuple<Guid, Guid>[] GetPrioritySoundMappings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetPrioritySoundMappings(GetToken()));
	}

	public override void SetPrioritySoundMappings(Tuple<Guid, Guid>[] mappings)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.SetPrioritySoundMappings(GetToken(), mappings);
		});
	}

	public override AlarmDataSettings GetAlarmDataSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetAlarmDataSettings(GetToken()));
	}

	public override void SetAlarmDataSettings(AlarmDataSettings alarmDataSettings)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandTokenClient client)
		{
			client.SetAlarmDataSettings(GetToken(), alarmDataSettings);
		});
	}

	public override KeyValuePair<string, string>[] GetAlarmProperties(string key)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.GetActiveProperties(GetToken(), key));
	}

	public override HeartbeatResp ClientSessionHeartbeat(HeartbeatParams heartbeatParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.ClientSessionHeartbeat(GetToken(), heartbeatParams));
	}

	public override UpdateEventsExpirationResp UpdateEventsExpiration(UpdateEventsExpirationParams updateEventsExpirationParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.UpdateEventsExpiration(GetToken(), updateEventsExpirationParams));
	}

	public override LookupItemResp LookupItem(LookupItemParams lookupItemParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandTokenClient client) => client.LookupItem(GetToken(), lookupItemParams));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
