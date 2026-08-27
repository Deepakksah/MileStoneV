using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Configuration;

public abstract class ConfigurationClient : AbstractClient
{
	public delegate void FaultExceptionHandler(Exception exception, int requestId);

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

	public delegate void GetEventRetentionEventHandler(int requestId, GetEventRetentionResp getEventRetentionResp);

	public delegate void GetDefaultEventRetentionEventHandler(int requestId, GetDefaultEventRetentionResp getDefaultEventRetentionResp);

	public delegate void SetEventRetentionEventHandler(int requestId, SetEventRetentionResp setEventRetentionRes);

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

	public delegate void GetAlarmPropertiesHandler(int requestId, KeyValuePair<string, string>[] properties);

	public delegate void ClientSessionHeartbeatHandler(int requestId, HeartbeatResp response);

	public delegate void UpdateEventsExpirationHandler(int requestId, UpdateEventsExpirationResp response);

	protected ServerId ServerId { get; private set; }

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

	public event GetEventRetentionEventHandler GetEventRetentionEvent;

	public event GetDefaultEventRetentionEventHandler GetDefaultEventRetentionEvent;

	public event SetEventRetentionEventHandler SetEventRetentionEvent;

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

	public event GetAlarmPropertiesHandler GetAlarmPropertiesEvent;

	public event ClientSessionHeartbeatHandler ClientSessionHeartbeatEvent;

	public event UpdateEventsExpirationHandler UpdateEventsExpirationEvent;

	protected ConfigurationClient(ServerId serverId, string eventServerAddress, Version eventServerVersion, Uri uri)
		: base(eventServerAddress, eventServerVersion, uri)
	{
		ServerId = serverId;
	}

