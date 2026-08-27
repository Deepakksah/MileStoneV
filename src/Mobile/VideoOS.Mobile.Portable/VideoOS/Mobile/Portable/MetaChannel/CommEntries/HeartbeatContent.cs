using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class HeartbeatContent
{
	private readonly NotifiedDevicesConfiguration _configuration;

	private readonly List<NotificationData> _notifications;

	public Guid ServerId { get; set; }

	public string ValidationSLC { get; set; }

	public DateTime Timestamp { get; set; }

	public int Timeout { get; set; }

	public bool NotificationsEnabled { get; set; }

	public bool SyncState { get; set; }

	public List<NotificationData> Notifications => _notifications;

	public NotifiedDevicesConfiguration Configuration => _configuration;

	public HeartbeatContent()
	{
		_notifications = new List<NotificationData>();
		_configuration = new NotifiedDevicesConfiguration();
	}

	public override bool Equals(object other)
	{
		if (!(other is HeartbeatContent heartbeatContent))
		{
			return false;
		}
		if (ServerId == heartbeatContent.ServerId && ValidationSLC == heartbeatContent.ValidationSLC && Timestamp == heartbeatContent.Timestamp && Timeout == heartbeatContent.Timeout && NotificationsEnabled == heartbeatContent.NotificationsEnabled && _notifications.SequenceEqual(heartbeatContent._notifications))
		{
			return _configuration.Equals(heartbeatContent._configuration);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((((((((((((_configuration != null) ? _configuration.GetHashCode() : 0) * 397) ^ ((_notifications != null) ? _notifications.GetHashCode() : 0)) * 397) ^ ServerId.GetHashCode()) * 397) ^ ((ValidationSLC != null) ? ValidationSLC.GetHashCode() : 0)) * 397) ^ Timestamp.GetHashCode()) * 397) ^ Timeout) * 397) ^ NotificationsEnabled.GetHashCode();
	}
}
