using System;

namespace VideoOS.Management.VmoClient;

public class ReadOnlyAddress
{
	private Uri _serverUrl;

	private AddressType _addressType;

	public Uri ServerUrl => _serverUrl;

	public AddressType AddressType => _addressType;

	public ReadOnlyAddress(Uri serverUrl, AddressType type)
	{
		_serverUrl = serverUrl;
		_addressType = type;
	}
}
