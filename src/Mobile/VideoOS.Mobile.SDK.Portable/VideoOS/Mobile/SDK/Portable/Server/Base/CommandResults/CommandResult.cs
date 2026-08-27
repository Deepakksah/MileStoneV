using System;
using System.Threading;
using VideoOS.Mobile.Portable.MetaChannel;
using VideoOS.Mobile.SDK.Portable.Server.Interfaces;

namespace VideoOS.Mobile.SDK.Portable.Server.Base.CommandResults;

public class CommandResult : IAsyncResult, IErrorResult
{
	private ManualResetEvent _resetEvent;

	private bool _isCompleted;

	public object AsyncState { get; set; }

	public WaitHandle AsyncWaitHandle => _resetEvent;

	public bool CompletedSynchronously { get; set; }

	public bool IsCompleted
	{
		get
		{
			return _isCompleted;
		}
		set
		{
			_isCompleted = value;
			if (_isCompleted)
			{
				_resetEvent.Set();
			}
		}
	}

	public Command.CommandResultTypes Result { get; set; }

	public string ErrorMessage { get; set; }

	public ErrorCodes ErrorCode { get; set; }

	public CommandResult()
	{
		InternalInit();
	}

	public CommandResult(IAsyncResult iAsyncResult, IErrorResult iErrorResult)
	{
		InternalInit();
		Init(iErrorResult);
		Init(iAsyncResult);
	}

	private void InternalInit()
	{
		Result = Command.CommandResultTypes.ResultError;
		ErrorCode = ErrorCodes.Unknown;
		IsCompleted = false;
		CompletedSynchronously = false;
		AsyncState = null;
		_resetEvent = new ManualResetEvent(IsCompleted);
	}

	public void Init(IAsyncResult iAsyncResult)
	{
		if (iAsyncResult != null)
		{
			AsyncState = iAsyncResult.AsyncState;
			CompletedSynchronously = iAsyncResult.CompletedSynchronously;
			IsCompleted = iAsyncResult.IsCompleted;
		}
	}

	public void Init(IErrorResult iErrorResult)
	{
		if (iErrorResult != null)
		{
			Result = iErrorResult.Result;
			ErrorCode = iErrorResult.ErrorCode;
			ErrorMessage = iErrorResult.ErrorMessage;
		}
	}
}
