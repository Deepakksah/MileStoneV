using System;
using System.Threading;
using VideoOS.Platform.Messaging.Internal;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

public class MessageCommunication : IDisposable
{
	private ServerId _serverId;

	private ICommunicationClient _client;

	private ReaderWriterLockSlim _clientLock = new ReaderWriterLockSlim();

	private FQID _serverEndPointFQID;

	private static Guid _broadcastGuid = new Guid("C581D16A-0A99-470B-BBAE-C159C8B5B72C");

	private static FQID _broadcastEndPointFQID = new FQID(new ServerId(EnvironmentType.Service.ToString(), "", 80, Guid.Empty), Guid.Empty, _broadcastGuid, FolderType.No, Kind.Server);

	public const string WhoAreOnlineRequest = "MessageCommunication.WhoAreOnlineRequest";

	public const string WhoAreOnlineResponse = "MessageCommunication.WhoAreOnlineResponse";

	public const string NewEndPointIndication = "MessageCommunication.NewEndPointIndication";

	public const string EndPointCloseIndication = "MessageCommunication.EndPointCloseIndication";

	public const string EndPointTableChangedIndication = "MessageCommunication.EndPointTableChangedIndication";

	public const string ProvideCurrentStateRequest = "MessageCommunication.ProvideCurrentStateRequest";

	public const string ProvideCurrentStateResponse = "MessageCommunication.ProvideCurrentStateResponse";

	public ServerId ServerId => _serverId;

	public Uri Uri
	{
		get
		{
			if (_client != null)
			{
				return _client.Uri;
			}
			return null;
		}
	}

	public bool IsConnected
	{
		get
		{
			if (_clientLock == null)
			{
				return false;
			}
			_clientLock.EnterReadLock();
			try
			{
				if (_client == null)
				{
					return false;
				}
				return _client.IsConnected;
			}
			finally
			{
				_clientLock.ExitReadLock();
			}
		}
	}

	public FQID ServerEndPointFQID => _serverEndPointFQID;

	public static FQID BroadcastEndPointFQID => _broadcastEndPointFQID;

	public event EventHandler ConnectionStateChangedEvent;

	public MessageCommunication()
	{
	}

