namespace VideoOS.Platform.Client;

public class LiveStreamInformationEventArgs
{
	public string Information { get; set; }

	public LiveStreamInformationEventArgs(string information)
	{
		Information = information;
	}
}
