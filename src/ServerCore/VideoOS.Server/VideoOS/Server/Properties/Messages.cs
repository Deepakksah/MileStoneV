namespace VideoOS.Server.Properties;

public static class Messages
{
	public static string LoadPasswordDiskError()
	{
		return "There was a problem loading the system configuration password from the disk. Go to the tray controller to enter a system configuration password.";
	}

	public static string EncryptionKeyReadError()
	{
		return "An error occurred while validating the system configuration password. Go to the tray controller to enter a valid system configuration password.";
	}

	public static string EncryptionKeyIsMissing()
	{
		return "The server expected to find the encryption key in the database. Your system might be in an unrecoverable state.";
	}

	public static string DefaultEncryptionKeyLoadError()
	{
		return "There was a problem loading the encryption key.";
	}
}
