namespace VideoOS.Platform.Client;

public enum OverlayTextId
{
	None = 0,
	NotInitialized = 3,
	NotConnected = 4,
	ConnectionFailed = 5,
	Connecting = 6,
	Connected = 7,
	BeforeDatabaseStart = 10,
	AfterDatabaseEnd = 11,
	ConnectionRefused = 12,
	ConnectionLost = 13,
	InternalError = 14,
	NoImagesInDatabase = 16,
	RunningOutOfDiskSpace = 22,
	CameraConnectionLost = 23,
	DatabaseFailed = 24,
	LiveFeedStoppedCausedByNoMotion = 25,
	ImmerVisionEnabledButFailedToInitialize = 32
}
