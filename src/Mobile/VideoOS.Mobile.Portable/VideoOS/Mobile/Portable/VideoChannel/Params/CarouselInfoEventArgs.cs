using System;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class CarouselInfoEventArgs : EventArgs
{
	public Guid CameraId;

	public CarouselInfoEventArgs()
	{
		CameraId = Guid.Empty;
	}

	public CarouselInfoEventArgs(Guid cameraId)
	{
		CameraId = cameraId;
	}
}
