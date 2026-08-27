using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;

namespace VideoOS.TrayController.Communication.Recorder;

public sealed class RecorderTrayControllerServiceImplementation : RecorderTrayControllerService.RecorderTrayControllerServiceBase
{
	private IMessageProvider _messageProvider;

	private IRecorderStateProvider _recorderStateProvider;

	public RecorderTrayControllerServiceImplementation(IMessageProvider messageProvider, IRecorderStateProvider recorderStateProvider)
	{
		_messageProvider = messageProvider;
		_recorderStateProvider = recorderStateProvider;
	}

	public override Task<ArrayOfMessages> GetMessages(GetMessages_Request request, ServerCallContext context)
	{
		return Utilities.GetMessages(request, context, _messageProvider);
	}

	public override Task<RecorderStateResponse> GetServiceState(Empty request, ServerCallContext context)
	{
		RecorderStateResponse recorderStateResponse = new RecorderStateResponse();
		RecorderState state = _recorderStateProvider.GetState();
		recorderStateResponse.ServiceState = TrayControllerCommunicationConverter.ToServiceState_Communication(state.serviceState);
		recorderStateResponse.ConnectStatus = TrayControllerCommunicationConverter.ToConnectStatus_Communication(state.connectStatus);
		return Task.FromResult(recorderStateResponse);
	}
}
