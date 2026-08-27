using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Common.Integration.LicenseInfo.Internal;

public class TotalLicense : ITotalLicense
{
	public int HardwareCount { get; set; }

	public int CameraCount { get; set; }

	public int ICPCameraCount { get; set; }

	public int ArculesCameraCount { get; set; }

	public TotalLicense(int hardwareCount, int cameraCount, int interconnectedCameraCount, int arculesCameraCount)
	{
		HardwareCount = hardwareCount;
		CameraCount = cameraCount;
		ICPCameraCount = interconnectedCameraCount;
		ArculesCameraCount = arculesCameraCount;
	}
}
