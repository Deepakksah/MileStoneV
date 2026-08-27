using System;
using VideoOS.EventServer.Communication.Status.Client;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.Status;

public abstract class StatusClient : AbstractClient
{
	public delegate void FaultExceptionHandler(Exception exception, int requestId);

	public delegate void GetStatusHandler(int requestId, MapObjectState[] states);

	public delegate void GetStatusTranslatedHandler(int requestId, MapObjectState[] states);

	public delegate void GetStatusForAllObjectsOfTypeHandler(int requestId, MapObjectStateTreeNode[] states);

	public delegate void GetStatusDetailsHandler(int requestId, string details);

	public delegate void SetVisualizationHandler(int requestId, DateTime time);

	public delegate void GetVisualizationHandler(int requestId, VisualizationState state);

	public delegate void GetVisualizationChangesHandler(int requestId, VisualizationState state);

	protected ServerId ServerId { get; private set; }

	public event FaultExceptionHandler FaultExceptionEvent;

	public event GetStatusHandler GetStatusEvent;

	public event GetStatusTranslatedHandler GetStatusTranslatedEvent;

	public event GetStatusForAllObjectsOfTypeHandler GetStatusForAllObjectsOfTypeEvent;

	public event GetStatusDetailsHandler GetStatusDetailsEvent;

	public event SetVisualizationHandler SetVisualizationEvent;

	public event GetVisualizationHandler GetVisualizationEvent;

	public event GetVisualizationChangesHandler GetVisualizationChangesEvent;

	protected StatusClient(ServerId serverId, string eventServerAddress, Version eventServerVersion, Uri uri)
		: base(eventServerAddress, eventServerVersion, uri)
	{
		ServerId = serverId;
	}

	protected override void HandleRequestObject(RequestObject requestObject)
	{
		if (requestObject is GetStatusRequestObject)
		{
			GetStatusRequestObject getStatusRequestObject = requestObject as GetStatusRequestObject;
			MapObjectState[] status = GetStatus(getStatusRequestObject.Id);
			FireGetStatus(getStatusRequestObject.RequestId, status);
			return;
		}
		if (requestObject is GetStatusTranslatedRequestObject)
		{
			GetStatusTranslatedRequestObject getStatusTranslatedRequestObject = requestObject as GetStatusTranslatedRequestObject;
			MapObjectState[] statusTranslated = GetStatusTranslated(getStatusTranslatedRequestObject.Id, getStatusTranslatedRequestObject.Language);
			FireGetStatusTranslated(getStatusTranslatedRequestObject.RequestId, statusTranslated);
			return;
		}
		if (requestObject is GetStatusForAllObjectsOfTypeRequestObject)
		{
			GetStatusForAllObjectsOfTypeRequestObject getStatusForAllObjectsOfTypeRequestObject = requestObject as GetStatusForAllObjectsOfTypeRequestObject;
			MapObjectStateTreeNode[] statusForAllObjectsOfType = GetStatusForAllObjectsOfType(getStatusForAllObjectsOfTypeRequestObject.Id, getStatusForAllObjectsOfTypeRequestObject.Type, getStatusForAllObjectsOfTypeRequestObject.RelationType, getStatusForAllObjectsOfTypeRequestObject.MaxLevels);
			FireGetStatusForAllObjectsOfType(getStatusForAllObjectsOfTypeRequestObject.RequestId, statusForAllObjectsOfType);
			return;
		}
		if (requestObject is GetStatusDetailsRequestObject)
		{
			GetStatusDetailsRequestObject getStatusDetailsRequestObject = requestObject as GetStatusDetailsRequestObject;
			string statusDetails = GetStatusDetails(getStatusDetailsRequestObject.Id, getStatusDetailsRequestObject.Type, getStatusDetailsRequestObject.Language);
			FireGetStatusDetails(getStatusDetailsRequestObject.RequestId, statusDetails);
			return;
		}
		if (requestObject is SetVisualizationRequestObject)
		{
			SetVisualizationRequestObject setVisualizationRequestObject = requestObject as SetVisualizationRequestObject;
			DateTime time = SetVisualization(setVisualizationRequestObject.VisualizationState);
			FireSetVisualization(setVisualizationRequestObject.RequestId, time);
			return;
		}
		if (requestObject is GetVisualizationRequestObject)
		{
			GetVisualizationRequestObject getVisualizationRequestObject = requestObject as GetVisualizationRequestObject;
			VisualizationState visualization = GetVisualization();
			FireGetVisualization(getVisualizationRequestObject.RequestId, visualization);
			return;
		}
		if (requestObject is GetVisualizationChangesRequestObject)
		{
			GetVisualizationChangesRequestObject getVisualizationChangesRequestObject = requestObject as GetVisualizationChangesRequestObject;
			VisualizationState visualizationChanges = GetVisualizationChanges(getVisualizationChangesRequestObject.Timestamp, getVisualizationChangesRequestObject.Timeout);
			FireGetVisualizationChanges(getVisualizationChangesRequestObject.RequestId, visualizationChanges);
			return;
		}
		throw new ArgumentException("Unknown request object");
	}

