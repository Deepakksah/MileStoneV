using System;

namespace VideoOS.Common.SharedComponent;

public class CameraInfo
{
	public Guid CameraId { get; private set; }

	public string CameraName { get; private set; }

	public ViewItemSettings ViewItemSettings { get; set; }

	public CameraInfo(Guid cameraId, string cameraName)
	{
		ViewItemSettings = new ViewItemSettings();
		CameraId = cameraId;
		CameraName = cameraName;
	}
}
