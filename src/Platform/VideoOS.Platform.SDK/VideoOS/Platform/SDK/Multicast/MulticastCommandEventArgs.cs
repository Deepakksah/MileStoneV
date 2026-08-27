using System;
using VideoOS.Platform.SDK.Config;

namespace VideoOS.Platform.SDK.Multicast;

public class MulticastCommandEventArgs : EventArgs
{
	protected Guid[] _deviceIds;

	protected Guid[] _sessionIds;

	public IRecorder Recorder { get; set; }

	public virtual Guid[] DeviceIds
	{
		get
		{
			return _deviceIds;
		}
		set
		{
			_deviceIds = value;
		}
	}

	public virtual Guid[] SessionIds
	{
		get
		{
			return _sessionIds;
		}
		set
		{
			_sessionIds = value;
		}
	}

	public MulticastCommandEventArgs(IRecorder recorder, Guid[] deviceIds, Guid[] sessionIds)
	{
		_deviceIds = deviceIds;
		_sessionIds = sessionIds;
		Recorder = recorder;
	}
}
