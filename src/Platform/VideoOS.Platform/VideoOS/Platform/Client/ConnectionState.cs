namespace VideoOS.Platform.Client;

public class ConnectionState
{
	public bool Connected { get; set; }

	public int NotConnectedState { get; set; }

	public ConnectionState(bool connected, int notConnectedState)
	{
		Connected = connected;
		NotConnectedState = notConnectedState;
	}
}
