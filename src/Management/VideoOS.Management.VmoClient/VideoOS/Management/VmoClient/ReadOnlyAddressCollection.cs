using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class ReadOnlyAddressCollection : IEnumerable<ReadOnlyAddress>, IEnumerable
{
	private List<ReadOnlyAddress> _internalStorage = new List<ReadOnlyAddress>();

	public int Count => _internalStorage.Count;

	public ReadOnlyAddress this[int index] => _internalStorage[index];

	public void Add(ReadOnlyAddress address)
	{
		if (!_internalStorage.Contains(address) && address != null)
		{
			_internalStorage.Add(address);
		}
	}

	public bool Remove(ReadOnlyAddress address)
	{
		if (_internalStorage.Contains(address))
		{
			_internalStorage.Remove(address);
			return true;
		}
		return false;
	}

	public IEnumerator GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	IEnumerator<ReadOnlyAddress> IEnumerable<ReadOnlyAddress>.GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	public ReadOnlyAddressCollection Clone()
	{
		ReadOnlyAddressCollection readOnlyAddressCollection = new ReadOnlyAddressCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ReadOnlyAddress readOnlyAddress = (ReadOnlyAddress)enumerator.Current;
				ReadOnlyAddress address = new ReadOnlyAddress(readOnlyAddress.ServerUrl, readOnlyAddress.AddressType);
				readOnlyAddressCollection.Add(address);
			}
			return readOnlyAddressCollection;
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
	}

	public bool IsEqual(ReadOnlyAddressCollection addressCollectionB)
	{
		if (addressCollectionB == null)
		{
			return false;
		}
		if (Count != addressCollectionB.Count)
		{
			return false;
		}
		foreach (ReadOnlyAddress item in addressCollectionB)
		{
			if (!TryGetValue(item.ServerUrl, out var _))
			{
				return false;
			}
		}
		return true;
	}

	public bool TryGetValue(Uri serverUrl, out ReadOnlyAddress address)
	{
		address = _internalStorage.FirstOrDefault((ReadOnlyAddress f) => f.ServerUrl == serverUrl);
		return address != null;
	}

	public ServerAddress[] ToServerAddress()
	{
		List<ServerAddress> list = new List<ServerAddress>();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				ReadOnlyAddress readOnlyAddress = (ReadOnlyAddress)enumerator.Current;
				ServerAddress serverAddress = new ServerAddress();
				serverAddress.URL = readOnlyAddress.ServerUrl.OriginalString;
				serverAddress.Type = (ServerAddressType)readOnlyAddress.AddressType;
				list.Add(serverAddress);
			}
		}
		finally
		{
			IDisposable disposable = enumerator as IDisposable;
			if (disposable != null)
			{
				disposable.Dispose();
			}
		}
		return list.ToArray();
	}

	public string ToServerAddressXml()
	{
		return ServerAddresses.GetAsXml(ToServerAddress());
	}
}
