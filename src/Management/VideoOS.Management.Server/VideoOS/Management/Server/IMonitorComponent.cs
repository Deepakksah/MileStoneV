using System;

namespace VideoOS.Management.Server;

public interface IMonitorComponent : IDisposable
{
	void CreateMonitor(string videoWallId, Monitor monitor, Guid sessionId);

	void DeleteMonitor(string monitorId, string sessionId);

	string QueryMonitorIdFromPreset(string monitorPresetId);

	string QueryVideoWallId(string monitorId);

	Monitor QueryMonitor(string monitorId);

	string[] QueryMonitorIds(string videoWallId);

	void UpdateMonitor(Monitor monitor, Guid sessionId);

	MonitorPreset QueryMonitorPreset(string presetId);

	string[] QueryMonitorPresetIdsByMonitorId(string monitorId);

	string[] QueryMonitorPresetIdsByVideoWallPresetId(string videoWallPresetId);

	MonitorPreset[] QueryMonitorPresetsByIds(string[] monitorPresetIds);

	Monitor[] QueryMonitors(string videoWallId);

	Monitor[] QueryMonitorsByIds(string[] monitorIds);

	void UpdateMonitorPreset(string presetId, string definitionXml, Guid sessionId, CustomProperty[] customProperties);

	void UpdateMonitorState(string monitorId, string xmlState);
}
