using System;

namespace VideoOS.Toolkit;

public interface ILiveStatusData : IOutOfBandData
{
	public enum StatusValue
	{
		NotAvailable,
		True,
		False
	}

	DateTime GetStatusTime();

	StatusValue GetCameraLiveFeedStarted();

	StatusValue GetCameraLiveMotion();

	StatusValue GetCameraRecording();

	StatusValue GetCameraEventNotification();

	StatusValue GetCameraConnectionLost();

	StatusValue GetCameraDatabaseFail();

	StatusValue GetServerRunningOutOfDiskSpace();

	StatusValue GetClientLiveFeedStopped();
}
