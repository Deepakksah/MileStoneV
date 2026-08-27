using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging.Internal;

public class CommunicationClient : ICommunicationClient
{
	private enum State
	{
		Running,
		Stopping,
		Stopped
	}

	private CommunicationServiceClient _communicationServiceClient;

	private ServerId _serverId;

	private Thread _thread;

	private State _state;

	private bool _serverKnowMe;

	private string _username = "";

	private Uri _uri;

	private EndPointIdentityData _endPointIdentityData;

	private readonly List<CommunicationFilter> _myRegisteredFilters = new List<CommunicationFilter>();

	private Collection<CommunicationFilter> _myRegisteredFiltersOnServer = new Collection<CommunicationFilter>();

	private readonly object _myRegisteredFiltersOnServerLock = new object();

	private readonly IWaitUtil _waitUtil = new WaitUtil();

	private readonly Dictionary<Guid, MessageReceiver> _messageReceivers = new Dictionary<Guid, MessageReceiver>();

	private Collection<Guid> _toBeRemovedFilters = new Collection<Guid>();

	private readonly Dictionary<Guid, CommunicationFilter> _serverDefinedFilters = new Dictionary<Guid, CommunicationFilter>();

	private LoginSettings _loginSettings;

	private readonly object _lockObject = new object();

	private object _msgObject;

	private MessageCommunication _messageCommunication;

	private bool _isConnected;

	private string _serviceUrl;

	private Collection<Type> _prohibitedClasses = new Collection<Type> { typeof(LoginSettings) };

	public override Uri Uri => _uri;

	public override bool IsConnected
	{
		get
		{
			return _isConnected;
		}
		set
		{
			if (_isConnected != value)
			{
				_isConnected = value;
				_messageCommunication.FireConnectionStateChanged();
				MessageCommunicationManager.FireConnectionStateChanged(_messageCommunication, _messageCommunication.ServerId, _messageCommunication.Uri);
			}
		}
	}

	public CommunicationClient(ServerId serverId, MessageCommunication messageCommunication)
	{
		_serverId = serverId;
		_messageCommunication = messageCommunication;
	}

