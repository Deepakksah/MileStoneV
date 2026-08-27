using System;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;
using VideoOS.Server.Webservice.Shared;

namespace VideoOS.Management.Server;

public sealed class UserDefinedEvent
{
	private const string _area = "ManagementServer";

	private const string _layer = "UserDefinedEvent";

	public string Id { get; set; }

	public string Name { get; set; }

	public DateTime CreatedDate { get; set; }

	public DateTime LastModified { get; set; }

	public string Subtype { get; set; }

	public EventDefinition Definition { get; set; }

	public CustomProperty[] CustomProperties { get; set; }

	public static string[] QueryUserDefinedEventIds(IGeminiRequestContext requestContext)
	{
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		return FilterUserDefinedEventsBasedOnPermissions(requestContext, userDefinedEventComponent.QueryUserDefinedEventIds());
	}

	public static UserDefinedEvent QueryUserDefinedEvent(IGeminiRequestContext requestContext, string userDefinedEventId)
	{
		Validation.CheckObjectId(ref userDefinedEventId, "id", allowNullOrEmpty: false);
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_READ", userDefinedEventId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			return null;
		}
		UserDefinedEvent userDefinedEvent = null;
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		return userDefinedEventComponent.QueryUserDefinedEvent(userDefinedEventId);
	}

	public static UserDefinedEvent[] QueryUserDefinedEvents(IGeminiRequestContext requestContext, string[] userDefinedEventIds)
	{
		Validation.CheckObjectIdArray(userDefinedEventIds, "ids", allowNullOrEmptyIds: false);
		UserDefinedEvent[] array = null;
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		return FilterUserDefinedEventsBasedOnPermissions(requestContext, userDefinedEventComponent.QueryUserDefinedEvents(userDefinedEventIds));
	}

	public static UserDefinedEvent CreateUserDefinedEvent(IGeminiRequestContext requestContext, UserDefinedEvent userDefinedEvent)
	{
		requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "CREATE_USER_DEFINED_EVENT", "/");
		Validation.CheckName(userDefinedEvent.Name, "name");
		Validation.CheckUserDefinedEvenSubtype(userDefinedEvent.Subtype, "subtype");
		userDefinedEvent.Id = GeminiResourceComponent.GetGuid(userDefinedEvent.Id);
		EventDefinition definition = new EventDefinition(new Guid(userDefinedEvent.Id), userDefinedEvent.Name, userDefinedEvent.Name, string.Empty, string.Empty, string.Empty, EventGeneratorType.External.ToString(), userDefinedEvent.Subtype, string.Empty, string.Empty, EventGeneratorType.External.ToString(), EventSource.FilterTypes.None, occursGlobally: true, builtIn: false, new Guid(userDefinedEvent.Id));
		userDefinedEvent.Definition = definition;
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		userDefinedEventComponent.CreateUserDefinedEvent(userDefinedEvent, requestContext.AuthenticatedUserSid);
		userDefinedEvent = FindUserDefinedEvent(requestContext, userDefinedEventComponent, userDefinedEvent.Id);
		Integration.TryNotifyServerAsync();
		Integration.FireConfigurationChanged(requestContext, "UserDefinedEvent", userDefinedEvent.Id, EventAction.Added, userDefinedEvent.Name);
		return userDefinedEvent;
	}

	public static UserDefinedEvent UpdateUserDefinedEvent(IGeminiRequestContext requestContext, UserDefinedEvent userDefinedEvent)
	{
		string objectId = userDefinedEvent.Id;
		Validation.CheckObjectId(ref objectId, "userDefinedEventId", allowNullOrEmpty: false);
		Validation.CheckName(userDefinedEvent.Name, "name");
		userDefinedEvent.Id = objectId;
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_WRITE", userDefinedEvent.Id);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new UserDefinedEventUpdateException();
		}
		EventDefinition definition = new EventDefinition(new Guid(userDefinedEvent.Id), userDefinedEvent.Name, userDefinedEvent.Name, string.Empty, string.Empty, string.Empty, EventGeneratorType.External.ToString(), userDefinedEvent.Subtype, string.Empty, string.Empty, EventGeneratorType.External.ToString(), EventSource.FilterTypes.None, occursGlobally: true, builtIn: false, new Guid(userDefinedEvent.Id));
		userDefinedEvent.Definition = definition;
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		userDefinedEventComponent.UpdateUserDefinedEvent(userDefinedEvent);
		userDefinedEvent = FindUserDefinedEvent(requestContext, userDefinedEventComponent, userDefinedEvent.Id);
		Integration.TryNotifyServerAsync();
		Integration.FireConfigurationChanged(requestContext, "UserDefinedEvent", userDefinedEvent.Id, EventAction.Updated, userDefinedEvent.Name);
		return userDefinedEvent;
	}

	public static void DeleteUserDefinedEvent(IGeminiRequestContext requestContext, string userDefinedEventId)
	{
		Validation.CheckObjectId(ref userDefinedEventId, "id", allowNullOrEmpty: false);
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "DELETE", userDefinedEventId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new UserDefinedEventNotFoundException();
		}
		using IUserDefinedEventComponent userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
		userDefinedEventComponent.DeleteUserDefinedEvent(userDefinedEventId);
		Integration.TryNotifyServerAsync();
		Integration.FireConfigurationChanged(requestContext, "UserDefinedEvent", userDefinedEventId, EventAction.Deleted, string.Empty);
	}

	public static void TriggerUserDefinedEvent(IGeminiRequestContext requestContext, string userDefinedEventId)
	{
		requestContext.RequestTracer.TraceEnter(0, "ManagementServer", "UserDefinedEvent", "TriggerUserDefinedEvent");
		Validation.CheckObjectId(ref userDefinedEventId, "userDefinedEventId", allowNullOrEmpty: false);
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "UserDefinedEvent", "{0}", "Security check");
		try
		{
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "TRIGGER", userDefinedEventId);
			requestContext.GetSecurityManagerService().CheckTimeBasedPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "TRIGGER", userDefinedEventId);
		}
		catch (SecurityObjectDoesNotExistException)
		{
			throw new UserDefinedEventNotFoundException();
		}
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "UserDefinedEvent", "{0}", "Find UserDefinedEvent");
		if (FindUserDefinedEvent(requestContext, null, userDefinedEventId) == null)
		{
			throw new UserDefinedEventNotFoundException();
		}
		requestContext.RequestTracer.Trace(0, TraceLevel.Verbose, "ManagementServer", "UserDefinedEvent", "{0}", "Call ServerService");
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = requestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
			{
				client.TriggerUserDefinedEvent(userDefinedEventId, new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress));
			}, null, delegate(Exception ex2, Action ac)
			{
				GeminiTrace.Error(TraceType.Configuration, "Triggering event via server service failed: TriggerUserDefinedEvent('{0}')", userDefinedEventId);
				if (ex2.GetType() == typeof(FaultException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
					GeminiTrace.TraceException(ex2);
					throw new RecordingServerCommunicationException();
				}
				if (ex2.GetType() == typeof(CommunicationException) || ex2.GetType() == typeof(EndpointNotFoundException))
				{
					GeminiTrace.Error(TraceType.Configuration, "Unable to connect to the Management Server service.");
					GeminiTrace.TraceException(ex2);
					throw new ManagementServerServiceUnavailableException(ex2.Message, ex2);
				}
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed");
				GeminiTrace.TraceException(ex2);
				throw new RecordingServerCommunicationException();
			});
		}
		finally
		{
			requestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
		requestContext.RequestTracer.TraceLeave(0, "ManagementServer", "UserDefinedEvent", "TriggerUserDefinedEvent");
	}

	private static UserDefinedEvent FindUserDefinedEvent(IGeminiRequestContext requestContext, IUserDefinedEventComponent userDefinedEventComponent, string userDefinedEventId)
	{
		bool flag = false;
		try
		{
			if (userDefinedEventComponent == null)
			{
				userDefinedEventComponent = requestContext.CreateUserDefinedEventComponent();
				flag = true;
			}
			return userDefinedEventComponent.QueryUserDefinedEvent(userDefinedEventId);
		}
		finally
		{
			if (flag)
			{
				userDefinedEventComponent.Dispose();
			}
		}
	}

	private static UserDefinedEvent[] FilterUserDefinedEventsBasedOnPermissions(IGeminiRequestContext requestContext, UserDefinedEvent[] userDefinedEvents)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_READ", userDefinedEvents.Select((UserDefinedEvent e) => (e: e, Id: e.Id))).ToArray();
	}

	private static string[] FilterUserDefinedEventsBasedOnPermissions(IGeminiRequestContext requestContext, string[] userDefinedEventIds)
	{
		return SecurityFilterUtilities.FilterItems(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_READ", userDefinedEventIds.Select((string id) => (id, id))).ToArray();
	}

	public static void TriggerBuiltInEventForDevice(IGeminiRequestContext requestContext, string eventId, string deviceId, DateTime time)
	{
		using ServerWebServiceManager serverWebServiceManager = new ServerWebServiceManager(int.Parse(Config.Instance.GetAppSetting("ServerPort", "8080"), CultureInfo.InvariantCulture));
		serverWebServiceManager.CallWithRetry(delegate(ServerWebServiceClient client)
		{
			client.TriggerBuiltInEventForDevice(eventId, deviceId, time, new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress));
		}, null, (Exception ex, Action ac) => false);
	}
}
