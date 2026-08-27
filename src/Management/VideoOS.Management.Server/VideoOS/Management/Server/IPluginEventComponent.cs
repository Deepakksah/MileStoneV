using System;

namespace VideoOS.Management.Server;

public interface IPluginEventComponent : IDisposable
{
	void CreatePluginEvent(PluginEvent newPluginEvent, string userSid);

	string[] QueryPluginEventIds();

	PluginEvent[] QueryPluginEvents();

	PluginEvent[] QueryPluginEvents(string[] pluginEventIds);

	PluginEvent QueryPluginEvent(string pluginEventId);

	PluginEvent QueryPluginEventByName(string pluginEventName);

	void UpdatePluginEvent(PluginEvent pluginEvent);

	void DeletePluginEvent(string pluginEventId);
}
