using System;

namespace VideoOS.Common.SharedComponent;

public class CameraViewItemInformation : ViewItemInformation
{
	private Guid _cameraId;

	public Guid CameraId => _cameraId;

	public CameraViewItemInformation(Guid cameraId)
	{
		_cameraId = cameraId;
	}
}