	protected override void HandleRequestObject(RequestObject requestObject)
	{
		if (requestObject is CreateAlarmDefinitionRequestObject)
		{
			CreateAlarmDefinitionRequestObject createAlarmDefinitionRequestObject = requestObject as CreateAlarmDefinitionRequestObject;
			bool success = CreateAlarmDefinition(createAlarmDefinitionRequestObject.AlarmDefinition);
			FireCreateAlarmDefinition(createAlarmDefinitionRequestObject.RequestId, success);
			return;
		}
		if (requestObject is UpdateAlarmDefinitionRequestObject)
		{
			UpdateAlarmDefinitionRequestObject updateAlarmDefinitionRequestObject = requestObject as UpdateAlarmDefinitionRequestObject;
			bool success2 = UpdateAlarmDefinition(updateAlarmDefinitionRequestObject.AlarmDefinition);
			FireUpdateAlarmDefinition(updateAlarmDefinitionRequestObject.RequestId, success2);
			return;
		}
		if (requestObject is DeleteAlarmDefinitionRequestObject)
		{
			DeleteAlarmDefinitionRequestObject deleteAlarmDefinitionRequestObject = requestObject as DeleteAlarmDefinitionRequestObject;
			bool success3 = DeleteAlarmDefinition(deleteAlarmDefinitionRequestObject.AlarmDefinitionId);
			FireDeleteAlarmDefinition(deleteAlarmDefinitionRequestObject.RequestId, success3);
			return;
		}
		if (requestObject is GetAlarmDefinitionRequestObject)
		{
			GetAlarmDefinitionRequestObject getAlarmDefinitionRequestObject = requestObject as GetAlarmDefinitionRequestObject;
			AlarmDefinition alarmDefinition = GetAlarmDefinition(getAlarmDefinitionRequestObject.AlarmDefinitionId);
			FireGetAlarmDefinition(getAlarmDefinitionRequestObject.RequestId, alarmDefinition);
			return;
		}
		if (requestObject is GetAlarmDefinitionsRequestObject)
		{
			GetAlarmDefinitionsRequestObject getAlarmDefinitionsRequestObject = requestObject as GetAlarmDefinitionsRequestObject;
			AlarmDefinition[] alarmDefinitions = GetAlarmDefinitions();
			FireGetAlarmDefinitions(getAlarmDefinitionsRequestObject.RequestId, alarmDefinitions);
			return;
		}
		if (requestObject is CreateTimeProfileDefinitionRequestObject)
		{
			CreateTimeProfileDefinitionRequestObject createTimeProfileDefinitionRequestObject = requestObject as CreateTimeProfileDefinitionRequestObject;
			bool success4 = CreateTimeProfileDefinition(createTimeProfileDefinitionRequestObject.TimeProfileDefinition);
			FireCreateTimeProfileDefinition(createTimeProfileDefinitionRequestObject.RequestId, success4);
			return;
		}
		if (requestObject is UpdateTimeProfileDefinitionRequestObject)
		{
			UpdateTimeProfileDefinitionRequestObject updateTimeProfileDefinitionRequestObject = requestObject as UpdateTimeProfileDefinitionRequestObject;
			bool success5 = UpdateTimeProfileDefinition(updateTimeProfileDefinitionRequestObject.TimeProfileDefinition);
			FireUpdateTimeProfileDefinition(updateTimeProfileDefinitionRequestObject.RequestId, success5);
			return;
		}
		if (requestObject is DeleteTimeProfileDefinitionRequestObject)
		{
			DeleteTimeProfileDefinitionRequestObject deleteTimeProfileDefinitionRequestObject = requestObject as DeleteTimeProfileDefinitionRequestObject;
			bool success6 = DeleteTimeProfileDefinition(deleteTimeProfileDefinitionRequestObject.TimeProfileDefinitionId);
			FireDeleteTimeProfileDefinition(deleteTimeProfileDefinitionRequestObject.RequestId, success6);
			return;
		}
		if (requestObject is GetTimeProfileDefinitionRequestObject)
		{
			GetTimeProfileDefinitionRequestObject getTimeProfileDefinitionRequestObject = requestObject as GetTimeProfileDefinitionRequestObject;
			TimeProfileDefinition timeProfileDefinition = GetTimeProfileDefinition(getTimeProfileDefinitionRequestObject.TimeProfileDefinitionId);
			FireGetTimeProfileDefinition(getTimeProfileDefinitionRequestObject.RequestId, timeProfileDefinition);
			return;
		}
		if (requestObject is GetTimeProfileDefinitionsRequestObject)
		{
			GetTimeProfileDefinitionsRequestObject getTimeProfileDefinitionsRequestObject = requestObject as GetTimeProfileDefinitionsRequestObject;
			TimeProfileDefinition[] timeProfileDefinitions = GetTimeProfileDefinitions();
			FireGetTimeProfileDefinitions(getTimeProfileDefinitionsRequestObject.RequestId, timeProfileDefinitions);
			return;
		}
		if (requestObject is GetGeneralSettingsRequestObject)
		{
			GetGeneralSettingsRequestObject getGeneralSettingsRequestObject = requestObject as GetGeneralSettingsRequestObject;
			GeneralSettings generalSettings = GetGeneralSettings();
			FireGetGeneralSettings(getGeneralSettingsRequestObject.RequestId, generalSettings);
			return;
		}
		if (requestObject is SetGeneralSettingsRequestObject)
		{
			SetGeneralSettingsRequestObject setGeneralSettingsRequestObject = requestObject as SetGeneralSettingsRequestObject;
			bool succes = SetGeneralSettings(setGeneralSettingsRequestObject.GeneralSettings);
			FireSetGeneralSettings(setGeneralSettingsRequestObject.RequestId, succes);
			return;
		}
		if (requestObject is ResetGeneralSettingsRequestObject)
		{
			ResetGeneralSettingsRequestObject resetGeneralSettingsRequestObject = requestObject as ResetGeneralSettingsRequestObject;
			bool succes2 = ResetGeneralSettings();
			FireResetGeneralSettings(resetGeneralSettingsRequestObject.RequestId, succes2);
			return;
		}
		if (requestObject is GetEventRetentionRequestObject)
		{
			GetEventRetentionRequestObject getEventRetentionRequestObject = (GetEventRetentionRequestObject)requestObject;
			GetEventRetentionResp eventRetention = GetEventRetention(getEventRetentionRequestObject.GetEventRetentionParams);
			FireGetEventRetention(getEventRetentionRequestObject.RequestId, eventRetention);
			return;
		}
		if (requestObject is GetDefaultEventRetentionRequestObject)
		{
			GetDefaultEventRetentionRequestObject getDefaultEventRetentionRequestObject = (GetDefaultEventRetentionRequestObject)requestObject;
			GetDefaultEventRetentionResp defaultEventRetention = GetDefaultEventRetention(getDefaultEventRetentionRequestObject.GetDefaultEventRetentionParams);
			FireGetDefaultEventRetention(getDefaultEventRetentionRequestObject.RequestId, defaultEventRetention);
			return;
		}
		if (requestObject is SetEventRetentionRequestObject)
		{
			SetEventRetentionRequestObject setEventRetentionRequestObject = (SetEventRetentionRequestObject)requestObject;
			SetEventRetentionResp setEventRetentionResp = SetEventRetention(setEventRetentionRequestObject.SetEventRetentionParams);
			FireSetEventRetention(setEventRetentionRequestObject.RequestId, setEventRetentionResp);
			return;
		}
		if (requestObject is GetSoundConfigurationRequestObject)
		{
			GetSoundConfigurationRequestObject getSoundConfigurationRequestObject = requestObject as GetSoundConfigurationRequestObject;
			SoundConfigurationItem[] soundConfiguration = GetSoundConfiguration();
			FireGetSoundConfiguration(getSoundConfigurationRequestObject.RequestId, soundConfiguration);
			return;
		}
		if (requestObject is UploadSoundRequestObject)
		{
			UploadSoundRequestObject uploadSoundRequestObject = requestObject as UploadSoundRequestObject;
			bool succes3 = UploadSound(uploadSoundRequestObject.SoundData);
			FireUploadSound(uploadSoundRequestObject.RequestId, succes3);
			return;
		}
		if (requestObject is DeleteSoundRequestObject)
		{
			DeleteSoundRequestObject deleteSoundRequestObject = requestObject as DeleteSoundRequestObject;
			bool succes4 = DeleteSound(deleteSoundRequestObject.Id);
			FireDeleteSound(deleteSoundRequestObject.RequestId, succes4);
			return;
		}
		if (requestObject is DownloadSoundRequestObject)
		{
			DownloadSoundRequestObject downloadSoundRequestObject = requestObject as DownloadSoundRequestObject;
			SoundDataItem soundData = DownloadSound(downloadSoundRequestObject.Id);
			FireDownloadSound(downloadSoundRequestObject.RequestId, soundData);
			return;
		}
		if (requestObject is GetEventTypesRequestObject)
		{
			GetEventTypesRequestObject getEventTypesRequestObject = requestObject as GetEventTypesRequestObject;
			EventType[] eventTypes = GetEventTypes();
			FireGetEventTypes(getEventTypesRequestObject.RequestId, eventTypes);
			return;
		}
		if (requestObject is GetEventTypesTranslatedRequestObject)
		{
			GetEventTypesTranslatedRequestObject getEventTypesTranslatedRequestObject = requestObject as GetEventTypesTranslatedRequestObject;
			EventType[] eventTypesTranslated = GetEventTypesTranslated(getEventTypesTranslatedRequestObject.CultureInfo);
			FireGetEventTypesTranslated(getEventTypesTranslatedRequestObject.RequestId, eventTypesTranslated);
			return;
		}
		if (requestObject is GetEventTypeGroupsRequestObject)
		{
			GetEventTypeGroupsRequestObject getEventTypeGroupsRequestObject = requestObject as GetEventTypeGroupsRequestObject;
			EventTypeGroup[] eventTypeGroups = GetEventTypeGroups();
			FireGetEventTypeGroups(getEventTypeGroupsRequestObject.RequestId, eventTypeGroups);
			return;
		}
		if (requestObject is GetEventTypesForUserRequestObject)
		{
			GetEventTypesForUserRequestObject getEventTypesForUserRequestObject = requestObject as GetEventTypesForUserRequestObject;
			EventType[] eventTypesForUser = GetEventTypesForUser(getEventTypesForUserRequestObject.SourceId, getEventTypesForUserRequestObject.LanguageId);
			FireGetEventTypesForUser(getEventTypesForUserRequestObject.RequestId, eventTypesForUser);
			return;
		}
		if (requestObject is GetDisableAlarmItemsRequestObject)
		{
			GetDisableAlarmItemsRequestObject getDisableAlarmItemsRequestObject = requestObject as GetDisableAlarmItemsRequestObject;
			DisableAlarmItem[] disableAlarmItems = GetDisableAlarmItems(Thread.CurrentThread.CurrentUICulture.Name);
			FireGetDisableAlarmItems(getDisableAlarmItemsRequestObject.RequestId, disableAlarmItems);
			return;
		}
		if (requestObject is AddDisableAlarmItemsRequestObject)
		{
			AddDisableAlarmItemsRequestObject addDisableAlarmItemsRequestObject = requestObject as AddDisableAlarmItemsRequestObject;
			AddDisableAlarmItems(addDisableAlarmItemsRequestObject.DisableAlarmItems);
			FireAddDisableAlarmItems(addDisableAlarmItemsRequestObject.RequestId);
			return;
		}
		if (requestObject is ModifyDisableAlarmItemRequestObject)
		{
			ModifyDisableAlarmItemRequestObject modifyDisableAlarmItemRequestObject = requestObject as ModifyDisableAlarmItemRequestObject;
			ModifyDisableAlarmItem(modifyDisableAlarmItemRequestObject.DisableAlarmItem);
			FireModifyDisableAlarmItem(modifyDisableAlarmItemRequestObject.RequestId);
			return;
		}
		if (requestObject is DeleteDisableAlarmItemRequestObject)
		{
			DeleteDisableAlarmItemRequestObject deleteDisableAlarmItemRequestObject = requestObject as DeleteDisableAlarmItemRequestObject;
			DeleteDisableAlarmItem(deleteDisableAlarmItemRequestObject.SourceId, deleteDisableAlarmItemRequestObject.DisableAlarmItemId);
			FireDeleteDisableAlarmItem(deleteDisableAlarmItemRequestObject.RequestId);
			return;
		}
		if (requestObject is GetSuppressStatusItemsRequestObject)
		{
			GetSuppressStatusItemsRequestObject getSuppressStatusItemsRequestObject = requestObject as GetSuppressStatusItemsRequestObject;
			SuppressStatusItem[] suppressStatusItems = GetSuppressStatusItems();
			FireGetSuppressStatusItems(getSuppressStatusItemsRequestObject.RequestId, suppressStatusItems);
			return;
		}
		if (requestObject is AddSuppressStatusItemsRequestObject)
		{
			AddSuppressStatusItemsRequestObject addSuppressStatusItemsRequestObject = requestObject as AddSuppressStatusItemsRequestObject;
			AddSuppressStatusItems(addSuppressStatusItemsRequestObject.SuppressStatusItems);
			FireAddSuppressStatusItems(addSuppressStatusItemsRequestObject.RequestId);
			return;
		}
		if (requestObject is ModifySuppressStatusItemRequestObject)
		{
			ModifySuppressStatusItemRequestObject modifySuppressStatusItemRequestObject = requestObject as ModifySuppressStatusItemRequestObject;
			ModifySuppressStatusItem(modifySuppressStatusItemRequestObject.SuppressStatusItem);
			FireModifySuppressStatusItem(modifySuppressStatusItemRequestObject.RequestId);
			return;
		}
		if (requestObject is DeleteSuppressStatusItemRequestObject)
		{
			DeleteSuppressStatusItemRequestObject deleteSuppressStatusItemRequestObject = requestObject as DeleteSuppressStatusItemRequestObject;
			DeleteSuppressStatusItem(deleteSuppressStatusItemRequestObject.SourceId, deleteSuppressStatusItemRequestObject.SuppressStatusItemId);
			FireDeleteSuppressStatusItem(deleteSuppressStatusItemRequestObject.RequestId);
			return;
		}
		if (requestObject is GetPrioritySoundMappingsRequestObject)
		{
			GetPrioritySoundMappingsRequestObject getPrioritySoundMappingsRequestObject = requestObject as GetPrioritySoundMappingsRequestObject;
			Tuple<Guid, Guid>[] prioritySoundMappings = GetPrioritySoundMappings();
			FireGetPrioritySoundMappings(getPrioritySoundMappingsRequestObject.RequestId, prioritySoundMappings);
			return;
		}
		if (requestObject is SetPrioritySoundMappingsRequestObject)
		{
			SetPrioritySoundMappingsRequestObject setPrioritySoundMappingsRequestObject = requestObject as SetPrioritySoundMappingsRequestObject;
			SetPrioritySoundMappings(setPrioritySoundMappingsRequestObject.Mappings);
			FireSetPrioritySoundMappings(setPrioritySoundMappingsRequestObject.RequestId);
			return;
		}
		if (requestObject is GetAlarmDataSettingsRequestObject)
		{
			GetAlarmDataSettingsRequestObject getAlarmDataSettingsRequestObject = requestObject as GetAlarmDataSettingsRequestObject;
			AlarmDataSettings alarmDataSettings = GetAlarmDataSettings();
			FireGetAlarmDataSettings(getAlarmDataSettingsRequestObject.RequestId, alarmDataSettings);
			return;
		}
		if (requestObject is SetAlarmDataSettingsRequestObject)
		{
			SetAlarmDataSettingsRequestObject setAlarmDataSettingsRequestObject = requestObject as SetAlarmDataSettingsRequestObject;
			SetAlarmDataSettings(setAlarmDataSettingsRequestObject.Settings);
			FireSetAlarmDataSettings(setAlarmDataSettingsRequestObject.RequestId);
			return;
		}
		if (requestObject is ClientSessionHeartbeatRequestObject)
		{
			ClientSessionHeartbeatRequestObject clientSessionHeartbeatRequestObject = requestObject as ClientSessionHeartbeatRequestObject;
			HeartbeatResp response = ClientSessionHeartbeat(clientSessionHeartbeatRequestObject.HeartbeatParams);
			FireClientSessionHeartbeat(clientSessionHeartbeatRequestObject.RequestId, response);
			return;
		}
		if (requestObject is UpdateEventsExpirationRequestObject)
		{
			UpdateEventsExpirationRequestObject updateEventsExpirationRequestObject = requestObject as UpdateEventsExpirationRequestObject;
			UpdateEventsExpirationResp response2 = UpdateEventsExpiration(updateEventsExpirationRequestObject.UpdateEventsExpirationParams);
			FireUpdateEventsExpiration(updateEventsExpirationRequestObject.RequestId, response2);
			return;
		}
		throw new ArgumentException("Unknown request object");
	}

