using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ServiceModel;

namespace VideoOS.WebService.Common.ServerCommandService;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand", ConfigurationName = "VideoOS.WebService.Common.ServerCommandService.IServerCommandService")]
public interface IServerCommandService
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersionResponse")]
	int GetVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersionResponse")]
	string GetServerVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfoResponse")]
	ProductInfo GetProductInfo();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfoResponse")]
	SystemLicenseInfo GetSystemLicenseInfo();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersionResponse")]
	SmartClientVersion GetSmartClientVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersionResponse")]
	SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnline", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnlineResponse")]
	bool IsOnline();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Login", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LoginResponse")]
	LoginInfo Login(Guid instanceId, string currentToken);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Logout", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LogoutResponse")]
	void Logout(Guid instanceId, string currentToken);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Authorize", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AuthorizeResponse")]
	string Authorize(string userDomainName, AuthorizationActionType type);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfoResponse")]
	UserInfo GetUserInfo(string token);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfiguration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationResponse")]
	ConfigurationInfo GetConfiguration(string token);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecorders", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecordersResponse")]
	RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardware", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardwareResponse")]
	HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabled", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabledResponse")]
	DeviceDisabledInfo[] GetDevicesDisabled(string token);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesList", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesListResponse")]
	DictionaryEntry[] GetVmoServicesList();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroups", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupsResponse")]
	ViewGroupInfo[] GetViewGroups(string token);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupDataResponse")]
	ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupDataResponse")]
	SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobalResponse")]
	CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobal2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobal2Response")]
	CustomSettingInternal GetCustomSettingDataGlobal2(string token, Guid customSettingId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUserResponse")]
	CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser2Response")]
	CustomSettingInternal GetCustomSettingDataUser2(string token, Guid customSettingId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobalResponse")]
	SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal2Response")]
	SetCustomSettingResultType SetCustomSettingDataGlobal2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUserResponse")]
	SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser2Response")]
	SetCustomSettingResultType SetCustomSettingDataUser2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTrigger", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerResponse")]
	void EventTrigger(string token, Guid eventId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadata", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadataResponse")]
	void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpan", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpanResponse")]
	AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCamera", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCameraResponse")]
	void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChanges", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChangesResponse")]
	Change[] QueryChanges(string token, DateTime changesFrom);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfoResponse")]
	RecorderInfo QueryRecorderInfo(string token, Guid recorderId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReferenceResponse")]
	BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreateResponse")]
	Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdateResponse")]
	Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDeleteResponse")]
	void BookmarkDelete(string token, Guid bookmarkId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetResponse")]
	Bookmark BookmarkGet(string token, Guid bookmarkId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTimeResponse")]
	Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2Response")]
	Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmarkResponse")]
	Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2Response")]
	Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegrationResponse")]
	void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrations", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrationsResponse")]
	void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QuerySystemMonitorStateSourcesByTypes", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QuerySystemMonitorStateSourcesByTypesResponse")]
	SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByTypes(string[] types);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QuerySystemMonitorStateSourcesByIds", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QuerySystemMonitorStateSourcesByIdsResponse")]
	SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByIds(string[] ids);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReferenceResponse")]
	MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreateResponse")]
	MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdateResponse")]
	MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDeleteResponse")]
	MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetResponse")]
	MarkedData MarkedDataGet(string token, Guid markedDataId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearch", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearchResponse")]
	MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearch2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearch2Response")]
	MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfileResponse")]
	ClientProfile GetClientProfile(string token, bool withSetting);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile2Response")]
	ClientProfile GetClientProfile2(string token, bool withSetting);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile3", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile3Response")]
	ClientProfile GetClientProfile3(string token, bool withSetting);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresetsResponse")]
	PresetInfo[] GetPresets(string token, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePresetResponse")]
	void DeletePreset(string token, Guid id, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetName", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetNameResponse")]
	void UpdatePresetName(string token, Guid id, Guid deviceId, string newName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPosition", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPositionResponse")]
	void UpdatePresetPosition(string token, Guid id, Guid deviceId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePresetResponse")]
	Guid CreatePreset(string token, Guid deviceId, string name);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresetsResponse")]
	void OrderPresets(string token, Guid deviceId, Guid[] ids);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreateResponse")]
	RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdateResponse")]
	RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDeleteResponse")]
	RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAllResponse")]
	RestrictedMedia[] RestrictedMediaQueryAll();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnter", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnterResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExit", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExitResponse")]
	RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdateResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDeleteResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAllResponse")]
	RestrictedMediaLive[] RestrictedMediaLiveQueryAll();
}