	public void Init(ServerId serverId)
	{
		_clientLock.EnterWriteLock();
		try
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				Guid id = serverId.Id;
				MipTrace.MgsCom("MessageCommunication", "Init - " + id.ToString());
			}
			if (_client != null)
			{
				return;
			}
			_serverId = serverId;
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
			{
				if (serverId.Id == EnvironmentManager.Instance.MasterSite.ServerId.Id)
				{
					_client = new CommunicationClientOnServer();
				}
				else
				{
					_client = new CommunicationClient(_serverId, this);
				}
			}
			else
			{
				_client = new CommunicationClient(_serverId, this);
			}
			try
			{
				_client.Init();
			}
			catch (Exception ex)
			{
				EnvironmentManager.Instance.Log(error: false, "MessageCommunication.Init:", ex.Message);
			}
			_serverEndPointFQID = new FQID(new ServerId(EnvironmentType.Service.ToString(), "", 80, Guid.Empty), Guid.Empty, MessageCommunicationManager.ServerEndPointObjectId, FolderType.No, Kind.Server);
		}
		finally
		{
			_clientLock.ExitWriteLock();
		}
	}

	public void Close()
	{
		if (_clientLock != null && _client != null)
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				Guid id = _serverId.Id;
				MipTrace.MgsCom("MessageCommunication", "Close - " + id.ToString());
			}
			_client.Close();
			_client = null;
		}
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			Close();
			if (_clientLock != null)
			{
				_clientLock.Dispose();
			}
			_clientLock = null;
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	public object RegisterCommunicationFilter(MessageReceiver messageReceiver, CommunicationIdFilter messageFilter)
	{
		if (messageFilter == null)
		{
			throw new ArgumentNullException("messageFilter");
		}
		if (messageFilter.MessageId == "Server.NewAlarmIndication" || messageFilter.MessageId == "Server.ChangedAlarmIndication" || messageFilter.MessageId == "Server.ChangedAlarmHistoryIndication" || messageFilter.MessageId == "Server.NewEventIndication" || messageFilter.MessageId == "Server.NewEventsIndication")
		{
			EnvironmentManager.Instance.Log(error: false, "RegisterCommunicationFilter", "Warning: Registration changed to ONLY subscribe from EventServer, please modify to specify EndPointType.Server");
			return RegisterCommunicationFilter(messageReceiver, messageFilter, null, EndPointType.Server);
		}
		return RegisterCommunicationFilter(messageReceiver, messageFilter, null, EndPointType.Any);
	}

	public void WaitForCommunicationFilterRegistration(CommunicationIdFilter messageFilter)
	{
		WaitForCommunicationFilterRegistration(messageFilter, TimeSpan.Zero);
	}

	public void WaitForCommunicationFilterRegistration(CommunicationIdFilter messageFilter, TimeSpan timeout)
	{
		if (messageFilter == null)
		{
			throw new ArgumentException("messageFilter is null!");
		}
		if (_client == null)
		{
			throw new MIPException("Message Communication is not started!");
		}
		_client.WaitForServerRegistration(messageFilter, timeout);
	}

	public object RegisterCommunicationFilter(MessageReceiver messageReceiver, CommunicationIdFilter communicationIdFilter, FQID endPoint, EndPointType endPointType)
	{
		if (communicationIdFilter == null)
		{
			throw new ArgumentException("cannot be null", "communicationIdFilter");
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageCommunication", "RegisterCommunicationFilter - " + communicationIdFilter.MessageId);
		}
		CommunicationFilter communicationFilter = new CommunicationFilter
		{
			CommunicationIdFilter = communicationIdFilter,
			EndPoint = endPoint,
			EndPointType = endPointType,
			Id = Guid.NewGuid(),
			RequesterEndPoint = MessageCommunicationManager.EndPointFQID
		};
		if (_clientLock == null)
		{
			EnvironmentManager.Instance.Log(error: true, "MessageCommunication", "Call to RegisterComminucationFilter after call to Dispose?");
			return communicationFilter;
		}
		_clientLock.EnterReadLock();
		try
		{
			if (_client != null)
			{
				_client.StoreForRegistration(messageReceiver, communicationFilter);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: true, "MessageCommunication.RegisterCommunicationFilter", "Client session not initialized - registration not done");
			}
		}
		finally
		{
			_clientLock.ExitReadLock();
		}
		return communicationFilter;
	}

	public void UnRegisterCommunicationFilter(object communicationFilterObject)
	{
		if (communicationFilterObject is CommunicationFilter communicationFilter)
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("MessageCommunication", "UnRegisterCommunicationFilter - " + communicationFilter.CommunicationIdFilter.MessageId);
			}
			if (_clientLock == null)
			{
				return;
			}
			_clientLock.EnterReadLock();
			try
			{
				if (_client != null)
				{
					_client.ToBeRemoved(communicationFilter.Id);
				}
				return;
			}
			finally
			{
				_clientLock.ExitReadLock();
			}
		}
		EnvironmentManager.Instance.Log(error: true, "UnRegisterCommunicationFilter", "Invalid or null parameter : communicationFilterObject");
	}

	public void TransmitMessage(Message message, FQID destinationEndPoint, FQID destinationObject, FQID source)
	{
		if (message == null)
		{
			throw new ArgumentException("cannot be null", "message");
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageCommunication", "TransmitMessage - " + message.MessageId);
		}
		if (_clientLock == null)
		{
			EnvironmentManager.Instance.Log(error: true, "MessageCommunication", "Call to TransmitMessage after call to Dispose?");
			return;
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageCommunication", "TransmitMessage before lock - " + message.MessageId);
		}
		_clientLock.EnterReadLock();
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageCommunication", "TransmitMessage inside lock - " + message.MessageId);
		}
		try
		{
			if (_client != null)
			{
				_client.Transmit(message, destinationEndPoint, destinationObject, source);
			}
			else
			{
				EnvironmentManager.Instance.Log(error: true, "TransmitMessage", "Communication client has been closed, cannot transmit to server");
			}
		}
		finally
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("MessageCommunication", "TransmitMessage after lock - " + message.MessageId);
			}
			_clientLock.ExitReadLock();
		}
	}

	public void FireConnectionStateChanged()
	{
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("MessageCommunication", "FireConnectionStateChanged - " + IsConnected);
		}
		if (this.ConnectionStateChangedEvent != null)
		{
			this.ConnectionStateChangedEvent(this, new ConnectionStateEventArgs(IsConnected));
		}
	}
}