	protected override void FireFaultException(Exception exception, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(exception, requestId);
		}
	}

	private void FireCreateAlarmDefinition(int requestId, bool success)
	{
		if (this.CreateAlarmDefinitionEvent != null)
		{
			this.CreateAlarmDefinitionEvent(requestId, success);
		}
	}

	public void BeginCreateAlarmDefinition(int requestId, AlarmDefinition alarmDefinition)
	{
		Enqueue(new CreateAlarmDefinitionRequestObject(requestId, alarmDefinition));
	}

	public abstract bool CreateAlarmDefinition(AlarmDefinition alarmDefinition);

	private void FireUpdateAlarmDefinition(int requestId, bool success)
	{
		if (this.UpdateAlarmDefinitionEvent != null)
		{
			this.UpdateAlarmDefinitionEvent(requestId, success);
		}
	}

	public void BeginUpdateAlarmDefinition(int requestId, AlarmDefinition alarmDefinition)
	{
		Enqueue(new UpdateAlarmDefinitionRequestObject(requestId, alarmDefinition));
	}

	public abstract bool UpdateAlarmDefinition(AlarmDefinition alarmDefinition);

	private void FireDeleteAlarmDefinition(int requestId, bool success)
	{
		if (this.DeleteAlarmDefinitionEvent != null)
		{
			this.DeleteAlarmDefinitionEvent(requestId, success);
		}
	}

	public void BeginDeleteAlarmDefinition(int requestId, Guid alarmDefinitionId)
	{
		Enqueue(new DeleteAlarmDefinitionRequestObject(requestId, alarmDefinitionId));
	}

	public abstract bool DeleteAlarmDefinition(Guid alarmDefinitionId);

	private void FireGetAlarmDefinition(int requestId, AlarmDefinition alarmDefinition)
	{
		if (this.GetAlarmDefinitionEvent != null)
		{
			this.GetAlarmDefinitionEvent(requestId, alarmDefinition);
		}
	}

	public void BeginGetAlarmDefinition(int requestId, Guid alarmDefinitionId)
	{
		Enqueue(new GetAlarmDefinitionRequestObject(requestId, alarmDefinitionId));
	}

	public abstract AlarmDefinition GetAlarmDefinition(Guid alarmDefinitionId);

	private void FireGetAlarmDefinitions(int requestId, AlarmDefinition[] alarmDefinitions)
	{
		if (this.GetAlarmDefinitionsEvent != null)
		{
			this.GetAlarmDefinitionsEvent(requestId, alarmDefinitions);
		}
	}

	public void BeginGetAlarmDefinitions(int requestId)
	{
		Enqueue(new GetAlarmDefinitionsRequestObject(requestId));
	}

	public abstract AlarmDefinition[] GetAlarmDefinitions();

	private void FireCreateTimeProfileDefinition(int requestId, bool success)
	{
		if (this.CreateTimeProfileDefinitionEvent != null)
		{
			this.CreateTimeProfileDefinitionEvent(requestId, success);
		}
	}

	public void BeginCreateTimeProfileDefinition(int requestId, TimeProfileDefinition timeProfileDefinition)
	{
		Enqueue(new CreateTimeProfileDefinitionRequestObject(requestId, timeProfileDefinition));
	}

	public abstract bool CreateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition);

	private void FireUpdateTimeProfileDefinition(int requestId, bool success)
	{
		if (this.UpdateTimeProfileDefinitionEvent != null)
		{
			this.UpdateTimeProfileDefinitionEvent(requestId, success);
		}
	}

	public void BeginUpdateTimeProfileDefinition(int requestId, TimeProfileDefinition timeProfileDefinition)
	{
		Enqueue(new UpdateTimeProfileDefinitionRequestObject(requestId, timeProfileDefinition));
	}

	public abstract bool UpdateTimeProfileDefinition(TimeProfileDefinition timeProfileDefinition);

	private void FireDeleteTimeProfileDefinition(int requestId, bool success)
	{
		if (this.DeleteTimeProfileDefinitionEvent != null)
		{
			this.DeleteTimeProfileDefinitionEvent(requestId, success);
		}
	}

	public void BeginDeleteTimeProfileDefinition(int requestId, Guid timeProfileDefinitionId)
	{
		Enqueue(new DeleteTimeProfileDefinitionRequestObject(requestId, timeProfileDefinitionId));
	}

	public abstract bool DeleteTimeProfileDefinition(Guid timeProfileDefinitionId);

	private void FireGetTimeProfileDefinition(int requestId, TimeProfileDefinition timeProfileDefinition)
	{
		if (this.GetTimeProfileDefinitionEvent != null)
		{
			this.GetTimeProfileDefinitionEvent(requestId, timeProfileDefinition);
		}
	}

	public void BeginGetTimeProfileDefinition(int requestId, Guid timeProfileDefinitionId)
	{
		Enqueue(new GetTimeProfileDefinitionRequestObject(requestId, timeProfileDefinitionId));
	}

	public abstract TimeProfileDefinition GetTimeProfileDefinition(Guid timeProfileDefinitionId);

	private void FireGetTimeProfileDefinitions(int requestId, TimeProfileDefinition[] timeProfileDefinitions)
	{
		if (this.GetTimeProfileDefinitionsEvent != null)
		{
			this.GetTimeProfileDefinitionsEvent(requestId, timeProfileDefinitions);
		}
	}

	public void BeginGetTimeProfileDefinitions(int requestId, Guid timeProfileDefinitionId)
	{
		Enqueue(new GetTimeProfileDefinitionsRequestObject(requestId));
	}

	public abstract TimeProfileDefinition[] GetTimeProfileDefinitions();

	private void FireGetGeneralSettings(int requestId, GeneralSettings generalSettings)
	{
		if (this.GetGeneralSettingsEvent != null)
		{
			this.GetGeneralSettingsEvent(requestId, generalSettings);
		}
	}

	public void BeginGetGeneralSettings(int requestId)
	{
		Enqueue(new GetGeneralSettingsRequestObject(requestId));
	}

	public abstract GeneralSettings GetGeneralSettings();

	private void FireSetGeneralSettings(int requestId, bool succes)
	{
		if (this.SetGeneralSettingsEvent != null)
		{
			this.SetGeneralSettingsEvent(requestId, succes);
		}
	}

	public void BeginSetGeneralSettings(int requestId, GeneralSettings generalSettings)
	{
		Enqueue(new SetGeneralSettingsRequestObject(requestId, generalSettings));
	}

	public abstract bool SetGeneralSettings(GeneralSettings generalSettings);

	private void FireResetGeneralSettings(int requestId, bool succes)
	{
		if (this.ResetGeneralSettingsEvent != null)
		{
			this.ResetGeneralSettingsEvent(requestId, succes);
		}
	}

	public void BeginResetGeneralSettings(int requestId)
	{
		Enqueue(new ResetGeneralSettingsRequestObject(requestId));
	}

	public abstract bool ResetGeneralSettings();

	private void FireGetEventRetention(int requestId, GetEventRetentionResp getEventRetentionResp)
	{
		this.GetEventRetentionEvent?.Invoke(requestId, getEventRetentionResp);
	}

	public void BeginGetEventRetention(int requestId, GetEventRetentionParams getEventRetentionParams)
	{
		Enqueue(new GetEventRetentionRequestObject(requestId, getEventRetentionParams));
	}

	public abstract GetEventRetentionResp GetEventRetention(GetEventRetentionParams getEventRetentionParams);

	private void FireGetDefaultEventRetention(int requestId, GetDefaultEventRetentionResp getDefaultEventRetentionResp)
	{
		this.GetDefaultEventRetentionEvent?.Invoke(requestId, getDefaultEventRetentionResp);
	}

	public void BeginGetDefaultEventRetention(int requestId, GetDefaultEventRetentionParams getDefaultEventRetentionParams)
	{
		Enqueue(new GetDefaultEventRetentionRequestObject(requestId, getDefaultEventRetentionParams));
	}

	public abstract GetDefaultEventRetentionResp GetDefaultEventRetention(GetDefaultEventRetentionParams getDefaultEventRetentionParams);

	private void FireSetEventRetention(int requestId, SetEventRetentionResp setEventRetentionResp)
	{
		this.SetEventRetentionEvent?.Invoke(requestId, setEventRetentionResp);
	}

	public void BeginSetEventRetention(int requestId, SetEventRetentionParams setEventRetentionParams)
	{
		Enqueue(new SetEventRetentionRequestObject(requestId, setEventRetentionParams));
	}

	public abstract SetEventRetentionResp SetEventRetention(SetEventRetentionParams setEventRetentionParams);

	private void FireGetSoundConfiguration(int requestId, SoundConfigurationItem[] soundConfiguration)
	{
		if (this.GetSoundConfigurationEvent != null)
		{
			this.GetSoundConfigurationEvent(requestId, soundConfiguration);
		}
	}

	public void BeginGetSoundConfiguration(int requestId)
	{
		Enqueue(new GetSoundConfigurationRequestObject(requestId));
	}

	public abstract SoundConfigurationItem[] GetSoundConfiguration();

	private void FireUploadSound(int requestId, bool succes)
	{
		if (this.UploadSoundEvent != null)
		{
			this.UploadSoundEvent(requestId, succes);
		}
	}

	public void BeginUploadSound(int requestId, SoundDataItem soundData)
	{
		Enqueue(new UploadSoundRequestObject(requestId, soundData));
	}

	public abstract bool UploadSound(SoundDataItem soundData);

	private void FireDeleteSound(int requestId, bool succes)
	{
		if (this.DeleteSoundEvent != null)
		{
			this.DeleteSoundEvent(requestId, succes);
		}
	}

	public void BeginDeleteSound(int requestId, Guid id)
	{
		Enqueue(new DeleteSoundRequestObject(requestId, id));
	}

	public abstract bool DeleteSound(Guid id);

	private void FireDownloadSound(int requestId, SoundDataItem soundData)
	{
		if (this.DownloadSoundEvent != null)
		{
			this.DownloadSoundEvent(requestId, soundData);
		}
	}

	public void BeginDownloadSound(int requestId, Guid id)
	{
		Enqueue(new DownloadSoundRequestObject(requestId, id));
	}

	public abstract SoundDataItem DownloadSound(Guid id);

	private void FireGetEventTypes(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesEvent != null)
		{
			this.GetEventTypesEvent(requestId, eventTypes);
		}
	}

	public void BeginGetEventTypes(int requestId)
	{
		Enqueue(new GetEventTypesRequestObject(requestId));
	}

	public abstract EventType[] GetEventTypes();

	private void FireGetEventTypesTranslated(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesTranslatedEvent != null)
		{
			this.GetEventTypesTranslatedEvent(requestId, eventTypes);
		}
	}

	public void BeginGetEventTypesTranslated(int requestId, CultureInfo culture)
	{
		Enqueue(new GetEventTypesTranslatedRequestObject(requestId, culture));
	}

	public abstract EventType[] GetEventTypesTranslated(CultureInfo culture);

	private void FireGetEventTypeGroups(int requestId, EventTypeGroup[] eventTypeGroups)
	{
		if (this.GetEventTypeGroupsEvent != null)
		{
			this.GetEventTypeGroupsEvent(requestId, eventTypeGroups);
		}
	}

	public void BeginGetEventTypeGroups(int requestId)
	{
		Enqueue(new GetEventTypeGroupsRequestObject(requestId));
	}

	public abstract EventTypeGroup[] GetEventTypeGroups();

	private void FireGetEventTypesForUser(int requestId, EventType[] eventTypes)
	{
		if (this.GetEventTypesForUserEvent != null)
		{
			this.GetEventTypesForUserEvent(requestId, eventTypes);
		}
	}

	public void BeginGetEventTypesForUser(int requestId, Guid sourceId, string languageId)
	{
		Enqueue(new GetEventTypesForUserRequestObject(requestId, sourceId, languageId));
	}

	public abstract EventType[] GetEventTypesForUser(Guid sourceId, string languageId);

	private void FireGetDisableAlarmItems(int requestId, DisableAlarmItem[] disableAlarmItems)
	{
		if (this.GetDisableAlarmItemsEvent != null)
		{
			this.GetDisableAlarmItemsEvent(requestId, disableAlarmItems);
		}
	}

	public void BeginGetDisableAlarmItems(int requestId)
	{
		Enqueue(new GetDisableAlarmItemsRequestObject(requestId));
	}

	public abstract DisableAlarmItem[] GetDisableAlarmItems(string languageId);

	private void FireAddDisableAlarmItems(int requestId)
	{
		if (this.AddDisableAlarmItemsEvent != null)
		{
			this.AddDisableAlarmItemsEvent(requestId);
		}
	}

	public void BeginAddDisableAlarmItems(int requestId, DisableAlarmItem[] disableAlarmItems)
	{
		Enqueue(new AddDisableAlarmItemsRequestObject(requestId, disableAlarmItems));
	}

	public abstract void AddDisableAlarmItems(DisableAlarmItem[] disableAlarmItems);

	private void FireModifyDisableAlarmItem(int requestId)
	{
		if (this.ModifyDisableAlarmItemEvent != null)
		{
			this.ModifyDisableAlarmItemEvent(requestId);
		}
	}

	public void BeginModifyDisableAlarmItem(int requestId, DisableAlarmItem disableAlarmItem)
	{
		Enqueue(new ModifyDisableAlarmItemRequestObject(requestId, disableAlarmItem));
	}

	public abstract void ModifyDisableAlarmItem(DisableAlarmItem disableAlarmItem);

	private void FireDeleteDisableAlarmItem(int requestId)
	{
		if (this.DeleteDisableAlarmItemEvent != null)
		{
			this.DeleteDisableAlarmItemEvent(requestId);
		}
	}

	public void BeginDeleteDisableAlarmItem(int requestId, Guid sourceId, Guid disableAlarmItemId)
	{
		Enqueue(new DeleteDisableAlarmItemRequestObject(requestId, sourceId, disableAlarmItemId));
	}

	public abstract void DeleteDisableAlarmItem(Guid sourceId, Guid disableAlarmItemId);

	private void FireGetSuppressStatusItems(int requestId, SuppressStatusItem[] suppressStatusItems)
	{
		if (this.GetSuppressStatusItemsEvent != null)
		{
			this.GetSuppressStatusItemsEvent(requestId, suppressStatusItems);
		}
	}

	public void BeginGetSuppressStatusItems(int requestId)
	{
		Enqueue(new GetSuppressStatusItemsRequestObject(requestId));
	}

	public abstract SuppressStatusItem[] GetSuppressStatusItems();

	private void FireAddSuppressStatusItems(int requestId)
	{
		if (this.AddSuppressStatusItemsEvent != null)
		{
			this.AddSuppressStatusItemsEvent(requestId);
		}
	}

	public void BeginAddSuppressStatusItems(int requestId, SuppressStatusItem[] suppressStatusItems)
	{
		Enqueue(new AddSuppressStatusItemsRequestObject(requestId, suppressStatusItems));
	}

	public abstract void AddSuppressStatusItems(SuppressStatusItem[] suppressStatusItems);

	private void FireModifySuppressStatusItem(int requestId)
	{
		if (this.ModifySuppressStatusItemEvent != null)
		{
			this.ModifySuppressStatusItemEvent(requestId);
		}
	}

	public void BeginModifySuppressStatusItem(int requestId, SuppressStatusItem suppressStatusItem)
	{
		Enqueue(new ModifySuppressStatusItemRequestObject(requestId, suppressStatusItem));
	}

	public abstract void ModifySuppressStatusItem(SuppressStatusItem suppressStatusItem);

	private void FireDeleteSuppressStatusItem(int requestId)
	{
		if (this.DeleteSuppressStatusItemEvent != null)
		{
			this.DeleteSuppressStatusItemEvent(requestId);
		}
	}

	public void BeginDeleteSuppressStatusItem(int requestId, Guid sourceId, Guid suppressStatusItemId)
	{
		Enqueue(new DeleteSuppressStatusItemRequestObject(requestId, sourceId, suppressStatusItemId));
	}

	public abstract void DeleteSuppressStatusItem(Guid sourceId, Guid suppressStatusItemId);

	private void FireGetPrioritySoundMappings(int requestId, Tuple<Guid, Guid>[] mappings)
	{
		if (this.GetPrioritySoundMappingsEvent != null)
		{
			this.GetPrioritySoundMappingsEvent(requestId, mappings);
		}
	}

	public void BeginGetPrioritySoundMappings(int requestId)
	{
		Enqueue(new GetPrioritySoundMappingsRequestObject(requestId));
	}

	public abstract Tuple<Guid, Guid>[] GetPrioritySoundMappings();

	private void FireSetPrioritySoundMappings(int requestId)
	{
		if (this.SetPrioritySoundMappingsEvent != null)
		{
			this.SetPrioritySoundMappingsEvent(requestId);
		}
	}

	public void BeginSetPrioritySoundMappings(int requestId, Tuple<Guid, Guid>[] mappings)
	{
		Enqueue(new SetPrioritySoundMappingsRequestObject(requestId, mappings));
	}

	public abstract void SetPrioritySoundMappings(Tuple<Guid, Guid>[] mappings);

	private void FireGetAlarmDataSettings(int requestId, AlarmDataSettings settings)
	{
		if (this.GetAlarmDataSettingsEvent != null)
		{
			this.GetAlarmDataSettingsEvent(requestId, settings);
		}
	}

	public void BeginGetAlarmDataSettings(int requestId)
	{
		Enqueue(new GetAlarmDataSettingsRequestObject(requestId));
	}

	public abstract AlarmDataSettings GetAlarmDataSettings();

	private void FireSetAlarmDataSettings(int requestId)
	{
		if (this.SetAlarmDataSettingsEvent != null)
		{
			this.SetAlarmDataSettingsEvent(requestId);
		}
	}

	public void BeginSetAlarmDataSettings(int requestId, AlarmDataSettings settings)
	{
		Enqueue(new SetAlarmDataSettingsRequestObject(requestId, settings));
	}

	public abstract void SetAlarmDataSettings(AlarmDataSettings alarmDataSettings);

	private void FireGetAlarmProperties(int requestId, KeyValuePair<string, string>[] properties)
	{
		if (this.GetAlarmPropertiesEvent != null)
		{
			this.GetAlarmPropertiesEvent(requestId, properties);
		}
	}

	public void BeginGetAlarmProperties(int requestId, string key)
	{
		Enqueue(new GetAlarmPropertiesRequestObject(requestId, key));
	}

	public abstract KeyValuePair<string, string>[] GetAlarmProperties(string key);

	private void FireClientSessionHeartbeat(int requestId, HeartbeatResp response)
	{
		if (this.ClientSessionHeartbeatEvent != null)
		{
			this.ClientSessionHeartbeatEvent(requestId, response);
		}
	}

	public void BeginClientSessionHeartbeat(int requestId, HeartbeatParams heartbeatParams)
	{
		Enqueue(new ClientSessionHeartbeatRequestObject(requestId, heartbeatParams));
	}

	public abstract HeartbeatResp ClientSessionHeartbeat(HeartbeatParams heartbeatParams);

	private void FireUpdateEventsExpiration(int requestId, UpdateEventsExpirationResp response)
	{
		if (this.UpdateEventsExpirationEvent != null)
		{
			this.UpdateEventsExpirationEvent(requestId, response);
		}
	}

	public void BeginUpdateEventsExpiration(int requestId, UpdateEventsExpirationParams updateEventsExpirationParams)
	{
		Enqueue(new UpdateEventsExpirationRequestObject(requestId, updateEventsExpirationParams));
	}

	public abstract UpdateEventsExpirationResp UpdateEventsExpiration(UpdateEventsExpirationParams updateEventsExpirationParams);

	public abstract LookupItemResp LookupItem(LookupItemParams lookupItemParams);
}
