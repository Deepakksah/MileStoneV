using System;
using System.Threading.Tasks;
using System.Xml;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Config;

public interface ILoginServer
{
	ILoginInfo LoginInfo { get; set; }

	ServerTypes ServerType { get; set; }

	UserContext UserContext { get; set; }

	IConfigurationInfo CorporateSystemInfo { get; }

	Exception LastException { get; }

	ISystemLicenseInfo GetSystemLicenseInfo();

	bool IsServerAvailable(Uri uri);

	bool Login(Uri uri);

	void GetGuidAndName();

	bool IsLoggedIn();

	void Logout();

	bool GetSystemInfoAndLogin();

	void RequestNewToken();

	IChange[] QueryChanges(DateTime changesFrom);

	IRecorderInfo QueryRecorderInfo(Guid recorderId);

	IRecorderInfo[] GetRecorderInfos(Guid[] recorderIds);

	IHardwareInfo[] GetHardwareInfos(Guid[] hardwareIds);

	IAlertOccurrence[] GetAlerts(Guid guid, DateTime time, ITimeDuration timeBefore, int maxCountBefore, ITimeDuration timeAfter, int maxCountAfter);

	void MatrixMonitorActivateCamera(Guid matrixGuid, Guid cameraGuid);

	void EventTrigger(Guid eventGuid);

	void EventTriggerWithMetadata(Guid eventGuid, IKeyValue[] metadata);

	ICustomSetting GetCustomSettingDataGlobal(Guid platformDefinitionId);

	ICustomSetting GetCustomSettingDataUser(Guid platformDefinitionId);

	void SetCustomSettingGlobal(Guid id, XmlNode xml, int version, bool global);

	Item GetItemConfiguration(Guid platformDefinitionId, Guid itemKind, Guid itemId);

	IBookmarkReference BookmarkGetNewReference(Guid deviceId, bool live);

	Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	Task<IBookmark[]> BookmarkSearchFromBookmarkAsync(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	Task<IBookmark[]> BookmarkSearchTimeAsync(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	void BookmarkDelete(Guid bookmarkId);

	IBookmark BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	IBookmark[] BookmarkSearchTime(DateTime time, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	IBookmark BookmarkGet(Guid bookmarkId);

	IBookmark BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description);

	IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr);

	IBookmark[] BookmarkSearchFromBookmark(Guid fromBookmarkId, ITimeDuration timeLimit, int countLimit, IMediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter);

	IRestrictedMediaResult RestrictedMediaCreate(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	IRestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId);

	IRestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds);

	IRestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart);

	IRestrictedMediaResult RestrictedMediaLiveModeExit(Guid restrictedMediaId, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd);

	IRestrictedMediaLive[] RestrictedMediaLiveQueryAll();

	IRestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart);

	IRestrictedMedia[] RestrictedMediaQueryAll();

	IRestrictedMediaResult RestrictedMediaUpdate(Guid restrictedMediaId, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd);

	IViewGroup GetViewGroupData(Guid groupId);

	IViewGroupInfo[] GetViewGroups();

	IPresetInfo[] GetPresets(Guid deviceId);

	void DeletePreset(Guid id, Guid deviceId);

	void UpdatePresetName(Guid id, Guid deviceId, string newName);

	void UpdatePresetPosition(Guid id, Guid deviceId);

	Guid CreatePreset(Guid deviceId, string name);

	void OrderPresets(Guid deviceId, Guid[] ids);

	SequenceCollection GetSequences(Guid deviceId, DateTime time, TimeSpan minutesBefore, int maxCountBefore, TimeSpan minutesAfter, int maxCountAfter, Guid seqType);

	void PTZMove(Guid deviceGuid, string direction);

	IDeviceDisabledInfo[] GetDisabledDevices();

	void RegisterIntegration(string instanceId, Guid integrationId, string integrationName, string version, string manufacturerName);
}
