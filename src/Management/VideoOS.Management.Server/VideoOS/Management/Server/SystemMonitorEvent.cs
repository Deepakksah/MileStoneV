using System;
using System.ServiceModel;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public sealed class SystemMonitorEvent
{
	public static void TriggerSystemMonitorEvent(IGeminiRequestContext geminiRequestContext, Guid systemMonitorEventId, Guid sourceId)
	{
		EnqueueSystemMonitorEvent(geminiRequestContext, systemMonitorEventId, sourceId, isDeviceEvent: false);
	}

	public static void TriggerSystemMonitorEventDevice(IGeminiRequestContext geminiRequestContext, Guid systemMonitorEventId, Guid sourceId)
	{
		EnqueueSystemMonitorEvent(geminiRequestContext, systemMonitorEventId, sourceId, isDeviceEvent: true);
	}

	private static void EnqueueSystemMonitorEvent(IGeminiRequestContext geminiRequestContext, Guid systemMonitorEventId, Guid sourceId, bool isDeviceEvent)
	{
		PluginEventInfo workItem = new PluginEventInfo
		{
			PluginEventId = systemMonitorEventId,
			SourceId = sourceId,
			CustomData = "",
			MetadataDeviceIds = null,
			MetadataDeviceGroupIds = null,
			IsDeviceEvent = isDeviceEvent
		};
		geminiRequestContext.GetSystemMonitorEventQueueService().EventQueue.EnqueueItem(workItem);
	}

	public static void ProcessQueuedEvents(PluginEventInfo[] events)
	{
		GeminiRequestContext systemRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
		try
		{
			ServerWebServiceManager serverWebServiceManager = null;
			try
			{
				serverWebServiceManager = systemRequestContext.GetService<ServerClientService>().GetClient();
				serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
				{
					client.TriggerPluginEventBulk(events, new BasicCallContextInfo(systemRequestContext.AuthenticatedUserSid, systemRequestContext.UserDomainAccountName, systemRequestContext.RemoteIPAddress));
				}, null, delegate(Exception ex, Action ac)
				{
					if (ex.GetType() == typeof(FaultException))
					{
						throw new RecordingServerCommunicationException();
					}
					if (ex.GetType() == typeof(CommunicationException) || ex.GetType() == typeof(EndpointNotFoundException))
					{
						throw new ManagementServerServiceUnavailableException(ex.Message, ex);
					}
					throw new RecordingServerCommunicationException();
				});
			}
			finally
			{
				systemRequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
			}
		}
		finally
		{
			if (systemRequestContext != null)
			{
				((IDisposable)systemRequestContext).Dispose();
			}
		}
	}
}
