using MediatR;
using VideoOS.DataCollector.Service.Common.Models;

namespace VideoOS.DataCollector.Service.Common;

public abstract record SendRequestBase<TResponse> : ISendRequest<TResponse>, IRequest<TResponse>, IBaseRequest, ILoggingRequest<TResponse>
{
	private readonly RetrySettings _retrySettings;

	private readonly LogSettings _logSettings;

	protected SendRequestBase(RetrySettings? retrySettings = null, LogSettings? logSettings = null)
	{
		_retrySettings = retrySettings ?? new RetrySettings();
		LogSettings? obj = logSettings ?? new LogSettings();
		_logSettings = obj with
		{
			Message = "Sending:"
		};
	}

	public virtual RetrySettings GetRetrySettings()
	{
		return _retrySettings;
	}

	public virtual LogSettings GetLogSettings()
	{
		return _logSettings;
	}
}
