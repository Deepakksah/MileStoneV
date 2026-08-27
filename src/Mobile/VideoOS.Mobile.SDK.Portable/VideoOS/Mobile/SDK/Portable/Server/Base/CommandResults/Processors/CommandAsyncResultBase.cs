using System;
using System.Threading;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults.Processors;

public class CommandAsyncResultBase : IAsyncResult, IErrorResult
{
	private readonly ManualResetEvent _resetEvent;

	public IConnection Connection { get; private set; }

	public object AsyncState { get; private set; }

	public WaitHandle AsyncWaitHandle => _resetEvent;

	public bool CompletedSynchronously { get; private set; }

	public bool IsCompleted { get; private set; }

	public Command.CommandResultTypes Result { get; private set; }

	public string ErrorMessage { get; private set; }

	public ErrorCodes ErrorCode { get; private set; }

	public CommandAsyncResultBase(IConnection iConnection)
	{
		Connection = iConnection;
		Result = Command.CommandResultTypes.ResultError;
		IsCompleted = false;
		CompletedSynchronously = false;
		AsyncState = null;
		_resetEvent = new ManualResetEvent(IsCompleted);
	}

	public virtual BaseCommandResponse OnError(Command request, Command response)
	{
		return new BaseCommandResponse(response);
	}

	public virtual BaseCommandResponse OnCommand(Command request, Command response)
	{
		return new BaseCommandResponse(response);
	}

	public void ProcessResult(CommunicationEntry response, CommunicationEntry request, OnResponse<BaseCommandResponse> onSuccess, OnResponse<BaseCommandResponse> onFail)
	{
		Result = response.Command.GetCommandResultType();
		ErrorMessage = response.Command.ErrorString;
		ErrorCode = response.Command.GetErrorCode();
		OnResponse<BaseCommandResponse> onResponse = onSuccess;
		Func<Command, Command, BaseCommandResponse> func = OnCommand;
		BaseCommandResponse baseCommandResponse = null;
		if (Result != Command.CommandResultTypes.ResultOk)
		{
			onResponse = onFail;
			func = OnError;
		}
		try
		{
			response.Command.ConvertToDictionary(Command.ParamsType.Output);
			baseCommandResponse = func(request.Command, response.Command);
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "CommandAsyncResult.ProcessResult", ex.Message);
			Result = Command.CommandResultTypes.ResultError;
			ErrorMessage = "Wrong Server Response !";
			onResponse = onFail;
			baseCommandResponse = BaseCommandResponse.Get(this, this);
		}
		onResponse?.Invoke(baseCommandResponse);
		AsyncState = baseCommandResponse;
		IsCompleted = true;
		_resetEvent.Set();
	}
}
