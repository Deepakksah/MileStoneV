namespace VideoOS.Platform.SDK.Platform;

public class ServerNotFoundMIPException : MIPException
{
	public ServerNotFoundMIPException(string server)
		: base(server)
	{
	}
}
