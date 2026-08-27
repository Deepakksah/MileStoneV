namespace VideoOS.Platform.Messaging;

public enum PtzErrorDetail
{
	None = 0,
	DeviceNotFound = 1,
	DeviceNotReady = 2,
	PtzNotSupported = 10,
	PresetNotSupported = 11,
	PtzRequestPriorityTooLow = 20,
	PtzRequestQueueTooLong = 21,
	PtzSessionNoneActive = 30,
	PtzSessionNotActive = 31,
	PtzSessionAlreadyStarted = 32,
	PtzSessionNotRunning = 33,
	PatrollingNoSession = 40,
	PatrollingResumedWhileNotPaused = 41,
	PatrollingSessionAttemptedReleased = 42,
	PatrollingSessionHasHigherPriority = 43,
	PatrollingSchemaNotFound = 50,
	InsufficientPermission = 60,
	AlreadyReserved = 70,
	PtzSessionIsReserved = 80,
	OtherUserManualSessionAttemptedReleased = 90
}
