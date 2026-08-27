using System;

namespace VideoOS.Management.VmoClient;

public class Address
{
	private Uri _serverUrl;

	private AddressType _addressType;

	public Uri ServerUrl
	{
		get
		{
			return _serverUrl;
		}
		set
		{
			_serverUrl = value;
			OnChanged();
		}
	}

	public AddressType AddressType
	{
		get
		{
			return _addressType;
		}
		set
		{
			_addressType = value;
			OnChanged();
		}
	}

	public event EventHandler ChangedEvent;

	public virtual void OnChanged()
	{
		this.ChangedEvent?.Invoke(this, new EventArgs());
	}

	public Address(Uri serverUrl, AddressType type)
	{
		_serverUrl = serverUrl;
		_addressType = type;
	}
}
