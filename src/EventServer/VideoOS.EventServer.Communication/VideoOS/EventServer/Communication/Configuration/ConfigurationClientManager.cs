using System;
using System.Collections.Generic;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Configuration;

public class ConfigurationClientManager
{
	public delegate void FaultExceptionHandler(Exception error, int requestId);

	public delegate void CreateAlarmDefinitionHandler(int requestId, bool success);

	public delegate void UpdateAlarmDefinitionHandler(int requestId, bool success);

	public delegate void DeleteAlarmDefinitionHandler(int requestId, bool success);

	public delegate void GetAlarmDefinitionHandler(int requestId, AlarmDefinition alarmDefinition);

	public delegate void GetAlarmDefinitionsHandler(int requestId, AlarmDefinition[] alarmDefinitions);

	public delegate void CreateTimeProfileDefinitionHandler(int requestId, bool success);

	public delegate void UpdateTimeProfileDefinitionHandler(int requestId, bool success);

	public delegate void DeleteTimeProfileDefinitionHandler(int requestId, bool success);

	public delegate void GetTimeProfileDefinitionHandler(int requestId, TimeProfileDefinition timeProfileDefinition);

	public delegate void GetTimeProfileDefinitionsHandler(int requestId, TimeProfileDefinition[] timeProfileDefinitions);

	public delegate void GetGeneralSettingsHandler(int requestId, GeneralSettings generalSettings);

	public delegate void SetGeneralSettingsHandler(int requestId, bool success);

	public delegate void ResetGeneralSettingsHandler(int requestId, bool success);

	public delegate void GetSoundConfigurationHandler(int requestId, SoundConfigurationItem[] soundConfiguration);

	public delegate void UploadSoundHandler(int requestId, bool success);

	public delegate void DeleteSoundHandler(int requestId, bool success);

	public delegate void DownloadSoundHandler(int requestId, SoundDataItem soundData);

	public delegate void GetEventTypesHandler(int requestId, EventType[] eventTypes);

	public delegate void GetEventTypesTranslatedHandler(int requestId, EventType[] eventTypes);

	public delegate void GetEventTypeGroupsHandler(int requestId, EventTypeGroup[] eventTypeGroups);

	public delegate void GetEventTypesForUserHandler(int requestId, EventType[] eventTypes);

	public delegate void GetDisableAlarmItemsHandler(int requestId, DisableAlarmItem[] disableAlarmItems);

	public delegate void AddDisableAlarmItemsHandler(int requestId);

	public delegate void ModifyDisableAlarmItemHandler(int requestId);

	public delegate void DeleteDisableAlarmItemHandler(int requestId);

	public delegate void GetSuppressStatusItemsHandler(int requestId, SuppressStatusItem[] suppressStatusItems);

	public delegate void AddSuppressStatusItemsHandler(int requestId);

	public delegate void ModifySuppressStatusItemHandler(int requestId);

	public delegate void DeleteSuppressStatusItemHandler(int requestId);

	public delegate void GetPrioritySoundMappingsHandler(int requestId, Tuple<Guid, Guid>[] mappings);

	public delegate void SetPrioritySoundMappingsHandler(int requestId);

	public delegate void GetAlarmDataSettingsHandler(int requestId, AlarmDataSettings settings);

	public delegate void SetAlarmDataSettingsHandler(int requestId);

	public delegate void ClientSessionHeartbeatHandler(int requestId, HeartbeatResp response);

	public delegate void UpdateEventsExpirationHandler(int requestId, UpdateEventsExpirationResp response);

	private Dictionary<ServerId, ConfigurationClient> _clients = new Dictionary<ServerId, ConfigurationClient>();

	private object _lockObject = new object();

	private ConfigurationClientFactory _clientFactory = new ConfigurationClientFactory();

	public event FaultExceptionHandler FaultExceptionEvent;

	public event CreateAlarmDefinitionHandler CreateAlarmDefinitionEvent;

	public event UpdateAlarmDefinitionHandler UpdateAlarmDefinitionEvent;

	public event DeleteAlarmDefinitionHandler DeleteAlarmDefinitionEvent;

	public event GetAlarmDefinitionHandler GetAlarmDefinitionEvent;

	public event GetAlarmDefinitionsHandler GetAlarmDefinitionsEvent;

	public event CreateTimeProfileDefinitionHandler CreateTimeProfileDefinitionEvent;

	public event UpdateTimeProfileDefinitionHandler UpdateTimeProfileDefinitionEvent;

	public event DeleteTimeProfileDefinitionHandler DeleteTimeProfileDefinitionEvent;

