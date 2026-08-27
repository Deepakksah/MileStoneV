using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Web;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.ServerAPI;
using VideoOS.Management.Server.GetConfiguration;
using VideoOS.Server.Webservice.Shared;
using VideoOS.Utilities.Network;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServerCommandService : GeminiWebServiceBase, IServerCommandService
{
	public ServerCommandService()
		: base(connect: false)
	{
	}

	public int GetVersion()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetVersion");
			EnterWebMethod(methodInformation);
			if (CheckDevelopmentRegistry.FakeHigherServerVersion)
			{
				Version version = new Version(BuildInfo.ProductVersion);
				return ServerVersion.GetProtocolVersion(new Version(version.Major + 1, version.Minor, version.Build, version.Revision));
			}
			return ServerVersion.GetProtocolVersion(BuildInfo.ProductVersion);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetServerVersion()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServerVersion");
			EnterWebMethod(methodInformation);
			return ServerVersion.GetServerVersion(Assembly.GetExecutingAssembly());
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ProductInfo GetProductInfo()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetProductInfo");
			EnterWebMethod(methodInformation);
			return CommandApiHandling.GetProductInfo();
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SystemLicenseInfo GetSystemLicenseInfo()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSystemLicenseInfo");
			EnterWebMethod(methodInformation);
			return CommandApiHandling.GetSystemLicenseInfo();
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SmartClientVersion GetSmartClientVersion()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSmartClientVersion");
			EnterWebMethod(methodInformation);
			return new ClientVersionHandling().GetSmartClientVersion(base.RequestContext);
		}
		catch (Exception)
		{
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool IsOnline()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("IsOnline");
			EnterWebMethod(methodInformation);
			return ServerInformation.GetServerStatus() == 1;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SmartClientVersion CheckSmartClientVersion(int major, int minor, string revision)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CheckSmartClientVersion");
			methodInformation.AddParameter("major", major);
			methodInformation.AddParameter("minor", minor);
			methodInformation.AddParameter("revision", revision);
			EnterWebMethod(methodInformation);
			return new ClientVersionHandling().CheckSmartClientVersion(base.RequestContext, major, minor, revision);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public LoginInfo Login(Guid instanceId, string currentToken)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("Login");
			methodInformation.AddParameter("instanceId", instanceId);
			methodInformation.AddParameter("currentToken", currentToken);
			EnterWebMethod(methodInformation);
			TimeSpan timeSpan = new TimeSpan(0L);
			bool flag = false;
			IDualAuthorizationHandling dualAuthorizationHandling = new DualAuthorizationHandling();
			string authorizationId = string.Empty;
			BasicCallContextInfo callContextInfo = GetCallContextInfo();
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				authorizationId = GetAuthorizationIdFromHeader();
				flag = dualAuthorizationHandling.CheckDualAuthorization(base.RequestContext, authorizationId, AuthorizationActionType.LogOn);
			}
			bool isRefreshToken = false;
			if (!string.IsNullOrEmpty(currentToken))
			{
				using TokenValidator tokenValidator = new TokenValidator(base.RequestContext);
				isRefreshToken = tokenValidator.CheckToken(currentToken) == TokenStatus.Ok;
			}
			SecurityToken securityToken = ServerSecurityToken.Login(base.RequestContext, callContextInfo.LocationIPAddress, isRefreshToken);
			LoginInfo loginInfo = new LoginInfo();
			loginInfo.Token = securityToken.Value;
			loginInfo.RegistrationTime = securityToken.RegistrationTime;
			loginInfo.TimeToLive = new TimeDuration();
			loginInfo.TimeToLive.MicroSeconds = securityToken.Expires.Ticks / 10;
			loginInfo.TimeToLiveLimited = securityToken.ExpireTimeLimited;
			timeSpan = securityToken.Expires;
			if (flag)
			{
				dualAuthorizationHandling.ExtendCookieTime(base.RequestContext, authorizationId, timeSpan);
			}
			return loginInfo;
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void Logout(Guid instanceId, string currentToken)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("Logout");
			methodInformation.AddParameter("instanceId", instanceId);
			methodInformation.AddParameter("currentToken", currentToken);
			EnterWebMethod(methodInformation);
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				string authorizationIdFromHeader = GetAuthorizationIdFromHeader();
				((IDualAuthorizationHandling)new DualAuthorizationHandling()).DeleteCookie((IGeminiRequestContext)base.RequestContext, authorizationIdFromHeader, AuthorizationActionType.LogOn);
			}
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("Authorize");
			methodInformation.AddParameter("userDomainName", userDomainName);
			methodInformation.AddParameter("type", type);
			EnterWebMethod(methodInformation);
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				string sidFromAccountName = IdentityAccessor.GetSidFromAccountName(base.RequestContext, userDomainName);
				return ((IDualAuthorizationHandling)new DualAuthorizationHandling()).Authorize((IGeminiRequestContext)base.RequestContext, sidFromAccountName, userDomainName, type);
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public UserInfo GetUserInfo(string token)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetUserInfo");
			methodInformation.AddParameter("token", token);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => serverWebServiceClient.GetUserInfo(token, GetCallContextInfo()), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetUserInfo('{0}')", token);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationInfo GetConfiguration(string token)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetConfiguration");
			methodInformation.AddParameter("token", token);
			EnterWebMethod(methodInformation);
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				((IDualAuthorizationHandling)new DualAuthorizationHandling()).CheckDualAuthorization(authorizationId: GetAuthorizationIdFromHeader(), context: (IGeminiRequestContext)base.RequestContext, action: AuthorizationActionType.LogOn);
			}
			return CallWithExceptionHandling(delegate
			{
				bool isPublicRequest = IPHelper.IsNatRequest(GetCallContextInfo().LocationIPAddress, base.RequestContext.LocalIPAddress);
				return ClientConfigurationProvider.GetConfiguration(base.RequestContext, isPublicRequest);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetConfiguration('{0}')", token);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RecorderInfo[] GetConfigurationRecorders(string token, Guid[] recorderIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetConfigurationRecorders");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("recorderIds", recorderIds);
			EnterWebMethod(methodInformation);
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				((IDualAuthorizationHandling)new DualAuthorizationHandling()).CheckDualAuthorization(authorizationId: GetAuthorizationIdFromHeader(), context: (IGeminiRequestContext)base.RequestContext, action: AuthorizationActionType.LogOn);
			}
			return CallWithExceptionHandling(delegate
			{
				bool isPublicRequest = IPHelper.IsNatRequest(GetCallContextInfo().LocationIPAddress, base.RequestContext.LocalIPAddress);
				return ClientConfigurationProvider.GetConfigurationRecorders(base.RequestContext, recorderIds, isPublicRequest);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetConfigurationRecorders('{0}')", token);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public HardwareInfo[] GetConfigurationHardware(string token, Guid[] hardwareIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetConfigurationHardware");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("hardwareIds", hardwareIds);
			EnterWebMethod(methodInformation);
			return CallWithExceptionHandling(() => ClientConfigurationProvider.GetConfigurationHardware(base.RequestContext, hardwareIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetConfigurationHardware('{0}')", token);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public DeviceDisabledInfo[] GetDevicesDisabled(string token)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDevicesDisabled");
			methodInformation.AddParameter("token", token);
			EnterWebMethod(methodInformation);
			return CallWithExceptionHandling(() => ClientConfigurationProvider.GetDevicesDisabled(base.RequestContext), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDisabledDevices('{0}')", token);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Change[] QueryChanges(string token, DateTime changesFrom)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryChanges");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("changesFrom", changesFrom);
			EnterWebMethod(methodInformation);
			return CallWithExceptionHandling(() => ClientConfigurationProvider.QueryChanges(base.RequestContext, changesFrom), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryChanges('{0}','{1}')", token, changesFrom);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RecorderInfo QueryRecorderInfo(string token, Guid recorderId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRecorderInfo");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("recorderId", recorderId);
			EnterWebMethod(methodInformation);
			return CallWithExceptionHandling(delegate
			{
				bool isPublicRequest = IPHelper.IsNatRequest(GetCallContextInfo().LocationIPAddress, base.RequestContext.LocalIPAddress);
				return ClientConfigurationProvider.GetRecorderInfoOnly(base.RequestContext, recorderId, isPublicRequest);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryRecorderInfo('{0}','{1}')", token, recorderId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public DictionaryEntry[] GetVmoServicesList()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetVmoServicesList");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => serverWebServiceClient.GetVmoServicesList(GetCallContextInfo()), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetVmoServicesList()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ViewGroupInfo[] GetViewGroups(string token)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetViewGroups");
			methodInformation.AddParameter("token", token);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceViewHandling.GetViewGroups(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ViewGroupInternal GetViewGroupData(string token, Guid viewGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetViewGroupData");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("viewGroupId", viewGroupId);
			EnterWebMethod(methodInformation);
			return ConvertInternalProxyClasses.ConvertToViewGroupInternal(ServerCommandServiceViewHandling.GetViewGroupData(base.RequestContext, viewGroupId));
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SetViewGroupDataResultType SetViewGroupData(string token, Guid viewGroupId, ViewGroupInternalData viewGroupInternalData)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetViewGroupData");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("viewGroupId", viewGroupId);
			methodInformation.AddParameter("viewGroupInternalData", viewGroupInternalData);
			EnterWebMethod(methodInformation);
			ViewGroupData viewGroupData = ConvertInternalProxyClasses.ConvertToViewGroupData(viewGroupInternalData);
			return ServerCommandServiceViewHandling.SetViewGroupData(base.RequestContext, viewGroupId, viewGroupData);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomSettingInternal GetCustomSettingDataGlobal(string token, Guid customSettingId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomSettingDataGlobal");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			EnterWebMethod(methodInformation);
			CustomSettingInternal customSettingInternal = MIPMigrationHandling.GetMIPItems(base.RequestContext, customSettingId);
			if (customSettingInternal == null)
			{
				customSettingInternal = CustomSettingHandling.GetCustomSettingDataGlobalLegacy(base.RequestContext, customSettingId);
			}
			return customSettingInternal;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomSettingInternal GetCustomSettingDataUser(string token, Guid customSettingId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomSettingDataUser");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			EnterWebMethod(methodInformation);
			return CustomSettingHandling.GetCustomSettingDataUserLegacy(base.RequestContext, customSettingId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal(string token, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetCustomSettingDataGlobal");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			methodInformation.AddParameter("customSettingInternalData", customSettingInternalData);
			methodInformation.AddParameter("forceOverride", forceOverride);
			EnterWebMethod(methodInformation);
			if (MIPMigrationHandling.SetMIPItems(base.RequestContext, customSettingId, customSettingInternalData, out var result))
			{
				return result;
			}
			return CustomSettingHandling.SetCustomSettingDataGlobalLegacy(base.RequestContext, customSettingId, customSettingInternalData, forceOverride);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SetCustomSettingResultType SetCustomSettingDataUser(string token, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetCustomSettingDataUser");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			methodInformation.AddParameter("customSettingInternalData", customSettingInternalData);
			methodInformation.AddParameter("forceOverride", forceOverride);
			EnterWebMethod(methodInformation);
			return CustomSettingHandling.SetCustomSettingDataUserLegacy(base.RequestContext, customSettingId, customSettingInternalData, forceOverride);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomSettingInternal GetCustomSettingDataGlobal2(string token, Guid customSettingId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomSettingDataGlobal2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			EnterWebMethod(methodInformation);
			CustomSettingInternal customSettingInternal = MIPMigrationHandling.GetMIPItems(base.RequestContext, customSettingId);
			if (customSettingInternal == null)
			{
				customSettingInternal = CustomSettingHandling.GetCustomSettingDataGlobal(base.RequestContext, customSettingId);
			}
			return customSettingInternal;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public CustomSettingInternal GetCustomSettingDataUser2(string token, Guid customSettingId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetCustomSettingDataUser2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			EnterWebMethod(methodInformation);
			return CustomSettingHandling.GetCustomSettingDataUser(base.RequestContext, customSettingId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SetCustomSettingResultType SetCustomSettingDataGlobal2(string token, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetCustomSettingDataGlobal2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			methodInformation.AddParameter("customSettingInternalData", customSettingInternalData);
			methodInformation.AddParameter("forceOverride", forceOverride);
			EnterWebMethod(methodInformation);
			if (MIPMigrationHandling.SetMIPItems(base.RequestContext, customSettingId, customSettingInternalData, out var result))
			{
				return result;
			}
			return CustomSettingHandling.SetCustomSettingDataGlobal(base.RequestContext, customSettingId, customSettingInternalData, forceOverride);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SetCustomSettingResultType SetCustomSettingDataUser2(string token, Guid customSettingId, CustomSettingInternalData customSettingInternalData, bool forceOverride)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SetCustomSettingDataUser2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("customSettingId", customSettingId);
			methodInformation.AddParameter("customSettingInternalData", customSettingInternalData);
			methodInformation.AddParameter("forceOverride", forceOverride);
			EnterWebMethod(methodInformation);
			return CustomSettingHandling.SetCustomSettingDataUser(base.RequestContext, customSettingId, customSettingInternalData, forceOverride);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void EventTrigger(string token, Guid eventId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EventTrigger");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("eventId", eventId);
			EnterWebMethod(methodInformation);
			EventTriggerWithMetadata(token, eventId, null);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, VideoOS.WebService.Common.ServerCommandService.KeyValue[] metadata)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("EventTriggerWithMetadata");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("eventId", eventId);
			methodInformation.AddArrayParameter("metadata", metadata);
			EnterWebMethod(methodInformation);
			bool userDefinedEventExists = false;
			FixLegacyTargetDevicesError(metadata);
			CallWithExceptionHandling(() => userDefinedEventExists = CommandApiHandling.CheckSecurityEventTriggerWithMetadata(base.RequestContext, eventId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: EventTriggerWithMetadata('{0}','{1}','[{2}]')", token, eventId, string.Join(",", metadata.Select((VideoOS.WebService.Common.ServerCommandService.KeyValue e) => "(" + e.Key + ";" + e.Value + ")")));
			});
			if (!userDefinedEventExists)
			{
				return;
			}
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				serverWebServiceClient.EventTriggerWithMetadata(token, eventId, metadata, GetCallContextInfo());
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: EventTriggerWithMetadata('{0}','{1}','[{2}]')", token, eventId, string.Join(",", metadata.Select((VideoOS.WebService.Common.ServerCommandService.KeyValue e) => "(" + e.Key + ";" + e.Value + ")")));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private static void FixLegacyTargetDevicesError(VideoOS.WebService.Common.ServerCommandService.KeyValue[] metadata)
	{
		if (metadata == null)
		{
			return;
		}
		foreach (VideoOS.WebService.Common.ServerCommandService.KeyValue item in metadata.Where((VideoOS.WebService.Common.ServerCommandService.KeyValue m) => string.Equals(m.Key, "TargetDevices", StringComparison.Ordinal)))
		{
			if (!string.IsNullOrWhiteSpace(item.Value))
			{
				item.Value = item.Value.Replace("devicesIds", "deviceIds");
			}
		}
	}

	public AlertOccurrence[] AlertsGetAroundWithSpan(string token, Guid[] alertIds, DateTime centerTime, TimeDuration maxTimeBefore, int maxCountBefore, TimeDuration maxTimeAfter, int maxCountAfter)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("AlertsGetAroundWithSpan");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("alertIds", alertIds);
			methodInformation.AddParameter("centerTime", centerTime);
			methodInformation.AddParameter("maxTimeBefore", maxTimeBefore);
			methodInformation.AddParameter("maxCountBefore", maxCountBefore);
			methodInformation.AddParameter("maxTimeAfter", maxTimeAfter);
			methodInformation.AddParameter("maxCountAfter", maxCountAfter);
			EnterWebMethod(methodInformation);
			return new AlertOccurrence[0];
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void MatrixMonitorActivateCamera(string token, Guid matrixMonitorId, Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MatrixMonitorActivateCamera");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("matrixMonitorId", matrixMonitorId);
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				serverWebServiceClient.MatrixMonitorActivateCamera(matrixMonitorId, deviceId, GetCallContextInfo());
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: MatrixMonitorActivateCamera('{0}','{1}','{2}')", token, matrixMonitorId, deviceId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public BookmarkReference BookmarkGetNewReference(string token, Guid deviceId, bool live)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkGetNewReference");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("live", live);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkGetNewReference(base.RequestContext, deviceId, live);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark BookmarkCreate(string token, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkCreate");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("timeBegin", timeBegin);
			methodInformation.AddParameter("timeTrigged", timeTrigged);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkCreate(base.RequestContext, deviceId, timeBegin, timeTrigged, timeEnd, reference, header, description);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark BookmarkUpdate(string token, Guid bookmarkId, Guid deviceId, DateTime timeBegin, DateTime timeTrigged, DateTime timeEnd, string reference, string header, string description)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkUpdate");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("bookmarkId", bookmarkId);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("timeBegin", timeBegin);
			methodInformation.AddParameter("timeTrigged", timeTrigged);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkUpdate(base.RequestContext, bookmarkId, timeBegin, timeTrigged, timeEnd, reference, header, description);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void BookmarkDelete(string token, Guid bookmarkId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkDelete");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("bookmarkId", bookmarkId);
			EnterWebMethod(methodInformation);
			ServerCommandServiceBookmarkHandling.BookmarkDelete(base.RequestContext, bookmarkId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark BookmarkGet(string token, Guid bookmarkId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkGet");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("bookmarkId", bookmarkId);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkGet(base.RequestContext, bookmarkId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark[] BookmarkSearchTime(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkSearchTime");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("time", time);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkSearch(base.RequestContext, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null, null);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark[] BookmarkSearchTime2(string token, DateTime time, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkSearchTime2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("time", time);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkSearch(base.RequestContext, time, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, null);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark[] BookmarkSearchFromBookmark(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkSearchFromBookmark");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("fromBookmarkId", fromBookmarkId);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkSearch(base.RequestContext, DateTime.UtcNow, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null, fromBookmarkId.ToString());
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Bookmark[] BookmarkSearchFromBookmark2(string token, Guid fromBookmarkId, TimeDuration timeLimit, int countLimit, MediaDeviceType[] deviceTypes, Guid[] optDeviceIds, string[] optUsers, string optSearchStr, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("BookmarkSearchFromBookmark2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("fromBookmarkId", fromBookmarkId);
			methodInformation.AddParameter("timeLimit", timeLimit);
			methodInformation.AddParameter("countLimit", countLimit);
			methodInformation.AddArrayParameter("deviceTypes", deviceTypes);
			methodInformation.AddArrayParameter("optDeviceIds", optDeviceIds);
			methodInformation.AddArrayParameter("optUsers", optUsers);
			methodInformation.AddParameter("optSearchStr", optSearchStr);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceBookmarkHandling.BookmarkSearch(base.RequestContext, DateTime.UtcNow, timeLimit, countLimit, deviceTypes, optDeviceIds, optUsers, optSearchStr, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, fromBookmarkId.ToString());
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RegisterIntegration(string token, string instanceId, Guid integrationId, string integrationVersion, string integrationName, string manufacturerName)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterIntegration");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("instanceId", instanceId);
			methodInformation.AddParameter("integrationId", integrationId);
			methodInformation.AddParameter("integrationVersion", integrationVersion);
			methodInformation.AddParameter("integrationName", integrationName);
			methodInformation.AddParameter("manufacturerName", manufacturerName);
			EnterWebMethod(methodInformation);
			IntegrationRegistration.CreateIntegrationRegistration(base.RequestContext, instanceId, integrationId, IntegrationType.Standalone, integrationVersion, integrationName, manufacturerName);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterPluginIntegrations");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("instanceId", instanceId);
			methodInformation.AddParameter("integrationType", integrationType);
			methodInformation.AddArrayParameter("integrationType", pluginIntegrations);
			EnterWebMethod(methodInformation);
			IntegrationRegistration.CreatePluginRegistrations(base.RequestContext, instanceId, integrationType, pluginIntegrations);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceId, bool live)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataGetNewReference");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceId", deviceId);
			methodInformation.AddParameter("live", live);
			EnterWebMethod(methodInformation);
			return new VideoOS.WebService.Common.ServerCommandService.MarkedDataReference
			{
				Reference = "MarkedDataRef",
				Time = DateTime.UtcNow
			};
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataCreate");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("markedDataId", markedDataId);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeTag", timeTag);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("useRetention", useRetention);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("markedDataType", markedDataType);
			methodInformation.AddParameter("retentionExpire", retentionExpire);
			methodInformation.AddParameter("retentionOption", retentionOption);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataCreate(base.RequestContext, markedDataId, deviceIds, timeStart, timeTag, timeEnd, useRetention, reference, header, description, markedDataType, retentionExpire, retentionOption);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult MarkedDataUpdate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataUpdate");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("markedDataId", markedDataId);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeTag", timeTag);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("useRetention", useRetention);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("markedDataType", markedDataType);
			methodInformation.AddParameter("retentionExpire", retentionExpire);
			methodInformation.AddParameter("retentionOption", retentionOption);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataUpdate(base.RequestContext, markedDataId, deviceIds, timeStart, timeTag, timeEnd, useRetention, reference, header, description, markedDataType, retentionExpire, retentionOption);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataDelete");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("markedDataIds", markedDataIds);
			EnterWebMethod(methodInformation);
			List<MarkedDataResult> list = new List<MarkedDataResult>();
			foreach (Guid markedDataId in markedDataIds)
			{
				MarkedDataResult item = ServerCommandServiceMarkedDataHandling.MarkedDataDelete(base.RequestContext, markedDataId);
				list.Add(item);
			}
			return list.ToArray();
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataGet");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("markedDataId", markedDataId);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataGet(base.RequestContext, markedDataId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataSearch");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFrom", retentionExpireFrom);
			methodInformation.AddParameter("retentionExpireTo", retentionExpireTo);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataSearch(base.RequestContext, deviceIds, searchText, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataSearch2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFrom", retentionExpireFrom);
			methodInformation.AddParameter("retentionExpireTo", retentionExpireTo);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataSearch(base.RequestContext, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaCreate");
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeEnd", timeEnd);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaCreate(base.RequestContext, id, deviceIds, header, description, timeStart, timeEnd);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaUpdate");
			methodInformation.AddParameter("deviceIdsToAdd", deviceIdsToAdd);
			methodInformation.AddParameter("deviceIdsToRemove", deviceIdsToRemove);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeEnd", timeEnd);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaUpdate(base.RequestContext, id, deviceIdsToAdd, deviceIdsToRemove, header, description, timeStart, timeEnd);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaDelete(Guid restrictedMediaId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaDelete");
			methodInformation.AddParameter("restrictedMediaId", restrictedMediaId);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaDelete(base.RequestContext, restrictedMediaId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.RestrictedMedia[] RestrictedMediaQueryAll()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaQueryAll");
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.QueryAll(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime timeStart)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveModeEnter");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveModeEnter(base.RequestContext, deviceIds, timeStart);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime timeStart, DateTime timeEnd)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveModeExit");
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeEnd", timeEnd);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveModeExit(base.RequestContext, id, deviceIds, header, description, timeStart, timeEnd);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime timeStart)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveUpdate");
			methodInformation.AddParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveUpdate(base.RequestContext, deviceIds, timeStart);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveDelete");
			methodInformation.AddParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveDelete(base.RequestContext, deviceIds);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLive[] RestrictedMediaLiveQueryAll()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveQueryAll");
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.QueryAllLive(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile(string token, bool withSettings)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetClientProfile");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("withSettings", withSettings);
			EnterWebMethod(methodInformation);
			return ClientProfileHandling.GetClientProfileLegacy(base.RequestContext, withSettings);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile2(string token, bool withSettings)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetClientProfile2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("withSettings", withSettings);
			EnterWebMethod(methodInformation);
			return ClientProfileHandling.GetClientProfileLegacy20R3(base.RequestContext, withSettings);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.ClientProfile GetClientProfile3(string token, bool withSettings)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetClientProfile3");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("withSettings", withSettings);
			EnterWebMethod(methodInformation);
			return ClientProfileHandling.GetClientProfile(base.RequestContext, withSettings);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public PresetInfo[] GetPresets(string token, Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetPresets");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			return PtzHandling.GetPresets(base.RequestContext, deviceId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void DeletePreset(string token, Guid id, Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("DeletePreset");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("id", id);
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			PtzHandling.DeletePresets(base.RequestContext, id, deviceId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdatePresetName(string token, Guid id, Guid deviceId, string newName)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdatePresetName");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("id", id);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("newName", newName);
			EnterWebMethod(methodInformation);
			PtzHandling.UpdatePresetName(base.RequestContext, id, deviceId, newName);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void UpdatePresetPosition(string token, Guid id, Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("UpdatePresetPosition");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("id", id);
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			PtzHandling.UpdatePresetPosition(base.RequestContext, id, deviceId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Guid CreatePreset(string token, Guid deviceId, string name)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("CreatePreset");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddParameter("name", name);
			EnterWebMethod(methodInformation);
			return PtzHandling.CreatePreset(base.RequestContext, deviceId, name);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void OrderPresets(string token, Guid deviceId, Guid[] ids)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("OrderPresets");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("deviceId", deviceId);
			methodInformation.AddArrayParameter("ids", ids);
			EnterWebMethod(methodInformation);
			PtzHandling.UpdatePresetOrder(base.RequestContext, deviceId, ids);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private static TResult CallWithExceptionHandling<TResult>(Func<TResult> call, Action exceptionTraceCall)
	{
		try
		{
			return call();
		}
		catch (Exception ex)
		{
			exceptionTraceCall?.Invoke();
			GeminiTrace.TraceException(ex);
			((GeminiHttpApplication)HttpContext.Current.ApplicationInstance).InitiateShutdownIfNecessary(ex);
			ExceptionDetail exceptionDetail = null;
			FaultCode faultCode = null;
			if (ex is UnauthorizedAccessException)
			{
				exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
				faultCode = new FaultCode(51000.ToString(CultureInfo.InvariantCulture));
			}
			else
			{
				if (ex is FaultException)
				{
					throw;
				}
				if (ex is TimeBasedUnauthorizedAccessException)
				{
					exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
					faultCode = new FaultCode(50000.ToString(CultureInfo.InvariantCulture));
				}
				else
				{
					exceptionDetail = new ExceptionDetail(ex);
					faultCode = new FaultCode(ex.GetType().ToString());
				}
			}
			throw new FaultException<ExceptionDetail>(exceptionDetail, exceptionDetail.Message, faultCode);
		}
	}

	private TResult CallWithRetryExceptionHandling<TResult>(Func<ServerWebServiceClient, TResult> call, Action exceptionTraceCall)
	{
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = base.RequestContext.GetService<ServerClientService>().GetClient();
			return serverWebServiceManager.CallWithRetry(call, exceptionTraceCall);
		}
		finally
		{
			base.RequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
	}

	private void CallWithRetryExceptionHandling(Action<ServerWebServiceClient> call, Action exceptionTraceCall)
	{
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = base.RequestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(call, exceptionTraceCall);
		}
		finally
		{
			base.RequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
	}

	private BasicCallContextInfo GetCallContextInfo()
	{
		return new BasicCallContextInfo(base.RequestContext.AuthenticatedUserSid, base.RequestContext.UserDomainAccountName, base.RequestContext.RemoteIPAddress);
	}

	private static string GetAuthorizationIdFromHeader()
	{
		MessageHeaders incomingMessageHeaders = OperationContext.Current.IncomingMessageHeaders;
		int num = incomingMessageHeaders.FindHeader("AuthorizationId", "http://videoos.net/2/XProtectCSServerCommand");
		if (num != -1)
		{
			return incomingMessageHeaders.GetHeader<string>(num);
		}
		return string.Empty;
	}
}
