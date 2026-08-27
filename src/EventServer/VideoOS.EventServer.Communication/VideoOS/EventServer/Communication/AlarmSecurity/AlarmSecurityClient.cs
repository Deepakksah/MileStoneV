using System;
using VideoOS.Management.Common;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public abstract class AlarmSecurityClient : AbstractClient
{
	public delegate void FaultExceptionHandler(Exception exception, int requestId);

	public delegate void GetUsersPermittedByObjectHandler(int requestId, Identity[] users);

	public delegate void GetAllUsersHandler(int requestId, Identity[] users);

	protected ServerId ServerId { get; private set; }

	public event FaultExceptionHandler FaultExceptionEvent;

	public event GetUsersPermittedByObjectHandler GetUsersPermittedByObjectEvent;

	public event GetAllUsersHandler GetAllUsersEvent;

	protected AlarmSecurityClient(ServerId serverId, string eventServerAddress, Version eventServerVersion, Uri uri)
		: base(eventServerAddress, eventServerVersion, uri)
	{
		ServerId = serverId;
	}

	protected override void HandleRequestObject(RequestObject requestObject)
	{
		if (requestObject is GetUsersPermittedByObjectRequestObject)
		{
			GetUsersPermittedByObjectRequestObject getUsersPermittedByObjectRequestObject = requestObject as GetUsersPermittedByObjectRequestObject;
			Identity[] usersPermittedByObject = GetUsersPermittedByObject(getUsersPermittedByObjectRequestObject.ObjectId);
			FireGetUsersPermittedByObject(getUsersPermittedByObjectRequestObject.RequestId, usersPermittedByObject);
			return;
		}
		if (requestObject is GetAllUsersRequestObject)
		{
			GetAllUsersRequestObject getAllUsersRequestObject = requestObject as GetAllUsersRequestObject;
			Identity[] allUsers = GetAllUsers();
			FireGetAllUsers(getAllUsersRequestObject.RequestId, allUsers);
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

	private void FireGetUsersPermittedByObject(int requestId, Identity[] users)
	{
		if (this.GetUsersPermittedByObjectEvent != null)
		{
			this.GetUsersPermittedByObjectEvent(requestId, users);
		}
	}

	public void BeginGetUsersPermittedByObject(int requestId, string objectId)
	{
		Enqueue(new GetUsersPermittedByObjectRequestObject(requestId, objectId));
	}

	public abstract Identity[] GetUsersPermittedByObject(string objectId);

	private void FireGetAllUsers(int requestId, Identity[] users)
	{
		if (this.GetAllUsersEvent != null)
		{
			this.GetAllUsersEvent(requestId, users);
		}
	}

	public void BeginGetAllUsers(int requestId)
	{
		Enqueue(new GetAllUsersRequestObject(requestId));
	}

	public abstract Identity[] GetAllUsers();
}
