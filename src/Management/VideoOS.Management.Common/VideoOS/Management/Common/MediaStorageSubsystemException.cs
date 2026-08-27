namespace VideoOS.Management.Common;

public class MediaStorageSubsystemException : RecorderSubsystemException
{
	protected override int ErrorCodeDefault => -1;

	public MediaStorageSubsystemException(string message)
		: base(message)
	{
	}
}
