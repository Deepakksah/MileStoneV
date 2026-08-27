using System;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Failover;

public class FailoverTrayControllerServer : IFailoverTrayControllerServer
{
	private Server _grpcServer;

	public FailoverTrayControllerServer(int portNumber, IMessageProvider messageProvider, IFailoverStateProvider failoverStateProvider)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		Server val = new Server();
		val.Services.Add(FailoverTrayControllerService.BindService(new FailoverTrayControllerServiceImplementation(messageProvider, failoverStateProvider)));
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
