namespace VideoOS.TrayController.Communication.Failover;

public class FailoverTrayControllerFactory
{
	public static IFailoverTrayControllerServer GetServer(int portNumber, IMessageProvider messageProvider, IFailoverStateProvider failoverStateProvider)
	{
		return new FailoverTrayControllerServer(portNumber, messageProvider, failoverStateProvider);
	}

	public static IFailoverTrayControllerClient GetClient(int portNumber)
	{
		return new FailoverTrayControllerClient(portNumber);
	}
}
