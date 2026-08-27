using System;
using System.Net;
using VideoOS.Platform.SDK.Config.Svc;

namespace VideoOS.Platform.SDK.Interface;

public interface IServerService : IDisposable
{
	ICredentials Credentials { get; set; }

	string Url { get; set; }

	event AlertsGetAroundWithSpanCompletedEventHandler AlertsGetAroundWithSpanCompleted;

	event CheckSmartClientVersionCompletedEventHandler CheckSmartClientVersionCompleted;

	event EventTriggerCompletedEventHandler EventTriggerCompleted;

	event EventTriggerWithMetadataCompletedEventHandler EventTriggerWithMetadataCompleted;

	event GetClientProfileCompletedEventHandler GetClientProfileCompleted;

	event GetConfigurationCompletedEventHandler GetConfigurationCompleted;

	event GetCustomSettingDataGlobalCompletedEventHandler GetCustomSettingDataGlobalCompleted;

	event GetCustomSettingDataUserCompletedEventHandler GetCustomSettingDataUserCompleted;

	event GetProductInfoCompletedEventHandler GetProductInfoCompleted;

	event GetServerVersionCompletedEventHandler GetServerVersionCompleted;

	event GetSmartClientVersionCompletedEventHandler GetSmartClientVersionCompleted;

	event GetSystemLicenseInfoCompletedEventHandler GetSystemLicenseInfoCompleted;

	event GetUserInfoCompletedEventHandler GetUserInfoCompleted;

	event GetVersionCompletedEventHandler GetVersionCompleted;

	event GetVmoServicesListCompletedEventHandler GetVmoServicesListCompleted;

	event LoginCompletedEventHandler LoginCompleted;

	event LogoutCompletedEventHandler LogoutCompleted;

	event MatrixMonitorActivateCameraCompletedEventHandler MatrixMonitorActivateCameraCompleted;

	event QueryChangesCompletedEventHandler QueryChangesCompleted;

	event QueryRecorderInfoCompletedEventHandler QueryRecorderInfoCompleted;

	event SetCustomSettingDataGlobalCompletedEventHandler SetCustomSettingDataGlobalCompleted;

	event SetCustomSettingDataUserCompletedEventHandler SetCustomSettingDataUserCompleted;

	event BookmarkCreateCompletedEventHandler BookmarkCreateCompleted;

	event BookmarkUpdateCompletedEventHandler BookmarkUpdateCompleted;

	event BookmarkSearchTimeCompletedEventHandler BookmarkSearchTimeCompleted;

	event BookmarkSearchFromBookmarkCompletedEventHandler BookmarkSearchFromBookmarkCompleted;

	event BookmarkGetNewReferenceCompletedEventHandler BookmarkGetNewReferenceCompleted;

	event BookmarkDeleteCompletedEventHandler BookmarkDeleteCompleted;

	event BookmarkGetCompletedEventHandler BookmarkGetCompleted;

	event MarkedDataGetNewReferenceCompletedEventHandler MarkedDataGetNewReferenceCompleted;

	event MarkedDataCreateCompletedEventHandler MarkedDataCreateCompleted;

	event MarkedDataUpdateCompletedEventHandler MarkedDataUpdateCompleted;

	event MarkedDataDeleteCompletedEventHandler MarkedDataDeleteCompleted;

	event MarkedDataGetCompletedEventHandler MarkedDataGetCompleted;

	event MarkedDataSearchCompletedEventHandler MarkedDataSearchCompleted;

	event GetPresetsCompletedEventHandler GetPresetsCompleted;

	event OrderPresetsCompletedEventHandler OrderPresetsCompleted;

	event UpdatePresetNameCompletedEventHandler UpdatePresetNameCompleted;

	event UpdatePresetPositionCompletedEventHandler UpdatePresetPositionCompleted;

	event CreatePresetCompletedEventHandler CreatePresetCompleted;

	event DeletePresetCompletedEventHandler DeletePresetCompleted;

