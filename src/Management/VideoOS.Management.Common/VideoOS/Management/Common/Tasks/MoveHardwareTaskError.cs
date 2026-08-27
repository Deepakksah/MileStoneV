namespace VideoOS.Management.Common.Tasks;

public static class MoveHardwareTaskError
{
	public const int DestinationRecordingServerNotConnected = 60261;

	public const int DestinationRecordingServerNotCompatible = 60262;

	public const int DestinationRecordingServerNotFound = 60263;

	public const int SourceRecordingServerNotConnected = 60264;

	public const int SourceRecordingServerNotCompatible = 60265;

	public const int HardwareAlreadyConfiguredOnDestinationRecordingServer = 60312;

	public const int SourceRecordingServerNotFound = 60266;

	public const int HardwareNotFound = 60267;

	public const int DatabaseCommunicationError = 60268;

	public const int StopHardwareFailed = 60269;

	public const int StopHardwareTimeout = 60271;

	public const int UpdateConfigurationFailed = 60272;

	public const int UpdateConfigurationConcurrencyError = 60308;

	public const int IgnoredSourceRecordingServerIsAvailable = 60273;

	public const int SourceRecordingStorageUnavailable = 60327;

	public const int AllRecordingStoragesOnTargetRecorderNotAvailable = 60328;
}
