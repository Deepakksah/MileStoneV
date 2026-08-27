using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Failover;

public sealed class FailoverTrayControllerServiceImplementation : FailoverTrayControllerService.FailoverTrayControllerServiceBase
{
	private IFailoverStateProvider _failoverStateProvider;

	private IMessageProvider _messageProvider;

	public FailoverTrayControllerServiceImplementation(IMessageProvider messageProvider, IFailoverStateProvider failoverStateProvider)
	{
		_failoverStateProvider = failoverStateProvider;
		_messageProvider = messageProvider;
	}

	public override Task<ArrayOfMessages> GetMessages(GetMessages_Request request, ServerCallContext context)
	{
		return Utilities.GetMessages(request, context, _messageProvider);
	}

	public override Task<FailoverStateResponse> GetServiceState(Empty request, ServerCallContext context)
	{
		FailoverStateResponse failoverStateResponse = new FailoverStateResponse();
		FailoverState state = _failoverStateProvider.GetState();
		failoverStateResponse.ServiceState = TrayControllerCommunicationConverter.ToServiceState_Communication(state.serviceState);
		failoverStateResponse.ConnectStatus = TrayControllerCommunicationConverter.ToConnectStatus_Communication(state.connectStatus);
		failoverStateResponse.FailoverRecorderState = state.failoverRecorderState;
		return Task.FromResult(failoverStateResponse);
	}
}
