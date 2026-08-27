using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class HeartbeatResponse
{
	private readonly List<Guid> _unregisteredDevices;

	private readonly List<Guid> _unrecognizedDevices;

	public DateTime AcknowledgedTimestamp { get; set; }

	public HeartbeatResult HeartbeatResult { get; set; }

	public bool HeartbeatOnly { get; set; }

	public int HeartbeatInterval { get; set; }

	public List<Guid> UnregisteredDevices => _unregisteredDevices;

	public List<Guid> UnrecognizedDevices => _unrecognizedDevices;

	public HeartbeatResponse()
	{
		HeartbeatOnly = false;
		HeartbeatInterval = 0;
		_unregisteredDevices = new List<Guid>();
		_unrecognizedDevices = new List<Guid>();
	}

	public override bool Equals(object other)
	{
		if (!(other is HeartbeatResponse heartbeatResponse))
		{
			return false;
		}
		if (AcknowledgedTimestamp.Equals(heartbeatResponse.AcknowledgedTimestamp) && HeartbeatResult == heartbeatResponse.HeartbeatResult && UnregisteredDevices.SequenceEqual(heartbeatResponse.UnregisteredDevices) && UnrecognizedDevices.SequenceEqual(heartbeatResponse.UnrecognizedDevices) && HeartbeatOnly == heartbeatResponse.HeartbeatOnly)
		{
			return HeartbeatInterval == heartbeatResponse.HeartbeatInterval;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (int)(((((uint)(((((((_unregisteredDevices != null) ? _unregisteredDevices.GetHashCode() : 0) * 397) ^ ((_unrecognizedDevices != null) ? _unrecognizedDevices.GetHashCode() : 0)) * 397) ^ AcknowledgedTimestamp.GetHashCode()) * 397) ^ (uint)HeartbeatResult) * 397) ^ (HeartbeatOnly ? 1u : 0u)) * 397) ^ HeartbeatInterval;
	}
}
