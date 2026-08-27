namespace VideoOS.Common.Interfaces.Licensing;

public interface ITotalLicense
{
	int HardwareCount { get; set; }

	int CameraCount { get; set; }

	int ICPCameraCount { get; set; }

	int ArculesCameraCount { get; set; }
}
