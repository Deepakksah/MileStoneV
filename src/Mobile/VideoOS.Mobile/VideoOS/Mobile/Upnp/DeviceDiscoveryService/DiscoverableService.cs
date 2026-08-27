using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using VideoOS.Mobile.Portable.Interfaces;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Net;

namespace VideoOS.Mobile.UPNP.DeviceDiscoveryService;

public class DiscoverableService : IDiscoverableService, IControl, IDisposable
{
	private const int MaxInputMsgLength = 1024;

	private readonly Dictionary<string, IMulticastClient> _dictionary = new Dictionary<string, IMulticastClient>();

	private readonly object _lockObject = new object();

	private readonly IDiscoveryProtocol _ssdp;

	private readonly INetworkInterface _iNetworkInterfaceProvider;

	private readonly IMulticastClientFactory _iMulticastClientFactory;

	private bool _disposed;

	public string ExternalHttpPath { get; set; }

	public ushort ExternalHttpPort { get; set; }

	public DiscoverableService(IDiscoveryProtocol iDiscoveryProtocol, INetworkInterface iNetworkInterfaceProvider = null, IMulticastClientFactory iMulticastClientFactory = null)
	{
		_ssdp = iDiscoveryProtocol;
		_iNetworkInterfaceProvider = iNetworkInterfaceProvider ?? new NetworkInterfacesImplementer();
		_iMulticastClientFactory = iMulticastClientFactory ?? MulticastClientFactory.Instance;
	}

	public void Start()
	{
		UpdateNetworkAdapters();
	}

	public void Stop()
	{
		ClearNetworkInterfaces();
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	private void UpdateNetworkAdapters()
	{
		try
		{
			Dictionary<string, IMulticastClient> dictionary;
			lock (_lockObject)
			{
				dictionary = new Dictionary<string, IMulticastClient>(_dictionary);
			}
			foreach (NetworkInterfaceProperties allNetworkInterface in _iNetworkInterfaceProvider.GetAllNetworkInterfaces())
			{
				if (dictionary.ContainsKey(allNetworkInterface.Address.Address))
				{
					dictionary.Remove(allNetworkInterface.Address.Address);
				}
				AddNetworkInterface(allNetworkInterface.Address, allNetworkInterface.Index);
			}
			foreach (KeyValuePair<string, IMulticastClient> item in dictionary)
			{
				RemoveNetworkInterface(item.Value);
			}
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
	}

	private void ClearNetworkInterfaces()
	{
		List<IMulticastClient> list;
		lock (_lockObject)
		{
			list = new List<IMulticastClient>(_dictionary.Values);
		}
		foreach (IMulticastClient item in list)
		{
			RemoveNetworkInterface(item);
		}
	}

	private void AddNetworkInterface(VideoOS.Mobile.Portable.Net.IPAddress ipAddress, int index)
	{
		IMulticastClient multicastClient = null;
		try
		{
			new IPEndPoint(System.Net.IPAddress.Parse(ipAddress.Address), 1900);
			string multicastAddress = ((ipAddress.AddressFamily == AddressFamily.InterNetwork) ? "239.255.255.250" : "[FF02::C]");
			multicastClient = _iMulticastClientFactory.CreateMulticastClient(ipAddress.Address, 1900, multicastAddress, index);
			multicastClient.Ttl = _ssdp.PacketTtl;
			IMulticastClient udpClient = null;
			lock (_lockObject)
			{
				if (_dictionary.ContainsKey(ipAddress.Address))
				{
					udpClient = _dictionary[ipAddress.Address];
				}
				_dictionary[ipAddress.Address] = multicastClient;
			}
			multicastClient.BeginReceive(RequestCallback, multicastClient);
			SendMulticastAnnounce(multicastClient, isByeBye: false);
			CloseClient(udpClient);
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
	}

	private void RequestCallback(IAsyncResult result)
	{
		try
		{
			IMulticastClient multicastClient = (IMulticastClient)result.AsyncState;
			string remote = null;
			byte[] array = multicastClient.EndReceive(result, ref remote);
			string[] array2 = remote.Split(":".ToCharArray());
			IPEndPoint endPoint = null;
			if (System.Net.IPAddress.TryParse(array2[0], out var address) && int.TryParse(array2[1], out var result2))
			{
				endPoint = new IPEndPoint(address, result2);
			}
			multicastClient.BeginReceive(RequestCallback, multicastClient);
			if (array != null)
			{
				ProcessData(array, multicastClient, endPoint);
			}
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
	}

	private void RemoveNetworkInterface(IMulticastClient udpClient)
	{
		lock (_lockObject)
		{
			if (_dictionary.ContainsValue(udpClient))
			{
				KeyValuePair<string, IMulticastClient> keyValuePair = _dictionary.FirstOrDefault((KeyValuePair<string, IMulticastClient> p) => p.Value == udpClient);
				if (keyValuePair.Key != null)
				{
					_dictionary.Remove(keyValuePair.Key);
				}
			}
		}
		if (udpClient != null)
		{
			try
			{
				SendMulticastAnnounce(udpClient, isByeBye: true);
				CloseClient(udpClient);
			}
			catch (Exception ex)
			{
				Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
			}
		}
	}

	private void CloseClient(IMulticastClient udpClient)
	{
		if (udpClient != null)
		{
			udpClient.Close();
			if (udpClient is IDisposable disposable)
			{
				disposable.Dispose();
			}
		}
	}

	private bool SendMulticastAnnounce(IMulticastClient udpClient, bool isByeBye)
	{
		try
		{
			_ssdp.AnouncePacket(udpClient, isByeBye);
			return true;
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
			return false;
		}
	}

	private void ProcessData(byte[] data, IMulticastClient client, IPEndPoint endPoint)
	{
		string strData = Encoding.UTF8.GetString(data);
		try
		{
			_ssdp.ProcessRequest(client, strData, endPoint);
		}
		catch (Exception ex)
		{
			Log.Error(MethodBase.GetCurrentMethod().Name, ex.Message);
		}
	}

	~DiscoverableService()
	{
		Dispose(disposing: true);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (!_disposed)
		{
			if (disposing)
			{
				Stop();
			}
			_disposed = true;
		}
	}
}
