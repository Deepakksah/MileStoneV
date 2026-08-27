using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using VideoOS.DataCollector.Service.Common.BackOff;
using VideoOS.DataCollector.Service.Common.ResultResponse;

namespace VideoOS.DataCollector.Service.Common.Behaviours;

public class SendBehaviour<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse> where TRequest : ISendRequest<TResponse> where TResponse : IResult
{
	private readonly IBackOffFactory _factory;

	private readonly ILogger<TRequest> _logger;

	private readonly int _customMaxRetryValue = int.MaxValue;

	public SendBehaviour(IBackOffFactory factory, ILogger<TRequest> logger, int customMaxRetryValue)
		: this(factory, logger)
	{
		_customMaxRetryValue = customMaxRetryValue;
	}

	public SendBehaviour(IBackOffFactory factory, ILogger<TRequest> logger)
	{
		ArgumentNullException.ThrowIfNull(factory, "factory");
		_factory = factory;
		ArgumentNullException.ThrowIfNull(logger, "logger");
		_logger = logger;
	}

	public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
	{
		request.GetRetrySettings().Deconstruct(out var MaxRetryAttempts, out var MaxRetryDelayMilliseconds, out var RetryDelayMilliseconds);
		int maxRetryAttempts = MaxRetryAttempts;
		int maxDelayMilliseconds = MaxRetryDelayMilliseconds;
		int delayMilliseconds = RetryDelayMilliseconds;
		IBackOffProvider backOff = _factory.Create(delayMilliseconds, maxDelayMilliseconds);
		Exception lastException = null;
		List<Error> exceptions = new List<Error>();
		int retry = 0;
		TResponse response = default(TResponse);
		while (retry < maxRetryAttempts)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				exceptions.Add(Error.Failure("Request was cancelled"));
				return CreateResponse(exceptions);
			}
			if (maxRetryAttempts != _customMaxRetryValue)
			{
				retry++;
			}
			try
			{
				response = await next();
			}
			catch (Exception ex)
			{
				if ((!(ex is ThreadInterruptedException) && !(ex is CommunicationException) && !(ex is TimeoutException)) || 1 == 0)
				{
					throw;
				}
				exceptions.Add(Error.Failure(ex.Message));
				if (lastException == null || lastException.GetType() != ex.GetType())
				{
					_logger.LogError($"Failed: {request.GetType().Name} - {ex}");
					lastException = ex;
				}
				if (retry >= maxRetryAttempts)
				{
					return CreateResponse(exceptions);
				}
				_logger.LogInformation("Retrying: " + request.GetType().Name);
				await backOff.Delay(cancellationToken);
				continue;
			}
			break;
		}
		return response;
	}

	private static TResponse CreateResponse(List<Error> exceptions)
	{
		return (TResponse)(typeof(TResponse).GetMethod("From", BindingFlags.Static | BindingFlags.Public, new Type[1] { typeof(List<Error>) })?.Invoke(null, new object[1] { exceptions }));
	}
}
