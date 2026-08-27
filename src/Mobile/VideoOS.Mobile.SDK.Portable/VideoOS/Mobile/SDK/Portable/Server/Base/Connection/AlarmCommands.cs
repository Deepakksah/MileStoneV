using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class AlarmCommands : IAlarmsAsync, IAlarmsSync, IErrorResult
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public AlarmCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult GetAlarmList(GetAlarmParams getAlarmParams, OnResponse<GetAlarmListResponse> onSuccess = null, OnResponse<GetAlarmListResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetAlarmList);
		getAlarmParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetAlarmListResponse(response));
	}

	public IAsyncResult UpdateAlarm(UpdateAlarmParams updateAlarmParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.UpdateAlarm);
		updateAlarmParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult GetAlarmDataSettings(OnResponse<GetAlarmDataSettingsResponse> onSuccess = null, OnResponse<GetAlarmDataSettingsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetAlarmDataSettings);
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetAlarmDataSettingsResponse(response));
	}

	public IAsyncResult GetPermittedUsers(PermittedUsersParams permittedUsersParams, OnResponse<GetPermittedUsersResponse> onSuccess = null, OnResponse<GetPermittedUsersResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.GetPermittedUsers);
		permittedUsersParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new GetPermittedUsersResponse(response));
	}

	public IAsyncResult AcknowledgeAlarm(AcknowledgeAlarmParams acknowledgeAlarmParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.AcknowledgeAlarm);
		acknowledgeAlarmParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public GetAlarmListResponse GetAlarmList(GetAlarmParams getAlarmParams, TimeSpan timeout)
	{
		IAsyncResult alarmList = GetAlarmList(getAlarmParams);
		return new GetAlarmListResponse(_connection.ProcessResult(alarmList, timeout));
	}

	public BaseCommandResponse UpdateAlarm(UpdateAlarmParams updateAlarmParams, TimeSpan timeout)
	{
		IAsyncResult iResult = UpdateAlarm(updateAlarmParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public GetAlarmDataSettingsResponse GetAlarmDataSettings(TimeSpan timeout)
	{
		IAsyncResult alarmDataSettings = GetAlarmDataSettings();
		return new GetAlarmDataSettingsResponse(_connection.ProcessResult(alarmDataSettings, timeout));
	}

	public BaseCommandResponse GetPermittedUsers(PermittedUsersParams permittedUsersParams, TimeSpan timeout)
	{
		IAsyncResult permittedUsers = GetPermittedUsers(permittedUsersParams);
		return _connection.ProcessResult(permittedUsers, timeout);
	}

	public BaseCommandResponse AcknowledgeAlarm(AcknowledgeAlarmParams acknowledgeAlarmParams, TimeSpan timeout)
	{
		IAsyncResult iResult = AcknowledgeAlarm(acknowledgeAlarmParams);
		return _connection.ProcessResult(iResult, timeout);
	}
}
