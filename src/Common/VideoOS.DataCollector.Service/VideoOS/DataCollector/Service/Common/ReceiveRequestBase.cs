using MediatR;
using VideoOS.DataCollector.Service.Common.Models;

namespace VideoOS.DataCollector.Service.Common;

public abstract record ReceiveRequestBase<TResponse> : ILoggingRequest<TResponse>, IRequest<TResponse>, IBaseRequest
{
	private readonly LogSettings _logSettings;

	protected ReceiveRequestBase(LogSettings? logSettings = null)
	{
		LogSettings? obj = logSettings ?? new LogSettings();
		_logSettings = obj with
		{
			Message = "Received:"
		};
	}

	public virtual LogSettings GetLogSettings()
	{
		return _logSettings;
	}
}
