using System;

namespace VideoOS.Management.Server;

public interface IVideoWallFunctionalityComponent : IDisposable
{
	Tuple<string, string>[] ActivateVideoWallPreset(string videoWallPresetId, Guid sessionId);

	void ApplyCamerasAndLayoutToMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, int fromPosition, string layoutId, Guid sessionId);

	void ApplyCamerasToMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, int fromPosition, Guid sessionId);

	void ApplyLayoutToMonitorView(Monitor monitor, string layoutId, Guid sessionId);

	void ApplyTextAndLayoutToMonitorView(Monitor monitor, string messageToDisplay, int atPosition, string layoutId, Guid sessionId);

	void ApplyViewItemsToMonitorView(Monitor monitor, string[] viewItemsDefinitionXml, int fromPosition, Guid sessionId);

	void ClearView(string viewId, Guid sessionId, string viewXml, string smartClientId);

	void ClearViewItem(string viewItemId, Guid sessionId, string smartClientId);

	void RemoveCamerasFromMonitorView(Monitor monitor, string[] cameras, string[] cameraGroups, Guid sessionId);

	string GetVideoWallId(string videoWallPresetId);
}
