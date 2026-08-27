using System;
using System.ServiceModel;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.LogServer.Communication.CommunicationFactory;
using VideoOS.LogServer.Communication.LogReaderServiceReference;
using VideoOS.LogServer.Communication.LogWriterServiceReference;

namespace VideoOS.LogServer.Communication;

public class ClientWrapper<T> : IClientWrapper<T> where T : ICommunicationObject
{
	private readonly IClientFactory<T> _clientFactory;

	private readonly ILogger _logger;

	private T _client;

	private int _retryCount = 1;

	private object _clientLock = new object();

	public int RetryCount
	{
		get
		{
			return _retryCount;
		}
		set
		{
			if (value < 0)
			{
				throw new ArgumentOutOfRangeException("value", "Retry count cannot be less than 0");
			}
			_retryCount = value;
		}
	}

	private T Client
	{
		get
		{
			lock (_clientLock)
			{
				if (object.Equals(_client, default(T)) || _client.State == CommunicationState.Faulted || _client.State == CommunicationState.Closing || _client.State == CommunicationState.Closed)
				{
					_client = _clientFactory.Create();
				}
				return _client;
			}
		}
	}

	public ClientWrapper(IClientFactory<T> clientFactory, ILogger logger)
	{
		if (clientFactory == null)
		{
			throw new ArgumentNullException("clientFactory");
		}
		if (logger == null)
		{
			throw new ArgumentNullException("logger");
		}
		_clientFactory = clientFactory;
		_logger = logger;
	}

	public async Task<TRes> CallWithRetryAsync<TRes>(Func<T, Task<TRes>> func)
	{
		if (func == null)
		{
			throw new ArgumentNullException("func");
		}
		int count = RetryCount;
		while (true)
		{
			T localClient = Client;
			try
			{
				return await func(localClient);
			}
			catch (TimeoutException ex)
			{
				LoggerExtensions.LogError(_logger, "Timeout error: {0}", new object[1] { ex });
				localClient.Abort();
				throw new CommunicationServiceException(CommunicationExceptionMessage.TimeoutError);
			}
			catch (FaultException<VideoOS.LogServer.Communication.LogWriterServiceReference.UserValidationFault> ex2)
			{
				LoggerExtensions.LogError(_logger, "LogWriterServiceReference.UserValidationFault: {0}", new object[1] { ex2 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.UserValidationError);
			}
			catch (FaultException<VideoOS.LogServer.Communication.LogReaderServiceReference.UserValidationFault> ex3)
			{
				LoggerExtensions.LogError(_logger, "LogReaderServiceReference.UserValidationFault: {0}", new object[1] { ex3 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.UserValidationError);
			}
			catch (FaultException<UnknownSessionFault> ex4)
			{
				LoggerExtensions.LogError(_logger, "LogReaderServiceReference.UnknownSessionFault: {0}", new object[1] { ex4 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.UnknownSessionError);
			}
			catch (FaultException<QueryTimeoutFault> ex5)
			{
				LoggerExtensions.LogError(_logger, "LogReaderServiceReference.QueryTimeoutFault: {0}", new object[1] { ex5 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.QueryTimeoutError);
			}
			catch (FaultException ex6)
			{
				LoggerExtensions.LogError(_logger, "Fault error: {0}", new object[1] { ex6 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.FaultError);
			}
			catch (CommunicationException ex7)
			{
				localClient.Abort();
				if (count > 0)
				{
					LoggerExtensions.LogWarning(_logger, "Communication error (retry): {0}", new object[1] { ex7 });
					count--;
					continue;
				}
				LoggerExtensions.LogError(_logger, "Communication error: {0}", new object[1] { ex7 });
				throw new CommunicationServiceException(CommunicationExceptionMessage.CommunicationError);
			}
			catch (Exception ex8)
			{
				LoggerExtensions.LogError(_logger, "Unexpected error occurred: {0}", new object[1] { ex8 });
				localClient.Abort();
				throw;
			}
		}
	}
}
