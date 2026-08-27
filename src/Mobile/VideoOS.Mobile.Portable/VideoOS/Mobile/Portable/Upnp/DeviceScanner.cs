using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Http.HttpMessage;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.Net;
using VideoOS.Mobile.Portable.Upnp.SsdpMessage;

namespace VideoOS.Mobile.Portable.Upnp;

public class DeviceScanner
{
	private class DeviceScanState
	{
		public Guid Token { get; set; }

		public IMulticastClient Client { get; set; }
	}

	private readonly Dictionary<DeviceSearchType, string> _deviceSearchTypeMap = new Dictionary<DeviceSearchType, string>
	{
		{
			DeviceSearchType.All,
			"ssdp:all"
		},
		{
			DeviceSearchType.InternetGateway,
			"urn:schemas-upnp-org:device:InternetGatewayDevice:1"
		},
		{
			DeviceSearchType.RootDevice,
			"upnp:rootdevice"
		},
		{
			DeviceSearchType.MobileServer,
			MobileServiceDescription.MobileServerDeviceType
		},
		{
			DeviceSearchType.BasicDevice,
			"urn:schemas-upnp-org:device:Basic:1"
		}
	};

	private IEnumerable<IMulticastClient> _clients = new List<IMulticastClient>();

	private readonly object _lockObject = new object();

	private readonly DeviceSearchType _searchType;

	private readonly int _timeOutSec = 2;

	private IMulticastClientProvider _iProvider;

	private readonly ManualResetEvent _handle = new ManualResetEvent(initialState: false);

	private Func<SsdpMessageSearchResponse, bool> _deviceFilter;

	private Guid _token = Guid.NewGuid();

	private const int RepeatIntervalMs = 2000;

	private const int NbRepeatMessage = 2;

	private bool _isRunning;

	private List<string> _foundDeviceList = new List<string>();

	private readonly HttpRequestSanitizer _requestSanitizer = new HttpRequestSanitizer();

	private readonly HttpResponseSanitizer _responseSanitizer = new HttpResponseSanitizer();

	public event EventHandler<DeviceArgs> DeviceFound;

	public event EventHandler<DeviceArgs> ScanningFinished;

	public DeviceScanner(DeviceSearchType searchType, int timeoutSec, Func<SsdpMessageSearchResponse, bool> deviceFilter = null)
	{
		_searchType = searchType;
		_timeOutSec = timeoutSec;
		_iProvider = new MulticastClientProvider(MulticastGroupProvider);
		_deviceFilter = deviceFilter;
	}

	private string MulticastGroupProvider(NetworkInterfaceProperties networkInterfaceProperties)
	{
		if (networkInterfaceProperties.Address.AddressFamily != AddressFamily.InterNetwork)
		{
			return "[FF02::C]";
		}
		return "239.255.255.250";
	}

	public void Start()
	{
		if (_isRunning)
		{
			return;
		}
		lock (_lockObject)
		{
			_foundDeviceList.Clear();
		}
		_isRunning = true;
		_clients = _iProvider.GetClients();
		if (_clients != null)
		{
			List<IMulticastClient> list = new List<IMulticastClient>(_clients);
			foreach (IMulticastClient item in list)
			{
				try
				{
					item.BeginReceive(RequestCallback, new DeviceScanState
					{
						Client = item,
						Token = _token
					});
				}
				catch (Exception ex)
				{
					Logger.Instance.Log(LogContent.Error, "DeviceScanner.Start", ex.Message);
				}
			}
			Task.Factory.StartNew(Action);
			for (int i = 0; i < 2; i++)
			{
				if (!_isRunning)
				{
					break;
				}
				foreach (IMulticastClient item2 in list)
				{
					SsdpMessageSearchRequest ssdpMessageSearchRequest = new SsdpMessageSearchRequest(_timeOutSec, _deviceSearchTypeMap[_searchType]);
					byte[] bytes = Encoding.UTF8.GetBytes(ssdpMessageSearchRequest.GenerateMessage(_requestSanitizer));
					item2.Send(bytes, bytes.Length, 1900);
				}
				_handle.WaitOne(2000);
			}
		}
		else
		{
			OnScanningFinished(new DeviceArgs());
		}
	}

	private void Action()
	{
		_handle.WaitOne(TimeSpan.FromSeconds(_timeOutSec + 1));
		Stop();
		OnScanningFinished(new DeviceArgs());
	}

	public void Stop()
	{
		_isRunning = false;
		_iProvider.ClearNetworkInterfaces();
	}

	private void RequestCallback(IAsyncResult result)
	{
		DeviceScanState deviceScanState = (DeviceScanState)result.AsyncState;
		try
		{
			if (_token.Equals(deviceScanState.Token))
			{
				string remote = null;
				byte[] array = deviceScanState.Client.EndReceive(result, ref remote);
				deviceScanState.Client.BeginReceive(RequestCallback, deviceScanState);
				if (array != null)
				{
					ProcessData(array, deviceScanState.Client);
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "DeviceScanner.RequestCallback", ex.Message);
		}
	}

	private void ProcessData(byte[] data, IMulticastClient multicastClient)
	{
		if (data.Length != 0 && SsdpMessageSearchResponse.TryParse(Encoding.UTF8.GetString(data, 0, data.Length), out var messageRequest) && _responseSanitizer.Sanitize(messageRequest) && messageRequest.SearchType.Equals(_deviceSearchTypeMap[_searchType], StringComparison.CurrentCultureIgnoreCase) && (_deviceFilter == null || (_deviceFilter != null && _deviceFilter(messageRequest))) && IsNewDevice(messageRequest.Usn))
		{
			OnDeviceFound(new DeviceArgs
			{
				LocalAddress = multicastClient.LocalAddress,
				Location = messageRequest.Location
			});
		}
	}

	private bool IsNewDevice(string usn)
	{
		bool result = true;
		lock (_lockObject)
		{
			if (!_foundDeviceList.Contains(usn.ToLower()))
			{
				_foundDeviceList.Add(usn.ToLower());
			}
			else
			{
				result = false;
			}
		}
		return result;
	}

	protected virtual void OnScanningFinished(DeviceArgs e)
	{
		_isRunning = false;
		this.ScanningFinished?.Invoke(this, e);
	}

	protected virtual void OnDeviceFound(DeviceArgs e)
	{
		EventHandler<DeviceArgs> handler = this.DeviceFound;
		if (handler == null)
		{
			return;
		}
		Task.Factory.StartNew(delegate
		{
			try
			{
				string description = GetDescription(e.Location);
				e.DeviceDescription = description;
				if (!string.IsNullOrEmpty(e.DeviceDescription))
				{
					handler(this, e);
				}
			}
			catch (Exception ex)
			{
				Logger.Instance.Log(LogContent.Error, "DeviceScanner.OnDeviceFound", ex.Message);
			}
		});
	}

	public string GetDescription(string location)
	{
		string result = string.Empty;
		WebClient webClient = new WebClient();
		try
		{
			result = webClient.DownloadString(location);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "StaticPortMapping.GetServiceUrl", ex.Message);
		}
		return result;
	}
}
