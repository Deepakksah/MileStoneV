using System.Collections.Generic;

namespace VideoOS.Mobile.Management;

public class InternetAccessConfig
{
	private string _address;

	private bool _isHttpPort;

	private bool _isHttpsPort;

	private ushort _port;

	private bool _enabled;

	private bool _isDynamicIp;

	private bool _disableDefaultAddress;

	public string Address
	{
		get
		{
			return _address;
		}
		set
		{
			_address = value;
		}
	}

	public bool IsHttpPort
	{
		get
		{
			return _isHttpPort;
		}
		set
		{
			_isHttpPort = value;
		}
	}

	public bool IsHttpsPort
	{
		get
		{
			return _isHttpsPort;
		}
		set
		{
			_isHttpsPort = value;
		}
	}

	public ushort Port
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

	public bool Enabled
	{
		get
		{
			return _enabled;
		}
		set
		{
			_enabled = value;
		}
	}

	public bool DynamicIp
	{
		get
		{
			return _isDynamicIp;
		}
		set
		{
			_isDynamicIp = value;
		}
	}

	public bool DisableDefaultAddress
	{
		get
		{
			return _disableDefaultAddress;
		}
		set
		{
			_disableDefaultAddress = value;
		}
	}

	public void InitFromConfig(Dictionary<string, string> item)
	{
		DictionaryHelper.InitFromSettings(ref _address, item, null, Definitions.AccessServerIpName, (string input) => input);
		DictionaryHelper.InitFromSettings(ref _port, item, null, Definitions.AccessServerPortName, ushort.Parse);
		DictionaryHelper.InitFromSettings(ref _isHttpPort, item, null, Definitions.AccessServerHttp, bool.Parse);
		DictionaryHelper.InitFromSettings(ref _isHttpsPort, item, null, Definitions.AccessServerHttps, bool.Parse);
		DictionaryHelper.InitFromSettings(ref _enabled, item, null, Definitions.AccessCustomConfigEnabled, bool.Parse);
		DictionaryHelper.InitFromSettings(ref _isDynamicIp, item, null, Definitions.AccessDynamicIpEnabledName, bool.Parse);
		DictionaryHelper.InitFromSettings(ref _disableDefaultAddress, item, null, Definitions.AccessDisableDefaultAddressName, bool.Parse);
	}

	public void SaveToItem(Dictionary<string, string> properties)
	{
		properties.SetProperty(Definitions.AccessServerIpName, _address);
		properties.SetProperty(Definitions.AccessServerHttp, _isHttpPort);
		properties.SetProperty(Definitions.AccessServerHttps, _isHttpsPort);
		properties.SetProperty(Definitions.AccessServerPortName, _port);
		properties.SetProperty(Definitions.AccessCustomConfigEnabled, _enabled);
		properties.SetProperty(Definitions.AccessDynamicIpEnabledName, _isDynamicIp);
		properties.SetProperty(Definitions.AccessDisableDefaultAddressName, _disableDefaultAddress);
	}
}
