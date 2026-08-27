using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class AddressBookServiceCommands : IAddressBookServiceAsync, IAddressBookServiceSync, IErrorResult
{
	private readonly IErrorResult _errorResult;

	private readonly IConnection _connection;

	private const string ServerAccessibility = "CheckServerAccessibility";

	private const string SetEndpoints = "SetServerConfiguration";

	private const string GetEndpoints = "GetServerConfiguration";

	private const string SendInvitationName = "SendInvitation";

	private const string GetInvitationLinksName = "GetInvitationLinks";

	private readonly Func<BaseCommandResponse, AddressBookServiceResponse> _absResponseFactory = (BaseCommandResponse baseCommandResponse) => new AddressBookServiceResponse(baseCommandResponse);

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public AddressBookServiceCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.CheckServerAccessibility);
		serverConfigurationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, _absResponseFactory, "CheckServerAccessibility");
	}

	public IAsyncResult SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.SetServerConfiguration);
		serverConfigurationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, _absResponseFactory, "SetServerConfiguration");
	}

	public IAsyncResult GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, OnResponse<AddressBookServiceResponse> onSuccess = null, OnResponse<AddressBookServiceResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetServerConfiguration);
		serverConfigurationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, _absResponseFactory, "GetServerConfiguration");
	}

	public IAsyncResult SendInvitation(InvitationParams invitationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.SendInvitation);
		invitationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, "SendInvitation");
	}

	public IAsyncResult GetInvitationLinks(OnResponse<GetInvitationLinksResponse> onSuccess = null, OnResponse<GetInvitationLinksResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetInvitationLinks);
		return _connection.SendCommand(command, delegate(BaseCommandResponse response)
		{
			onSuccess?.Invoke(new GetInvitationLinksResponse(response));
		}, delegate(BaseCommandResponse response)
		{
			onFail?.Invoke(new GetInvitationLinksResponse(response));
		}, "GetInvitationLinks");
	}

	public AddressBookServiceResponse CheckServerAccessibility(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CheckServerAccessibility(serverConfigurationParams);
		return new AddressBookServiceResponse(_connection.ProcessResult(iResult, timeout));
	}

	public AddressBookServiceResponse SetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = SetServerEndpoints(serverConfigurationParams);
		return new AddressBookServiceResponse(_connection.ProcessResult(iResult, timeout));
	}

	public AddressBookServiceResponse GetServerEndpoints(ServerConfigurationParams serverConfigurationParams, TimeSpan timeout)
	{
		IAsyncResult serverEndpoints = GetServerEndpoints(serverConfigurationParams);
		return new AddressBookServiceResponse(_connection.ProcessResult(serverEndpoints, timeout));
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
