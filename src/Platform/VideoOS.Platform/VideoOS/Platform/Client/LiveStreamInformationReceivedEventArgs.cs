using System;

namespace VideoOS.Platform.Client;

public class LiveStreamInformationReceivedEventArgs : EventArgs
{
	public string Information { get; private set; }

	public LiveStreamInformationReceivedEventArgs(string information)
	{
		Information = information;
	}
}
