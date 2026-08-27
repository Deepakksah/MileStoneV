using System;
using VideoOS.Common.Integration.Definition;

namespace VideoOS.Management.Server;

public class DeviceEvent : IEquatable<DeviceEvent>
{
	public string EventId { get; set; }

	public string EventName { get; set; }

	public string DeviceId { get; set; }

	public bool IsUsed { get; set; }

	public bool IsEnabled { get; set; }

	public string EventIndex { get; set; }

	public string DriverEventId { get; set; }

	public string EventIndexDisplayName { get; public set; }

	public EventHwid Hwid { get; public set; }

	public bool Equals(DeviceEvent other)
	{
		if (other == null)
		{
			return false;
		}
		if (!string.Equals(EventId, other.EventId) || !string.Equals(EventName, other.EventName))
		{
			return false;
		}
		if (!string.Equals(DeviceId, other.DeviceId))
		{
			return false;
		}
		if (!IsUsed == other.IsUsed || !IsEnabled == other.IsEnabled)
		{
			return false;
		}
		if (!string.Equals(EventIndex, other.EventIndex) || !string.Equals(EventIndexDisplayName, other.EventIndexDisplayName))
		{
			return false;
		}
		if (!object.Equals(Hwid, other.Hwid))
		{
			return false;
		}
		return true;
	}

	public override bool Equals(object obj)
	{
		return Equals(obj as DeviceEvent);
	}

	public override int GetHashCode()
	{
		return (EventId, EventName, DeviceId, IsUsed, IsEnabled, EventIndex, EventIndexDisplayName, Hwid).GetHashCode();
	}
}
