using System;
using System.Collections.Generic;
using System.ServiceModel;
using VideoOS.Management.Common;
using VideoOS.Platform;

namespace VideoOS.EventServer.Communication.AlarmSecurity;

public class AlarmSecurityClientManager
{
	public delegate void FaultExceptionHandler(Exception error, int requestId);

	public delegate void GetUsersPermittedByObjectHandler(int requestId, Identity[] users);

	public delegate void GetUsersPermittedByObjectListHandler(int requestId, Identity[] users);

	public delegate void GetUsersPermittedByKindHandler(int requestId, Identity[] users);

	public delegate void GetUsersPermittedByKindOnServerHandler(int requestId, Identity[] users);

	public delegate void GetAllUsersHandler(int requestId, Identity[] users);

	private Dictionary<ServerId, AlarmSecurityClient> _clients = new Dictionary<ServerId, AlarmSecurityClient>();

	private object _lockObject = new object();

	private AlarmSecurityClientFactory _clientFactory = new AlarmSecurityClientFactory();

	public event FaultExceptionHandler FaultExceptionEvent;

	public event GetUsersPermittedByObjectHandler GetUsersPermittedByObjectEvent;

	public event GetUsersPermittedByObjectListHandler GetUsersPermittedByObjectListEvent;

	public event GetUsersPermittedByKindHandler GetUsersPermittedByKindEvent;

	public event GetUsersPermittedByKindOnServerHandler GetUsersPermittedByKindOnServerEvent;

	public event GetAllUsersHandler GetAllUsersEvent;

	public AlarmSecurityClient GetAlarmSecurityClient(ServerId serverId)
	{
		lock (_lockObject)
		{
			AlarmSecurityClient value = null;
			if (!_clients.TryGetValue(serverId, out value))
			{
				value = CreateAlarmSecurityClient(serverId);
				if (value == null)
				{
					throw new ServiceNotRegisteredException();
				}
				_clients.Add(serverId, value);
			}
			return value;
		}
	}

	public void Close()
	{
		lock (_lockObject)
		{
			foreach (AlarmSecurityClient value in _clients.Values)
			{
				if (value.State == CommunicationState.Opened)
				{
					DestroyAlarmSecurityClient(value);
				}
			}
			_clients.Clear();
		}
	}

	private AlarmSecurityClient CreateAlarmSecurityClient(ServerId serverId)
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
			AlarmSecurityClient alarmSecurityClient = _clientFactory.Create(serverId, registeredServiceUriInfo[0].UriArray[0], eventServerVersion);
			alarmSecurityClient.FaultExceptionEvent += FireFaultException;
			alarmSecurityClient.GetUsersPermittedByObjectEvent += FireGetUsersPermittedByObject;
			alarmSecurityClient.GetAllUsersEvent += FireGetAllUsers;
			return alarmSecurityClient;
		}
		catch (CommunicationMIPException innerException)
		{
			throw new ServiceNotRegisteredException("Error retrieving registered services", innerException);
		}
	}

	private void DestroyAlarmSecurityClient(AlarmSecurityClient alarmSecurityClient)
	{
		if (alarmSecurityClient != null)
		{
			alarmSecurityClient.FaultExceptionEvent -= FireFaultException;
			alarmSecurityClient.GetUsersPermittedByObjectEvent -= FireGetUsersPermittedByObject;
			alarmSecurityClient.GetAllUsersEvent -= FireGetAllUsers;
			alarmSecurityClient.Close();
		}
	}

	private void FireFaultException(Exception error, int requestId)
	{
		if (this.FaultExceptionEvent != null)
		{
			this.FaultExceptionEvent(error, requestId);
		}
	}

	private void FireGetUsersPermittedByObject(int requestId, Identity[] users)
	{
		if (this.GetUsersPermittedByObjectEvent != null)
		{
			this.GetUsersPermittedByObjectEvent(requestId, users);
		}
	}

	private void FireGetUsersPermittedByObjectList(int requestId, Identity[] users)
	{
		if (this.GetUsersPermittedByObjectListEvent != null)
		{
			this.GetUsersPermittedByObjectListEvent(requestId, users);
		}
	}

	private void FireGetUsersPermittedByKind(int requestId, Identity[] users)
	{
		if (this.GetUsersPermittedByKindEvent != null)
		{
			this.GetUsersPermittedByKindEvent(requestId, users);
		}
	}

	private void FireGetUsersPermittedByKindOnServer(int requestId, Identity[] users)
	{
		if (this.GetUsersPermittedByKindOnServerEvent != null)
		{
			this.GetUsersPermittedByKindOnServerEvent(requestId, users);
		}
	}

	private void FireGetAllUsers(int requestId, Identity[] users)
	{
		if (this.GetAllUsersEvent != null)
		{
			this.GetAllUsersEvent(requestId, users);
		}
	}
}
