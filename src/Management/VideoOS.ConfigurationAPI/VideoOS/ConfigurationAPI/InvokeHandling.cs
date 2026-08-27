namespace VideoOS.ConfigurationAPI;

public static class InvokeHandling
{
	public static class AddHardwareErrorCodes
	{
		public const int RecordingServerNoDefaultStorage = 1;

		public const int RecordingServerCommunicationNoResponse = 2;

		public const int HardwareAlreadyAdded = 3;

		public const int RecordingServerUnavailable = 4;

		public const int RecordingServerNotSupported = 5;

		public const int RecordingServerCommunicationTimeout = 6;

		public const int RecordingServerUnknownError = 7;

		public const int RecordingServerException = 8;

		public const int HardwareSetupError = 9;

		public const int HardwareNotSupported = 10;

		public const int ConfigurationDatabaseError = 11;
	}
}
