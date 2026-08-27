using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Platform.Telemetry;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging;

public class MessageCommunicationManager
{
	public enum SecurityLevel
	{
		High = 1,
		Normal,
		Low
	}

	private static Dictionary<Guid, MessageCommunication> _messageCommunications;

	private static Dictionary<Guid, int> _startCallsByServerId;

	private static FQID _fqid;

	private static readonly object _fqidLock;

	public static Guid ServerEndPointObjectId;

	private static SecurityLevel _securityLevel;

	private static bool IsTraceEnabled
	{
		get
		{
			if (EnvironmentManager.Instance != null)
			{
				return EnvironmentManager.Instance.TraceMessageCommunication;
			}
			return false;
		}
	}

	public static EndPointType EndPointType
	{
		get
		{
			if (EnvironmentManager.Instance.EnvironmentType != EnvironmentType.Service)
			{
				return EndPointType.Client;
			}
			return EndPointType.Server;
		}
	}

	public static FQID EndPointFQID
	{
		get
		{
			if (NeedSetServerType())
			{
				lock (_fqidLock)
				{
					if (NeedSetServerType())
					{
						_fqid.ServerId.ServerType = EnvironmentManager.Instance.EnvironmentType.ToString();
					}
				}
			}
			return _fqid;
		}
	}

	public static SecurityLevel Security
	{
		get
		{
			return _securityLevel;
		}
		set
		{
			_securityLevel = value;
		}
	}

	public static event ConnectionStateChangedEventHandler ConnectionStateChangedEvent;

	static MessageCommunicationManager()
	{
		_messageCommunications = new Dictionary<Guid, MessageCommunication>();
		_startCallsByServerId = new Dictionary<Guid, int>();
		_fqidLock = new object();
		ServerEndPointObjectId = new Guid("CBE4184C-9AB7-409C-A981-5F7E7B53B641");
		_securityLevel = SecurityLevel.Normal;
		SetId();
	}

	public static void Init()
	{
		lock (_messageCommunications)
		{
			if (IsTraceEnabled)
			{
				MipTrace.MgsCom("MessageCommunicationManager", "Init");
			}
			foreach (Guid key in _messageCommunications.Keys)
			{
				Item item = Configuration.Instance.GetItem(key, Kind.Server);
				if (item != null)
				{
					_messageCommunications[key].Init(item.FQID.ServerId);
				}
			}
		}
	}

	public static void Close()
	{
		if (IsTraceEnabled)
		{
			MipTrace.MgsCom("MessageCommunicationManager", "Close");
		}
		bool flag = false;
		while (!flag)
		{
			Guid guid = Guid.Empty;
			lock (_messageCommunications)
			{
				if (_messageCommunications.Count > 0)
				{
					guid = _messageCommunications.Keys.ElementAt(0);
				}
			}
			if (guid != Guid.Empty)
			{
				CloseMessageCommunicationClient(guid);
			}
			else
			{
				flag = true;
			}
		}
		SetId();
	}

	private static void SetId()
	{
		Guid guid = ((EnvironmentManager.Instance != null && EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service) ? ServerEndPointObjectId : Guid.NewGuid());
		_fqid = new FQID(new ServerId((EnvironmentManager.Instance != null) ? EnvironmentManager.Instance.EnvironmentType.ToString() : "", Environment.MachineName, 80, guid), Guid.Empty, guid, FolderType.No, Kind.Server);
	}

	private static void CloseMessageCommunicationClient(Guid id)
	{
		if (IsTraceEnabled)
		{
			MipTrace.MgsCom("MessageCommunicationManager", "CloseMessageCommunctionClient");
		}
		MessageCommunication messageCommunication = _messageCommunications[id];
		messageCommunication.Close();
		messageCommunication.Dispose();
		_messageCommunications.Remove(id);
	}

	private static bool NeedSetServerType()
	{
		if (EnvironmentManager.Instance != null)
		{
			return string.IsNullOrEmpty(_fqid.ServerId.ServerType);
		}
		return false;
	}

	public static void Start(ServerId serverId)
	{
		lock (_messageCommunications)
		{
			if (!_startCallsByServerId.ContainsKey(serverId.Id))
			{
				_startCallsByServerId.Add(serverId.Id, 0);
			}
			if (IsTraceEnabled)
			{
				Guid id = serverId.Id;
				MipTrace.MgsCom("MessageCommunicationManager", "Start - " + id.ToString());
			}
			if (EnvironmentManager.Instance.EnvironmentType == EnvironmentType.Service)
			{
				if (!_messageCommunications.ContainsKey(serverId.Id))
				{
					MessageCommunication messageCommunication = new MessageCommunication();
					_messageCommunications.Add(serverId.Id, messageCommunication);
					if (EnvironmentManager.Instance.ApplicationLoggedOn)
					{
						messageCommunication.Init(serverId);
						new TrackPageViewData(typeof(MessageCommunication)).Send();
					}
				}
			}
			else
			{
				Migration.InitCheckForMigration();
				if (!_messageCommunications.ContainsKey(serverId.Id))
				{
					MessageCommunication messageCommunication2 = new MessageCommunication();
					_messageCommunications.Add(serverId.Id, messageCommunication2);
					if (EnvironmentManager.Instance.ApplicationLoggedOn)
					{
						messageCommunication2.Init(serverId);
						new TrackPageViewData(typeof(MessageCommunication)).Send();
					}
					else
					{
						EnvironmentManager.Instance.Log(error: false, "MessageCommunicationManager.Start", "Login process not completed - MessageCommunication cannot start yet.  Wait for the ApplicationLoggedOn or ConfigurationChangedIndication before using MessageCommunication");
					}
				}
			}
			_startCallsByServerId[serverId.Id]++;
		}
	}

	public static void Stop(ServerId serverId)
	{
		lock (_messageCommunications)
		{
			if (IsTraceEnabled)
			{
				Guid id = serverId.Id;
				MipTrace.MgsCom("MessageCommunicationManager", "Stop - " + id.ToString());
			}
			if (!_messageCommunications.ContainsKey(serverId.Id))
			{
				return;
			}
			MessageCommunication messageCommunication = _messageCommunications[serverId.Id];
			if ((messageCommunication.ServerId.UserContext == null || messageCommunication.ServerId.UserContext.InstanceGuid == serverId.UserContext.InstanceGuid) && _startCallsByServerId.ContainsKey(serverId.Id))
			{
				_startCallsByServerId[serverId.Id]--;
				if (_startCallsByServerId[serverId.Id] <= 0)
				{
					CloseMessageCommunicationClient(serverId.Id);
				}
			}
		}
	}

	public static MessageCommunication Get(ServerId serverId)
	{
		lock (_messageCommunications)
		{
			if (_messageCommunications.ContainsKey(serverId.Id))
			{
				return _messageCommunications[serverId.Id];
			}
		}
		return null;
	}

	public static void FireConnectionStateChanged(object sender, ServerId serverId, Uri uri)
	{
		if (IsTraceEnabled)
		{
			Guid id = serverId.Id;
			MipTrace.MgsCom("MessageCommunicationManager", "FireConnectionStateChanged - " + id.ToString());
		}
		if (MessageCommunicationManager.ConnectionStateChangedEvent != null)
		{
			MessageCommunicationManager.ConnectionStateChangedEvent(sender, new ConnectionStateChangedEventArgs(serverId, uri, Configuration.MAPServiceId));
		}
	}
}
