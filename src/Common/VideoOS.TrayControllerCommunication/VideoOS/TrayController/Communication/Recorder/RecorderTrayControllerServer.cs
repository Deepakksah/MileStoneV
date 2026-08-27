using System;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Recorder;

public class RecorderTrayControllerServer : IRecorderTrayControllerServer
{
	private Server _grpcServer;

	public RecorderTrayControllerServer(int portNumber, IMessageProvider messageProvider, IRecorderStateProvider recorderStateProvider)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Server val = new Server();
		val.Services.Add(RecorderTrayControllerService.BindService(new RecorderTrayControllerServiceImplementation(messageProvider, recorderStateProvider)));
		_grpcServer = val;
		Utilities.AddLoopbackServerPorts(portNumber, _grpcServer);
	}

	public void StartServer()
	{
		_grpcServer.Start();
	}

	public void StopServer()
	{
		try
		{
			_grpcServer.ShutdownAsync().Wait();
		}
		catch (Exception)
		{
		}
	}
}
