using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading;
using VideoOS.Platform.Login;
using VideoOS.Platform.Util;

namespace VideoOS.Platform.Messaging.Internal;

public class CommunicationClientOnServer : ICommunicationClient
{
	private CommunicationServiceClient _communicationServiceClient;

	private Thread _thread;

	private bool _stop;

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

	private readonly Collection<TransmitHolder> _toBeTransmitted = new Collection<TransmitHolder>();

	private LoginSettings _loginSettings;

	private readonly object _lockObject = new object();

	public override Uri Uri => _uri;

	public override bool IsConnected
	{
		get
		{
			if (_communicationServiceClient != null)
			{
				return _communicationServiceClient.State == CommunicationState.Opened;
			}
			return false;
		}
		set
		{
		}
	}

	public CommunicationClientOnServer()
	{
	}

	public override void Init()
	{
		try
		{
			if (_thread == null)
			{
				_stop = false;
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
			string serviceUrl = GetServiceUrl();
			lock (_lockObject)
			{
				if (_communicationServiceClient != null && _communicationServiceClient.State == CommunicationState.Opened)
				{
					return;
				}
				EndpointAddress remoteAddress = new EndpointAddress(serviceUrl);
				Binding binding = null;
				binding = ((!serviceUrl.Contains("https")) ? ((Binding)new NetTcpBinding
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
				}) : ((Binding)new BasicHttpBinding
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
				_loginSettings = LoginSettingsCache.GetLoginSettings(Configuration.Instance.ServerFQID.ServerId);
				if (_loginSettings != null)
				{
					_username = _loginSettings.UserName;
					Uri uriPrefix = new Uri(serviceUrl);
					NetworkCredential networkCredential = _loginSettings.CredentialCache.GetCredential(uriPrefix, "Negotiate");
					if (networkCredential == null)
					{
						networkCredential = _loginSettings.CredentialCache.GetCredential(uriPrefix, "Basic") ?? _loginSettings.CredentialCache.GetCredential(uriPrefix, _username);
					}
					_communicationServiceClient.ClientCredentials.Windows.ClientCredential = networkCredential;
					_communicationServiceClient.InnerChannel.Open();
					long num = DateTime.Now.Ticks + 50000000;
					while (DateTime.Now.Ticks < num && _communicationServiceClient.State == CommunicationState.Opening)
					{
						Thread.Sleep(5);
					}
					if (_communicationServiceClient.State == CommunicationState.Opening)
					{
						EnvironmentManager.Instance.Log(error: true, "CommunicationClientOnServer", "Did not reach OPEN state within 5 seconds");
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
		_stop = true;
	}

	private string GetServiceUrl()
	{
		List<Configuration.ServiceURIInfo> registeredServiceUriInfo = Configuration.Instance.GetRegisteredServiceUriInfo(Configuration.MAPServiceId, EnvironmentManager.Instance.MasterSite.ServerId);
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
			InitSession();
			lock (_lockObject)
			{
				if (_communicationServiceClient == null || _communicationServiceClient.State != CommunicationState.Opened)
				{
					throw new MIPException("Unable to reach EventServer");
				}
				_communicationServiceClient.TransmitMessage(_loginSettings.Token, destinationEndPoint, message2);
			}
		}
		catch (Exception ex)
		{
			if (ex is MIPException)
			{
				throw;
			}
			throw new MIPException("Unable to Transmit", ex);
		}
	}

	private void Run()
	{
		try
		{
			EnvironmentManager.Instance.Log(error: false, "CommunicationClient background thread", "Now starting...", null);
			DateTime dateTime = DateTime.MinValue;
			while (!_stop)
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
							Thread.Sleep(5000);
							goto end_IL_0022;
						}
					}
					if (_communicationServiceClient.State == CommunicationState.Opened)
					{
						if (_myRegisteredFilters.Count > 0)
						{
							lock (_lockObject)
							{
								foreach (CommunicationFilter myRegisteredFilter in _myRegisteredFilters)
								{
									_communicationServiceClient.RegisterCommunicationFilter(_loginSettings.Token, myRegisteredFilter);
									lock (_myRegisteredFiltersOnServerLock)
									{
										_myRegisteredFiltersOnServer.Add(myRegisteredFilter);
									}
								}
								_myRegisteredFilters.Clear();
							}
						}
						if (!_serverKnowMe)
						{
							TellWhoIAm();
						}
						if (_toBeRemovedFilters.Count > 0)
						{
							lock (_lockObject)
							{
								Collection<Guid> toBeRemovedFilters = _toBeRemovedFilters;
								_toBeRemovedFilters = new Collection<Guid>();
								foreach (Guid item in toBeRemovedFilters)
								{
									_communicationServiceClient.UnRegisterCommunicationFilter(_loginSettings.Token, MessageCommunicationManager.EndPointFQID, item);
									if (_messageReceivers.ContainsKey(item))
									{
										_messageReceivers.Remove(item);
									}
									foreach (CommunicationFilter item2 in _myRegisteredFiltersOnServer)
									{
										if (item2.Id == item)
										{
											lock (_myRegisteredFiltersOnServerLock)
											{
												_myRegisteredFiltersOnServer.Remove(item2);
											}
											break;
										}
									}
								}
							}
						}
						if (_myRegisteredFiltersOnServer.Count > 0)
						{
							object[] array = _communicationServiceClient.Receive(_loginSettings.Token, MessageCommunicationManager.EndPointFQID, 10000);
							if (array != null && array.Length != 0)
							{
								lock (_lockObject)
								{
									object[] array2 = array;
									for (int i = 0; i < array2.Length; i++)
									{
										if (!(array2[i] is ExternalMessage externalMessage) || (MessageCommunicationManager.Security != MessageCommunicationManager.SecurityLevel.Low && externalMessage.MessageId.EndsWith("Command", StringComparison.InvariantCulture)))
										{
											continue;
										}
										foreach (CommunicationFilter item3 in _myRegisteredFiltersOnServer)
										{
											if (!item3.CommunicationIdFilter.Match(externalMessage, item3.EndPoint, item3.EndPointType))
											{
												continue;
											}
											try
											{
												if (_messageReceivers.ContainsKey(item3.Id))
												{
													Message message = new Message(externalMessage.MessageId)
													{
														Data = externalMessage.Data,
														Reason = externalMessage.Reason,
														RelatedFQID = externalMessage.RelatedFQID,
														ExternalMessageSourceEndPoint = externalMessage.SourceEndPoint
													};
													_messageReceivers[item3.Id](message, externalMessage.Destination, externalMessage.Source);
												}
											}
											catch (Exception ex2)
											{
												EnvironmentManager.Instance.Log(error: false, "CommunicationClient.MessageDispatcher", ex2.Message);
											}
										}
									}
								}
							}
							Thread.Sleep(10);
						}
						else if (_toBeTransmitted.Count == 0)
						{
							Thread.Sleep(1000);
						}
						if (_toBeTransmitted.Count > 0)
						{
							lock (_lockObject)
							{
								foreach (TransmitHolder item4 in _toBeTransmitted)
								{
									_communicationServiceClient.TransmitMessage(_loginSettings.Token, item4.DestinationEndPoint, item4.ExternalMessage);
								}
								_toBeTransmitted.Clear();
							}
						}
					}
					goto IL_04ea;
					end_IL_0022:;
				}
				catch (MessageFilterNotRegisteredMIPException)
				{
					RefillMyRegisteredFilters();
					goto IL_04ea;
				}
				catch (EndpointNotFoundException)
				{
					Thread.Sleep(5000);
					goto IL_04ea;
				}
				catch (Exception ex5)
				{
					RefillMyRegisteredFilters();
					if (dateTime < DateTime.UtcNow.AddMinutes(-1.0))
					{
						EnvironmentManager.Instance.Log(error: true, "CommunicationClient", ex5.Message);
						dateTime = DateTime.UtcNow;
					}
					goto IL_04ea;
				}
				continue;
				IL_04ea:
				if (EnvironmentManager.Instance == null || !EnvironmentManager.Instance.ApplicationLoggedOn)
				{
					_stop = true;
				}
			}
			ClientClose();
		}
		catch (Exception ex6)
		{
			EnvironmentManager.Instance.Log(error: true, "CommunicationClient background thread", "Exception ...", new object[1] { ex6 });
		}
		finally
		{
			EnvironmentManager.Instance.Log(error: false, "CommunicationClient background thread", "Now stopping...", null);
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

	private void ClientClose()
	{
		if (_communicationServiceClient == null)
		{
			return;
		}
		lock (_lockObject)
		{
			try
			{
				if (_communicationServiceClient != null && _communicationServiceClient.State == CommunicationState.Opened)
				{
					_communicationServiceClient.Close();
				}
			}
			catch (CommunicationException)
			{
				_communicationServiceClient.Abort();
			}
			catch (TimeoutException)
			{
				_communicationServiceClient.Abort();
			}
			catch (Exception)
			{
				_communicationServiceClient.Abort();
			}
			_communicationServiceClient = null;
		}
	}

	private bool HasRegisteredFiltersOnServer(CommunicationIdFilter messageFilter)
	{
		lock (_myRegisteredFiltersOnServerLock)
		{
			return _myRegisteredFiltersOnServer.Any((CommunicationFilter s) => s.CommunicationIdFilter == messageFilter);
		}
	}
}