	public event GetTimeProfileDefinitionHandler GetTimeProfileDefinitionEvent;

	public event GetTimeProfileDefinitionsHandler GetTimeProfileDefinitionsEvent;

	public event GetGeneralSettingsHandler GetGeneralSettingsEvent;

	public event SetGeneralSettingsHandler SetGeneralSettingsEvent;

	public event ResetGeneralSettingsHandler ResetGeneralSettingsEvent;

	public event GetSoundConfigurationHandler GetSoundConfigurationEvent;

	public event UploadSoundHandler UploadSoundEvent;

	public event DeleteSoundHandler DeleteSoundEvent;

	public event DownloadSoundHandler DownloadSoundEvent;

	public event GetEventTypesHandler GetEventTypesEvent;

	public event GetEventTypesTranslatedHandler GetEventTypesTranslatedEvent;

	public event GetEventTypeGroupsHandler GetEventTypeGroupsEvent;

	public event GetEventTypesForUserHandler GetEventTypesForUserEvent;

	public event GetDisableAlarmItemsHandler GetDisableAlarmItemsEvent;

	public event AddDisableAlarmItemsHandler AddDisableAlarmItemsEvent;

	public event ModifyDisableAlarmItemHandler ModifyDisableAlarmItemEvent;

	public event DeleteDisableAlarmItemHandler DeleteDisableAlarmItemEvent;

	public event GetSuppressStatusItemsHandler GetSuppressStatusItemsEvent;

	public event AddSuppressStatusItemsHandler AddSuppressStatusItemsEvent;

	public event ModifySuppressStatusItemHandler ModifySuppressStatusItemEvent;

	public event DeleteSuppressStatusItemHandler DeleteSuppressStatusItemEvent;

	public event GetPrioritySoundMappingsHandler GetPrioritySoundMappingsEvent;

	public event SetPrioritySoundMappingsHandler SetPrioritySoundMappingsEvent;

	public event GetAlarmDataSettingsHandler GetAlarmDataSettingsEvent;

	public event SetAlarmDataSettingsHandler SetAlarmDataSettingsEvent;

	public event ClientSessionHeartbeatHandler ClientSessionHeartbeatEvent;

	public event UpdateEventsExpirationHandler UpdateEventsExpirationEvent;

