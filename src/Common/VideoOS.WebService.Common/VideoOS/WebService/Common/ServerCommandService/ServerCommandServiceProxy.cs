using System;
using System.Collections;
using System.ServiceModel;
using System.ServiceModel.Channels;
using VideoOS.IdentityServer.Common.Handlers;
using VideoOS.WebService.Common.Framework;

namespace VideoOS.WebService.Common.ServerCommandService;

public class ServerCommandServiceProxy : WebServiceManagerBase<ServerCommandServiceClient, IServerCommandService>, IServerCommandServiceProxy, ITimeoutSettings, IAdditionalMessageHeaders, IDisposable
{
	private const string AuthorizationIdHeaderName = "AuthorizationId";

	private string dummyToken = string.Empty;

	public static IServerCommandServiceProxy CreateNew(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServerCommandServiceProxy(hostName, port, runSecure, tokenCache, exceptionHandler);
	}

	private ServerCommandServiceProxy(string hostName, int port, bool runSecure, ITokenCache tokenCache, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(runSecure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServerCommandServiceOAuth.svc").Uri, "http://videoos.net/2/XProtectCSServerCommand", tokenCache, exceptionHandler)
	{
	}

	public static IServerCommandServiceProxy CreateNew(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServerCommandServiceProxy(hostName, port, exceptionHandler);
	}

	public static IServerCommandServiceProxy CreateNew(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServerCommandServiceProxy(hostName, port, userName, password, domain, exceptionHandler);
	}

	public static IServerCommandServiceProxy CreateNew(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler = null)
	{
		return new ServerCommandServiceProxy(hostName, securePort, userName, password, exceptionHandler);
	}

	public ServerCommandServiceProxy(string hostName, int port, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServerCommandService.svc").Uri, "http://videoos.net/2/XProtectCSServerCommand", exceptionHandler)
	{
	}

	private ServerCommandServiceProxy(string hostName, int port, string userName, string password, string domain, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttp, hostName, port, "ManagementServer/ServerCommandService.svc").Uri, "http://videoos.net/2/XProtectCSServerCommand", userName, password, domain, exceptionHandler)
	{
	}

	private ServerCommandServiceProxy(string hostName, int securePort, string userName, string password, IWebServiceManagerExceptionHandler exceptionHandler)
		: base(new UriBuilder(Uri.UriSchemeHttps, hostName, securePort, "ManagementServer/ServerCommandService.svc").Uri, "http://videoos.net/2/XProtectCSServerCommand", userName, password, exceptionHandler)
	{
	}

	protected override ServerCommandServiceClient CreateClientObject(Binding binding, EndpointAddress remoteAddress)
	{
		return new ServerCommandServiceClient(binding, remoteAddress);
	}

	void IAdditionalMessageHeaders.AddMessageHeader(string ns, string name, string content)
	{
		AddMessageHeader(ns, name, content);
	}

	void IServerCommandServiceProxy.Abort()
	{
		base.Instance.Abort();
	}

	void IServerCommandServiceProxy.SetDualAutorizationId(string authorizationId)
	{
		AddMessageHeader("http://videoos.net/2/XProtectCSServerCommand", "AuthorizationId", authorizationId);
	}

	int IServerCommandServiceProxy.GetVersion()
	{
		return CallWithRetry((IServerCommandService c) => c.GetVersion());
	}

	string IServerCommandServiceProxy.GetServerVersion()
	{
		return CallWithRetry((IServerCommandService c) => c.GetServerVersion());
	}

	ProductInfo IServerCommandServiceProxy.GetProductInfo()
	{
		return CallWithRetry((IServerCommandService c) => c.GetProductInfo());
	}

	SystemLicenseInfo IServerCommandServiceProxy.GetSystemLicenseInfo()
	{
		return CallWithRetry((IServerCommandService c) => c.GetSystemLicenseInfo());
	}

	SmartClientVersion IServerCommandServiceProxy.GetSmartClientVersion()
	{
		return CallWithRetry((IServerCommandService c) => c.GetSmartClientVersion());
	}

	SmartClientVersion IServerCommandServiceProxy.CheckSmartClientVersion(int major, int minor, string revision)
	{
		return CallWithRetry((IServerCommandService c) => c.CheckSmartClientVersion(major, minor, revision));
	}

	bool IServerCommandServiceProxy.IsOnline()
	{
		return CallWithRetry((IServerCommandService c) => c.IsOnline());
	}

	LoginInfo IServerCommandServiceProxy.Login(Guid instanceId, string currentToken)
	{
		return CallWithRetry((IServerCommandService c) => c.Login(instanceId, currentToken));
	}

	void IServerCommandServiceProxy.Logout(Guid instanceId, string currentToken)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.Logout(instanceId, currentToken);
		});
	}

	string IServerCommandServiceProxy.Authorize(string userDomainName, AuthorizationActionType type)
	{
		return CallWithRetry((IServerCommandService c) => c.Authorize(userDomainName, type));
	}

	UserInfo IServerCommandServiceProxy.GetUserInfo(string token)
	{
		return CallWithRetry((IServerCommandService c) => c.GetUserInfo(token));
	}

	ConfigurationInfo IServerCommandServiceProxy.GetConfiguration()
	{
		return CallWithRetry((IServerCommandService c) => c.GetConfiguration(dummyToken));
	}

	RecorderInfo[] IServerCommandServiceProxy.GetConfigurationRecorders(Guid[] recorderIds)
	{
		return CallWithRetry((IServerCommandService c) => c.GetConfigurationRecorders(dummyToken, recorderIds));
	}

	HardwareInfo[] IServerCommandServiceProxy.GetConfigurationHardware(Guid[] hardwareIds)
	{
		return CallWithRetry((IServerCommandService c) => c.GetConfigurationHardware(dummyToken, hardwareIds));
	}

	DeviceDisabledInfo[] IServerCommandServiceProxy.GetDevicesDisabled()
	{
		return CallWithRetry((IServerCommandService c) => c.GetDevicesDisabled(dummyToken));
	}

	DictionaryEntry[] IServerCommandServiceProxy.GetVmoServicesList()
	{
		return CallWithRetry((IServerCommandService c) => c.GetVmoServicesList());
	}

	ViewGroupInfo[] IServerCommandServiceProxy.GetViewGroups()
	{
		return CallWithRetry((IServerCommandService c) => c.GetViewGroups(dummyToken));
	}

	ViewGroupInternal IServerCommandServiceProxy.GetViewGroupData(Guid viewGroupId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetViewGroupData(dummyToken, viewGroupId));
	}

	SetViewGroupDataResultType IServerCommandServiceProxy.SetViewGroupData(Guid viewGroupId, ViewGroupInternalData viewGroupdata)
	{
		return CallWithRetry((IServerCommandService c) => c.SetViewGroupData(dummyToken, viewGroupId, viewGroupdata));
	}

	CustomSettingInternal IServerCommandServiceProxy.GetCustomSettingDataGlobal(Guid customSettingId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetCustomSettingDataGlobal(dummyToken, customSettingId));
	}

	CustomSettingInternal IServerCommandServiceProxy.GetCustomSettingDataGlobal2(Guid customSettingId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetCustomSettingDataGlobal2(dummyToken, customSettingId));
	}

	CustomSettingInternal IServerCommandServiceProxy.GetCustomSettingDataUser(Guid customSettingId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetCustomSettingDataUser(dummyToken, customSettingId));
	}

	CustomSettingInternal IServerCommandServiceProxy.GetCustomSettingDataUser2(Guid customSettingId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetCustomSettingDataUser2(dummyToken, customSettingId));
	}

	SetCustomSettingResultType IServerCommandServiceProxy.SetCustomSettingDataGlobal(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return CallWithRetry((IServerCommandService c) => c.SetCustomSettingDataGlobal(dummyToken, customSettingId, customSettingData, forceOverride));
	}

	SetCustomSettingResultType IServerCommandServiceProxy.SetCustomSettingDataGlobal2(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return CallWithRetry((IServerCommandService c) => c.SetCustomSettingDataGlobal2(dummyToken, customSettingId, customSettingData, forceOverride));
	}

	SetCustomSettingResultType IServerCommandServiceProxy.SetCustomSettingDataUser(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return CallWithRetry((IServerCommandService c) => c.SetCustomSettingDataUser(dummyToken, customSettingId, customSettingData, forceOverride));
	}

	SetCustomSettingResultType IServerCommandServiceProxy.SetCustomSettingDataUser2(Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return CallWithRetry((IServerCommandService c) => c.SetCustomSettingDataUser2(dummyToken, customSettingId, customSettingData, forceOverride));
	}

	void IServerCommandServiceProxy.EventTrigger(Guid eventId)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.EventTrigger(dummyToken, eventId);
		});
	}

	void IServerCommandServiceProxy.EventTriggerWithMetadata(Guid eventId, KeyValue[] metadata)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.EventTriggerWithMetadata(dummyToken, eventId, metadata);
		});
	}

	AlertOccurrence[] IServerCommandServiceProxy.AlertsGetAroundWithSpan(Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		return CallWithRetry((IServerCommandService c) => c.AlertsGetAroundWithSpan(dummyToken, alertIds, centerTime, maxTimeBefore, maxCountBefore, maxTimeAfter, maxCountAfter));
	}

	void IServerCommandServiceProxy.MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.MatrixMonitorActivateCamera(dummyToken, matrixMonitorId, deviceId);
		});
	}

	Change[] IServerCommandServiceProxy.QueryChanges(DateTime changesFrom)
	{
		return CallWithRetry((IServerCommandService c) => c.QueryChanges(dummyToken, changesFrom));
	}

	RecorderInfo IServerCommandServiceProxy.QueryRecorderInfo(Guid recorderId)
	{
		return CallWithRetry((IServerCommandService c) => c.QueryRecorderInfo(dummyToken, recorderId));
	}

	BookmarkReference IServerCommandServiceProxy.BookmarkGetNewReference(Guid deviceId, bool live)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkGetNewReference(dummyToken, deviceId, live));
	}

	Bookmark IServerCommandServiceProxy.BookmarkCreate(Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkCreate(dummyToken, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description));
	}

	Bookmark IServerCommandServiceProxy.BookmarkUpdate(Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkUpdate(dummyToken, bookmarkId, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description));
	}

	void IServerCommandServiceProxy.BookmarkDelete(Guid bookmarkId)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.BookmarkDelete(dummyToken, bookmarkId);
		});
	}

	Bookmark IServerCommandServiceProxy.BookmarkGet(Guid bookmarkId)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkGet(dummyToken, bookmarkId));
	}

	Bookmark[] IServerCommandServiceProxy.BookmarkSearchTime(DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkSearchTime(dummyToken, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr));
	}

	Bookmark[] IServerCommandServiceProxy.BookmarkSearchTime2(DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkSearchTime2(dummyToken, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter));
	}

	Bookmark[] IServerCommandServiceProxy.BookmarkSearchFromBookmark(Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkSearchFromBookmark(dummyToken, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr));
	}

	Bookmark[] IServerCommandServiceProxy.BookmarkSearchFromBookmark2(Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		return CallWithRetry((IServerCommandService c) => c.BookmarkSearchFromBookmark2(dummyToken, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter));
	}

	void IServerCommandServiceProxy.RegisterIntegration(string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.RegisterIntegration(dummyToken, instanceId, integrationId, integrationVersion, integrationName, manufacturerName);
		});
	}

	void IServerCommandServiceProxy.RegisterPluginIntegrations(string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.RegisterPluginIntegrations(dummyToken, instanceId, integrationType, pluginIntegrations);
		});
	}

	SystemMonitorStateSource[] IServerCommandServiceProxy.QuerySystemMonitorStateSourcesByTypes(string[] types)
	{
		return CallWithRetry((IServerCommandService c) => c.QuerySystemMonitorStateSourcesByTypes(types));
	}

	SystemMonitorStateSource[] IServerCommandServiceProxy.QuerySystemMonitorStateSourcesByIds(string[] ids)
	{
		return CallWithRetry((IServerCommandService c) => c.QuerySystemMonitorStateSourcesByIds(ids));
	}

	MarkedDataReference IServerCommandServiceProxy.MarkedDataGetNewReference(Guid[] deviceId, bool live)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataGetNewReference(dummyToken, deviceId, live));
	}

	MarkedDataResult IServerCommandServiceProxy.MarkedDataCreate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataCreate(dummyToken, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption));
	}

	MarkedDataResult IServerCommandServiceProxy.MarkedDataUpdate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataUpdate(dummyToken, markedDataId, deviceIds, timeStart, timeTag, timeEnd, reference, header, description, markedDataType, useRetention, retentionExpire, retentionOption));
	}

	MarkedDataResult[] IServerCommandServiceProxy.MarkedDataDelete(Guid[] markedDataIds)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataDelete(dummyToken, markedDataIds));
	}

	MarkedData IServerCommandServiceProxy.MarkedDataGet(Guid markedDataId)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataGet(dummyToken, markedDataId));
	}

	MarkedData[] IServerCommandServiceProxy.MarkedDataSearch(Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataSearch(dummyToken, deviceIds, searchText, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending));
	}

	MarkedData[] IServerCommandServiceProxy.MarkedDataSearch2(Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		return CallWithRetry((IServerCommandService c) => c.MarkedDataSearch2(dummyToken, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending));
	}

	RestrictedMediaResult IServerCommandServiceProxy.RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaCreate(id, deviceIds, header, description, timeStart, timeEnd));
	}

	RestrictedMediaResult IServerCommandServiceProxy.RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaUpdate(id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd));
	}

	RestrictedMediaResult IServerCommandServiceProxy.RestrictedMediaDelete(Guid restrictedMediaId)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaDelete(restrictedMediaId));
	}

	RestrictedMedia[] IServerCommandServiceProxy.RestrictedMediaQueryAll()
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaQueryAll());
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaLiveModeEnter(deviceIds, timeStart));
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaLiveModeExit(id, deviceIds, header, description, timeStart, timeEnd));
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaLiveUpdate(deviceIds, timeStart));
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaLiveDelete(deviceIds));
	}

	public RestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		return CallWithRetry((IServerCommandService c) => c.RestrictedMediaLiveQueryAll());
	}

	ClientProfile IServerCommandServiceProxy.GetClientProfile(bool withSetting)
	{
		return CallWithRetry((IServerCommandService c) => c.GetClientProfile(dummyToken, withSetting));
	}

	ClientProfile IServerCommandServiceProxy.GetClientProfile2(bool withSetting)
	{
		return CallWithRetry((IServerCommandService c) => c.GetClientProfile2(dummyToken, withSetting));
	}

	ClientProfile IServerCommandServiceProxy.GetClientProfile3(bool withSetting)
	{
		return CallWithRetry((IServerCommandService c) => c.GetClientProfile3(dummyToken, withSetting));
	}

	PresetInfo[] IServerCommandServiceProxy.GetPresets(Guid deviceId)
	{
		return CallWithRetry((IServerCommandService c) => c.GetPresets(dummyToken, deviceId));
	}

	void IServerCommandServiceProxy.DeletePreset(Guid id, Guid deviceId)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.DeletePreset(dummyToken, id, deviceId);
		});
	}

	void IServerCommandServiceProxy.UpdatePresetName(Guid id, Guid deviceId, string newName)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.UpdatePresetName(dummyToken, id, deviceId, newName);
		});
	}

	void IServerCommandServiceProxy.UpdatePresetPosition(Guid id, Guid deviceId)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.UpdatePresetPosition(dummyToken, id, deviceId);
		});
	}

	Guid IServerCommandServiceProxy.CreatePreset(Guid deviceId, string name)
	{
		return CallWithRetry((IServerCommandService c) => c.CreatePreset(dummyToken, deviceId, name));
	}

	void IServerCommandServiceProxy.OrderPresets(Guid deviceId, Guid[] ids)
	{
		CallWithRetry(delegate(IServerCommandService c)
		{
			c.OrderPresets(dummyToken, deviceId, ids);
		});
	}
}
