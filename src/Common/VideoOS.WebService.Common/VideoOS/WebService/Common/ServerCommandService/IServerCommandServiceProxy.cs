using System;
using System.Collections;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.ServerCommandService;

public interface IServerCommandServiceProxy : ITimeoutSettings, IAdditionalMessageHeaders, IDisposable
{
	void Abort();

	void SetDualAutorizationId(string authorizationId);

	int GetVersion();

	string GetServerVersion();

	ProductInfo GetProductInfo();

	SystemLicenseInfo GetSystemLicenseInfo();

	SmartClientVersion GetSmartClientVersion();

	SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	bool IsOnline();

	LoginInfo Login(Guid instanceId, string currentToken);

	void Logout(Guid instanceId, string currentToken);

	string Authorize(string userDomainName, AuthorizationActionType type);

	UserInfo GetUserInfo(string token);

	ConfigurationInfo GetConfiguration();

	RecorderInfo[] GetConfigurationRecorders(Guid[] recorderIds);

	HardwareInfo[] GetConfigurationHardware(Guid[] hardwareIds);

	DeviceDisabledInfo[] GetDevicesDisabled();

	DictionaryEntry[] GetVmoServicesList();

	ViewGroupInfo[] GetViewGroups();

	ViewGroupInternal GetViewGroupData(Guid viewGroupId);

	SetViewGroupDataResultType SetViewGroupData(Guid viewGroupId, ViewGroupInternalData viewGroupdata);

	CustomSettingInternal GetCustomSettingDataGlobal(Guid customSettingId);

	CustomSettingInternal GetCustomSettingDataGlobal2(Guid customSettingId);

	CustomSettingInternal GetCustomSettingDataUser(Guid customSettingId);

	CustomSettingInternal GetCustomSettingDataUser2(Guid customSettingId);

	SetCustomSettingResultType SetCustomSettingDataGlobal(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	SetCustomSettingResultType SetCustomSettingDataGlobal2(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	SetCustomSettingResultType SetCustomSettingDataUser(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	SetCustomSettingResultType SetCustomSettingDataUser2(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	void EventTrigger(Guid eventId);

	void EventTriggerWithMetadata(Guid eventId, KeyValue[] metadata);

	AlertOccurrence[] AlertsGetAroundWithSpan(Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId);

	Change[] QueryChanges(DateTime changesFrom);

	RecorderInfo QueryRecorderInfo(Guid recorderId);

	BookmarkReference BookmarkGetNewReference(Guid deviceId, bool live);

	Bookmark BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	Bookmark BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	void BookmarkDelete(Guid bookmarkId);

	Bookmark BookmarkGet(Guid bookmarkId);

	Bookmark[] BookmarkSearchTime(DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	Bookmark[] BookmarkSearchTime2(DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	Bookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	Bookmark[] BookmarkSearchFromBookmark2(Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	void RegisterIntegration(string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName);

	void RegisterPluginIntegrations(string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByTypes(string[] types);

	SystemMonitorStateSource[] QuerySystemMonitorStateSourcesByIds(string[] ids);

	MarkedDataReference MarkedDataGetNewReference(Guid[] deviceId, bool live);

	MarkedDataResult MarkedDataCreate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	MarkedDataResult MarkedDataUpdate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	MarkedDataResult[] MarkedDataDelete(Guid[] markedDataIds);

	MarkedData MarkedDataGet(Guid markedDataId);

	MarkedData[] MarkedDataSearch(Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	MarkedData[] MarkedDataSearch2(Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd);

	RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId);

	RestrictedMedia[] RestrictedMediaQueryAll();

	RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart);

	RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart);

	RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	RestrictedMediaLive[] RestrictedMediaLiveQueryAll();

	ClientProfile GetClientProfile(bool withSetting);

	ClientProfile GetClientProfile2(bool withSetting);

	ClientProfile GetClientProfile3(bool withSetting);

	PresetInfo[] GetPresets(Guid deviceId);

	void DeletePreset(Guid id, Guid deviceId);

	void UpdatePresetName(Guid id, Guid deviceId, string newName);

	void UpdatePresetPosition(Guid id, Guid deviceId);

	Guid CreatePreset(Guid deviceId, string name);

	void OrderPresets(Guid deviceId, Guid[] ids);
}
