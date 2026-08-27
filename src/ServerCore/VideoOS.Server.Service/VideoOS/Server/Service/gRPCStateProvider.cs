using VideoOS.TrayController.Communication.Server;

namespace VideoOS.Server.Service;

public sealed class gRPCStateProvider
{
	private IServerTrayControllerServer _server;

	public gRPCStateProvider(ServerServiceState state, int port)
	{
		_server = ServerTrayControllerFactory.GetServer(port, state, state);
	}

	public void Start()
	{
		_server.StartServer();
	}

	public void Stop()
	{
		_server.StopServer();
	}
}
