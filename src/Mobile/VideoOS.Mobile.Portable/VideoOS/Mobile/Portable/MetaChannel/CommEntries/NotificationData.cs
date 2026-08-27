using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class NotificationData
{
	public readonly Guid EventId;

	public readonly Guid DeviceId;

	public readonly string Message;

	public readonly List<string> MessageArgsList;

	public uint SourceTypes { get; private set; }

	public NotificationData(Guid deviceId, string message, List<string> messageArgs, Guid eventId, uint sourceTypes)
	{
		DeviceId = deviceId;
		Message = message;
		EventId = eventId;
		MessageArgsList = ((messageArgs != null) ? new List<string>(messageArgs) : new List<string>());
		SourceTypes = sourceTypes;
	}

	public NotificationData(Guid deviceId, string message, List<string> messageArgs, Guid eventId, Dictionary<string, string> properties)
		: this(deviceId, message, messageArgs, eventId, 1u)
	{
		if (properties != null && properties.TryGetValue("NotificationSourceType", out var value) && uint.TryParse(value, out var result))
		{
			SourceTypes = result;
		}
	}

	public override bool Equals(object other)
	{
		if (!(other is NotificationData notificationData))
		{
			return false;
		}
		if (DeviceId == notificationData.DeviceId && Message == notificationData.Message && EventId == notificationData.EventId && MessageArgsList.SequenceEqual(notificationData.MessageArgsList))
		{
			return SourceTypes == notificationData.SourceTypes;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((((EventId.GetHashCode() * 397) ^ DeviceId.GetHashCode()) * 397) ^ ((Message != null) ? Message.GetHashCode() : 0)) * 397) ^ ((MessageArgsList != null) ? MessageArgsList.GetHashCode() : 0)) * 397) ^ SourceTypes.GetHashCode();
	}
}
