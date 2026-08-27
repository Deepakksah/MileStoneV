namespace VideoOS.Platform.Client;

public class ConnectResponseEventEventArgs
{
	public short ConnectionGranted { get; set; }

	public ConnectResponseEventEventArgs(short connected)
	{
		ConnectionGranted = connected;
	}
}
