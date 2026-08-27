using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.Connection;

public class TwoStepVerification : ITwoStepVerificationAsync, ITwoStepVerificationSync
{
	private readonly IConnection _connection;

	private readonly OnResponse<BaseCommandResponse> _onSuccessVerifyCode;

	public TwoStepVerification(IConnection connection, OnResponse<BaseCommandResponse> onSuccessVerifyCode)
	{
		_connection = connection;
		_onSuccessVerifyCode = onSuccessVerifyCode;
	}

	public IAsyncResult RequestSecondStepVerificationCode(OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.RequestSecondStepVerificationCode);
		return _connection.SendCommand(command, onSuccess, onFail);
	}

	public IAsyncResult VerifySecondStepVerificationCode(string code, OnResponse<BaseCommandResponse> onSuccess = null, OnResponse<BaseCommandResponse> onFail = null)
	{
		Command command = new Command();
		command.SetCommandKind(Command.CommandKinds.VerifySecondStepVerificationCode);
		command.InputParams.Add(new Parameter("SecondStepAuthenticationPin", code));
		return _connection.SendCommand(command, (OnResponse<BaseCommandResponse>)Delegate.Combine(_onSuccessVerifyCode, onSuccess), onFail);
	}

	public BaseCommandResponse RequestSecondStepVerificationCode(TimeSpan timeout)
	{
		IAsyncResult iResult = RequestSecondStepVerificationCode();
		return _connection.ProcessResult(iResult, timeout);
	}

	public BaseCommandResponse VerifySecondStepVerificationCode(string code, TimeSpan timeout)
	{
		IAsyncResult iResult = VerifySecondStepVerificationCode(code);
		return _connection.ProcessResult(iResult, timeout);
	}
}
