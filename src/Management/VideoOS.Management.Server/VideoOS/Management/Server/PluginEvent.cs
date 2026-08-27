using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.ServiceModel;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public sealed class PluginEvent
{
	private const string _area = "ManagementServer";

	private const string _layer = "PluginEvent";

	public string Id { get; set; }

	public string Name { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string Definition { get; set; }

	public PluginEventType GeneratorType { get; set; }

	public string GeneratorId { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static string[] QueryPluginEventIds(IGeminiRequestContext requestContext)
	{
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		return FilterPluginEventsBasedOnPermissions(pluginEventComponent.QueryPluginEventIds());
	}

	public static PluginEvent QueryPluginEvent(IGeminiRequestContext requestContext, string pluginEventId)
	{
		Validation.CheckObjectId(ref pluginEventId, "id", allowNullOrEmpty: false);
		PluginEvent pluginEvent = null;
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		return pluginEventComponent.QueryPluginEvent(pluginEventId);
	}

	public static PluginEvent[] QueryPluginEvents(IGeminiRequestContext requestContext, string[] pluginEventIds)
	{
		Validation.CheckObjectIdArray(pluginEventIds, "ids", allowNullOrEmptyIds: false);
		PluginEvent[] array = null;
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		return FilterPluginEventsBasedOnPermissions(pluginEventComponent.QueryPluginEvents(pluginEventIds));
	}

	public static PluginEvent CreatePluginEvent(IGeminiRequestContext requestContext, PluginEvent pluginEvent)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		pluginEvent.Id = GeminiResourceComponent.GetGuid(pluginEvent.Id);
		Validation.CheckName(pluginEvent.Name, "name");
		ValidateEventDefinition(pluginEvent);
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		pluginEventComponent.CreatePluginEvent(pluginEvent, requestContext.AuthenticatedUserSid);
		pluginEvent = FindPluginEvent(requestContext, pluginEventComponent, pluginEvent.Id);
		Integration.TryNotifyServerAsync();
		return pluginEvent;
	}

	public static PluginEvent UpdatePluginEvent(IGeminiRequestContext requestContext, PluginEvent pluginEvent)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		string objectId = pluginEvent.Id;
		Validation.CheckObjectId(ref objectId, "pluginEventId", allowNullOrEmpty: false);
		Validation.CheckName(pluginEvent.Name, "name");
		pluginEvent.Id = objectId;
		ValidateEventDefinition(pluginEvent);
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		pluginEventComponent.UpdatePluginEvent(pluginEvent);
		pluginEvent = FindPluginEvent(requestContext, pluginEventComponent, pluginEvent.Id);
		Integration.TryNotifyServerAsync();
		return pluginEvent;
	}

	public static void DeletePluginEvent(IGeminiRequestContext requestContext, string pluginEvent)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref pluginEvent, "id", allowNullOrEmpty: false);
		using IPluginEventComponent pluginEventComponent = requestContext.CreatePluginEventComponent();
		pluginEventComponent.DeletePluginEvent(pluginEvent);
		Integration.TryNotifyServerAsync();
	}

	public static void TriggerPluginEvent(IGeminiRequestContext requestContext, Guid pluginEventId, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData)
	{
		requestContext.RequestTracer.TraceEnter(0, "ManagementServer", "PluginEvent", "TriggerPluginEvent");
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "PluginEvent", "{0}", "Security check");
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "PluginEvent", "{0}", "Find PluginEvent");
		PluginEvent pluginEvent = FindPluginEvent(requestContext, null, pluginEventId.ToString());
		if (pluginEvent == null)
		{
			throw new PluginEventNotFoundException();
		}
		bool isDeviceEvent = pluginEvent.GeneratorType == PluginEventType.MIPDevice;
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "PluginEvent", "{0}", "Call ServerService");
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = requestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.TriggerPluginEvent(pluginEventId, isDeviceEvent, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData, new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress));
			}, null, delegate(Exception ex, Action ac)
			{
				GeminiTrace.Error(TraceType.Configuration, "Triggering event via server service failed: TriggerUserDefinedEvent('{0}')", pluginEventId);
				if (ex.GetType() == typeof(FaultException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
					GeminiTrace.TraceException(ex);
					throw new RecordingServerCommunicationException();
				}
				if (ex.GetType() == typeof(CommunicationException) || ex.GetType() == typeof(EndpointNotFoundException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Unable to connect to the Management Server service.");
					GeminiTrace.TraceException(ex);
					throw new ManagementServerServiceUnavailableException(ex.Message, ex);
				}
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
				GeminiTrace.TraceException(ex);
				throw new RecordingServerCommunicationException();
			});
		}
		finally
		{
			requestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
		requestContext.RequestTracer.TraceLeave(0, "ManagementServer", "PluginEvent", "TriggerPluginEvent");
	}

	public static void TriggerPluginEventBulk(IGeminiRequestContext requestContext, PluginEventInfo[] pluginEvents)
	{
		requestContext.RequestTracer.TraceEnter(0, "ManagementServer", "PluginEvent", "TriggerPluginEventBulk");
		if (pluginEvents == null)
		{
			return;
		}
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "PluginEvent", "{0}: {1}", "Bulk size", pluginEvents.Length);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = requestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.TriggerPluginEventBulk(pluginEvents, new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress));
			}, null, delegate(Exception ex, Action ac)
			{
				GeminiTrace.Error(TraceType.Configuration, "Triggering event via server service failed: TriggerUserDefinedEventBulk()");
				if (ex.GetType() == typeof(FaultException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
					GeminiTrace.TraceException(ex);
					throw new RecordingServerCommunicationException();
				}
				if (ex.GetType() == typeof(CommunicationException) || ex.GetType() == typeof(EndpointNotFoundException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Unable to connect to the Management Server service.");
					GeminiTrace.TraceException(ex);
					throw new ManagementServerServiceUnavailableException(ex.Message, ex);
				}
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
				GeminiTrace.TraceException(ex);
				throw new RecordingServerCommunicationException();
			});
		}
		finally
		{
			requestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
		requestContext.RequestTracer.TraceLeave(0, "ManagementServer", "PluginEvent", "TriggerPluginEventBulk");
	}

	private static void ValidateEventDefinition(PluginEvent pluginEvent)
	{
		Validation.CheckObjectReference(pluginEvent.Definition, "definition");
		EventDefinition eventDefinition;
		try
		{
			eventDefinition = new EventDefinition(pluginEvent.Definition);
		}
		catch (XmlException innerException)
		{
			throw new ArgumentException("Invalid event definition", innerException);
		}
		catch (XPathException innerException2)
		{
			throw new ArgumentException("Invalid event definition", innerException2);
		}
		switch (pluginEvent.GeneratorType)
		{
		case PluginEventType.MIP:
			new MIPEventDefinition(eventDefinition);
			break;
		case PluginEventType.MIPDevice:
			new MIPDeviceEventDefinition(eventDefinition);
			break;
		}
		string objectId = eventDefinition.GeneratorID;
		Validation.CheckObjectId(ref objectId, "GeneratorID", allowNullOrEmpty: false);
		eventDefinition.GeneratorID = objectId;
		pluginEvent.GeneratorId = objectId;
		Guid counterEventID = (eventDefinition.ID = new Guid(pluginEvent.Id));
		eventDefinition.Name = pluginEvent.Name;
		eventDefinition.GeneratorID = objectId;
		if (string.IsNullOrEmpty(eventDefinition.DisplayName))
		{
			eventDefinition.DisplayName = pluginEvent.Name;
		}
		if (string.IsNullOrEmpty(eventDefinition.DisplayNameReferenceId))
		{
			eventDefinition.DisplayNameReferenceId = counterEventID.ToString();
		}
		if (eventDefinition.CounterEventID == Guid.Empty)
		{
			eventDefinition.CounterEventID = counterEventID;
		}
		pluginEvent.Definition = eventDefinition.SaveAsXml();
	}

	private static PluginEvent FindPluginEvent(IGeminiRequestContext requestContext, IPluginEventComponent pluginEventComponent, string pluginEventId)
	{
		bool flag = false;
		try
		{
			if (pluginEventComponent == null)
			{
				pluginEventComponent = requestContext.CreatePluginEventComponent();
				flag = true;
			}
			return pluginEventComponent.QueryPluginEvent(pluginEventId);
		}
		finally
		{
			if (flag)
			{
				pluginEventComponent.Dispose();
			}
		}
	}

	private static PluginEvent[] FilterPluginEventsBasedOnPermissions(PluginEvent[] pluginEvents)
	{
		List<PluginEvent> list = new List<PluginEvent>();
		foreach (PluginEvent item in pluginEvents)
		{
			list.Add(item);
		}
		return list.ToArray();
	}

	private static string[] FilterPluginEventsBasedOnPermissions(string[] pluginEventIds)
	{
		List<string> list = new List<string>();
		foreach (string item in pluginEventIds)
		{
			list.Add(item);
		}
		return list.ToArray();
	}
}
