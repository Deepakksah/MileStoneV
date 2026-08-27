namespace VideoOS.Management.Common;

public abstract class StorageCreationException : RecorderSubsystemException
{
	public StorageCreationException(string message)
		: base(message)
	{
	}
}
