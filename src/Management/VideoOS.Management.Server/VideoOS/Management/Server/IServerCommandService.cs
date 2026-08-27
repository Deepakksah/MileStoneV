using System;
using System.Collections;
using System.ServiceModel;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

[ServiceContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public interface IServerCommandService
{
	[OperationContract]
	int GetVersion();

	[OperationContract]
	string GetServerVersion();

	[OperationContract]
	ProductInfo GetProductInfo();

	[OperationContract]
	SystemLicenseInfo GetSystemLicenseInfo();

	[OperationContract]
	SmartClientVersion GetSmartClientVersion();

	[OperationContract]
	SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision);

	[OperationContract]
	bool IsOnline();

	[OperationContract]
	LoginInfo Login(Guid instanceId, string currentToken);

	[OperationContract]
	void Logout(Guid instanceId, string currentToken);

	[OperationContract]
	string Authorize(string userDomainName, AuthorizationActionType type);

	[OperationContract]
	UserInfo GetUserInfo(string token);

	[OperationContract]
	ConfigurationInfo GetConfiguration(string token);

	[OperationContract]
	RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds);

	[OperationContract]
	HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds);

	[OperationContract]
	DeviceDisabledInfo[] GetDevicesDisabled(string token);

	[OperationContract]
	DictionaryEntry[] GetVmoServicesList();

	[OperationContract]
	ViewGroupInfo[] GetViewGroups(string token);

	[OperationContract]
	ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId);

	[OperationContract]
	SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata);

	[OperationContract]
	CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId);

	[OperationContract]
	CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId);

	[OperationContract]
	SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract]
	SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract]
	CustomSettingInternal GetCustomSettingDataGlobal2(string token, Guid customSettingId);

	[OperationContract]
	CustomSettingInternal GetCustomSettingDataUser2(string token, Guid customSettingId);

	[OperationContract]
	SetCustomSettingResultType SetCustomSettingDataGlobal2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract]
	SetCustomSettingResultType SetCustomSettingDataUser2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride);

	[OperationContract]
	void EventTrigger(string token, Guid eventId);

	[OperationContract]
	void EventTriggerWithMetadata(string token, Guid eventId, VideoOS.WebService.Common.ServerCommandService.KeyValue[] metadata);

	[OperationContract]
	AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter);

	[OperationContract]
	void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId);

	[OperationContract]
	Change[] QueryChanges(string token, DateTime changesFrom);

	[OperationContract]
	RecorderInfo QueryRecorderInfo(string token, Guid recorderId);

	[OperationContract]
	BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live);

	[OperationContract]
	Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract]
	Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	[OperationContract]
	void BookmarkDelete(string token, Guid bookmarkId);

	[OperationContract]
	Bookmark BookmarkGet(string token, Guid bookmarkId);

	[OperationContract]
	Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract]
	Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract]
	Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	[OperationContract]
	Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	[OperationContract]
	void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName);

	[OperationContract]
	void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live);

	[OperationContract]
	MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract]
	MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption);

	[OperationContract]
	MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData MarkedDataGet(string token, Guid markedDataId);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.RestrictedMedia[] RestrictedMediaQueryAll();

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart);

	[OperationContract]
	RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart);

	[OperationContract]
	RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	[OperationContract]
	RestrictedMediaLive[] RestrictedMediaLiveQueryAll();

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile(string token, bool withSetting);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile2(string token, bool withSetting);

	[OperationContract]
	VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile3(string token, bool withSetting);

	[OperationContract]
	PresetInfo[] GetPresets(string token, Guid deviceId);

	[OperationContract]
	void DeletePreset(string token, Guid id, Guid deviceId);

	[OperationContract]
	void UpdatePresetName(string token, Guid id, Guid deviceId, string newName);

	[OperationContract]
	void UpdatePresetPosition(string token, Guid id, Guid deviceId);

	[OperationContract]
	Guid CreatePreset(string token, Guid deviceId, string name);

	[OperationContract]
	void OrderPresets(string token, Guid deviceId, Guid[] ids);
}
