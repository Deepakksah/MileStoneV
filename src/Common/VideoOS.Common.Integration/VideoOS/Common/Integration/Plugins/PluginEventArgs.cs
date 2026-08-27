using System;

namespace VideoOS.Common.Integration.Plugins;

public class PluginEventArgs : EventArgs
{
	public Guid EventTypeId { get; private set; }

	public DateTime Time { get; private set; }

	public PluginEventArgs(Guid eventTypeId, DateTime time)
	{
		EventTypeId = eventTypeId;
		Time = time;
	}
}
