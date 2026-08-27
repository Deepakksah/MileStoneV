using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Upnp;

namespace VideoOS.Mobile.Portable.Net;

public class MulticastClientProvider : IMulticastClientProvider
{
	private readonly Dictionary<string, IMulticastClient> _clients = new Dictionary<string, IMulticastClient>();

	private readonly object _lockObject = new object();

	private readonly INetworkInterface _networkInterface;

	private readonly Func<NetworkInterfaceProperties, string> _multicastGroupProvider;

	public MulticastClientProvider(Func<NetworkInterfaceProperties, string> multicastGroupProvider)
	{
		_multicastGroupProvider = multicastGroupProvider;
		_networkInterface = new NetworkInterfacesImplementer();
	}

	public IEnumerable<IMulticastClient> GetClients()
	{
		foreach (NetworkInterfaceProperties allNetworkInterface in _networkInterface.GetAllNetworkInterfaces())
		{
			if (!IsNetworkInterfaceAdded(allNetworkInterface))
			{
				AddNetworkInterface(allNetworkInterface);
			}
		}
		return _clients.Values;
	}

	public void RemoveNetworkInterface(IMulticastClient multicastClient)
	{
		lock (_lockObject)
		{
			if (_clients.ContainsValue(multicastClient))
			{
				KeyValuePair<string, IMulticastClient> keyValuePair = _clients.FirstOrDefault((KeyValuePair<string, IMulticastClient> p) => p.Value == multicastClient);
				if (keyValuePair.Key != null)
				{
					_clients.Remove(keyValuePair.Key);
				}
			}
		}
		if (multicastClient != null)
		{
			try
			{
				CloseClient(multicastClient);
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "MulticastClientProvider.RemoveNetworkInterface", ex.Message);
			}
		}
	}

	private void CloseClient(IMulticastClient multicastClient)
	{
		multicastClient.Close();
		if (multicastClient is IDisposable disposable)
		{
			disposable.Dispose();
		}
	}

	private bool IsNetworkInterfaceAdded(NetworkInterfaceProperties networkInterface)
	{
		lock (_lockObject)
		{
			return _clients.ContainsKey(networkInterface.Address.Address);
		}
	}

	private void AddNetworkInterface(NetworkInterfaceProperties networkInterface)
	{
		IMulticastClient multicastClient = null;
		try
		{
			string multicastAddress = _multicastGroupProvider(networkInterface);
			multicastClient = MulticastClientFactory.Instance.CreateMulticastClient(networkInterface.Address.Address, multicastAddress, networkInterface.Index);
			multicastClient.Ttl = 4;
			lock (_lockObject)
			{
				_clients.Add(networkInterface.Address.Address, multicastClient);
			}
		}
		catch (Exception ex)
		{
			RemoveNetworkInterface(multicastClient);
			Logger.Instance.Log(LogContent.Error, "MulticastClientProvider.AddNetworkInterface", ex.Message);
		}
	}

	public void ClearNetworkInterfaces()
	{
		List<IMulticastClient> list;
		lock (_lockObject)
		{
			list = new List<IMulticastClient>(_clients.Values);
		}
		foreach (IMulticastClient item in list)
		{
			RemoveNetworkInterface(item);
		}
	}
}
