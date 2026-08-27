namespace VideoOS.Management.Common;

public static class PerformanceStateRuleConfigType
{
	public const int Undefined = 0;

	public const int ServerCpuUsage = 1;

	public const int ServerMemoryUsage = 2;

	public const int CameraLiveFps = 3;

	public const int CameraRecordingFps = 4;

	public const int CameraUsedSpace = 5;

	public const int DiskFreeSpace = 6;

	public const int StorageRetentionTime = 7;

	public const int ServiceAvailable = 8;

	public const int HostOnline = 9;

	public const int CameraState = 10;

	public const int ArchiveAvailable = 11;

	public const int DatabaseAvailable = 12;

	public const int FailoverStarted = 14;

	public const int GPUDecoding = 15;

	public const int GPUMemory = 16;

	public const int GPURendering = 17;

	public const int FailoverErrorState = 18;

	public static int[] CameraTypes = new int[4] { 3, 4, 5, 10 };

	public static int[] ServerTypes = new int[11]
	{
		1, 2, 6, 7, 11, 12, 14, 15, 16, 17,
		18
	};

	public static int[] HiddenServerTypes = new int[2] { 8, 9 };

	public static int[] HiddenCameraTypes = new int[2] { 9, 10 };

	public static int[] RecordingServerOnly = new int[3] { 11, 12, 7 };

	public static int[] GPUEvents = new int[3] { 15, 16, 17 };

	public static int[] TriggeredByManagementServer = new int[1] { 7 };
}
