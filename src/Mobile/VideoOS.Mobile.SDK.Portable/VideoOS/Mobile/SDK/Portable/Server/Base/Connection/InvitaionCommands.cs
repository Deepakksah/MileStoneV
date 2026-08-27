using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class InvitaionCommands : IInvitaionAsync, IInvitaionSync, IErrorResult
{
	private readonly IErrorResult _errorResult;

	private readonly IConnection _connection;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public InvitaionCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult SendInvitation(InvitationParams invitationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.SendInvitationInternal);
		invitationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult GetInvitationLinks(OnResponse<GetInvitationLinksResponse> onSuccess = null, OnResponse<GetInvitationLinksResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetInvitationLinks);
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetInvitationLinksResponse(response));
	}

	public BaseCommandResponse SendInvitation(InvitationParams invitationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = SendInvitation(invitationParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public GetInvitationLinksResponse GetInvitationLinks(TimeSpan timeout)
	{
		IAsyncResult invitationLinks = GetInvitationLinks();
		return new GetInvitationLinksResponse(_connection.ProcessResult(invitationLinks, timeout));
	}
}
