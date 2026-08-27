using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerCommandServiceClient : ClientBase<IServerCommandService>, IServerCommandService
{
	public ServerCommandServiceClient()
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandServiceClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public ServerCommandServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	public string GetServerVersion()
	{
		return base.Channel.GetServerVersion();
	}

	public ProductInfo GetProductInfo()
	{
		return base.Channel.GetProductInfo();
	}

	public SystemLicenseInfo GetSystemLicenseInfo()
	{
		return base.Channel.GetSystemLicenseInfo();
	}

	public SmartClientVersion GetSmartClientVersion()
	{
		return base.Channel.GetSmartClientVersion();
	}

	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		return base.Channel.CheckSmartClientVersion(major, minor, revision);
	}

	public bool IsOnline()
	{
		return base.Channel.IsOnline();
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return base.Channel.Login(instanceId, currentToken);
	}

	public void Logout(Guid instanceId, string currentToken)
	{
		base.Channel.Logout(instanceId, currentToken);
	}

	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		return base.Channel.Authorize(userDomainName, type);
	}

	public UserInfo GetUserInfo(string token)
	{
		return base.Channel.GetUserInfo(token);
	}

	public ConfigurationInfo GetConfiguration(string token)
	{
		return base.Channel.GetConfiguration(token);
	}

	public RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds)
	{
		return base.Channel.GetConfigurationRecorders(token, recorderIds);
	}

	public HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds)
	{
		return base.Channel.GetConfigurationHardware(token, hardwareIds);
	}

	public DeviceDisabledInfo[] GetDevicesDisabled(string token)
	{
		return base.Channel.GetDevicesDisabled(token);
	}

	public DictionaryEntry[] GetVmoServicesList()
	{
		return base.Channel.GetVmoServicesList();
	}

	public ViewGroupInfo[] GetViewGroups(string token)
	{
		return base.Channel.GetViewGroups(token);
	}

	public ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId)
	{
		return base.Channel.GetViewGroupData(token, viewGroupId);
	}

	public SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		return base.Channel.SetViewGroupData(token, viewGroupId, viewGroupdata);
	}

	public CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal(token, customSettingId);
	}

	public CustomSettingInternal GetCustomSettingDataGlobal2(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal2(token, customSettingId);
	}

	public CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser(token, customSettingId);
	}

	public CustomSettingInternal GetCustomSettingDataUser2(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser2(token, customSettingId);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal2(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser2(token, customSettingId, customSettingData, forceOverride);
	}

	public void EventTrigger(string token, Guid eventId)
	{
		base.Channel.EventTrigger(token, eventId);
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata)
	{
		base.Channel.EventTriggerWithMetadata(token, eventId, metadata);
	}

	public AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.AlertsGetAroundWithSpan(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId)
	{
		base.Channel.MatrixMonitorActivateCamera(token, matrixMonitorId, deviceId);
	}

	public Change[] QueryChanges(string token, DateTime changesFrom)
	{
		return base.Channel.QueryChanges(token, changesFrom);
	}

	public RecorderInfo QueryRecorderInfo(string token, Guid recorderId)
	{
		return base.Channel.QueryRecorderInfo(token, recorderId);
	}

	public BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live)
	{
		return base.Channel.BookmarkGetNewReference(token, deviceId, live);
	}

	public Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkCreate(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkUpdate(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public void BookmarkDelete(string token, Guid bookmarkId)
	{
		base.Channel.BookmarkDelete(token, bookmarkId);
	}

	public Bookmark BookmarkGet(string token, Guid bookmarkId)
	{
		return base.Channel.BookmarkGet(token, bookmarkId);
	}

	public Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchTime(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchTime2(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchFromBookmark2(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		base.Channel.RegisterIntegration(token, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
	}

	public void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		base.Channel.RegisterPluginIntegrations(token, instanceId, integrationType, pluginIntegrations);
	}

	public MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live)
	{
		return base.Channel.MarkedDataGetNewReference(token, deviceId, live);
	}

	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataCreate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataUpdate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		return base.Channel.MarkedDataDelete(token, markedDataIds);
	}

	public MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		return base.Channel.MarkedDataGet(token, markedDataId);
	}

	public MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch2(token, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public ClientProfile GetClientProfile(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile(token, withSetting);
	}

	public ClientProfile GetClientProfile2(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile2(token, withSetting);
	}

	public ClientProfile GetClientProfile3(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile3(token, withSetting);
	}

	public SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		return base.Channel.QuerySystemMonitorStateSourcesByIds(ids);
	}

	public SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		return base.Channel.QuerySystemMonitorStateSourcesByTypes(types);
	}

	public PresetInfo[] GetPresets(string token, Guid deviceId)
	{
		return base.Channel.GetPresets(token, deviceId);
	}

	public void DeletePreset(string token, Guid id, Guid deviceId)
	{
		base.Channel.DeletePreset(token, id, deviceId);
	}

	public void UpdatePresetName(string token, Guid id, Guid deviceId, string newName)
	{
		base.Channel.UpdatePresetName(token, id, deviceId, newName);
	}

	public void UpdatePresetPosition(string token, Guid id, Guid deviceId)
	{
		base.Channel.UpdatePresetPosition(token, id, deviceId);
	}

	public Guid CreatePreset(string token, Guid deviceId, string name)
	{
		return base.Channel.CreatePreset(token, deviceId, name);
	}

	public void OrderPresets(string token, Guid deviceId, Guid[] ids)
	{
		base.Channel.OrderPresets(token, deviceId, ids);
	}

	public RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd);
	}

	public RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd);
	}

	public RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		return base.Channel.RestrictedMediaDelete(restrictedMediaId);
	}

	public RestrictedMedia[] RestrictedMediaQueryAll()
	{
		return base.Channel.RestrictedMediaQueryAll();
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveModeEnter(deviceIds, timeStart);
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd);
	}

	RestrictedMediaLiveResult IServerCommandService.RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveUpdate(deviceIds, timeStart);
	}

	RestrictedMediaLiveResult IServerCommandService.RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		return base.Channel.RestrictedMediaLiveDelete(deviceIds);
	}

	RestrictedMediaLive[] IServerCommandService.RestrictedMediaLiveQueryAll()
	{
		return base.Channel.RestrictedMediaLiveQueryAll();
	}
}
