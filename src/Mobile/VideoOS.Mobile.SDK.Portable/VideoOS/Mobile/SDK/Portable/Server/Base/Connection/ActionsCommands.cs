using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class ActionsCommands : IActionsAsync, IActionsSync
{
	private readonly IConnection _connection;

	public ActionsCommands(IConnection connection)
	{
		_connection = connection;
	}

	public GetActionsResponse GetActions(OutputsEventsParams actionsParams, TimeSpan timeout)
	{
		IAsyncResult actions = GetActions(actionsParams);
		return new GetActionsResponse(_connection.ProcessResult(actions, timeout));
	}

	public BaseCommandResponse ActivateAction(ActivateParams activateParams, TimeSpan timeout)
	{
		IAsyncResult iResult = ActivateAction(activateParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public IAsyncResult GetActions(OutputsEventsParams actionsParams, OnResponse<GetActionsResponse> onSuccess = null, OnResponse<GetActionsResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetOutputsAndEvents);
		actionsParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetActionsResponse(response));
	}

	public IAsyncResult ActivateAction(ActivateParams activateParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.RequestActivation);
		activateParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}
}
