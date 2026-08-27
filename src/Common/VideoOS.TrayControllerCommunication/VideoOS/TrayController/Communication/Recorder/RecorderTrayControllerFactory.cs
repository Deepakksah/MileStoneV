namespace VideoOS.TrayController.Communication.Recorder;

public class RecorderTrayControllerFactory
{
	public static IRecorderTrayControllerServer GetServer(int portNumber, IMessageProvider messageProvider, IRecorderStateProvider recorderStateProvider)
	{
		return new RecorderTrayControllerServer(portNumber, messageProvider, recorderStateProvider);
	}

	public static IRecorderTrayControllerClient GetClient(int portNumber)
	{
		return new RecorderTrayControllerClient(portNumber);
	}

	public static int GetDefaultPort()
	{
		return 8966;
	}
}
