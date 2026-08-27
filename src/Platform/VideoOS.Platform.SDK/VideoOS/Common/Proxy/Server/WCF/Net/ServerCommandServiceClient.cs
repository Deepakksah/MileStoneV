using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using System.Xml;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
public class ServerCommandServiceClient : ClientBase<IServerCommandService>, IServerCommandService
{
	public enum EndpointConfiguration
	{
		BasicHttpBinding_IServerCommandService
	}

	public ServerCommandServiceClient()
		: base(GetDefaultBinding(), GetDefaultEndpointAddress())
	{
		base.Endpoint.Name = EndpointConfiguration.BasicHttpBinding_IServerCommandService.ToString();
	}

	public ServerCommandServiceClient(EndpointConfiguration endpointConfiguration)
		: base(GetBindingForEndpoint(endpointConfiguration), GetEndpointAddress(endpointConfiguration))
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public ServerCommandServiceClient(EndpointConfiguration endpointConfiguration, string remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), new EndpointAddress(remoteAddress))
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public ServerCommandServiceClient(EndpointConfiguration endpointConfiguration, EndpointAddress remoteAddress)
		: base(GetBindingForEndpoint(endpointConfiguration), remoteAddress)
	{
		base.Endpoint.Name = endpointConfiguration.ToString();
	}

	public ServerCommandServiceClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public int GetVersion()
	{
		return base.Channel.GetVersion();
	}

	public Task<int> GetVersionAsync()
	{
		return base.Channel.GetVersionAsync();
	}

	public string GetServerVersion()
	{
		return base.Channel.GetServerVersion();
	}

	public Task<string> GetServerVersionAsync()
	{
		return base.Channel.GetServerVersionAsync();
	}

	public ProductInfo GetProductInfo()
	{
		return base.Channel.GetProductInfo();
	}

	public Task<ProductInfo> GetProductInfoAsync()
	{
		return base.Channel.GetProductInfoAsync();
	}

	public SystemLicenseInfo GetSystemLicenseInfo()
	{
		return base.Channel.GetSystemLicenseInfo();
	}

	public Task<SystemLicenseInfo> GetSystemLicenseInfoAsync()
	{
		return base.Channel.GetSystemLicenseInfoAsync();
	}

	public SmartClientVersion GetSmartClientVersion()
	{
		return base.Channel.GetSmartClientVersion();
	}

	public Task<SmartClientVersion> GetSmartClientVersionAsync()
	{
		return base.Channel.GetSmartClientVersionAsync();
	}

	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		return base.Channel.CheckSmartClientVersion(major, minor, revision);
	}

	public Task<SmartClientVersion> CheckSmartClientVersionAsync(int major, int minor, string revision)
	{
		return base.Channel.CheckSmartClientVersionAsync(major, minor, revision);
	}

	public bool IsOnline()
	{
		return base.Channel.IsOnline();
	}

	public Task<bool> IsOnlineAsync()
	{
		return base.Channel.IsOnlineAsync();
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return base.Channel.Login(instanceId, currentToken);
	}

	public Task<LoginInfo> LoginAsync(Guid instanceId, string currentToken)
	{
		return base.Channel.LoginAsync(instanceId, currentToken);
	}

	public void Logout(Guid instanceId, string currentToken)
	{
		base.Channel.Logout(instanceId, currentToken);
	}

	public Task LogoutAsync(Guid instanceId, string currentToken)
	{
		return base.Channel.LogoutAsync(instanceId, currentToken);
	}

	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		return base.Channel.Authorize(userDomainName, type);
	}

	public Task<string> AuthorizeAsync(string userDomainName, AuthorizationActionType type)
	{
		return base.Channel.AuthorizeAsync(userDomainName, type);
	}

	public UserInfo GetUserInfo(string token)
	{
		return base.Channel.GetUserInfo(token);
	}

	public Task<UserInfo> GetUserInfoAsync(string token)
	{
		return base.Channel.GetUserInfoAsync(token);
	}

	public ConfigurationInfo GetConfiguration(string token)
	{
		return base.Channel.GetConfiguration(token);
	}

	public Task<ConfigurationInfo> GetConfigurationAsync(string token)
	{
		return base.Channel.GetConfigurationAsync(token);
	}

	public RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds)
	{
		return base.Channel.GetConfigurationRecorders(token, recorderIds);
	}

	public Task<RecorderInfo[]> GetConfigurationRecordersAsync(string token, Guid[] recorderIds)
	{
		return base.Channel.GetConfigurationRecordersAsync(token, recorderIds);
	}

	public HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds)
	{
		return base.Channel.GetConfigurationHardware(token, hardwareIds);
	}

	public Task<HardwareInfo[]> GetConfigurationHardwareAsync(string token, Guid[] hardwareIds)
	{
		return base.Channel.GetConfigurationHardwareAsync(token, hardwareIds);
	}

	public DeviceDisabledInfo[] GetDevicesDisabled(string token)
	{
		return base.Channel.GetDevicesDisabled(token);
	}

	public Task<DeviceDisabledInfo[]> GetDevicesDisabledAsync(string token)
	{
		return base.Channel.GetDevicesDisabledAsync(token);
	}

	public System.Collections.DictionaryEntry[] GetVmoServicesList()
	{
		return base.Channel.GetVmoServicesList();
	}

	public Task<System.Collections.DictionaryEntry[]> GetVmoServicesListAsync()
	{
		return base.Channel.GetVmoServicesListAsync();
	}

	public ViewGroupInfo[] GetViewGroups(string token)
	{
		return base.Channel.GetViewGroups(token);
	}

	public Task<ViewGroupInfo[]> GetViewGroupsAsync(string token)
	{
		return base.Channel.GetViewGroupsAsync(token);
	}

	public ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId)
	{
		return base.Channel.GetViewGroupData(token, viewGroupId);
	}

	public Task<ViewGroupInternal> GetViewGroupDataAsync(string token, Guid viewGroupId)
	{
		return base.Channel.GetViewGroupDataAsync(token, viewGroupId);
	}

	public SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		return base.Channel.SetViewGroupData(token, viewGroupId, viewGroupdata);
	}

	public Task<SetViewGroupDataResultType> SetViewGroupDataAsync(string token, Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		return base.Channel.SetViewGroupDataAsync(token, viewGroupId, viewGroupdata);
	}

	public CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal(token, customSettingId);
	}

	public Task<CustomSettingInternal> GetCustomSettingDataGlobalAsync(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobalAsync(token, customSettingId);
	}

	public CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser(token, customSettingId);
	}

	public Task<CustomSettingInternal> GetCustomSettingDataUserAsync(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUserAsync(token, customSettingId);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride);
	}

	public Task<SetCustomSettingResultType> SetCustomSettingDataGlobalAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobalAsync(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride);
	}

	public Task<SetCustomSettingResultType> SetCustomSettingDataUserAsync(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUserAsync(token, customSettingId, customSettingData, forceOverride);
	}

	public CustomSettingInternal GetCustomSettingDataGlobal2(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal2(token, customSettingId);
	}

	public Task<CustomSettingInternal> GetCustomSettingDataGlobal2Async(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataGlobal2Async(token, customSettingId);
	}

	public CustomSettingInternal GetCustomSettingDataUser2(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser2(token, customSettingId);
	}

	public Task<CustomSettingInternal> GetCustomSettingDataUser2Async(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser2Async(token, customSettingId);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal2(token, customSettingId, customSettingData, forceOverride);
	}

	public Task<SetCustomSettingResultType> SetCustomSettingDataGlobal2Async(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal2Async(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser2(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser2(token, customSettingId, customSettingData, forceOverride);
	}

	public Task<SetCustomSettingResultType> SetCustomSettingDataUser2Async(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser2Async(token, customSettingId, customSettingData, forceOverride);
	}

	public void EventTrigger(string token, Guid eventId)
	{
		base.Channel.EventTrigger(token, eventId);
	}

	public Task EventTriggerAsync(string token, Guid eventId)
	{
		return base.Channel.EventTriggerAsync(token, eventId);
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata)
	{
		base.Channel.EventTriggerWithMetadata(token, eventId, metadata);
	}

	public Task EventTriggerWithMetadataAsync(string token, Guid eventId, KeyValue[] metadata)
	{
		return base.Channel.EventTriggerWithMetadataAsync(token, eventId, metadata);
	}

	public AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.AlertsGetAroundWithSpan(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public Task<AlertOccurrence[]> AlertsGetAroundWithSpanAsync(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return base.Channel.AlertsGetAroundWithSpanAsync(token, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter);
	}

	public void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId)
	{
		base.Channel.MatrixMonitorActivateCamera(token, matrixMonitorId, deviceId);
	}

	public Task MatrixMonitorActivateCameraAsync(string token, Guid matrixMonitorId, Guid deviceId)
	{
		return base.Channel.MatrixMonitorActivateCameraAsync(token, matrixMonitorId, deviceId);
	}

	public Change[] QueryChanges(string token, DateTime changesFrom)
	{
		return base.Channel.QueryChanges(token, changesFrom);
	}

	public Task<Change[]> QueryChangesAsync(string token, DateTime changesFrom)
	{
		return base.Channel.QueryChangesAsync(token, changesFrom);
	}

	public RecorderInfo QueryRecorderInfo(string token, Guid recorderId)
	{
		return base.Channel.QueryRecorderInfo(token, recorderId);
	}

	public Task<RecorderInfo> QueryRecorderInfoAsync(string token, Guid recorderId)
	{
		return base.Channel.QueryRecorderInfoAsync(token, recorderId);
	}

	public BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live)
	{
		return base.Channel.BookmarkGetNewReference(token, deviceId, live);
	}

	public Task<BookmarkReference> BookmarkGetNewReferenceAsync(string token, Guid deviceId, bool live)
	{
		return base.Channel.BookmarkGetNewReferenceAsync(token, deviceId, live);
	}

	public Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkCreate(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public Task<Bookmark> BookmarkCreateAsync(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkCreateAsync(token, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkUpdate(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public Task<Bookmark> BookmarkUpdateAsync(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return base.Channel.BookmarkUpdateAsync(token, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
	}

	public void BookmarkDelete(string token, Guid bookmarkId)
	{
		base.Channel.BookmarkDelete(token, bookmarkId);
	}

	public Task BookmarkDeleteAsync(string token, Guid bookmarkId)
	{
		return base.Channel.BookmarkDeleteAsync(token, bookmarkId);
	}

	public Bookmark BookmarkGet(string token, Guid bookmarkId)
	{
		return base.Channel.BookmarkGet(token, bookmarkId);
	}

	public Task<Bookmark> BookmarkGetAsync(string token, Guid bookmarkId)
	{
		return base.Channel.BookmarkGetAsync(token, bookmarkId);
	}

	public Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchTime(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Task<Bookmark[]> BookmarkSearchTimeAsync(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchTimeAsync(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchTime2(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public Task<Bookmark[]> BookmarkSearchTime2Async(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchTime2Async(token, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Task<Bookmark[]> BookmarkSearchFromBookmarkAsync(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchFromBookmarkAsync(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchFromBookmark2(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public Task<Bookmark[]> BookmarkSearchFromBookmark2Async(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return base.Channel.BookmarkSearchFromBookmark2Async(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter);
	}

	public void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		base.Channel.RegisterIntegration(token, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
	}

	public Task RegisterIntegrationAsync(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		return base.Channel.RegisterIntegrationAsync(token, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
	}

	public void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		base.Channel.RegisterPluginIntegrations(token, instanceId, integrationType, pluginIntegrations);
	}

	public Task RegisterPluginIntegrationsAsync(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		return base.Channel.RegisterPluginIntegrationsAsync(token, instanceId, integrationType, pluginIntegrations);
	}

	public MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live)
	{
		return base.Channel.MarkedDataGetNewReference(token, deviceId, live);
	}

	public Task<MarkedDataReference> MarkedDataGetNewReferenceAsync(string token, Guid[] deviceId, bool live)
	{
		return base.Channel.MarkedDataGetNewReferenceAsync(token, deviceId, live);
	}

	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataCreate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public Task<MarkedDataResult> MarkedDataCreateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataCreateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataUpdate(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public Task<MarkedDataResult> MarkedDataUpdateAsync(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return base.Channel.MarkedDataUpdateAsync(token, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption);
	}

	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		return base.Channel.MarkedDataDelete(token, markedDataIds);
	}

	public Task<MarkedDataResult[]> MarkedDataDeleteAsync(string token, Guid[] markedDataIds)
	{
		return base.Channel.MarkedDataDeleteAsync(token, markedDataIds);
	}

	public MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		return base.Channel.MarkedDataGet(token, markedDataId);
	}

	public Task<MarkedData> MarkedDataGetAsync(string token, Guid markedDataId)
	{
		return base.Channel.MarkedDataGetAsync(token, markedDataId);
	}

	public MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public Task<MarkedData[]> MarkedDataSearchAsync(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearchAsync(token, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch2(token, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public Task<MarkedData[]> MarkedDataSearch2Async(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return base.Channel.MarkedDataSearch2Async(token, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
	}

	public RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd);
	}

	public Task<RestrictedMediaResult> RestrictedMediaCreateAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaCreateAsync(id, deviceIds, header, description, timeStart, timeEnd);
	}

	public RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd);
	}

	public Task<RestrictedMediaResult> RestrictedMediaUpdateAsync(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaUpdateAsync(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd);
	}

	public RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		return base.Channel.RestrictedMediaDelete(restrictedMediaId);
	}

	public Task<RestrictedMediaResult> RestrictedMediaDeleteAsync(Guid restrictedMediaId)
	{
		return base.Channel.RestrictedMediaDeleteAsync(restrictedMediaId);
	}

	public RestrictedMedia[] RestrictedMediaQueryAll()
	{
		return base.Channel.RestrictedMediaQueryAll();
	}

	public Task<RestrictedMedia[]> RestrictedMediaQueryAllAsync()
	{
		return base.Channel.RestrictedMediaQueryAllAsync();
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveModeEnter(deviceIds, timeStart);
	}

	public Task<RestrictedMediaLiveResult> RestrictedMediaLiveModeEnterAsync(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveModeEnterAsync(deviceIds, timeStart);
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd);
	}

	public Task<RestrictedMediaResult> RestrictedMediaLiveModeExitAsync(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return base.Channel.RestrictedMediaLiveModeExitAsync(id, deviceIds, header, description, timeStart, timeEnd);
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveUpdate(deviceIds, timeStart);
	}

	public Task<RestrictedMediaLiveResult> RestrictedMediaLiveUpdateAsync(Guid[] deviceIds, DateTime timeStart)
	{
		return base.Channel.RestrictedMediaLiveUpdateAsync(deviceIds, timeStart);
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		return base.Channel.RestrictedMediaLiveDelete(deviceIds);
	}

	public Task<RestrictedMediaLiveResult> RestrictedMediaLiveDeleteAsync(Guid[] deviceIds)
	{
		return base.Channel.RestrictedMediaLiveDeleteAsync(deviceIds);
	}

	public RestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		return base.Channel.RestrictedMediaLiveQueryAll();
	}

	public Task<RestrictedMediaLive[]> RestrictedMediaLiveQueryAllAsync()
	{
		return base.Channel.RestrictedMediaLiveQueryAllAsync();
	}

	public ClientProfile GetClientProfile(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile(token, withSetting);
	}

	public Task<ClientProfile> GetClientProfileAsync(string token, bool withSetting)
	{
		return base.Channel.GetClientProfileAsync(token, withSetting);
	}

	public ClientProfile GetClientProfile2(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile2(token, withSetting);
	}

	public Task<ClientProfile> GetClientProfile2Async(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile2Async(token, withSetting);
	}

	public ClientProfile GetClientProfile3(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile3(token, withSetting);
	}

	public Task<ClientProfile> GetClientProfile3Async(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile3Async(token, withSetting);
	}

	public PresetInfo[] GetPresets(string token, Guid deviceId)
	{
		return base.Channel.GetPresets(token, deviceId);
	}

	public Task<PresetInfo[]> GetPresetsAsync(string token, Guid deviceId)
	{
		return base.Channel.GetPresetsAsync(token, deviceId);
	}

	public void DeletePreset(string token, Guid id, Guid deviceId)
	{
		base.Channel.DeletePreset(token, id, deviceId);
	}

	public Task DeletePresetAsync(string token, Guid id, Guid deviceId)
	{
		return base.Channel.DeletePresetAsync(token, id, deviceId);
	}

	public void UpdatePresetName(string token, Guid id, Guid deviceId, string newName)
	{
		base.Channel.UpdatePresetName(token, id, deviceId, newName);
	}

	public Task UpdatePresetNameAsync(string token, Guid id, Guid deviceId, string newName)
	{
		return base.Channel.UpdatePresetNameAsync(token, id, deviceId, newName);
	}

	public void UpdatePresetPosition(string token, Guid id, Guid deviceId)
	{
		base.Channel.UpdatePresetPosition(token, id, deviceId);
	}

	public Task UpdatePresetPositionAsync(string token, Guid id, Guid deviceId)
	{
		return base.Channel.UpdatePresetPositionAsync(token, id, deviceId);
	}

	public Guid CreatePreset(string token, Guid deviceId, string name)
	{
		return base.Channel.CreatePreset(token, deviceId, name);
	}

	public Task<Guid> CreatePresetAsync(string token, Guid deviceId, string name)
	{
		return base.Channel.CreatePresetAsync(token, deviceId, name);
	}

	public void OrderPresets(string token, Guid deviceId, Guid[] ids)
	{
		base.Channel.OrderPresets(token, deviceId, ids);
	}

	public Task OrderPresetsAsync(string token, Guid deviceId, Guid[] ids)
	{
		return base.Channel.OrderPresetsAsync(token, deviceId, ids);
	}

	public virtual Task OpenAsync()
	{
		return Task.Factory.FromAsync(((ICommunicationObject)this).BeginOpen((AsyncCallback)null, (object)null), ((ICommunicationObject)this).EndOpen);
	}

	private static Binding GetBindingForEndpoint(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IServerCommandService)
		{
			return new BasicHttpBinding
			{
				MaxBufferSize = int.MaxValue,
				ReaderQuotas = XmlDictionaryReaderQuotas.Max,
				MaxReceivedMessageSize = 2147483647L,
				AllowCookies = true,
				Security = 
				{
					Mode = BasicHttpSecurityMode.Transport
				}
			};
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static EndpointAddress GetEndpointAddress(EndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == EndpointConfiguration.BasicHttpBinding_IServerCommandService)
		{
			return new EndpointAddress("https://dkws-apimipsdk1.milestone.dk/ManagementServer/ServerCommandService.svc");
		}
		throw new InvalidOperationException($"Could not find endpoint with name '{endpointConfiguration}'.");
	}

	private static Binding GetDefaultBinding()
	{
		return GetBindingForEndpoint(EndpointConfiguration.BasicHttpBinding_IServerCommandService);
	}

	private static EndpointAddress GetDefaultEndpointAddress()
	{
		return GetEndpointAddress(EndpointConfiguration.BasicHttpBinding_IServerCommandService);
	}
}
