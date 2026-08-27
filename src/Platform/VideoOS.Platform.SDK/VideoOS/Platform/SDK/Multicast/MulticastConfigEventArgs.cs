using System;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Multicast;

public class MulticastConfigEventArgs : EventArgs
{
	protected Guid _deviceId;

	protected string _ipAddress;

	protected int _port;

	protected int _ssrc;

	protected string _privacyMask;

	protected int _privacyMaskVersion;

	protected Guid _existingSessionId;

	public IRecorder Recorder { get; set; }

	public virtual Guid DeviceId
	{
		get
		{
			return _deviceId;
		}
		set
		{
			_deviceId = value;
		}
	}

	public virtual string IPAddress
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

	public virtual int Port
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

	public virtual int SSRC
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

	public virtual string PrivacyMask
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

	public virtual int PrivacyMaskVersion
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

	public virtual Guid ExistingSessionId
	{
		get
		{
			return _existingSessionId;
		}
		set
		{
			_existingSessionId = value;
		}
	}

	public MulticastConfigEventArgs(IRecorder recorder, Guid deviceId, string ipAddress, int port, int ssrc, string privacyMask, int privacyMaskVersion, Guid existingSessionId)
	{
		_deviceId = deviceId;
		_ipAddress = ipAddress;
		_port = port;
		_ssrc = ssrc;
		_privacyMask = privacyMask;
		_privacyMaskVersion = privacyMaskVersion;
		_existingSessionId = existingSessionId;
		Recorder = recorder;
	}
}
