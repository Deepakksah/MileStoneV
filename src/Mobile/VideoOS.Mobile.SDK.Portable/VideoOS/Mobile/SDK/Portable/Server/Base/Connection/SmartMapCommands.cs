using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class SmartMapCommands : IErrorResult, ISmartMapAsync, ISmartMapSync
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public SmartMapCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult GetTileServers(OnResponse<GetTileServersResponse> onSuccess = null, OnResponse<GetTileServersResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetTileServers);
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetTileServersResponse(response));
	}

	public GetTileServersResponse GetTileServers(TimeSpan timeout)
	{
		IAsyncResult tileServers = GetTileServers();
		return new GetTileServersResponse(_connection.ProcessResult(tileServers, timeout));
	}
}