	protected override void FireFaultException(Exception exception, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(exception, requestId);
		}
	}

	private void FireGetStatus(int requestId, MapObjectState[] states)
	{
		if (this.GetStatusEvent != null)
		{
			this.GetStatusEvent(requestId, states);
		}
	}

	public void BeginGetStatus(int requestId, string id)
	{
		Enqueue(new GetStatusRequestObject(requestId, id));
	}

	public abstract MapObjectState[] GetStatus(string id);

	private void FireGetStatusTranslated(int requestId, MapObjectState[] states)
	{
		if (this.GetStatusTranslatedEvent != null)
		{
			this.GetStatusTranslatedEvent(requestId, states);
		}
	}

	public void BeginGetStatusTranslated(int requestId, string id, string language)
	{
		Enqueue(new GetStatusTranslatedRequestObject(requestId, id, language));
	}

	public abstract MapObjectState[] GetStatusTranslated(string id, string language);

	public abstract MapObjectState[] GetStatusByUserIds(string token, string id, MapUserId[] userIds);

	private void FireGetStatusForAllObjectsOfType(int requestId, MapObjectStateTreeNode[] states)
	{
		if (this.GetStatusForAllObjectsOfTypeEvent != null)
		{
			this.GetStatusForAllObjectsOfTypeEvent(requestId, states);
		}
	}

	public void BeginGetStatusForAllObjectsOfType(int requestId, string id, Guid type, Guid relationType, int maxLevels)
	{
		Enqueue(new GetStatusForAllObjectsOfTypeRequestObject(requestId, id, type, relationType, maxLevels));
	}

	public abstract MapObjectStateTreeNode[] GetStatusForAllObjectsOfType(string id, Guid type, Guid relationType, int maxLevels);

	private void FireGetStatusDetails(int requestId, string details)
	{
		if (this.GetStatusDetailsEvent != null)
		{
			this.GetStatusDetailsEvent(requestId, details);
		}
	}

	public void BeginGetStatusDetails(int requestId, string id, Guid type, string language)
	{
		Enqueue(new GetStatusDetailsRequestObject(requestId, id, type, language));
	}

	public abstract string GetStatusDetails(string id, Guid type, string language);

	private void FireSetVisualization(int requestId, DateTime time)
	{
		if (this.SetVisualizationEvent != null)
		{
			this.SetVisualizationEvent(requestId, time);
		}
	}

	public void BeginSetVisualization(int requestId, VisualizationState visualizationState)
	{
		Enqueue(new SetVisualizationRequestObject(requestId, visualizationState));
	}

	public abstract DateTime SetVisualization(VisualizationState visualizationState);

	private void FireGetVisualization(int requestId, VisualizationState state)
	{
		if (this.GetVisualizationEvent != null)
		{
			this.GetVisualizationEvent(requestId, state);
		}
	}

	public void BeginGetVisualization(int requestId)
	{
		Enqueue(new GetVisualizationRequestObject(requestId));
	}

	public abstract VisualizationState GetVisualization();

	private void FireGetVisualizationChanges(int requestId, VisualizationState state)
	{
		if (this.GetVisualizationChangesEvent != null)
		{
			this.GetVisualizationChangesEvent(requestId, state);
		}
	}

	public void BeginGetVisualizationChanges(int requestId, DateTime timestamp, int timeout)
	{
		Enqueue(new GetVisualizationChangesRequestObject(requestId, timestamp, timeout));
	}

	public abstract VisualizationState GetVisualizationChanges(DateTime timestamp, int timeout);
}
