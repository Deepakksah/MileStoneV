using System;
using System.Net;
using System.Net.Sockets;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.TCP;

public class TcpListenerExt
{
	public delegate void OnTcpClient(TcpClient tNewClient);

	protected TcpListener _server;

	protected CycleTaskMember _serverThread;

	public OnTcpClient OnNewTcpClient { get; set; }

	public EndPoint LocalEndpoint => _server.LocalEndpoint;

	public TcpListenerExt(string szAddress, int nPortNumber)
	{
		if (string.IsNullOrWhiteSpace(szAddress))
		{
			_server = new TcpListener(IPAddress.Any, nPortNumber);
		}
		else
		{
			IPAddress localaddr = IPAddress.Parse(szAddress);
			_server = new TcpListener(localaddr, nPortNumber);
		}
		_serverThread = new CycleTaskMember((Action)ServerThreadFunc, (Action)null, (Action)OnServerThreadStop, 10);
		_serverThread.Name = "TcpListenerExt.ServerThreadFunc";
	}

	public virtual void Start()
	{
		_server.Start();
		_serverThread.StartTask();
	}

	public virtual void Stop()
	{
		_serverThread.SignalTaskToStop();
		_serverThread.WaitTaskToStop(1000);
	}

	private void ServerThreadFunc()
	{
		try
		{
			if (_server.Pending())
			{
				OnNewTcpClient(_server.AcceptTcpClient());
			}
		}
		catch (Exception ex)
		{
			Log.Error("TcpListenerExt.ThreadFunc", ex.Message);
		}
	}

	private void OnServerThreadStop()
	{
		try
		{
			_server.Stop();
		}
		catch (Exception ex)
		{
			Log.Error("TcpListenerExt.OnThreadStop", ex.Message);
		}
	}
}
