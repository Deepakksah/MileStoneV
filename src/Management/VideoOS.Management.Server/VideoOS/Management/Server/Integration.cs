using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.Common.Integration;
using VideoOS.Platform;
using VideoOS.Platform.ConfigurationItems;
using VideoOS.Platform.Proxy.RestApi;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public static class Integration
{
	public static class RecordingServer
	{
		public static bool VerifyRecordingStoragePath(string recordingServerId, string recordingStorageId, string path, bool newPath)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.VerifyRecordingStoragePath(recordingServerId, recordingStorageId, path, newPath), null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		public static bool VerifyArchiveStoragePath(string recordingServerId, string archiveStorageId, string path, bool newPath)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.VerifyArchiveStoragePath(recordingServerId, archiveStorageId, path, newPath), null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		public static bool VerifyStorageAndArchiveOnline(string recordingServerId, string deviceId, string recordingStorageId, string archiveStorageId)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.VerifyStorageAndArchiveOnline(recordingServerId, deviceId, recordingStorageId, archiveStorageId), null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		public static string[] ReadDrivesOnRecorder(string recordingServerId)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.ReadDrivesOnRecorder(recordingServerId), null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		public static string[] ReadDirectoriesOnRecorder(string recordingServerId, string path)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.ReadDirectoriesOnRecorder(recordingServerId, path), null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		public static void CreateDirectoryOnRecorder(string recordingServerId, string path)
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.CreateDirectoryOnRecorder(recordingServerId, path);
			}, null, delegate(Exception ex, Action ac)
			{
				throw TranslateExceptions(ex);
			});
		}

		private static Exception TranslateExceptions(Exception ex)
		{
			if (ex.GetType() == typeof(FaultException<RecordingServerUnavailableFault>))
			{
				GeminiTrace.Error(TraceType.Configuration, "The recording server unavailable.");
				GeminiTrace.TraceException(ex);
				return new RecordingServerUnavailableException();
			}
			if (ex.GetType() == typeof(FaultException<RecordingServerCommunicationTimeoutFault>))
			{
				GeminiTrace.Error(TraceType.Configuration, "Communication with the recording server timed out");
				GeminiTrace.TraceException(ex);
				return new RecordingServerTimeoutException();
			}
			if (ex.GetType() == typeof(FaultException<RecordingServerOperationFailedFault>))
			{
				GeminiTrace.Error(TraceType.Configuration, "Operation failed on the recording server");
				GeminiTrace.TraceException(ex);
				return new RecordingServerOperationFailedException();
			}
			if (ex.GetType() == typeof(FaultException))
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
				GeminiTrace.TraceException(ex);
				return new RecordingServerCommunicationException();
			}
			if (ex.GetType() == typeof(CommunicationException))
			{
				GeminiTrace.Error(TraceType.Configuration, "Unable to connect to the Management Server service.");
				GeminiTrace.TraceException(ex);
				return new ManagementServerServiceUnavailableException();
			}
			GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
			GeminiTrace.TraceException(ex);
			return new RecordingServerCommunicationException();
		}
	}

	private static VideoWallChangedEventSender _videoWallChangedEventSender;

	private static SystemConfigurationChangedEventSender _systemConfigurationChangedEventSender;

	private static bool _enableViewEvents;

	private static bool _enableViewItemEvents;

	private static bool _enableViewGroupEvents;

	static Integration()
	{
		_enableViewEvents = bool.Parse(Config.Instance.GetAppSetting("EnableServiceChannelEvents_View", "true"));
		_enableViewItemEvents = bool.Parse(Config.Instance.GetAppSetting("EnableServiceChannelEvents_ViewItem", "true"));
		_enableViewGroupEvents = bool.Parse(Config.Instance.GetAppSetting("EnableServiceChannelEvents_ViewGroup", "true"));
		AppDomain.CurrentDomain.ProcessExit += IntegrationDestructor;
	}

	private static void IntegrationDestructor(object sender, EventArgs e)
	{
		_videoWallChangedEventSender?.Dispose();
	}

	public static void FireVideoWallChangedEvent(IGeminiRequestContext requestContext, string videoWallId, EventAction action)
	{
		try
		{
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "VideoWallChanged", videoWallId, action);
			GetVideoWallChangedEventSender().Send(new Guid(videoWallId), Guid.Empty, Kind.SmartWall, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireVideoWallDeletedEvent(IGeminiRequestContext requestContext, string videoWallId, IEnumerable<string> monitors)
	{
		try
		{
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "VideoWallChanged", videoWallId, EventAction.Deleted);
			SendVideoWallDeletedEvent(new Guid(videoWallId), monitors);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireVideoWallPresetChangedEvent(IGeminiRequestContext requestContext, string videoWallPresetId, string videoWallId, EventAction action)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("VideoWallId", videoWallId);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "VideoWallPresetChanged", videoWallPresetId, action, dictionary);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireMonitorChanged(IGeminiRequestContext requestContext, string monitorId, string videoWallId, EventAction action)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("VideoWallId", videoWallId);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "MonitorChanged", monitorId, action, dictionary);
			GetVideoWallChangedEventSender().Send(new Guid(monitorId), new Guid(videoWallId), Kind.SmartWallMonitor, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireMonitorPresetChanged(IGeminiRequestContext requestContext, string monitorPresetId, string videoWallId, EventAction action)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("VideoWallId", videoWallId);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "MonitorPresetChanged", monitorPresetId, action, dictionary);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireMonitorViewChanged(IGeminiRequestContext requestContext, string viewId, string monitorId, string videoWallId, EventAction action)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("VideoWallId", videoWallId);
			if (!Guid.TryParse(monitorId, out var result))
			{
				result = Guid.Empty;
			}
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "MonitorViewChanged", viewId, action, dictionary, isHighPriority: true);
			GetVideoWallChangedEventSender().Send(new Guid(viewId), result, Kind.View, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireViewChanged(IGeminiRequestContext requestContext, string viewId, string viewGroupId, EventAction action)
	{
		if (_enableViewEvents)
		{
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ViewGroupId", viewGroupId);
				new Event().FireAsyncEventNoPermissionCheck(requestContext, "ViewChanged", viewId, action, dictionary);
			}
			catch (Exception ex)
			{
				GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
			}
		}
	}

	public static void FireViewItemChanged(IGeminiRequestContext requestContext, string viewItemId, string viewId, EventAction action)
	{
		if (_enableViewItemEvents)
		{
			try
			{
				Dictionary<string, string> dictionary = new Dictionary<string, string>();
				dictionary.Add("ViewId", viewId);
				new Event().FireAsyncEventNoPermissionCheck(requestContext, "ViewItemChanged", viewItemId, action, dictionary);
			}
			catch (Exception ex)
			{
				GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
			}
		}
	}

	public static void FireViewGroupChanged(IGeminiRequestContext requestContext, string viewGroupId, string parentGroupId, EventAction action)
	{
		if (!_enableViewGroupEvents)
		{
			return;
		}
		try
		{
			if (ServerGroup.IsNonSecurityGroup(parentGroupId))
			{
				parentGroupId = Guid.Empty.ToString();
			}
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("ParentGroupId", parentGroupId);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "ViewGroupChanged", viewGroupId, action, dictionary);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FirePatrollingChanged(IGeminiRequestContext requestContext, string patrollingProfileId, string cameraId, EventAction action)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("DeviceId", cameraId);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "PatrollingProfileChanged", patrollingProfileId, action, dictionary);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireEdgeJobUpdatedEvent(IGeminiRequestContext requestContext, string taskId, EventAction action)
	{
		try
		{
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "EdgeJobUpdated", taskId, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message, ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireConfigurationChanged(IGeminiRequestContext requestContext, string itemType, string id, string parentItemType, string parentId, EventAction action, string objectDisplayName)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("EntityName", itemType);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "ConfigurationChanged", id, action, dictionary);
			GetSystemConfigurationChangedEventSender().Send(new Guid(id), objectDisplayName, itemType, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message + ". Failing ResourceID: '" + id + "'", ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void FireConfigurationChanged(IGeminiRequestContext requestContext, string itemType, string id, EventAction action, string objectDisplayName)
	{
		try
		{
			Dictionary<string, string> dictionary = new Dictionary<string, string>();
			dictionary.Add("EntityName", itemType);
			new Event().FireAsyncEventNoPermissionCheck(requestContext, "ConfigurationChanged", id, action, dictionary);
			GetSystemConfigurationChangedEventSender().Send(new Guid(id), objectDisplayName, itemType, action);
		}
		catch (Exception ex)
		{
			GeminiApplicationCore.LogException(requestContext, ex.Message + ". Failing ResourceID: '" + id + "'", ex, GeminiEventId.DataChangedEventException, EventLogEntryType.Error);
		}
	}

	public static void TryNotifyServerAsync()
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.ProcessPendingTasks("909749AE-88C4-4780-A381-383D691E98FA");
			}, null, (Exception ex, Action ac) => false);
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyServerLogServerChangedAsync()
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.LogServerChanged();
			}, null, (Exception ex, Action ac) => false);
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyServerMipItemPositionChangedAsync(FQID fqid, PositioningInformation newPosition)
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.MipItemPositionChanged(fqid.ToXmlNode().OuterXml, newPosition.ToString());
			}, null, (Exception ex, Action ac) => false);
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyServerMipItemChangedAsync(FQID fqid, int changeType)
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.MipItemChanged(fqid.ToXmlNode().OuterXml, changeType);
			}, null, (Exception ex, Action ac) => false);
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyServerEntityChangedAsync(string restResourceType, Guid id, string objectName, EventAction action)
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			string itemType = new NameConvert().ResourcePluralToItemType(restResourceType);
			KeyValuePair<Guid, string> keyValuePair = Factory.KindToItemType.FirstOrDefault((KeyValuePair<Guid, string> kv) => kv.Value == itemType);
			if (!(keyValuePair.Key == Guid.Empty))
			{
				Guid key = keyValuePair.Key;
				FQID fqid = new FQID(new ServerId(), Guid.Empty, id, FolderType.No, key)
				{
					ObjectIdString = objectName
				};
				int changeType = (int)action;
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.MipItemChanged(fqid.ToXmlNode().OuterXml, changeType);
				}, null, (Exception ex, Action ac) => false);
			}
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyRecordingServerAsync(string recordingServerId)
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.ProcessPendingTasks(recordingServerId);
			}, null, (Exception ex, Action ac) => false);
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyRecordingServerAsync(Guid recordingServerId)
	{
		TryNotifyRecordingServerAsync(recordingServerId.ToString());
	}

	public static bool IsRecorderOnline(string recordingServerId)
	{
		using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
		return serverWebServiceManager.CallWithRetry((ServerWebServiceClient client) => client.IsRecorderOnline(recordingServerId), null, (Exception ex, Action ac) => false);
	}

	public static void TryClearIdentityCacheServerAsync()
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
			ServerWebServiceManager serverWebServiceManager = null;
			try
			{
				serverWebServiceManager = geminiRequestContext.GetService<ServerClientService>().GetClient();
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.RefreshIdentityCache();
				}, null, (Exception ex, Action ac) => false);
			}
			finally
			{
				geminiRequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
			}
		}, TaskCreationOptions.LongRunning);
	}

	public static void TryNotifyServerRegistrationChangedAsync()
	{
		System.Threading.Tasks.Task.Factory.StartNew(delegate
		{
			using GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
			ServerWebServiceManager serverWebServiceManager = null;
			try
			{
				serverWebServiceManager = geminiRequestContext.GetService<ServerClientService>().GetClient();
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.ServerRegistrationChanged();
				}, null, (Exception ex, Action ac) => false);
			}
			finally
			{
				geminiRequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
			}
		}, TaskCreationOptions.LongRunning);
	}

	private static void SendVideoWallDeletedEvent(Guid videoWallId, IEnumerable<string> monitors)
	{
		if (monitors != null)
		{
			foreach (string monitor in monitors)
			{
				GetVideoWallChangedEventSender().Send(new Guid(monitor), videoWallId, Kind.SmartWallMonitor, EventAction.Deleted);
			}
		}
		GetVideoWallChangedEventSender().Send(videoWallId, Guid.Empty, Kind.SmartWall, EventAction.Deleted);
	}

	private static VideoWallChangedEventSender GetVideoWallChangedEventSender()
	{
		return _videoWallChangedEventSender ?? (_videoWallChangedEventSender = new VideoWallChangedEventSender());
	}

	private static SystemConfigurationChangedEventSender GetSystemConfigurationChangedEventSender()
	{
		return _systemConfigurationChangedEventSender ?? (_systemConfigurationChangedEventSender = new SystemConfigurationChangedEventSender());
	}
}