	public override void Init()
	{
		try
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				Guid id = _serverId.Id;
				MipTrace.MgsCom("CommunicationClient", "Init - " + id.ToString());
			}
			if (_thread == null)
			{
				_state = State.Running;
				_thread = new Thread(Run);
				_thread.IsBackground = true;
				_thread.Name = "CommunicationClient";
				_thread.Start();
			}
		}
		catch (Exception innerException)
		{
			throw new MIPException("Unable to connect to EventServer.CommunicationService", innerException);
		}
	}

	private void InitSession()
	{
		try
		{
			if (_serviceUrl == null)
			{
				_serviceUrl = GetServiceUrl();
			}
			lock (_lockObject)
			{
				if (EnvironmentManager.Instance.TraceMessageCommunication)
				{
					Guid id = _serverId.Id;
					MipTrace.MgsCom("CommunicationClient", "InitSession - " + id.ToString());
				}
				if (_communicationServiceClient != null && _communicationServiceClient.State == CommunicationState.Opening)
				{
					int num = 2000;
					while (num > 0 && _communicationServiceClient.State == CommunicationState.Opening)
					{
						Thread.Sleep(50);
						num -= 50;
					}
					if (_communicationServiceClient.State == CommunicationState.Opening)
					{
						EnvironmentManager.Instance.Log(error: true, "CommunicationClient", "Consider to wait for Open session state, longer than 2 seconds?");
					}
				}
				else
				{
					if (_communicationServiceClient != null && _communicationServiceClient.State == CommunicationState.Opened)
					{
						return;
					}
					EndpointAddress remoteAddress = new EndpointAddress(_serviceUrl);
					System.ServiceModel.Channels.Binding binding = null;
					binding = ((!_serviceUrl.Contains("https")) ? ((System.ServiceModel.Channels.Binding)new NetTcpBinding
					{
						Security = 
						{
							Mode = SecurityMode.None,
							Transport = 
							{
								ClientCredentialType = TcpClientCredentialType.None
							}
						},
						ReaderQuotas = 
						{
							MaxStringContentLength = int.MaxValue
						},
						TransferMode = TransferMode.Streamed,
						MaxReceivedMessageSize = 2147483647L,
						MaxBufferSize = int.MaxValue,
						MaxBufferPoolSize = 2147483647L,
						ReceiveTimeout = TimeSpan.FromSeconds(10.0),
						CloseTimeout = TimeSpan.FromSeconds(5.0),
						OpenTimeout = TimeSpan.FromSeconds(5.0),
						SendTimeout = TimeSpan.FromSeconds(15.0)
					}) : ((System.ServiceModel.Channels.Binding)new BasicHttpBinding
					{
						Security = 
						{
							Mode = BasicHttpSecurityMode.Transport
						},
						ReaderQuotas = 
						{
							MaxStringContentLength = int.MaxValue
						},
						TransferMode = TransferMode.Streamed,
						MaxReceivedMessageSize = 2147483647L,
						MaxBufferSize = int.MaxValue,
						MaxBufferPoolSize = 2147483647L,
						ReceiveTimeout = TimeSpan.FromSeconds(10.0),
						CloseTimeout = TimeSpan.FromSeconds(5.0),
						OpenTimeout = TimeSpan.FromSeconds(5.0),
						SendTimeout = TimeSpan.FromSeconds(15.0)
					}));
					_communicationServiceClient = new CommunicationServiceClient(binding, remoteAddress);
					_loginSettings = LoginSettingsCache.GetLoginSettings(_serverId);
					if (_loginSettings != null)
					{
						if (_loginSettings.IsOAuthIdentity)
						{
							_username = "Unknown OAuth user";
						}
						else if (_loginSettings.UserInformation != null)
						{
							_username = _loginSettings.UserInformation.AccountName;
						}
						else
						{
							_username = _loginSettings.UserName;
						}
						_communicationServiceClient.InnerChannel.Open();
						long num2 = DateTime.Now.Ticks + 50000000;
						while (DateTime.Now.Ticks < num2 && _communicationServiceClient.State == CommunicationState.Opening)
						{
							Thread.Sleep(5);
						}
						if (_communicationServiceClient.State == CommunicationState.Opening)
						{
							EnvironmentManager.Instance.Log(error: true, "CommunicationClient", "Did not reach OPEN state with 5 seconds");
						}
					}
				}
			}
		}
		catch (Exception innerException)
		{
			throw new MIPException("Unable to connect to EventServer.CommunicationService", innerException);
		}
	}

	public override void Close()
	{
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			Guid id = _serverId.Id;
			MipTrace.MgsCom("CommunicationClient", "Close - Start " + id.ToString());
		}
		_state = State.Stopping;
		int num = 10;
		while (_thread != null && num-- > 0)
		{
			Thread.Sleep(25);
		}
		if (_msgObject != null)
		{
			EnvironmentManager.Instance.UnRegisterReceiver(_msgObject);
			_msgObject = null;
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			Guid id = _serverId.Id;
			MipTrace.MgsCom("CommunicationClient", "Close - Complete " + id.ToString());
		}
	}

	private string GetServiceUrl()
	{
		Configuration.Instance.GetItem(_serverId.Id, Kind.Server);
		List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.MAPServiceId, _serverId);
		if (registeredServiceUriInfo.Count == 0 || registeredServiceUriInfo[0].UriArray.Count == 0)
		{
			throw new MIPException("Event server not found in registered services");
		}
		Configuration.ServiceURIInfo serviceURIInfo = registeredServiceUriInfo[0];
		int num = new Uri(serviceURIInfo.UriArray[0]).Port;
		if (num == 0)
		{
			num = 22331;
		}
		string host = new Uri(serviceURIInfo.UriArray[0]).Host;
		string text = null;
		if (serviceURIInfo.UriArray[0].Contains("https"))
		{
			text = "https://" + host + string.Format(CultureInfo.InvariantCulture, ":{0}/Central/CommunicationService", num);
		}
		else
		{
			num += 2;
			text = "net.tcp://" + host + string.Format(CultureInfo.InvariantCulture, ":{0}/CommunicationService", num);
		}
		_uri = new Uri(text);
		return text;
	}

	public override void StoreForRegistration(MessageReceiver messageReceiver, CommunicationFilter communicationFilter)
	{
		lock (_lockObject)
		{
			_myRegisteredFilters.Add(communicationFilter);
			_messageReceivers[communicationFilter.Id] = messageReceiver;
		}
	}

	public override void WaitForServerRegistration(CommunicationIdFilter messageFilter, TimeSpan timeout)
	{
		_waitUtil.Wait(() => HasRegisteredFiltersOnServer(messageFilter), TimeSpan.FromMilliseconds(50.0), timeout, string.Format(CultureInfo.InvariantCulture, "Timeout waiting for the communication filter: {0}", messageFilter.MessageId));
	}

	public override void ToBeRemoved(Guid id)
	{
		_toBeRemovedFilters.Add(id);
	}

	public override void Transmit(Message message, FQID destinationEndPoint, FQID destinationObject, FQID sourceObject)
	{
		try
		{
			if (EnvironmentManager.Instance == null || message.MessageId == MessageId.System.SystemConfigurationChangedIndication || message.MessageId == "Server.ConfigurationChangedIndication")
			{
				return;
			}
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("CommunicationClient", "Transmit - " + message.MessageId);
			}
			if (Application.MessageLoop && message.MessageId != "MessageCommunication.EndPointCloseIndication")
			{
				EnvironmentManager.Instance.Log(error: false, "MessageCommunication.Transmit", " Being called on the UI thread (Not recommended)");
			}
			ExternalMessage message2 = new ExternalMessage(message.MessageId)
			{
				SourceEndPointType = EndPointType.Client,
				SourceEndPoint = MessageCommunicationManager.EndPointFQID,
				Destination = destinationObject,
				Source = sourceObject,
				Reason = message.Reason,
				RelatedFQID = message.RelatedFQID,
				Data = message.Data
			};
			if (message.MessageId == "MessageCommunication.EndPointCloseIndication" && (_communicationServiceClient == null || _communicationServiceClient.State != CommunicationState.Opened))
			{
				return;
			}
			InitSession();
			if (_communicationServiceClient == null || _communicationServiceClient.State != CommunicationState.Opened)
			{
				throw new MIPException("Unable to reach EventServer");
			}
			lock (_lockObject)
			{
				try
				{
					_communicationServiceClient.TransmitMessage(_loginSettings.Token, destinationEndPoint, message2);
				}
				catch (Exception)
				{
					if (EnvironmentManager.Instance.TraceMessageCommunication)
					{
						MipTrace.MgsCom("CommunicationClient", "Transmit retry - " + message.MessageId);
					}
					_communicationServiceClient.TransmitMessage(_loginSettings.Token, destinationEndPoint, message2);
				}
			}
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("CommunicationClient", "Transmit end - " + message.MessageId);
			}
		}
		catch (Exception ex2)
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("CommunicationClient", "Transmit exception - " + ex2.Message);
			}
			if (ex2 is MIPException)
			{
				throw;
			}
			throw new MIPException("Unable to Transmit", ex2);
		}
	}

	private void Run()
	{
		try
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				EnvironmentManager.Instance.Log(error: false, "CommunicationClient background thread", "Now starting...", null);
			}
			while (_state != State.Stopped)
			{
				try
				{
					if (_communicationServiceClient == null || _communicationServiceClient.State != CommunicationState.Opened)
					{
						try
						{
							ClientClose();
							_serverKnowMe = false;
							InitSession();
						}
						catch (Exception)
						{
							if (_state == State.Stopping)
							{
								_state = State.Stopped;
							}
							else
							{
								Thread.Sleep(5000);
							}
							goto end_IL_002b;
						}
						if (_communicationServiceClient == null || _communicationServiceClient.State != CommunicationState.Opened)
						{
							EnvironmentManager.Instance.Log(error: false, "CommunicationClient initialize session failed", "Thread will sleep for 5 seconds before retry.");
							Thread.Sleep(5000);
						}
					}
					if (_communicationServiceClient.State == CommunicationState.Opened)
					{
						object[] array = RunTryGetFromServer();
						RunCheckToRegisterOnServer();
						if (!_serverKnowMe)
						{
							TellWhoIAm();
							IsConnected = true;
						}
						RunCheckToRemoveFilterOnserver();
						if (_state == State.Stopping)
						{
							_state = State.Stopped;
							continue;
						}
						if (array != null && array.Length != 0)
						{
							List<MessageHolder> list = new List<MessageHolder>();
							lock (_lockObject)
							{
								object[] array2 = array;
								foreach (object obj in array2)
								{
									if (obj is CommunicationFilter communicationFilter)
									{
										if (EnvironmentManager.Instance.TraceMessageCommunication)
										{
											MipTrace.MgsCom("CommunicationClient", "Thread - Server communication filter received - " + communicationFilter.CommunicationIdFilter.MessageId);
										}
										if (!_serverDefinedFilters.ContainsKey(communicationFilter.Id))
										{
											_serverDefinedFilters.Add(communicationFilter.Id, communicationFilter);
										}
									}
									if (obj is Guid key && _serverDefinedFilters.ContainsKey(key))
									{
										_serverDefinedFilters.Remove(key);
									}
									if (!(obj is ExternalMessage externalMessage))
									{
										continue;
									}
									if (externalMessage.MessageId == "MESSAGECOMTRACE")
									{
										EnvironmentManager.Instance.TraceMessageCommunication = "YES".Equals(externalMessage.Data);
										continue;
									}
									if (externalMessage.MessageId == "TRACESENDDETAILS")
									{
										EnvironmentManager.Instance.TraceSendDetails = "YES".Equals(externalMessage.Data);
										continue;
									}
									if (externalMessage.MessageId == "TRACEFUNCTIONCALLS")
									{
										EnvironmentManager.Instance.TraceFunctionCalls = "YES".Equals(externalMessage.Data);
										continue;
									}
									if (EnvironmentManager.Instance.TraceMessageCommunication)
									{
										MipTrace.MgsCom("CommunicationClient", "Thread - Message received - " + externalMessage.MessageId);
									}
									if (MessageCommunicationManager.Security != MessageCommunicationManager.SecurityLevel.Low && externalMessage.MessageId.EndsWith("Command", StringComparison.InvariantCulture))
									{
										continue;
									}
									foreach (CommunicationFilter item in _myRegisteredFiltersOnServer)
									{
										if (!item.CommunicationIdFilter.Match(externalMessage, item.EndPoint, item.EndPointType))
										{
											continue;
										}
										try
										{
											if (_messageReceivers.ContainsKey(item.Id))
											{
												Message message = new Message(externalMessage.MessageId)
												{
													Data = externalMessage.Data,
													Reason = externalMessage.Reason,
													RelatedFQID = externalMessage.RelatedFQID,
													ExternalMessageSourceEndPoint = externalMessage.SourceEndPoint
												};
												list.Add(new MessageHolder
												{
													Message = message,
													MessageReceiver = _messageReceivers[item.Id],
													Source = externalMessage.Source,
													Destination = externalMessage.Destination
												});
											}
										}
										catch (Exception ex2)
										{
											EnvironmentManager.Instance.Log(error: false, "CommunicationClient.MessageDispatcher", ex2.Message);
										}
									}
								}
								if (_serverDefinedFilters.Count > 0)
								{
									if (_msgObject == null)
									{
										_msgObject = EnvironmentManager.Instance.RegisterReceiver(MessageHandler, new AnyFilter());
									}
								}
								else if (_msgObject != null)
								{
									EnvironmentManager.Instance.UnRegisterReceiver(_msgObject);
									_msgObject = null;
								}
							}
							foreach (MessageHolder item2 in list)
							{
								if (_state == State.Stopping)
								{
									continue;
								}
								try
								{
									if (EnvironmentManager.Instance.TraceMessageCommunication)
									{
										MipTrace.MgsCom("CommunicationClient", "Thread - Invoke Message Receiver START - " + item2.Message.MessageId + ", Receive Method:" + item2.MessageReceiver.Method.DeclaringType.Name);
									}
									item2.MessageReceiver(item2.Message, item2.Destination, item2.Source);
									if (EnvironmentManager.Instance.TraceMessageCommunication)
									{
										MipTrace.MgsCom("CommunicationClient", "Thread - Invoke Message Receiver END - " + item2.Message.MessageId + ", Receive Method:" + item2.MessageReceiver.Method.DeclaringType.Name);
									}
								}
								catch (Exception ex3)
								{
									EnvironmentManager.Instance.Log(error: true, "MessageCommunication", "Exception from plugin:" + item2.MessageReceiver.Method.DeclaringType.Name + "." + item2.MessageReceiver.Method.Name + " - " + ex3.Message);
								}
							}
						}
						goto IL_065a;
					}
					if (EnvironmentManager.Instance.TraceMessageCommunication)
					{
						MipTrace.MgsCom("CommunicationClient", "Thread ComunicationState = " + _communicationServiceClient.State);
					}
					goto IL_065a;
					end_IL_002b:;
				}
				catch (MessageFilterNotRegisteredMIPException)
				{
					if (EnvironmentManager.Instance.TraceMessageCommunication)
					{
						MipTrace.MgsCom("CommunicationClient", "Thread MessageFilterNotRegisteredMIPException ");
					}
					RefillMyRegisteredFilters();
					goto IL_065a;
				}
				catch (EndpointNotFoundException)
				{
					if (EnvironmentManager.Instance.TraceMessageCommunication)
					{
						MipTrace.MgsCom("CommunicationClient", "Thread EndpointNotFoundException");
					}
					ClientClose(serverUnavail: true);
					Thread.Sleep(5000);
					goto IL_065a;
				}
				catch (Exception ex6)
				{
					ClientClose(serverUnavail: true);
					RefillMyRegisteredFilters();
					EnvironmentManager.Instance.Log(error: false, "CommunicationClient", ex6.Message);
					Thread.Sleep(2000);
					goto IL_065a;
				}
				continue;
				IL_065a:
				if (EnvironmentManager.Instance == null || !EnvironmentManager.Instance.ApplicationLoggedOn)
				{
					MipTrace.MgsCom("CommunicationClient", "Thread Application Stopped??  " + EnvironmentManager.Instance.ApplicationLoggedOn);
					_state = State.Stopped;
				}
			}
			ClientClose();
		}
		catch (Exception ex7)
		{
			if (EnvironmentManager.Instance != null)
			{
				EnvironmentManager instance = EnvironmentManager.Instance;
				object[] array2 = new Exception[1] { ex7 };
				instance.Log(error: true, "CommunicationClient background thread", "Exception ...", array2);
			}
		}
		finally
		{
			if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.TraceMessageCommunication)
			{
				EnvironmentManager.Instance.Log(error: false, "CommunicationClient background thread", "Now stopping...", null);
			}
		}
		_thread = null;
	}

	private void RefillMyRegisteredFilters()
	{
		lock (_myRegisteredFiltersOnServerLock)
		{
			_myRegisteredFilters.AddRange(_myRegisteredFiltersOnServer);
			_myRegisteredFiltersOnServer = new Collection<CommunicationFilter>();
		}
	}

	private bool HasRegisteredFiltersOnServer(CommunicationIdFilter messageFilter)
	{
		lock (_myRegisteredFiltersOnServerLock)
		{
			return _myRegisteredFiltersOnServer.Any((CommunicationFilter s) => s.CommunicationIdFilter == messageFilter);
		}
	}

	private object[] RunTryGetFromServer()
	{
		object[] array = null;
		int timeoutMilliSeconds = ((_myRegisteredFilters.Count != 0 || _toBeRemovedFilters.Count != 0) ? 100 : 3000);
		if (_state == State.Running)
		{
			if (EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("CommunicationClient", "Thread - Received Message Try ..... " + MessageCommunicationManager.EndPointFQID.ObjectId.ToString());
			}
			array = _communicationServiceClient.Receive(_loginSettings.Token, MessageCommunicationManager.EndPointFQID, timeoutMilliSeconds);
			if (array != null && array.Length != 0 && EnvironmentManager.Instance.TraceMessageCommunication)
			{
				MipTrace.MgsCom("CommunicationClient", "Thread - Received Message Array  - " + array.Length.ToString(CultureInfo.InvariantCulture));
			}
		}
		else if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("CommunicationClient", "Thread state = " + _state);
		}
		return array;
	}

	private void RunCheckToRegisterOnServer()
	{
		if (_state != State.Running || _myRegisteredFilters.Count <= 0)
		{
			return;
		}
		lock (_lockObject)
		{
			foreach (CommunicationFilter filter in _myRegisteredFilters)
			{
				if (!_myRegisteredFiltersOnServer.Any((CommunicationFilter f) => f.Id.Equals(filter.Id)))
				{
					if (EnvironmentManager.Instance.TraceMessageCommunication)
					{
						MipTrace.MgsCom("CommunicationClient", "Thread - Register on server - " + filter.CommunicationIdFilter.MessageId);
					}
					_communicationServiceClient.RegisterCommunicationFilter(_loginSettings.Token, filter);
				}
				lock (_myRegisteredFiltersOnServerLock)
				{
					_myRegisteredFiltersOnServer.Add(filter);
				}
			}
			_myRegisteredFilters.Clear();
		}
		IsConnected = true;
	}

	private void RunCheckToRemoveFilterOnserver()
	{
		if (_toBeRemovedFilters.Count <= 0)
		{
			return;
		}
		lock (_lockObject)
		{
			Collection<Guid> toBeRemovedFilters = _toBeRemovedFilters;
			_toBeRemovedFilters = new Collection<Guid>();
			foreach (Guid item in toBeRemovedFilters)
			{
				foreach (CommunicationFilter item2 in _myRegisteredFiltersOnServer)
				{
					if (!(item2.Id == item))
					{
						continue;
					}
					bool flag = false;
					foreach (CommunicationFilter item3 in _myRegisteredFiltersOnServer)
					{
						if (item2.CommunicationIdFilter.MessageId == item3.CommunicationIdFilter.MessageId && item3.Id != item2.Id)
						{
							flag = true;
						}
					}
					if (!flag)
					{
						if (EnvironmentManager.Instance.TraceMessageCommunication)
						{
							MipTrace.MgsCom("CommunicationClient", "Thread - Remove from server - " + item2.CommunicationIdFilter.MessageId);
						}
						lock (_myRegisteredFiltersOnServerLock)
						{
							_myRegisteredFiltersOnServer.Remove(item2);
						}
					}
					break;
				}
				_communicationServiceClient.UnRegisterCommunicationFilter(_loginSettings.Token, MessageCommunicationManager.EndPointFQID, item);
				if (_messageReceivers.ContainsKey(item))
				{
					_messageReceivers.Remove(item);
				}
			}
		}
	}

	private void TellWhoIAm()
	{
		try
		{
			if (!_serverKnowMe)
			{
				_endPointIdentityData = new EndPointIdentityData
				{
					EndPointFQID = MessageCommunicationManager.EndPointFQID,
					IdentityName = _username
				};
				Transmit(new Message("MessageCommunication.NewEndPointIndication", _endPointIdentityData), null, null, null);
				_serverKnowMe = true;
			}
		}
		catch (Exception)
		{
		}
	}

	private void TellLoggingOff()
	{
		try
		{
			if (_serverKnowMe)
			{
				_endPointIdentityData = new EndPointIdentityData
				{
					EndPointFQID = MessageCommunicationManager.EndPointFQID,
					IdentityName = _username
				};
				Transmit(new Message("MessageCommunication.EndPointCloseIndication", _endPointIdentityData), null, null, null);
				_serverKnowMe = false;
			}
		}
		catch (Exception)
		{
		}
	}

	private void ClientClose(bool serverUnavail = false)
	{
		try
		{
			lock (_lockObject)
			{
				if (_communicationServiceClient != null && _communicationServiceClient.State == CommunicationState.Opened)
				{
					if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.TraceMessageCommunication)
					{
						Guid id = _serverId.Id;
						MipTrace.MgsCom("CommunicationClient", "ClientClose start - " + id.ToString());
					}
					if (_communicationServiceClient.State == CommunicationState.Opened && !serverUnavail)
					{
						TellLoggingOff();
					}
					_communicationServiceClient.Close();
					if (EnvironmentManager.Instance != null && EnvironmentManager.Instance.TraceMessageCommunication)
					{
						Guid id = _serverId.Id;
						MipTrace.MgsCom("CommunicationClient", "ClientClose end - " + id.ToString());
					}
				}
			}
			IsConnected = false;
		}
		catch (CommunicationException)
		{
			if (_communicationServiceClient != null)
			{
				_communicationServiceClient.Abort();
			}
		}
		catch (TimeoutException)
		{
			if (_communicationServiceClient != null)
			{
				_communicationServiceClient.Abort();
			}
		}
		catch (Exception)
		{
			if (_communicationServiceClient != null)
			{
				_communicationServiceClient.Abort();
			}
		}
		_communicationServiceClient = null;
	}

	private object MessageHandler(Message message, FQID dest, FQID source)
	{
		if (MessageCommunicationManager.Security == MessageCommunicationManager.SecurityLevel.High)
		{
			return null;
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("CommunicationClient", "MessageHandler start - " + message.MessageId);
		}
		lock (_lockObject)
		{
			if (message.Data != null && _prohibitedClasses.Contains(message.Data.GetType()))
			{
				return null;
			}
			foreach (Guid key in _serverDefinedFilters.Keys)
			{
				CommunicationFilter communicationFilter = _serverDefinedFilters[key];
				if (communicationFilter.CommunicationIdFilter.MatchId(message.MessageId))
				{
					try
					{
						Transmit(message, communicationFilter.RequesterEndPoint, dest, source);
					}
					catch (Exception ex)
					{
						EnvironmentManager.Instance.Log(error: true, "MessageCommunication", "Unable to Transmit to EventServer (Message ignored):" + ex.Message);
					}
				}
			}
		}
		if (EnvironmentManager.Instance.TraceMessageCommunication)
		{
			MipTrace.MgsCom("CommunicationClient", "MessageHandler end - " + message.MessageId);
		}
		return null;
	}
}
