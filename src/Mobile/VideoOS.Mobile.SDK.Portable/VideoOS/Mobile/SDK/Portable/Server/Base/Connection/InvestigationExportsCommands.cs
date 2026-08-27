using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class InvestigationExportsCommands : IInvestigationExportsAsync, IInvestigationExportsSync, IErrorResult
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public InvestigationExportsCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult StartInvestigationExport(ExportParams exportParams, OnResponse<StartExportResponse> onSuccess = null, OnResponse<StartExportResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.StartInvestigationExport);
		exportParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new StartExportResponse(response));
	}

	public IAsyncResult DeleteInvestigationExport(ExportParams exportParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.DeleteInvestigationExport);
		exportParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public StartExportResponse StartInvestigationExport(ExportParams exportParams, TimeSpan timeout)
	{
		IAsyncResult iResult = StartInvestigationExport(exportParams);
		return new StartExportResponse(_connection.ProcessResult(iResult, timeout));
	}

	public BaseCommandResponse DeleteInvestigationExport(ExportParams exportParams, TimeSpan timeout)
	{
		IAsyncResult iResult = DeleteInvestigationExport(exportParams);
		return _connection.ProcessResult(iResult, timeout);
	}
}