	public ConfigurationClient GetConfigurationClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			if (!_clients.TryGetValue(serverId, out var _))
			{
				ConfigurationClient configurationClient = CreateConfigurationClient(serverId);
				if (configurationClient == null)
				{
					throw new ServiceNotRegisteredException();
				}
				_clients.Add(serverId, configurationClient);
			}
			return _clients[serverId];
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (ConfigurationClient value in _clients.Values)
			{
				DestroyConfigurationClient(value);
			}
			_clients.Clear();
		}
	}

	private ConfigurationClient CreateConfigurationClient(ServerId serverId)
	{
		VideoOS.Platform.Configuration instance = VideoOS.Platform.Configuration.Instance;
		try
		{
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
			{
				throw new ServiceNotRegisteredException();
			}
			Version eventServerVersion = ServerReceiver.GetEventServerVersion2(registeredServiceUriInfo[0]);
			ConfigurationClient configurationClient = _clientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], eventServerVersion);
			configurationClient.FaultExceptionEvent += FireFaultException;
			configurationClient.CreateAlarmDefinitionEvent += FireCreateAlarmDefinition;
			configurationClient.UpdateAlarmDefinitionEvent += FireUpdateAlarmDefinition;
			configurationClient.DeleteAlarmDefinitionEvent += FireDeleteAlarmDefinition;
			configurationClient.GetAlarmDefinitionEvent += FireGetAlarmDefinition;
			configurationClient.GetAlarmDefinitionsEvent += FireGetAlarmDefinitions;
			configurationClient.CreateTimeProfileDefinitionEvent += FireCreateTimeProfileDefinition;
			configurationClient.UpdateTimeProfileDefinitionEvent += FireUpdateTimeProfileDefinition;
			configurationClient.DeleteTimeProfileDefinitionEvent += FireDeleteTimeProfileDefinition;
			configurationClient.GetTimeProfileDefinitionEvent += FireGetTimeProfileDefinition;
			configurationClient.GetTimeProfileDefinitionsEvent += FireGetTimeProfileDefinitions;
			configurationClient.GetGeneralSettingsEvent += FireGetGeneralSettings;
			configurationClient.SetGeneralSettingsEvent += FireSetGeneralSettings;
			configurationClient.ResetGeneralSettingsEvent += FireResetGeneralSettings;
			configurationClient.GetSoundConfigurationEvent += FireGetSoundConfiguration;
			configurationClient.UploadSoundEvent += FireUploadSound;
			configurationClient.DeleteSoundEvent += FireDeleteSound;
			configurationClient.DownloadSoundEvent += FireDownloadSound;
			configurationClient.GetEventTypesEvent += FireGetEventTypes;
			configurationClient.GetEventTypesTranslatedEvent += FireGetEventTypesTranslated;
			configurationClient.GetEventTypeGroupsEvent += FireGetEventTypeGroups;
			configurationClient.GetEventTypesForUserEvent += FireGetEventTypesForUser;
			configurationClient.GetDisableAlarmItemsEvent += FireGetDisableAlarmItems;
			configurationClient.AddDisableAlarmItemsEvent += FireAddDisableAlarmItems;
			configurationClient.ModifyDisableAlarmItemEvent += FireModifyDisableAlarmItem;
			configurationClient.DeleteDisableAlarmItemEvent += FireDeleteDisableAlarmItem;
			configurationClient.GetSuppressStatusItemsEvent += FireGetSuppressStatusItems;
			configurationClient.AddSuppressStatusItemsEvent += FireAddSuppressStatusItems;
			configurationClient.ModifySuppressStatusItemEvent += FireModifySuppressStatusItem;
			configurationClient.DeleteSuppressStatusItemEvent += FireDeleteSuppressStatusItem;
			configurationClient.GetPrioritySoundMappingsEvent += FireGetPrioritySoundMappings;
			configurationClient.SetPrioritySoundMappingsEvent += FireSetPrioritySoundMappings;
			configurationClient.GetAlarmDataSettingsEvent += FireGetAlarmDataSettings;
			configurationClient.SetAlarmDataSettingsEvent += FireSetAlarmDataSettings;
			configurationClient.ClientSessionHeartbeatEvent += FireClientSessionHeartbeat;
			configurationClient.UpdateEventsExpirationEvent += FireUpdateEventsExpiration;
			return configurationClient;
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
		}
	}

	private void DestroyConfigurationClient(ConfigurationClient configurationClient)
	{
		if (configurationClient != null)
		{
			configurationClient.FaultExceptionEvent -= FireFaultException;
			configurationClient.CreateAlarmDefinitionEvent -= FireCreateAlarmDefinition;
			configurationClient.UpdateAlarmDefinitionEvent -= FireUpdateAlarmDefinition;
			configurationClient.DeleteAlarmDefinitionEvent -= FireDeleteAlarmDefinition;
			configurationClient.GetAlarmDefinitionEvent -= FireGetAlarmDefinition;
			configurationClient.GetAlarmDefinitionsEvent -= FireGetAlarmDefinitions;
			configurationClient.CreateTimeProfileDefinitionEvent -= FireCreateTimeProfileDefinition;
			configurationClient.UpdateTimeProfileDefinitionEvent -= FireUpdateTimeProfileDefinition;
			configurationClient.DeleteTimeProfileDefinitionEvent -= FireDeleteTimeProfileDefinition;
			configurationClient.GetTimeProfileDefinitionEvent -= FireGetTimeProfileDefinition;
			configurationClient.GetTimeProfileDefinitionsEvent -= FireGetTimeProfileDefinitions;
			configurationClient.GetGeneralSettingsEvent -= FireGetGeneralSettings;
			configurationClient.SetGeneralSettingsEvent -= FireSetGeneralSettings;
			configurationClient.ResetGeneralSettingsEvent -= FireResetGeneralSettings;
			configurationClient.GetSoundConfigurationEvent -= FireGetSoundConfiguration;
			configurationClient.UploadSoundEvent -= FireUploadSound;
			configurationClient.DeleteSoundEvent -= FireDeleteSound;
			configurationClient.DownloadSoundEvent -= FireDownloadSound;
			configurationClient.GetEventTypesEvent -= FireGetEventTypes;
			configurationClient.GetEventTypesTranslatedEvent -= FireGetEventTypesTranslated;
			configurationClient.GetEventTypeGroupsEvent -= FireGetEventTypeGroups;
			configurationClient.GetEventTypesForUserEvent -= FireGetEventTypesForUser;
			configurationClient.GetDisableAlarmItemsEvent -= FireGetDisableAlarmItems;
			configurationClient.AddDisableAlarmItemsEvent -= FireAddDisableAlarmItems;
			configurationClient.ModifyDisableAlarmItemEvent -= FireModifyDisableAlarmItem;
			configurationClient.DeleteDisableAlarmItemEvent -= FireDeleteDisableAlarmItem;
			configurationClient.GetSuppressStatusItemsEvent -= FireGetSuppressStatusItems;
			configurationClient.AddSuppressStatusItemsEvent -= FireAddSuppressStatusItems;
			configurationClient.ModifySuppressStatusItemEvent -= FireModifySuppressStatusItem;
			configurationClient.DeleteSuppressStatusItemEvent -= FireDeleteSuppressStatusItem;
			configurationClient.GetPrioritySoundMappingsEvent -= FireGetPrioritySoundMappings;
			configurationClient.SetPrioritySoundMappingsEvent -= FireSetPrioritySoundMappings;
			configurationClient.GetAlarmDataSettingsEvent -= FireGetAlarmDataSettings;
			configurationClient.SetAlarmDataSettingsEvent -= FireSetAlarmDataSettings;
			configurationClient.UpdateEventsExpirationEvent -= FireUpdateEventsExpiration;
			configurationClient.Close();
		}
	}

	private void FireFaultException(Exception error, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(error, requestId);
		}
	}

	private void FireCreateAlarmDefinition(int requestId, bool success)
	{
		if (this.CreateAlarmDefinitionEvent != null)
		{
			this.CreateAlarmDefinitionEvent(requestId, success);
		}
	}

	private void FireUpdateAlarmDefinition(int requestId, bool success)
	{
		if (this.UpdateAlarmDefinitionEvent != null)
		{
			this.UpdateAlarmDefinitionEvent(requestId, success);
		}
	}

	private void FireDeleteAlarmDefinition(int requestId, bool success)
	{
		if (this.DeleteAlarmDefinitionEvent != null)
		{
			this.DeleteAlarmDefinitionEvent(requestId, success);
		}
	}

	private void FireGetAlarmDefinition(int requestId, AlarmDefinition alarmDefinition)
	{
		if (this.GetAlarmDefinitionEvent != null)
		{
			this.GetAlarmDefinitionEvent(requestId, alarmDefinition);
		}
	}

	private void FireGetAlarmDefinitions(int requestId, AlarmDefinition[] alarmDefinitions)
	{
		if (this.GetAlarmDefinitionsEvent != null)
		{
			this.GetAlarmDefinitionsEvent(requestId, alarmDefinitions);
		}
	}

	private void FireCreateTimeProfileDefinition(int requestId, bool success)
	{
		if (this.CreateTimeProfileDefinitionEvent != null)
		{
			this.CreateTimeProfileDefinitionEvent(requestId, success);
		}
	}

	private void FireUpdateTimeProfileDefinition(int requestId, bool success)
	{
		if (this.UpdateTimeProfileDefinitionEvent != null)
		{
			this.UpdateTimeProfileDefinitionEvent(requestId, success);
		}
	}

	private void FireDeleteTimeProfileDefinition(int requestId, bool success)
	{
		if (this.DeleteTimeProfileDefinitionEvent != null)
		{
			this.DeleteTimeProfileDefinitionEvent(requestId, success);
		}
	}

	private void FireGetTimeProfileDefinition(int requestId, TimeProfileDefinition timeProfileDefinition)
	{
		if (this.GetTimeProfileDefinitionEvent != null)
		{
			this.GetTimeProfileDefinitionEvent(requestId, timeProfileDefinition);
		}
	}

	private void FireGetTimeProfileDefinitions(int requestId, TimeProfileDefinition[] timeProfileDefinitions)
	{
		if (this.GetTimeProfileDefinitionsEvent != null)
		{
			this.GetTimeProfileDefinitionsEvent(requestId, timeProfileDefinitions);
		}
	}

	private void FireGetGeneralSettings(int requestId, GeneralSettings generalSettings)
	{
		if (this.GetGeneralSettingsEvent != null)
		{
			this.GetGeneralSettingsEvent(requestId, generalSettings);
		}
	}

	private void FireSetGeneralSettings(int requestId, bool success)
	{
		if (this.SetGeneralSettingsEvent != null)
		{
			this.SetGeneralSettingsEvent(requestId, success);
		}
	}

	private void FireResetGeneralSettings(int requestId, bool success)
	{
		if (this.ResetGeneralSettingsEvent != null)
		{
			this.ResetGeneralSettingsEvent(requestId, success);
		}
	}

	private void FireGetSoundConfiguration(int requestId, SoundConfigurationItem[] soundConfiguration)
	{
		if (this.GetSoundConfigurationEvent != null)
		{
			this.GetSoundConfigurationEvent(requestId, soundConfiguration);
		}
	}

	private void FireUploadSound(int requestId, bool success)
	{
		if (this.UploadSoundEvent != null)
		{
			this.UploadSoundEvent(requestId, success);
		}
	}

	private void FireDeleteSound(int requestId, bool success)
	{
		if (this.DeleteSoundEvent != null)
		{
			this.DeleteSoundEvent(requestId, success);
		}
	}

	private void FireDownloadSound(int requestId, SoundDataItem soundData)
	{
		if (this.DownloadSoundEvent != null)
		{
			this.DownloadSoundEvent(requestId, soundData);
		}
	}

	private void FireGetEventTypes(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesEvent != null)
		{
			this.GetEventTypesEvent(requestId, eventTypes);
		}
	}

	private void FireGetEventTypesTranslated(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesTranslatedEvent != null)
		{
			this.GetEventTypesTranslatedEvent(requestId, eventTypes);
		}
	}

	private void FireGetEventTypeGroups(int requestId, EventTypeGroup[] eventTypeGroups)
	{
		if (this.GetEventTypeGroupsEvent != null)
		{
			this.GetEventTypeGroupsEvent(requestId, eventTypeGroups);
		}
	}

	private void FireGetEventTypesForUser(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesForUserEvent != null)
		{
			this.GetEventTypesForUserEvent(requestId, eventTypes);
		}
	}

	private void FireGetDisableAlarmItems(int requestId, DisableAlarmItem[] disableAlarmItems)
	{
		if (this.GetDisableAlarmItemsEvent != null)
		{
			this.GetDisableAlarmItemsEvent(requestId, disableAlarmItems);
		}
	}

	private void FireAddDisableAlarmItems(int requestId)
	{
		if (this.AddDisableAlarmItemsEvent != null)
		{
			this.AddDisableAlarmItemsEvent(requestId);
		}
	}

	private void FireModifyDisableAlarmItem(int requestId)
	{
		if (this.ModifyDisableAlarmItemEvent != null)
		{
			this.ModifyDisableAlarmItemEvent(requestId);
		}
	}

	private void FireDeleteDisableAlarmItem(int requestId)
	{
		if (this.DeleteDisableAlarmItemEvent != null)
		{
			this.DeleteDisableAlarmItemEvent(requestId);
		}
	}

	private void FireGetSuppressStatusItems(int requestId, SuppressStatusItem[] suppressStatusItems)
	{
		if (this.GetSuppressStatusItemsEvent != null)
		{
			this.GetSuppressStatusItemsEvent(requestId, suppressStatusItems);
		}
	}

	private void FireAddSuppressStatusItems(int requestId)
	{
		if (this.AddSuppressStatusItemsEvent != null)
		{
			this.AddSuppressStatusItemsEvent(requestId);
		}
	}

	private void FireModifySuppressStatusItem(int requestId)
	{
		if (this.ModifySuppressStatusItemEvent != null)
		{
			this.ModifySuppressStatusItemEvent(requestId);
		}
	}

	private void FireDeleteSuppressStatusItem(int requestId)
	{
		if (this.DeleteSuppressStatusItemEvent != null)
		{
			this.DeleteSuppressStatusItemEvent(requestId);
		}
	}

	private void FireGetPrioritySoundMappings(int requestId, Tuple<Guid, Guid>[] mappings)
	{
		if (this.GetPrioritySoundMappingsEvent != null)
		{
			this.GetPrioritySoundMappingsEvent(requestId, mappings);
		}
	}

	private void FireSetPrioritySoundMappings(int requestId)
	{
		if (this.SetPrioritySoundMappingsEvent != null)
		{
			this.SetPrioritySoundMappingsEvent(requestId);
		}
	}

	private void FireGetAlarmDataSettings(int requestId, AlarmDataSettings settings)
	{
		if (this.GetAlarmDataSettingsEvent != null)
		{
			this.GetAlarmDataSettingsEvent(requestId, settings);
		}
	}

	private void FireSetAlarmDataSettings(int requestId)
	{
		if (this.SetAlarmDataSettingsEvent != null)
		{
			this.SetAlarmDataSettingsEvent(requestId);
		}
	}

	private void FireClientSessionHeartbeat(int requestId, HeartbeatResp response)
	{
		if (this.ClientSessionHeartbeatEvent != null)
		{
			this.ClientSessionHeartbeatEvent(requestId, response);
		}
	}

	private void FireUpdateEventsExpiration(int requestId, UpdateEventsExpirationResp response)
	{
		if (this.UpdateEventsExpirationEvent != null)
		{
			this.UpdateEventsExpirationEvent(requestId, response);
		}
	}
}
