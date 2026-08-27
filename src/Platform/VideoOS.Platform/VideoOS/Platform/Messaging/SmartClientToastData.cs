using System;

namespace VideoOS.Platform.Messaging;

public abstract class SmartClientToastData
{
	public Guid Id { get; }

	public TimeSpan Duration { get; }

	public Action<SmartClientToastData> Dismissed { get; }

	public Action<SmartClientToastData> Activated { get; }

	public Action<SmartClientToastData> Expired { get; }

	public SmartClientToastData(Guid id, TimeSpan duration, Action<SmartClientToastData> dismissed, Action<SmartClientToastData> activated, Action<SmartClientToastData> expired)
	{
		Id = id;
		Duration = duration;
		Dismissed = dismissed;
		Activated = activated;
		Expired = expired;
	}
}
