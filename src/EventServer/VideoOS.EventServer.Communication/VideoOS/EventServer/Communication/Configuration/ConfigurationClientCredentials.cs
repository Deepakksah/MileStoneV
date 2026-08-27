using System;
using System.Collections.Generic;
using System.Globalization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.Configuration;

public class ConfigurationClientCredentials : ConfigurationClient
{
	private readonly ClientPool<ConfigurationCommandCredentialsClient> _clientPool;

	public ConfigurationClientCredentials(ServerId serverId, string eventServerAddress, Version eventServerVersion)
		: base(serverId, eventServerAddress, eventServerVersion, new Uri(new Uri(eventServerAddress), "Central/ConfigurationServiceCredentials"))
	{
		_clientPool = new ClientPool<ConfigurationCommandCredentialsClient>(CreateClient, 2);
	}

	private ConfigurationCommandCredentialsClient CreateClient()
	{
		ConfigurationCommandCredentialsClient configurationCommandCredentialsClient = new ConfigurationCommandCredentialsClient(BindingFactory.CreateWSHttpBindingCredentials(_uri.IsHttps()), new EndpointAddress(identity: AbstractClient.CreateSpnIdentity(SpnFactory.GetSpn(_uri)), uri: _uri, addressHeaders: Array.Empty<AddressHeader>()));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		configurationCommandCredentialsClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(configurationCommandCredentialsClient);
		configurationCommandCredentialsClient.EnsureLargeObjectGraphsAllowed<ConfigurationCommandCredentialsClient, IConfigurationCommandCredentials>();
		return configurationCommandCredentialsClient;
	}

	public override bool CreateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.CreateAlarmDefinition(alarmDefinition));
	}

	public override bool UpdateAlarmDefinition(AlarmDefinition alarmDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.UpdateAlarmDefinition(alarmDefinition));
	}

	public override bool DeleteAlarmDefinition(Guid alarmDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.DeleteAlarmDefinition(alarmDefinitionId));
	}

	public override AlarmDefinition GetAlarmDefinition(Guid alarmDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetAlarmDefinition(alarmDefinitionId));
	}

	public override AlarmDefinition[] GetAlarmDefinitions()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetAlarmDefinitions());
	}

	public override bool CreateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.CreateTimeProfileDefinition(timeProfileDefinition));
	}

	public override bool UpdateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.UpdateTimeProfileDefinition(timeProfileDefinition));
	}

	public override bool DeleteTimeProfileDefinition(Guid timeProfileDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.DeleteTimeProfileDefinition(timeProfileDefinitionId));
	}

	public override TimeProfileDefinition GetTimeProfileDefinition(Guid timeProfileDefinitionId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetTimeProfileDefinition(timeProfileDefinitionId));
	}

	public override TimeProfileDefinition[] GetTimeProfileDefinitions()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetTimeProfileDefinitions());
	}

	public override GeneralSettings GetGeneralSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetGeneralSettings());
	}

	public override bool SetGeneralSettings(GeneralSettings generalSettings)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.SetGeneralSettings(generalSettings));
	}

	public override bool ResetGeneralSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.ResetGeneralSettings());
	}

	public override GetEventRetentionResp GetEventRetention(GetEventRetentionParams getEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetEventRetention(getEventRetentionParams));
	}

	public override GetDefaultEventRetentionResp GetDefaultEventRetention(GetDefaultEventRetentionParams getDefaultEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetDefaultEventRetention(getDefaultEventRetentionParams));
	}

	public override SetEventRetentionResp SetEventRetention(SetEventRetentionParams setEventRetentionParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.SetEventRetention(setEventRetentionParams));
	}

	public override SoundConfigurationItem[] GetSoundConfiguration()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetSoundConfiguration());
	}

	public override bool UploadSound(SoundDataItem soundData)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.UploadSound(soundData));
	}

	public override bool DeleteSound(Guid id)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.DeleteSound(id));
	}

	public override SoundDataItem DownloadSound(Guid id)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.DownloadSound(id));
	}

	public override EventType[] GetEventTypes()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetEventTypes(Thread.CurrentThread.CurrentUICulture.Name));
	}

	public override EventType[] GetEventTypesTranslated(CultureInfo culture)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetEventTypes(culture.Name));
	}

	public override EventTypeGroup[] GetEventTypeGroups()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetEventTypeGroups(Thread.CurrentThread.CurrentUICulture.Name));
	}

	public override EventType[] GetEventTypesForUser(Guid sourceId, string languageId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetEventTypesForUser(sourceId, languageId));
	}

	public override DisableAlarmItem[] GetDisableAlarmItems(string languageId)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetDisableAlarmItems(languageId));
	}

	public override void AddDisableAlarmItems(DisableAlarmItem[] disableAlarmItems)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.AddDisableAlarmItems(disableAlarmItems);
		});
	}

	public override void ModifyDisableAlarmItem(DisableAlarmItem disableAlarmItem)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.ModifyDisableAlarmItem(disableAlarmItem);
		});
	}

	public override void DeleteDisableAlarmItem(Guid sourceId, Guid disableAlarmItemId)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.DeleteDisableAlarmItem(sourceId, disableAlarmItemId);
		});
	}

	public override SuppressStatusItem[] GetSuppressStatusItems()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetSuppressStatusItems());
	}

	public override void AddSuppressStatusItems(SuppressStatusItem[] suppressStatusItems)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.AddSuppressStatusItems(suppressStatusItems);
		});
	}

	public override void ModifySuppressStatusItem(SuppressStatusItem suppressStatusItem)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.ModifySuppressStatusItem(suppressStatusItem);
		});
	}

	public override void DeleteSuppressStatusItem(Guid sourceId, Guid suppressStatusItemId)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.DeleteSuppressStatusItem(sourceId, suppressStatusItemId);
		});
	}

	public override Tuple<Guid, Guid>[] GetPrioritySoundMappings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetPrioritySoundMappings());
	}

	public override void SetPrioritySoundMappings(Tuple<Guid, Guid>[] mappings)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.SetPrioritySoundMappings(mappings);
		});
	}

	public override AlarmDataSettings GetAlarmDataSettings()
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetAlarmDataSettings());
	}

	public override void SetAlarmDataSettings(AlarmDataSettings alarmDataSettings)
	{
		_clientPool.CallWithRetry(delegate(ConfigurationCommandCredentialsClient client)
		{
			client.SetAlarmDataSettings(alarmDataSettings);
		});
	}

	public override KeyValuePair<string, string>[] GetAlarmProperties(string key)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.GetActiveProperties(key));
	}

	public override HeartbeatResp ClientSessionHeartbeat(HeartbeatParams heartbeatParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.ClientSessionHeartbeat(heartbeatParams));
	}

	public override UpdateEventsExpirationResp UpdateEventsExpiration(UpdateEventsExpirationParams updateEventsExpirationParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.UpdateEventsExpiration(updateEventsExpirationParams));
	}

	public override LookupItemResp LookupItem(LookupItemParams lookupItemParams)
	{
		return _clientPool.CallWithRetry((ConfigurationCommandCredentialsClient client) => client.LookupItem(lookupItemParams));
	}

	public override void Close()
	{
		base.Close();
		_clientPool.Dispose();
	}
}
