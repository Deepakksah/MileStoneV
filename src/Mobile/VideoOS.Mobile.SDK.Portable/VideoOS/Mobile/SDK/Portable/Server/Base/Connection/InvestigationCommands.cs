using System;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.Portable.Utilities;
using VideoOS.Mobile.Portable.VideoChannel.Params;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class InvestigationCommands : IInvestigationsAsync, IInvestigationsSync, IInvestigationsAwait, IErrorResult
{
	private readonly IConnection _connection;

	private readonly IErrorResult _errorResult;

	public Command.CommandResultTypes Result => _errorResult.Result;

	public string ErrorMessage => _errorResult.ErrorMessage;

	public ErrorCodes ErrorCode => _errorResult.ErrorCode;

	public InvestigationCommands(IConnection connection)
	{
		_connection = connection;
		_errorResult = connection as IErrorResult;
	}

	public IAsyncResult CreateInvestigation(InvestigationParams investigationParams, OnResponse<CreateInvestigationResponse> onSuccess = null, OnResponse<CreateInvestigationResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.CreateInvestigation);
		investigationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail, (BaseCommandResponse response) => new CreateInvestigationResponse(response));
	}

	public IAsyncResult UpdateInvestigation(InvestigationParams investigationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.UpdateInvestigation);
		investigationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult UpdateInvestigationData(InvestigationParams investigationParams, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command tCommand = new Command();
		tCommand.SetCommandKind(Command.CommandKinds.UpdateInvestigationData);
		investigationParams.ConvertToCommand(ref tCommand);
		return _connection.SendCommand(tCommand, onSuccess, onFail);
	}

	public IAsyncResult CancelInvestigationUpdate(Guid investigationId, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.CancelInvestigationUpdate);
		command.InputParams.Add(new Parameter("ItemId", investigationId.ToString()));
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public IAsyncResult DeleteInvestigation(Guid investigationId, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.DeleteInvestigation);
		command.InputParams.Add(new Parameter("ItemId", investigationId.ToString()));
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public IAsyncResult GetInvestigation(Guid investigationId, bool includePackages = false, OnResponse<GetInvestigationsResponse> onSuccess = null, OnResponse<GetInvestigationsResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetInvestigation);
		command.InputParams.Add(new Parameter("ItemId", investigationId.ToString()));
		command.InputParams.Add(new Parameter("IncludePackages", ParamsHelper.BoolConvertionTo(includePackages)));
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new GetInvestigationsResponse(response));
	}

	public IAsyncResult CheckForInvestigationChanges(OnResponse<CheckForChangesResponse> onSuccess = null, OnResponse<CheckForChangesResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.GetChangesStateId);
		command.InputParams.Add(new Parameter("Investigations", "Yes"));
		return _connection.SendCommand(command, onSuccess, onFail, (BaseCommandResponse response) => new CheckForChangesResponse(response));
	}

	public CreateInvestigationResponse CreateInvestigation(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = CreateInvestigation(investigationParams);
		return new CreateInvestigationResponse(_connection.ProcessResult(iResult, timeout));
	}

	public BaseCommandResponse UpdateInvestigation(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = UpdateInvestigation(investigationParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse UpdateInvestigationData(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult iResult = UpdateInvestigationData(investigationParams);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse CancelInvestigationUpdate(Guid investigationId, TimeSpan timeout)
	{
		IAsyncResult iResult = CancelInvestigationUpdate(investigationId);
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse DeleteInvestigation(Guid investigationId, TimeSpan timeout)
	{
		IAsyncResult iResult = DeleteInvestigation(investigationId);
		return _connection.ProcessResult(iResult, timeout);
	}

	public GetInvestigationsResponse GetInvestigation(Guid investigationId, TimeSpan timeout, bool includePackages = false)
	{
		IAsyncResult investigation = GetInvestigation(investigationId, includePackages);
		return new GetInvestigationsResponse(_connection.ProcessResult(investigation, timeout));
	}

	public CheckForChangesResponse CheckForInvestigationChanges(TimeSpan timeout)
	{
		IAsyncResult iResult = CheckForInvestigationChanges();
		return new CheckForChangesResponse(_connection.ProcessResult(iResult, timeout));
	}

	public async Task<CreateInvestigationResponse> CreateInvestigationAsync(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = CreateInvestigation(investigationParams);
		return new CreateInvestigationResponse(await _connection.ProcessResultTask(asyncResult, timeout));
	}

	public async Task<BaseCommandResponse> UpdateInvestigationAsync(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = UpdateInvestigation(investigationParams);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}

	public async Task<BaseCommandResponse> UpdateInvestigationDataAsync(InvestigationParams investigationParams, TimeSpan timeout)
	{
		IAsyncResult asyncResult = UpdateInvestigationData(investigationParams);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}

	public async Task<BaseCommandResponse> CancelInvestigationUpdateAsync(Guid investigationId, TimeSpan timeout)
	{
		IAsyncResult asyncResult = CancelInvestigationUpdate(investigationId);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}

	public async Task<BaseCommandResponse> DeleteInvestigationAsync(Guid investigationId, TimeSpan timeout)
	{
		IAsyncResult asyncResult = DeleteInvestigation(investigationId);
		return await _connection.ProcessResultTask(asyncResult, timeout);
	}

	public async Task<GetInvestigationsResponse> GetInvestigationAsync(Guid investigationId, TimeSpan timeout, bool includePackages = false)
	{
		IAsyncResult investigation = GetInvestigation(investigationId, includePackages);
		return new GetInvestigationsResponse(await _connection.ProcessResultTask(investigation, timeout));
	}

	public async Task<CheckForChangesResponse> CheckForInvestigationChangesAsync(TimeSpan timeout)
	{
		IAsyncResult asyncResult = CheckForInvestigationChanges();
		return new CheckForChangesResponse(await _connection.ProcessResultTask(asyncResult, timeout));
	}
}
