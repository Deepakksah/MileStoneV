using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Net;
using System.Runtime.Remoting;
using System.Runtime.Remoting.Channels;
using System.Runtime.Remoting.Channels.Tcp;
using System.ServiceProcess;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.SNMPCOMCommBridge;

namespace VideoOS.Common.Integration;

public class SnmpManager : ISNMPManager, IDisposable
{
	private readonly SNMPAgentRemotingCommObject _snmpAgentCommObject;

	private TcpServerChannel _tc;

	private bool _ipv6;

	private int _port = 6474;

	private Queue<string> _trapQueue = new Queue<string>();

	private EventWaitHandle _trapNamedEvent;

	private Timer _checAgentAlivenessTimer;

	private DateTime _keepAliveTimestamp = DateTime.Now;

	private bool _agentAlive;

	private readonly ILogger _logger;

	public int Port
	{
		get
		{
			return _port;
		}
		set
		{
			_port = value;
		}
	}

	public SnmpManager(bool ipv6, SnmpConfig snmpConfig, ILogger logger)
	{
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Expected O, but got Unknown
		if (snmpConfig == null)
		{
			throw new ArgumentNullException("snmpConfig");
		}
		_ipv6 = ipv6;
		_snmpAgentCommObject = new SNMPAgentRemotingCommObject((ISNMPManager)(object)this);
		_checAgentAlivenessTimer = new Timer(CheckAgentAliveness, this, 0, 60000);
		_trapNamedEvent = new EventWaitHandle(initialState: false, EventResetMode.AutoReset, "Global\\" + snmpConfig.SnmpTrapNamedEvent);
		_logger = logger;
	}

	private void CheckAgentAliveness(object state)
	{
		lock (this)
		{
			if (_agentAlive && DateTime.Now - _keepAliveTimestamp > new TimeSpan(0, 5, 0))
			{
				_trapQueue.Clear();
				_agentAlive = false;
				LoggerExtensions.LogInformation(_logger, "The SNMP agent has not checked in for 5 min", Array.Empty<object>());
			}
		}
	}

	public void Start()
	{
		ListDictionary listDictionary = new ListDictionary();
		listDictionary.Add("name", "VideoOS.Recorder.Service.SNMPAgentComm");
		listDictionary.Add("port", _port);
		if (_ipv6)
		{
			listDictionary.Add("bindTo", IPAddress.IPv6Loopback.ToString());
		}
		else
		{
			listDictionary.Add("bindTo", IPAddress.Loopback.ToString());
		}
		listDictionary.Add("rejectRemoteRequests", "true");
		listDictionary.Add("suppressChannelData", "true");
		_tc = new TcpServerChannel(listDictionary, new BinaryServerFormatterSinkProvider());
		ChannelServices.RegisterChannel(_tc, ensureSecurity: false);
		RemotingServices.Marshal((MarshalByRefObject)(object)_snmpAgentCommObject, "SNMPAgentComm.rem");
		using System.ServiceProcess.ServiceController serviceController = new System.ServiceProcess.ServiceController("SNMP");
		try
		{
			if (serviceController.Status != ServiceControllerStatus.Running)
			{
				throw new SnmpServiceNotStartedException();
			}
			lock (this)
			{
				_agentAlive = true;
				_keepAliveTimestamp = DateTime.Now;
			}
		}
		catch (InvalidOperationException ex)
		{
			throw new SnmpServiceNotInstalledException(ex.Message, ex);
		}
		catch (Win32Exception ex2)
		{
			throw new SnmpServiceNotInstalledException(ex2.Message, ex2);
		}
	}

	public void Stop()
	{
		if (_tc != null)
		{
			_tc.StopListening(null);
			ChannelServices.UnregisterChannel(_tc);
		}
		_checAgentAlivenessTimer.Dispose();
	}

	public string ProcessSNMPRequest(string request)
	{
		LoggerExtensions.LogInformation(_logger, "SNMPmanager: Received SNMP request", Array.Empty<object>());
		return string.Empty;
	}

	public string ProcessSNMPTrapRequest()
	{
		string text = string.Empty;
		lock (((ICollection)_trapQueue).SyncRoot)
		{
			if (_trapQueue.Count > 0)
			{
				text = _trapQueue.Dequeue();
			}
		}
		LoggerExtensions.LogInformation(_logger, "SNMPmanager: Received SNMP trap request, trap exist: {XmlIsNullOrEmpty}", new object[1] { string.IsNullOrEmpty(text) ? "No" : "Yes" });
		return text;
	}

	public void AddSNMPTrap(string trapXml)
	{
		lock (this)
		{
			if (!_agentAlive)
			{
				return;
			}
		}
		lock (((ICollection)_trapQueue).SyncRoot)
		{
			_trapQueue.Enqueue(trapXml);
			LoggerExtensions.LogInformation(_logger, "SNMPmanager: Added trap to queue", Array.Empty<object>());
		}
		_trapNamedEvent.Set();
	}

	public void KeepAlive()
	{
		lock (this)
		{
			_agentAlive = true;
			_keepAliveTimestamp = DateTime.Now;
			LoggerExtensions.LogDebug(_logger, "SNMPmanager: Keep alive received", Array.Empty<object>());
		}
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	~SnmpManager()
	{
		Dispose(disposing: false);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_trapNamedEvent.Dispose();
			_checAgentAlivenessTimer.Dispose();
			_trapNamedEvent = null;
			_checAgentAlivenessTimer = null;
		}
	}
}
