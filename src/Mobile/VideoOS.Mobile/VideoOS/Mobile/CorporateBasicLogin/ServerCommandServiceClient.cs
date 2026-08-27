using System;
using System.CodeDom.Compiler;
using System.Collections;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;

namespace VideoOS.Mobile.CorporateBasicLogin;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class ServerCommandServiceClient : ClientBase<IServerCommandService>, IServerCommandService
{
	public ServerCommandServiceClient()
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

	public SmartClientVersion GetSmartClientVersion()
	{
		return base.Channel.GetSmartClientVersion();
	}

	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		return base.Channel.CheckSmartClientVersion(major, minor, revision);
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		return base.Channel.Login(instanceId, currentToken);
	}

	public void Logout(Guid instanceId, string currentToken)
	{
		base.Channel.Logout(instanceId, currentToken);
	}

	public UserInfo GetUserInfo(string token)
	{
		return base.Channel.GetUserInfo(token);
	}

	public ConfigurationInfo GetConfiguration(string token)
	{
		return base.Channel.GetConfiguration(token);
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

	public CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		return base.Channel.GetCustomSettingDataUser(token, customSettingId);
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataGlobal(token, customSettingId, customSettingData, forceOverride);
	}

	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingData, bool forceOverride)
	{
		return base.Channel.SetCustomSettingDataUser(token, customSettingId, customSettingData, forceOverride);
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

	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		return base.Channel.BookmarkSearchFromBookmark(token, fromBookmarkId, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr);
	}

	public ClientProfile GetClientProfile(string token, bool withSetting)
	{
		return base.Channel.GetClientProfile(token, withSetting);
	}
}
