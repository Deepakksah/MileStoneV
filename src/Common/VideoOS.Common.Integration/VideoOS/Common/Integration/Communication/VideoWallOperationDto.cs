using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public enum VideoWallOperationDto
{
	SetPreset,
	SetMonitorCameras,
	SetMonitorView,
	SetMonitorText,
	RemoveMonitorCameras
}
