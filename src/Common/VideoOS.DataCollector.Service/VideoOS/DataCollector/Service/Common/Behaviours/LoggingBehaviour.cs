using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.Models;

namespace VideoOS.DataCollector.Service.Common.Behaviours;

public class LoggingBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ILoggingRequest<TResponse>
{
	private readonly ILogger<TRequest> _logger;

	public LoggingBehaviour(ILogger<TRequest> logger)
	{
		ArgumentNullException.ThrowIfNull(logger, "logger");
		_logger = logger;
	}

	public Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		LogSettings logSettings = request.GetLogSettings();
		_logger.Log(logSettings.LogLevel, $"{logSettings.Message} {request}");
		return next();
	}
}
