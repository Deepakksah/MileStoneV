using System;

namespace VideoOS.Platform.SDK.Multicast;

public class MulticastDevice
{
	public enum MulticastMode
	{
		Stopped,
		Starting,
		Started,
		Error
	}

	private Guid _deviceId = Guid.Empty;

	private Guid _sessionId = Guid.Empty;

	private MulticastMode _deviceMode;

	private volatile int _refCount;

	private DateTime _nextKeepAlive = DateTime.Now;

	private TimeSpan _keepAliveInterval = TimeSpan.Zero;

	private TimeSpan _keepAliveTimerInterval = TimeSpan.Zero;

	private string _ipAddress = string.Empty;

	private int _port;

	private bool _encrypted;

	private int _ssrc;

	private string _privacyMask = string.Empty;

	private int _privacyMaskVersion;

	public Guid DeviceID => _deviceId;

	public Guid SessionID
	{
		get
		{
			return _sessionId;
		}
		set
		{
			_sessionId = value;
			_deviceMode = MulticastMode.Started;
		}
	}

	public string IPAddress
	{
		get
		{
			return _ipAddress;
		}
		set
		{
			_ipAddress = value;
		}
	}

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

	public int SSRC
	{
		get
		{
			return _ssrc;
		}
		set
		{
			_ssrc = value;
		}
	}

	public string PrivacyMask
	{
		get
		{
			return _privacyMask;
		}
		set
		{
			_privacyMask = value;
		}
	}

	public int PrivacyMaskVersion
	{
		get
		{
			return _privacyMaskVersion;
		}
		set
		{
			_privacyMaskVersion = value;
		}
	}

	public TimeSpan KeepAliveInterval
	{
		get
		{
			return _keepAliveInterval;
		}
		set
		{
			_keepAliveInterval = value;
		}
	}

	public bool Encrypted
	{
		get
		{
			return _encrypted;
		}
		set
		{
			_encrypted = value;
		}
	}

	public TimeSpan KeepAliveTimerInterval
	{
		get
		{
			return _keepAliveTimerInterval;
		}
		set
		{
			_keepAliveTimerInterval = value;
		}
	}

	public int NumUsers => _refCount;

	public MulticastDevice(Guid deviceId)
	{
		_deviceId = deviceId;
	}

	public void Register(out string ipAddress, out int port, out int ssrc, out string privacyMask, out int privacyMaskVersion)
	{
		ipAddress = string.Empty;
		port = 0;
		ssrc = 0;
		privacyMask = string.Empty;
		privacyMaskVersion = 0;
		_refCount++;
		MulticastLogger.LogDebug("MulticastDevice.Register", "Multicast - New refcount: " + _refCount + "(Dev:" + _deviceId.ToString() + ")");
		if (_deviceMode == MulticastMode.Started)
		{
			ipAddress = _ipAddress;
			port = _port;
			ssrc = _ssrc;
			privacyMask = _privacyMask;
			privacyMaskVersion = _privacyMaskVersion;
		}
	}

	public void Unregister()
	{
		_refCount--;
		MulticastLogger.LogDebug("MulticastDevice.Unregister", "Multicast - New refcount: " + _refCount + "(Dev:" + _deviceId.ToString() + ")");
	}

	public bool Start()
	{
		if (_deviceMode != MulticastMode.Stopped && _deviceMode != MulticastMode.Error)
		{
			return false;
		}
		if (_refCount <= 0)
		{
			return false;
		}
		_deviceMode = MulticastMode.Starting;
		return true;
	}

	public bool Stop()
	{
		if (_deviceMode == MulticastMode.Stopped)
		{
			return false;
		}
		if (_refCount > 0)
		{
			return false;
		}
		_deviceMode = MulticastMode.Stopped;
		return true;
	}

	public void SetError()
	{
		if (_refCount > 0)
		{
			_deviceMode = MulticastMode.Error;
		}
	}

	public bool KeepAlive(bool forceAll)
	{
		if (_sessionId == Guid.Empty || _deviceMode != MulticastMode.Started)
		{
			return false;
		}
		if (forceAll || _nextKeepAlive < DateTime.Now)
		{
			_nextKeepAlive = DateTime.Now + _keepAliveInterval - _keepAliveTimerInterval;
			return true;
		}
		return false;
	}

	public bool InError()
	{
		if (_deviceMode == MulticastMode.Error)
		{
			return true;
		}
		return false;
	}

	public bool HasPendingDevices()
	{
		if (_deviceMode == MulticastMode.Starting || _deviceMode == MulticastMode.Error)
		{
			return true;
		}
		return false;
	}
}
