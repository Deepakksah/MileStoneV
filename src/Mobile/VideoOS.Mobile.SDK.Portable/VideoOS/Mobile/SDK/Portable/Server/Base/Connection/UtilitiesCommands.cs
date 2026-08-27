using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class UtilitiesCommands : IErrorResult, IUtilitiesSync, IUtilitiesAsync
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	private Action<ServerFeatures> _serverFeatures;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public UtilitiesCommands(IConnection connection, Action<ServerFeatures> onServerFeatures)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
		_serverFeatures = onServerFeatures;
	}

	public GetServerStatusResponse GetServerStatus(GetServerStatusParams serverStatusParams, TimeSpan timeout)
	{
		IAsyncResult serverStatus = GetServerStatus(serverStatusParams);
		return new GetServerStatusResponse(_connection.ProcessResult(serverStatus, timeout));
	}

	public GetConfigurationResponse GetConfiguration(string name, Guid id, TimeSpan timeout)
	{
		IAsyncResult configuration = GetConfiguration(name, id);
		return new GetConfigurationResponse(_connection.ProcessResult(configuration, timeout));
	}

	public CameraCapabilitiesResponse GetCapabilities(CapabilitiesParams tCapabilitiesParams, TimeSpan timeout)
	{
		IAsyncResult capabilities = GetCapabilities(tCapabilitiesParams);
		return new CameraCapabilitiesResponse(_connection.ProcessResult(capabilities, timeout));
	}

	public ServerCapabilitiesResponse GetServerCapabilities(TimeSpan timeout)
	{
		CapabilitiesParams tCapabilitiesParams = new CapabilitiesParams
		{
			ItemKind = "Server"
		};
		IAsyncResult capabilities = GetCapabilities(tCapabilitiesParams);
		return new ServerCapabilitiesResponse(_connection.ProcessResult(capabilities, timeout));
	}

	public IAsyncResult GetServerStatus(GetServerStatusParams tServerStatusParams = null, OnResponse<GetServerStatusResponse> onSuccess = null, OnResponse<GetServerStatusResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetServerStatus);
		tServerStatusParams?.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetServerStatusResponse(response));
	}

	public IAsyncResult GetConfiguration(string name, Guid id, OnResponse<GetConfigurationResponse> onSuccess = null, OnResponse<GetConfigurationResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetConfiguration);
		if (string.IsNullOrWhiteSpace(name))
		{
			command.InputParams.Add(new Parameter("Name", name));
		}
		if (id != Guid.Empty)
		{
			command.InputParams.Add(new Parameter("Id", id.ToString()));
		}
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetConfigurationResponse(response));
	}

	public IAsyncResult GetCapabilities(CapabilitiesParams tCapabilitiesParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetCapabilities);
		tCapabilitiesParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, delegate(BaseCommandResponse response)
		{
			if (tCapabilitiesParams.ItemKind == "Server" && response != null && response.ErrorCode == ErrorCodes.Ok)
			{
				ServerFeatures serverFeatures = new ServerFeatures();
				serverFeatures.InitFromCommand(response.OutputParams);
				_serverFeatures(serverFeatures);
			}
			onSuccess?.Invoke(response);
		}, onFail);
	}

	public IAsyncResult GetServerCapabilities(CapabilitiesParams tCapabilitiesParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetCapabilities);
		tCapabilitiesParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}
}
