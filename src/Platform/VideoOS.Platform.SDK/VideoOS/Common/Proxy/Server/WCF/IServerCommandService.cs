using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ServiceModel;
using System.Threading.Tasks;

namespace VideoOS.Common.Proxy.Server.WCF;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand", ConfigurationName = "VideoOS.Common.Proxy.Server.WCF.IServerCommandService")]
public interface IServerCommandService
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersionResponse")]
	int GetVersion();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersionResponse")]
	IAsyncResult BeginGetVersion(AsyncCallback callback, object asyncState);

	int EndGetVersion(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersionResponse")]
	string GetServerVersion();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersionResponse")]
	IAsyncResult BeginGetServerVersion(AsyncCallback callback, object asyncState);

	string EndGetServerVersion(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfoResponse")]
	ProductInfo GetProductInfo();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfoResponse")]
	IAsyncResult BeginGetProductInfo(AsyncCallback callback, object asyncState);

	ProductInfo EndGetProductInfo(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfoResponse")]
	SystemLicenseInfo GetSystemLicenseInfo();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSystemLicenseInfoResponse")]
	IAsyncResult BeginGetSystemLicenseInfo(AsyncCallback callback, object asyncState);

	SystemLicenseInfo EndGetSystemLicenseInfo(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersionResponse")]
	SmartClientVersion GetSmartClientVersion();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersionResponse")]
	IAsyncResult BeginGetSmartClientVersion(AsyncCallback callback, object asyncState);

	SmartClientVersion EndGetSmartClientVersion(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersionResponse")]
	SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersionResponse")]
	IAsyncResult BeginCheckSmartClientVersion(int major, int minor, string revision, AsyncCallback callback, object asyncState);

	SmartClientVersion EndCheckSmartClientVersion(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnline", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnlineResponse")]
	bool IsOnline();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnline", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/IsOnlineResponse")]
	IAsyncResult BeginIsOnline(AsyncCallback callback, object asyncState);

	bool EndIsOnline(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Login", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LoginResponse")]
	LoginInfo Login(Guid instanceId, string currentToken);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Login", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LoginResponse")]
	IAsyncResult BeginLogin(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState);

	LoginInfo EndLogin(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Logout", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LogoutResponse")]
	void Logout(Guid instanceId, string currentToken);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Logout", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LogoutResponse")]
	IAsyncResult BeginLogout(Guid instanceId, string currentToken, AsyncCallback callback, object asyncState);

	void EndLogout(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Authorize", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AuthorizeResponse")]
	string Authorize(string userDomainName, AuthorizationActionType type);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Authorize", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AuthorizeResponse")]
	IAsyncResult BeginAuthorize(string userDomainName, AuthorizationActionType type, AsyncCallback callback, object asyncState);

	string EndAuthorize(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfoResponse")]
	UserInfo GetUserInfo(string token);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfoResponse")]
	IAsyncResult BeginGetUserInfo(string token, AsyncCallback callback, object asyncState);

	UserInfo EndGetUserInfo(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfiguration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationResponse")]
	ConfigurationInfo GetConfiguration(string token);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfiguration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationResponse")]
	IAsyncResult BeginGetConfiguration(string token, AsyncCallback callback, object asyncState);

	ConfigurationInfo EndGetConfiguration(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecorders", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecordersResponse")]
	RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecorders", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationRecordersResponse")]
	IAsyncResult BeginGetConfigurationRecorders(string token, Guid[] recorderIds, AsyncCallback callback, object asyncState);

	RecorderInfo[] EndGetConfigurationRecorders(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardware", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardwareResponse")]
	HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardware", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationHardwareResponse")]
	IAsyncResult BeginGetConfigurationHardware(string token, Guid[] hardwareIds, AsyncCallback callback, object asyncState);

	HardwareInfo[] EndGetConfigurationHardware(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabled", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabledResponse")]
	DeviceDisabledInfo[] GetDevicesDisabled(string token);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabled", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetDevicesDisabledResponse")]
	IAsyncResult BeginGetDevicesDisabled(string token, AsyncCallback callback, object asyncState);

	DeviceDisabledInfo[] EndGetDevicesDisabled(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesList", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesListResponse")]
	System.Collections.DictionaryEntry[] GetVmoServicesList();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesList", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVmoServicesListResponse")]
	IAsyncResult BeginGetVmoServicesList(AsyncCallback callback, object asyncState);

	System.Collections.DictionaryEntry[] EndGetVmoServicesList(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroups", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupsResponse")]
	ViewGroupInfo[] GetViewGroups(string token);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroups", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupsResponse")]
	IAsyncResult BeginGetViewGroups(string token, AsyncCallback callback, object asyncState);

	ViewGroupInfo[] EndGetViewGroups(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupDataResponse")]
	ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetViewGroupDataResponse")]
	IAsyncResult BeginGetViewGroupData(string token, Guid viewGroupId, AsyncCallback callback, object asyncState);

	ViewGroupInternal EndGetViewGroupData(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupDataResponse")]
	SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupData", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetViewGroupDataResponse")]
	IAsyncResult BeginSetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata, AsyncCallback callback, object asyncState);

	SetViewGroupDataResultType EndSetViewGroupData(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobalResponse")]
	CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataGlobalResponse")]
	IAsyncResult BeginGetCustomSettingDataGlobal(string token, Guid customSettingId, AsyncCallback callback, object asyncState);

	CustomSettingInternal EndGetCustomSettingDataGlobal(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUserResponse")]
	CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUserResponse")]
	IAsyncResult BeginGetCustomSettingDataUser(string token, Guid customSettingId, AsyncCallback callback, object asyncState);

	CustomSettingInternal EndGetCustomSettingDataUser(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobalResponse")]
	SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobalResponse")]
	IAsyncResult BeginSetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState);

	SetCustomSettingResultType EndSetCustomSettingDataGlobal(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUserResponse")]
	SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUserResponse")]
	IAsyncResult BeginSetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride, AsyncCallback callback, object asyncState);

	SetCustomSettingResultType EndSetCustomSettingDataUser(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTrigger", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerResponse")]
	void EventTrigger(string token, Guid eventId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTrigger", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerResponse")]
	IAsyncResult BeginEventTrigger(string token, Guid eventId, AsyncCallback callback, object asyncState);

	void EndEventTrigger(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadata", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadataResponse")]
	void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadata", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/EventTriggerWithMetadataResponse")]
	IAsyncResult BeginEventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, AsyncCallback callback, object asyncState);

	void EndEventTriggerWithMetadata(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpan", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpanResponse")]
	AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpan", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/AlertsGetAroundWithSpanResponse")]
	IAsyncResult BeginAlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter, AsyncCallback callback, object asyncState);

	AlertOccurrence[] EndAlertsGetAroundWithSpan(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCamera", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCameraResponse")]
	void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCamera", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MatrixMonitorActivateCameraResponse")]
	IAsyncResult BeginMatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndMatrixMonitorActivateCamera(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChanges", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChangesResponse")]
	Change[] QueryChanges(string token, DateTime changesFrom);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChanges", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryChangesResponse")]
	IAsyncResult BeginQueryChanges(string token, DateTime changesFrom, AsyncCallback callback, object asyncState);

	Change[] EndQueryChanges(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfoResponse")]
	RecorderInfo QueryRecorderInfo(string token, Guid recorderId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/QueryRecorderInfoResponse")]
	IAsyncResult BeginQueryRecorderInfo(string token, Guid recorderId, AsyncCallback callback, object asyncState);

	RecorderInfo EndQueryRecorderInfo(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReferenceResponse")]
	BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetNewReferenceResponse")]
	IAsyncResult BeginBookmarkGetNewReference(string token, Guid deviceId, bool live, AsyncCallback callback, object asyncState);

	BookmarkReference EndBookmarkGetNewReference(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreateResponse")]
	Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkCreateResponse")]
	IAsyncResult BeginBookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState);

	Bookmark EndBookmarkCreate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdateResponse")]
	Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkUpdateResponse")]
	IAsyncResult BeginBookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description, AsyncCallback callback, object asyncState);

	Bookmark EndBookmarkUpdate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDeleteResponse")]
	void BookmarkDelete(string token, Guid bookmarkId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkDeleteResponse")]
	IAsyncResult BeginBookmarkDelete(string token, Guid bookmarkId, AsyncCallback callback, object asyncState);

	void EndBookmarkDelete(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetResponse")]
	Bookmark BookmarkGet(string token, Guid bookmarkId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkGetResponse")]
	IAsyncResult BeginBookmarkGet(string token, Guid bookmarkId, AsyncCallback callback, object asyncState);

	Bookmark EndBookmarkGet(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTimeResponse")]
	Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTimeResponse")]
	IAsyncResult BeginBookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState);

	Bookmark[] EndBookmarkSearchTime(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2Response")]
	Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchTime2Response")]
	IAsyncResult BeginBookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallback callback, object asyncState);

	Bookmark[] EndBookmarkSearchTime2(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmarkResponse")]
	Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmarkResponse")]
	IAsyncResult BeginBookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, AsyncCallback callback, object asyncState);

	Bookmark[] EndBookmarkSearchFromBookmark(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2Response")]
	Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark2Response")]
	IAsyncResult BeginBookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, AsyncCallback callback, object asyncState);

	Bookmark[] EndBookmarkSearchFromBookmark2(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegrationResponse")]
	void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterIntegrationResponse")]
	Task RegisterIntegrationAsync(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrations", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrationsResponse")]
	void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrations", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RegisterPluginIntegrationsResponse")]
	Task RegisterPluginIntegrationsAsync(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReferenceResponse")]
	MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReference", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetNewReferenceResponse")]
	IAsyncResult BeginMarkedDataGetNewReference(string token, Guid[] deviceId, bool live, AsyncCallback callback, object asyncState);

	MarkedDataReference EndMarkedDataGetNewReference(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreateResponse")]
	MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataCreateResponse")]
	IAsyncResult BeginMarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState);

	MarkedDataResult EndMarkedDataCreate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdateResponse")]
	MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataUpdateResponse")]
	IAsyncResult BeginMarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption, AsyncCallback callback, object asyncState);

	MarkedDataResult EndMarkedDataUpdate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDeleteResponse")]
	MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataDeleteResponse")]
	IAsyncResult BeginMarkedDataDelete(string token, Guid[] markedDataIds, AsyncCallback callback, object asyncState);

	MarkedDataResult[] EndMarkedDataDelete(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetResponse")]
	MarkedData MarkedDataGet(string token, Guid markedDataId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGet", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataGetResponse")]
	IAsyncResult BeginMarkedDataGet(string token, Guid markedDataId, AsyncCallback callback, object asyncState);

	MarkedData EndMarkedDataGet(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearch", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearchResponse")]
	MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearch", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/MarkedDataSearchResponse")]
	IAsyncResult BeginMarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending, AsyncCallback callback, object asyncState);

	MarkedData[] EndMarkedDataSearch(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreateResponse")]
	RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaCreateResponse")]
	IAsyncResult BeginRestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState);

	RestrictedMediaResult EndRestrictedMediaCreate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdateResponse")]
	RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaUpdateResponse")]
	IAsyncResult BeginRestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState);

	RestrictedMediaResult EndRestrictedMediaUpdate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDeleteResponse")]
	RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaDeleteResponse")]
	IAsyncResult BeginRestrictedMediaDelete(Guid restrictedMediaId, AsyncCallback callback, object asyncState);

	RestrictedMediaResult EndRestrictedMediaDelete(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAllResponse")]
	RestrictedMedia[] RestrictedMediaQueryAll();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaQueryAllResponse")]
	IAsyncResult BeginRestrictedMediaQueryAll(AsyncCallback callback, object asyncState);

	RestrictedMedia[] EndRestrictedMediaQueryAll(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnter", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnterResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnter", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeEnterResponse")]
	IAsyncResult BeginRestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart, AsyncCallback callback, object asyncState);

	RestrictedMediaLiveResult EndRestrictedMediaLiveModeEnter(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExit", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExitResponse")]
	RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExit", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveModeExitResponse")]
	IAsyncResult BeginRestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd, AsyncCallback callback, object asyncState);

	RestrictedMediaResult EndRestrictedMediaLiveModeExit(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdateResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdate", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveUpdateResponse")]
	IAsyncResult BeginRestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart, AsyncCallback callback, object asyncState);

	RestrictedMediaLiveResult EndRestrictedMediaLiveUpdate(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDeleteResponse")]
	RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDelete", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveDeleteResponse")]
	IAsyncResult BeginRestrictedMediaLiveDelete(Guid[] deviceIds, AsyncCallback callback, object asyncState);

	RestrictedMediaLiveResult EndRestrictedMediaLiveDelete(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAllResponse")]
	RestrictedMediaLive[] RestrictedMediaLiveQueryAll();

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAll", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/RestrictedMediaLiveQueryAllResponse")]
	IAsyncResult BeginRestrictedMediaLiveQueryAll(AsyncCallback callback, object asyncState);

	RestrictedMediaLive[] EndRestrictedMediaLiveQueryAll(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfileResponse")]
	ClientProfile GetClientProfile(string token, bool withSetting);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfileResponse")]
	IAsyncResult BeginGetClientProfile(string token, bool withSetting, AsyncCallback callback, object asyncState);

	ClientProfile EndGetClientProfile(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresetsResponse")]
	PresetInfo[] GetPresets(string token, Guid deviceId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetPresetsResponse")]
	IAsyncResult BeginGetPresets(string token, Guid deviceId, AsyncCallback callback, object asyncState);

	PresetInfo[] EndGetPresets(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePresetResponse")]
	void DeletePreset(string token, Guid id, Guid deviceId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/DeletePresetResponse")]
	IAsyncResult BeginDeletePreset(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndDeletePreset(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetName", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetNameResponse")]
	void UpdatePresetName(string token, Guid id, Guid deviceId, string newName);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetName", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetNameResponse")]
	IAsyncResult BeginUpdatePresetName(string token, Guid id, Guid deviceId, string newName, AsyncCallback callback, object asyncState);

	void EndUpdatePresetName(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPosition", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPositionResponse")]
	void UpdatePresetPosition(string token, Guid id, Guid deviceId);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPosition", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/UpdatePresetPositionResponse")]
	IAsyncResult BeginUpdatePresetPosition(string token, Guid id, Guid deviceId, AsyncCallback callback, object asyncState);

	void EndUpdatePresetPosition(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePresetResponse")]
	Guid CreatePreset(string token, Guid deviceId, string name);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePreset", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CreatePresetResponse")]
	IAsyncResult BeginCreatePreset(string token, Guid deviceId, string name, AsyncCallback callback, object asyncState);

	Guid EndCreatePreset(IAsyncResult result);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresetsResponse")]
	void OrderPresets(string token, Guid deviceId, Guid[] ids);

	[OperationContract(AsyncPattern = true, Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresets", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/OrderPresetsResponse")]
	IAsyncResult BeginOrderPresets(string token, Guid deviceId, Guid[] ids, AsyncCallback callback, object asyncState);

	void EndOrderPresets(IAsyncResult result);
}
