using System;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;
using VideoOS.Mobile.SDK.Portable.Server.Base.Connection;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ResponseHandler<T> : IErrorResult where T : BaseCommandResponse
{
	public VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection Connection { get; }

	public IAsyncResult AsyncResult { get; set; }

	public Command.CommandResultTypes Result { get; private set; }

	public string ErrorMessage { get; private set; }

	public ErrorCodes ErrorCode { get; private set; }

	public event EventHandler<ResponseHandlerEventArgs<T>> OnResponse;

	public ResponseHandler(VideoOS.Mobile.SDK.Portable.Server.Base.Connection.Connection connection)
	{
		Connection = connection;
	}

	public void OnResponseHandler(T connectResponse)
	{
		if (connectResponse != null)
		{
			Result = ((IErrorResult)connectResponse).Result;
			ErrorMessage = ((IErrorResult)connectResponse).ErrorMessage;
			ErrorCode = ((IErrorResult)connectResponse).ErrorCode;
		}
		else
		{
			Result = Command.CommandResultTypes.ResultError;
			ErrorMessage = "Command Timed Out !";
			ErrorCode = ErrorCodes.CommandTimedOut;
		}
		InvokeOnResponse(connectResponse);
	}

	protected virtual void InvokeOnResponse(T response)
	{
		this.OnResponse?.Invoke(this, new ResponseHandlerEventArgs<T>
		{
			Response = response
		});
	}
}