	ICustomSettingData Create(CustomSettingData customSetting);

	IKeyValue[] Create(KeyValue[] metadata);

	IViewGroupData Create(ViewGroupData viewGroupData);

	IRetentionOption Create(RetentionOption retentionOption);

	IAlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, ITimeDuration maxTimeBefore, int maxCountBefore, ITimeDuration maxTimeAfter, int maxCountAfter);

	void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, ITimeDuration maxTimeBefore, int maxCountBefore, ITimeDuration maxTimeAfter, int maxCountAfter);

	void AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, ITimeDuration maxTimeBefore, int maxCountBefore, ITimeDuration maxTimeAfter, int maxCountAfter, object userState);

	IAsyncResult BeginAlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, ITimeDuration maxTimeBefore, int maxCountBefore, ITimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState);

	IAlertOccurrence[] EndAlertsGetAroundWithSpan(IAsyncResult asyncResult);

	ISmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	void CheckSmartClientVersionAsync(int major, int minor, string revision);

	void CheckSmartClientVersionAsync(int major, int minor, string revision, object userState);

	IAsyncResult BeginCheckSmartClientVersion(int major, int minor, string revision, AsyncCallback callback, object asyncState);

	ISmartClientVersion EndCheckSmartClientVersion(IAsyncResult asyncResult);

	void EventTrigger(string token, Guid eventId);

	void EventTriggerAsync(string token, Guid eventId);

	void EventTriggerAsync(string token, Guid eventId, object userState);

	IAsyncResult BeginEventTrigger(string token, Guid eventId, AsyncCallback callback, object asyncState);

	void EndEventTrigger(IAsyncResult asyncResult);

	void EventTriggerWithMetadata(string token, Guid eventId, IKeyValue[] metadata);

	void EventTriggerWithMetadataAsync(string token, Guid eventId, IKeyValue[] metadata);

	void EventTriggerWithMetadataAsync(string token, Guid eventId, IKeyValue[] metadata, object userState);

	IAsyncResult BeginEventTriggerWithMetadata(string token, Guid eventId, IKeyValue[] metadata, AsyncCallback callback, object asyncState);

	void EndEventTriggerWithMetadata(IAsyncResult asyncResult);

	IClientProfile GetClientProfile(string token, bool withSetting);

	void GetClientProfileAsync(string token, bool withSetting);

	void GetClientProfileAsync(string token, bool withSetting, object userState);

	IAsyncResult BeginGetClientProfile(string token, bool withSetting, AsyncCallback callback, object asyncState);

	IClientProfile EndGetClientProfile(IAsyncResult asyncResult);

	IConfigurationInfo GetConfiguration(string token);

	void GetConfigurationAsync(string token);

	void GetConfigurationAsync(string token, object userState);

	IAsyncResult BeginGetConfiguration(string token, AsyncCallback callback, object asyncState);

	IConfigurationInfo EndGetConfiguration(IAsyncResult asyncResult);

	ICustomSetting GetCustomSettingDataGlobal(string token, Guid customSettingId);

	void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId);

	void GetCustomSettingDataGlobalAsync(string token, Guid customSettingId, object userState);

	IAsyncResult BeginGetCustomSettingDataGlobal(string token, Guid customSettingId, AsyncCallback callback, object asyncState);

	ICustomSetting EndGetCustomSettingDataGlobal(IAsyncResult asyncResult);

	ICustomSetting GetCustomSettingDataUser(string token, Guid customSettingId);

	void GetCustomSettingDataUserAsync(string token, Guid customSettingId);

	void GetCustomSettingDataUserAsync(string token, Guid customSettingId, object userState);

	IAsyncResult BeginGetCustomSettingDataUser(string token, Guid customSettingId, AsyncCallback callback, object asyncState);

	ICustomSetting EndGetCustomSettingDataUser(IAsyncResult asyncResult);

	IProductInfo GetProductInfo();

	void GetProductInfoAsync();

	void GetProductInfoAsync(object userState);

	IAsyncResult BeginGetProductInfo(AsyncCallback callback, object asyncState);

	IProductInfo EndGetProductInfo(IAsyncResult asyncResult);

	string GetServerVersion();

	void GetServerVersionAsync();

	void GetServerVersionAsync(object userState);

	IAsyncResult BeginGetServerVersion(AsyncCallback callback, object asyncState);

	string EndGetServerVersion(IAsyncResult asyncResult);

	ISmartClientVersion GetSmartClientVersion();

	void GetSmartClientVersionAsync();

	void GetSmartClientVersionAsync(object userState);

	IAsyncResult BeginGetSmartClientVersion(AsyncCallback callback, object asyncState);

	ISmartClientVersion EndGetSmartClientVersion(IAsyncResult asyncResult);

	ISystemLicenseInfo GetSystemLicenseInfo();

	void GetSystemLicenseInfoAsync();

	void GetSystemLicenseInfoAsync(object userState);

	IAsyncResult BeginGetSystemLicenseInfo(AsyncCallback callback, object asyncState);

	ISystemLicenseInfo EndGetSystemLicenseInfo(IAsyncResult asyncResult);

	IUserInfo GetUserInfo(string token);

	void GetUserInfoAsync(string token);

	void GetUserInfoAsync(string token, object userState);

	IAsyncResult BeginGetUserInfo(string token, AsyncCallback callback, object asyncState);

	IUserInfo EndGetUserInfo(IAsyncResult asyncResult);

	int GetVersion();

	void GetVersionAsync();

	void GetVersionAsync(object userState);

	IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState);

	int EndGetVersion(IAsyncResult asyncResult);

	IViewGroup GetViewGroupData(string token, Guid viewGroupId);

	void GetViewGroupDataAsync(string token, Guid viewGroupId);

	void GetViewGroupDataAsync(string token, Guid viewGroupId, object userState);

	IAsyncResult BeginGetViewGroupData(string token, Guid viewGroupId, AsyncCallback callback, object asyncState);

	IViewGroup EndGetViewGroupData(IAsyncResult asyncResult);

	IViewGroupInfo[] GetViewGroups(string token);

	void GetViewGroupsAsync(string token);

	void GetViewGroupsAsync(string token, object userState);

	IAsyncResult BeginGetViewGroups(string token, AsyncCallback callback, object asyncState);

	IViewGroupInfo[] EndGetViewGroups(IAsyncResult asyncResult);

	IDictionaryEntry[] GetVmoServicesList();

	void GetVmoServicesListAsync();

	void GetVmoServicesListAsync(object userState);

	IAsyncResult BeginGetVmoServicesList(AsyncCallback callback, object asyncState);

	IDictionaryEntry[] EndGetVmoServicesList(IAsyncResult asyncResult);

	void SetDualAuthorizationId(string authorizationId);

	string Authorize(string userDomainName, AuthorizationActionType type);

	ILoginInfo Login(Guid instanceId, string currentToken);

	void LoginAsync(Guid instanceId, string currentToken);

	void LoginAsync(Guid instanceId, string currentToken, object userState);

	IAsyncResult BeginLogin(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState);

	ILoginInfo EndLogin(IAsyncResult asyncResult);

	void Logout(Guid instanceId, string currentToken);

	void LogoutAsync(Guid instanceId, string currentToken);

	void LogoutAsync(Guid instanceId, string currentToken, object userState);

	IAsyncResult BeginLogout(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState);

	void EndLogout(IAsyncResult asyncResult);

	void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId);

	void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId);

	void MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId, object userState);

	IAsyncResult BeginMatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndMatrixMonitorActivateCamera(IAsyncResult asyncResult);

	IChange[] QueryChanges(string token, DateTime changesFrom);

	void QueryChangesAsync(string token, DateTime changesFrom);

	void QueryChangesAsync(string token, DateTime changesFrom, object userState);

	IAsyncResult BeginQueryChanges(string token, DateTime changesFrom, AsyncCallback callback, object asyncState);

	IChange[] EndQueryChanges(IAsyncResult asyncResult);

	IRecorderInfo QueryRecorderInfo(string token, Guid recorderId);

	void QueryRecorderInfoAsync(string token, Guid recorderId);

	void QueryRecorderInfoAsync(string token, Guid recorderId, object userState);

	IAsyncResult BeginQueryRecorderInfo(string token, Guid recorderId, AsyncCallback callback, object asyncState);

	IRecorderInfo EndQueryRecorderInfo(IAsyncResult asyncResult);

	SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride);

	void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride);

	void SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride, object userState);

	IAsyncResult BeginSetCustomSettingDataGlobal(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState);

	SetCustomSettingResultType EndSetCustomSettingDataGlobal(IAsyncResult asyncResult);

	SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride);

	void SetCustomSettingDataUserAsync(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride);

	void SetCustomSettingDataUserAsync(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride, object userState);

	IAsyncResult BeginSetCustomSettingDataUser(string token, Guid customSettingId, ICustomSettingData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState);

	SetCustomSettingResultType EndSetCustomSettingDataUser(IAsyncResult asyncResult);

	SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, IViewGroupData viewGroupdata);

	void SetViewGroupDataAsync(string token, Guid viewGroupId, IViewGroupData viewGroupdata);

	void SetViewGroupDataAsync(string token, Guid viewGroupId, IViewGroupData viewGroupdata, object userState);

	IAsyncResult BeginSetViewGroupData(string token, Guid viewGroupId, IViewGroupData viewGroupdata, AsyncCallback callback, object asyncState);

	SetViewGroupDataResultType EndSetViewGroupData(IAsyncResult asyncResult);

	IBookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	void BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState);

	IAsyncResult BeginBookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState);

	IBookmark EndBookmarkCreate(IAsyncResult asyncResult);

	void BookmarkDelete(string token, Guid bookmarkId);

	void BookmarkDeleteAsync(string token, Guid bookmarkId);

	void BookmarkDeleteAsync(string token, Guid bookmarkId, object userState);

	IAsyncResult BeginBookmarkDelete(string token, Guid bookmarkId, AsyncCallback callback, object asyncState);

	void EndBookmarkDelete(IAsyncResult asyncResult);

	IBookmark BookmarkGet(string token, Guid bookmarkId);

	void BookmarkGetAsync(string token, Guid bookmarkId);

	void BookmarkGetAsync(string token, Guid bookmarkId, object userState);

	IAsyncResult BeginBookmarkGet(string token, Guid bookmarkId, AsyncCallback callback, object asyncState);

	IBookmark EndBookmarkGet(IAsyncResult asyncResult);

	IBookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live);

	void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live);

	void BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live, object userState);

	IAsyncResult BeginBookmarkGetNewReference(string token, Guid deviceId, bool live, AsyncCallback callback, object asyncState);

	IBookmarkReference EndBookmarkGetNewReference(IAsyncResult asyncResult);

	IBookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	void BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState);

	IAsyncResult BeginBookmarkSearchFromBookmark(string token, Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState);

	IBookmark[] EndBookmarkSearchFromBookmark(IAsyncResult asyncResult);

	IBookmark[] BookmarkSearchTime(string token, DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	void BookmarkSearchTimeAsync(string token, DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	void BookmarkSearchTimeAsync(string token, DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, object userState);

	IAsyncResult BeginBookmarkSearchTime(string token, DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState);

	IBookmark[] EndBookmarkSearchTime(IAsyncResult asyncResult);

	IBookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	void BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, object userState);

	IAsyncResult BeginBookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState);

	IBookmark EndBookmarkUpdate(IAsyncResult asyncResult);

	IPresetInfo[] GetPresets(string token, Guid deviceId);

	void GetPresetsAsync(string token, Guid deviceId);

	void GetPresetsAsync(string token, Guid deviceId, object userState);

	IAsyncResult BeginGetPresets(string token, Guid deviceId, AsyncCallback callback, object asyncState);

	IPresetInfo[] EndGetPresets(IAsyncResult asyncResult);

	void OrderPresets(string token, Guid deviceId, Guid[] ids);

	void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids);

	void OrderPresetsAsync(string token, Guid deviceId, Guid[] ids, object userState);

	IAsyncResult BeginOrderPresets(string token, Guid deviceId, Guid[] ids, AsyncCallback callback, object asyncState);

	void EndOrderPresets(IAsyncResult asyncResult);

	void UpdatePresetName(string token, Guid id, Guid deviceId, string newName);

	void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName);

	void UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName, object userState);

	IAsyncResult BeginUpdatePresetName(string token, Guid id, Guid deviceId, string newName, AsyncCallback callback, object asyncState);

	void EndUpdatePresetName(IAsyncResult asyncResult);

	void UpdatePresetPosition(string token, Guid id, Guid deviceId);

	void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId);

	void UpdatePresetPositionAsync(string token, Guid id, Guid deviceId, object userState);

	IAsyncResult BeginUpdatePresetPosition(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndUpdatePresetPosition(IAsyncResult asyncResult);

	Guid CreatePreset(string token, Guid deviceId, string name);

	void CreatePresetAsync(string token, Guid deviceId, string name);

	void CreatePresetAsync(string token, Guid deviceId, string name, object userState);

	IAsyncResult BeginCreatePreset(string token, Guid deviceId, string name, AsyncCallback callback, object asyncState);

	Guid EndCreatePreset(IAsyncResult asyncResult);

	void DeletePreset(string token, Guid id, Guid deviceId);

	void DeletePresetAsync(string token, Guid id, Guid deviceId);

	void DeletePresetAsync(string token, Guid id, Guid deviceId, object userState);

	IAsyncResult BeginDeletePreset(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndDeletePreset(IAsyncResult asyncResult);

	IMarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live);

	void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceId, bool live);

	void MarkedDataGetNewReferenceAsync(string token, Guid[] deviceId, bool live, object userState);

	IAsyncResult BeginMarkedDataGetNewReference(string token, Guid[] deviceId, bool live, AsyncCallback callback, object asyncState);

	IMarkedDataReference EndMarkedDataGetNewReference(IAsyncResult asyncResult);

	IMarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption);

	void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption);

	void MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption, object userState);

	IAsyncResult BeginMarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption, AsyncCallback callback, object asyncState);

	IMarkedDataResult EndMarkedDataCreate(IAsyncResult asyncResult);

	IMarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption);

	void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption);

	void MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption, object userState);

	IAsyncResult BeginMarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, IRetentionOption retentionOption, AsyncCallback callback, object asyncState);

	IMarkedDataResult EndMarkedDataUpdate(IAsyncResult asyncResult);

	IMarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataId);

	void MarkedDataDeleteAsync(string token, Guid[] markedDataId);

	void MarkedDataDeleteAsync(string token, Guid[] markedDataId, object userState);

	IAsyncResult BeginMarkedDataDelete(string token, Guid[] markedDataId, AsyncCallback callback, object asyncState);

	IMarkedDataResult[] EndMarkedDataDelete(IAsyncResult asyncResult);

	IMarkedData MarkedDataGet(string token, Guid markedDataId);

	void MarkedDataGetAsync(string token, Guid markedDataId);

	void MarkedDataGetAsync(string token, Guid markedDataId, object userState);

	IAsyncResult BeginMarkedDataGet(string token, Guid markedDataId, AsyncCallback callback, object asyncState);

	IMarkedData EndMarkedDataGet(IAsyncResult asyncResult);

	IMarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	void MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, object userState);

	IAsyncResult BeginMarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, AsyncCallback callback, object asyncState);

	IMarkedData[] EndMarkedDataSearch(IAsyncResult asyncResult);
}
