using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.ServiceModel;

namespace VideoOS.Mobile.CorporateBasicLogin;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand", ConfigurationName = "CorporateBasicLogin.IServerCommandService")]
public interface IServerCommandService
{
	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetVersionResponse")]
	int GetVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetServerVersionResponse")]
	string GetServerVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetProductInfoResponse")]
	ProductInfo GetProductInfo();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetSmartClientVersionResponse")]
	SmartClientVersion GetSmartClientVersion();

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersion", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/CheckSmartClientVersionResponse")]
	SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Login", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LoginResponse")]
	LoginInfo Login(Guid instanceId, string currentToken);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/Logout", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/LogoutResponse")]
	void Logout(Guid instanceId, string currentToken);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfo", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetUserInfoResponse")]
	UserInfo GetUserInfo(string token);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfiguration", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetConfigurationResponse")]
	ConfigurationInfo GetConfiguration(string token);

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

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetCustomSettingDataUserResponse")]
	CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobal", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataGlobalResponse")]
	SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUser", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/SetCustomSettingDataUserResponse")]
	SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

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

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmark", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/BookmarkSearchFromBookmarkResponse")]
	Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract(Action = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfile", ReplyAction = "http://videoos.net/2/XProtectCSServerCommand/IServerCommandService/GetClientProfileResponse")]
	ClientProfile GetClientProfile(string token, bool withSetting);
}
