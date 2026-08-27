namespace VideoOS.Common.Integration.LicenseInfo;

public class SiteLicense
{
	public string Id { get; private set; }

	public string Name { get; private set; }

	public int HardwareCount { get; private set; }

	public int ICPCameraCount { get; private set; }

	public int CameraCount { get; private set; }

	public SiteLicense(string id, string name, int hardwareCount, int interconnectedCameraCount, int cameraCount)
	{
		Id = id;
		Name = name;
		HardwareCount = hardwareCount;
		ICPCameraCount = interconnectedCameraCount;
		CameraCount = cameraCount;
	}
}
