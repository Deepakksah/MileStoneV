using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Server;

public sealed class ServerTrayControllerServiceImplementation : ServerTrayControllerService.ServerTrayControllerServiceBase
{
	private IMessageProvider _messageProvider;

	private IServerStateProvider _serverStateProvider;

	public ServerTrayControllerServiceImplementation(IMessageProvider messageProvider, IServerStateProvider serverStateProvider)
	{
		_messageProvider = messageProvider;
		_serverStateProvider = serverStateProvider;
	}

	public override Task<ArrayOfMessages> GetMessages(GetMessages_Request request, ServerCallContext context)
	{
		return Utilities.GetMessages(request, context, _messageProvider);
	}

	public override Task<ServerStateResponse> GetServiceState(Empty request, ServerCallContext context)
	{
		ServerStateResponse serverStateResponse = new ServerStateResponse();
		ServerState state = _serverStateProvider.GetState();
		serverStateResponse.ServiceState = TrayControllerCommunicationConverter.ToServiceState_Communication(state.serviceState);
		return Task.FromResult(serverStateResponse);
	}
}
