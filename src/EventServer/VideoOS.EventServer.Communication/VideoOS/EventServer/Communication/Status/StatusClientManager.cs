using System;
using System.Collections.Generic;
using VideoOS.EventServer.Communication.Status.Client;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Status;

public class StatusClientManager
{
	public delegate void FaultExceptionHandler(Exception error, int requestId);

	public delegate void GetStatusHandler(int requestId, MapObjectState[] states);

	public delegate void GetStatusTranslatedHandler(int requestId, MapObjectState[] states);

	public delegate void GetStatusForAllObjectsOfTypeHandler(int requestId, MapObjectStateTreeNode[] states);

	public delegate void GetStatusDetailsHandler(int requestId, string details);

	public delegate void SetVisualizationHandler(int requestId, DateTime time);

	public delegate void GetVisualizationHandler(int requestId, VisualizationState visualizationState);

	public delegate void GetVisualizationChangesHandler(int requestId, VisualizationState visualizationState);

	private Dictionary<ServerId, StatusClient> _clients = new Dictionary<ServerId, StatusClient>();

	private object _lockObject = new object();

	private StatusClientFactory _clientFactory = new StatusClientFactory();

	public event FaultExceptionHandler FaultExceptionEvent;

	public event GetStatusHandler GetStatusEvent;

	public event GetStatusTranslatedHandler GetStatusTranslatedEvent;

	public event GetStatusForAllObjectsOfTypeHandler GetStatusForAllObjectsOfTypeEvent;

	public event GetStatusDetailsHandler GetStatusDetailsEvent;

	public event SetVisualizationHandler SetVisualizationEvent;

	public event GetVisualizationHandler GetVisualizationEvent;

	public event GetVisualizationChangesHandler GetVisualizationChangesEvent;

	public StatusClient GetStatusClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			if (!_clients.TryGetValue(serverId, out var value))
			{
				StatusClient statusClient = CreateStatusClient(serverId);
				if (statusClient == null)
				{
					throw new ServiceNotRegisteredException();
				}
				_clients.Add(serverId, statusClient);
			}
			else
			{
				StatusClient statusClient = value;
			}
			return _clients[serverId];
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (StatusClient value in _clients.Values)
			{
				DestroyStatusClient(value);
			}
			_clients.Clear();
		}
	}

	private StatusClient CreateStatusClient(ServerId serverId)
	{
		VideoOS.Platform.Configuration instance = VideoOS.Platform.Configuration.Instance;
		try
		{
			List<VideoOS.Platform.Configuration.ServiceURIInfo> registeredServiceUriInfo = instance.GetRegisteredServiceUriInfo(ServiceTypes.ServiceType_EventServerService, serverId);
			if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
			{
				throw new ServiceNotRegisteredException();
			}
			Version eventServerVersion = ServerReceiver.GetEventServerVersion2(registeredServiceUriInfo[0]);
			StatusClient statusClient = _clientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], eventServerVersion);
			statusClient.FaultExceptionEvent += FireFaultException;
			statusClient.GetStatusEvent += FireGetStatus;
			statusClient.GetStatusTranslatedEvent += FireGetStatusTranslated;
			statusClient.GetStatusForAllObjectsOfTypeEvent += FireGetStatusForAllObjectsOfType;
			statusClient.GetStatusDetailsEvent += FireGetStatusDetails;
			statusClient.SetVisualizationEvent += FireSetVisualization;
			statusClient.GetVisualizationEvent += FireGetVisualization;
			statusClient.GetVisualizationChangesEvent += FireGetVisualizationChanges;
			return statusClient;
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
		}
	}

	private void DestroyStatusClient(StatusClient statusclient)
	{
		if (statusclient != null)
		{
			statusclient.FaultExceptionEvent -= FireFaultException;
			statusclient.GetStatusEvent -= FireGetStatus;
			statusclient.GetStatusTranslatedEvent -= FireGetStatusTranslated;
			statusclient.GetStatusForAllObjectsOfTypeEvent -= FireGetStatusForAllObjectsOfType;
			statusclient.GetStatusDetailsEvent -= FireGetStatusDetails;
			statusclient.SetVisualizationEvent -= FireSetVisualization;
			statusclient.GetVisualizationEvent -= FireGetVisualization;
			statusclient.GetVisualizationChangesEvent -= FireGetVisualizationChanges;
			statusclient.Close();
		}
	}

	private void FireFaultException(Exception error, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(error, requestId);
		}
	}

	private void FireGetStatus(int requestId, MapObjectState[] states)
	{
		if (this.GetStatusEvent != null)
		{
			this.GetStatusEvent(requestId, states);
		}
	}

	private void FireGetStatusTranslated(int requestId, MapObjectState[] states)
	{
		if (this.GetStatusTranslatedEvent != null)
		{
			this.GetStatusTranslatedEvent(requestId, states);
		}
	}

	private void FireGetStatusForAllObjectsOfType(int requestId, MapObjectStateTreeNode[] states)
	{
		if (this.GetStatusForAllObjectsOfTypeEvent != null)
		{
			this.GetStatusForAllObjectsOfTypeEvent(requestId, states);
		}
	}

	private void FireGetStatusDetails(int requestId, string details)
	{
		if (this.GetStatusDetailsEvent != null)
		{
			this.GetStatusDetailsEvent(requestId, details);
		}
	}

	private void FireSetVisualization(int requestId, DateTime time)
	{
		if (this.SetVisualizationEvent != null)
		{
			this.SetVisualizationEvent(requestId, time);
		}
	}

	private void FireGetVisualization(int requestId, VisualizationState visualizationState)
	{
		if (this.GetVisualizationEvent != null)
		{
			this.GetVisualizationEvent(requestId, visualizationState);
		}
	}

	private void FireGetVisualizationChanges(int requestId, VisualizationState visualizationState)
	{
		if (this.GetVisualizationChangesEvent != null)
		{
			this.GetVisualizationChangesEvent(requestId, visualizationState);
		}
	}
}
