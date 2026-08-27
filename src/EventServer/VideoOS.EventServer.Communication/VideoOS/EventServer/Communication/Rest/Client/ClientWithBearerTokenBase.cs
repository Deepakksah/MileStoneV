using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.EventServer.Communication.Rest.Client;

public abstract class ClientWithBearerTokenBase : IClientWithBearerToken
{
	public string BearerToken { get; set; }

	public Func<string> BearerTokenProvider { get; set; }

	protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Expected O, but got Unknown
		//IL_003e: Expected O, but got Unknown
		if (BearerTokenProvider != null)
		{
			BearerToken = BearerTokenProvider();
		}
		HttpRequestMessage val = new HttpRequestMessage();
		val.Headers.Authorization = new AuthenticationHeaderValue("Bearer", BearerToken);
		return Task.FromResult<HttpRequestMessage>(val);
	}

	public static async Task<T> WithRetryAsync<T, TApiException>(Func<Task<T>> call, int retryCount, int retryPeriodMs, Func<TApiException, int> exceptionToStatusCode) where TApiException : Exception
	{
		if (call == null)
		{
			throw new ArgumentNullException("call");
		}
		if (retryPeriodMs < 0)
		{
			throw new ArgumentException("Must be >= 0", "retryPeriodMs");
		}
		if (retryCount < 1)
		{
			throw new ArgumentException("Must be >= 1", "retryCount");
		}
		int retry = retryCount;
		while (retry-- > 0)
		{
			try
			{
				return await call().ConfigureAwait(continueOnCapturedContext: false);
			}
			catch (TApiException arg)
			{
				int num = exceptionToStatusCode(arg);
				if ((num != 502 && num != 503) || retry == 0)
				{
					throw;
				}
			}
			catch (InvalidOperationException)
			{
				throw;
			}
			catch (HttpRequestException)
			{
				if (retry == 0)
				{
					throw;
				}
			}
			if (retryPeriodMs > 0)
			{
				await Task.Delay(retryPeriodMs).ConfigureAwait(continueOnCapturedContext: false);
			}
		}
		return default(T);
	}

	public static async Task<T> WithRetryAsync<T, TApiException>(Func<Task<T>> call, Func<TApiException, int> exceptionToStatusCode) where TApiException : Exception
	{
		return await WithRetryAsync(call, 3, 500, exceptionToStatusCode).ConfigureAwait(continueOnCapturedContext: false);
	}
}
