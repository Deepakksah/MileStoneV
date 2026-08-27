using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

public class AddressCollection : IEnumerable<Address>, IEnumerable
{
	private List<Address> _internalStorage = new List<Address>();

	public int Count => _internalStorage.Count;

	public Address this[int index] => _internalStorage[index];

	public event EventHandler CollectionChangedEvent;

	public virtual void OnChanged()
	{
		this.CollectionChangedEvent?.Invoke(this, new EventArgs());
	}

	private void AddressChanged(object sender, EventArgs e)
	{
		OnChanged();
	}

	public void Add(Address address)
	{
		if (!_internalStorage.Contains(address) && address != null)
		{
			address.ChangedEvent += AddressChanged;
			_internalStorage.Add(address);
			OnChanged();
		}
	}

	public bool Remove(Address address)
	{
		if (_internalStorage.Contains(address) && address != null && _internalStorage.Remove(address))
		{
			address.ChangedEvent -= AddressChanged;
			OnChanged();
			return true;
		}
		return false;
	}

	public void Clear()
	{
		if (_internalStorage.Count <= 0)
		{
			return;
		}
		foreach (Address item in _internalStorage)
		{
			item.ChangedEvent -= AddressChanged;
		}
		_internalStorage.Clear();
		OnChanged();
	}

	public IEnumerator GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	IEnumerator<Address> IEnumerable<Address>.GetEnumerator()
	{
		return _internalStorage.GetEnumerator();
	}

	public AddressCollection Clone()
	{
		AddressCollection addressCollection = new AddressCollection();
		IEnumerator enumerator = GetEnumerator();
		try
		{
			while (enumerator.MoveNext())
			{
				Address address = (Address)enumerator.Current;
				Address address2 = new Address(address.ServerUrl, address.AddressType);
				addressCollection.Add(address2);
			}
			return addressCollection;
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

	public bool IsEqual(AddressCollection addressCollectionB)
	{
		if (addressCollectionB == null)
		{
			return false;
		}
		if (Count != addressCollectionB.Count)
		{
			return false;
		}
		foreach (Address item in addressCollectionB)
		{
			if (!TryGetValue(item.ServerUrl, out var _))
			{
				return false;
			}
		}
		return true;
	}

	public bool TryGetValue(Uri serverUrl, out Address address)
	{
		if (serverUrl == null)
		{
			throw new ArgumentNullException("serverUrl");
		}
		address = _internalStorage.FirstOrDefault((Address f) => f.ServerUrl == serverUrl);
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
				Address address = (Address)enumerator.Current;
				ServerAddress serverAddress = new ServerAddress();
				serverAddress.URL = address.ServerUrl.OriginalString;
				serverAddress.Type = (ServerAddressType)address.AddressType;
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
