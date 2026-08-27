namespace VideoOS.TrayController.Communication.Server;

public class ServerTrayControllerFactory
{
	public static IServerTrayControllerServer GetServer(int portNumber, IMessageProvider messageProvider, IServerStateProvider serverStateProvider)
	{
		return new ServerTrayControllerServer(portNumber, messageProvider, serverStateProvider);
	}

	public static IServerTrayControllerClient GetClient(int portNumber)
	{
		return new ServerTrayControllerClient(portNumber);
	}
}
