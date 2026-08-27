using System;
using System.Threading.Tasks;
using VideoOS.EventServer.Common.RestApi.Client;

namespace VideoOS.EventServer.Events.RestApi.Client;

public abstract class ClientWithBearerToken : ClientWithBearerTokenBase
{
	public static async Task<T> WithRetryAsync<T>(Func<Task<T>> call, int retryCount, int retryPeriodMs)
	{
		return await ClientWithBearerTokenBase.WithRetryAsync(call, retryCount, retryPeriodMs, (ApiException apiExpeception) => apiExpeception.StatusCode).ConfigureAwait(continueOnCapturedContext: false);
	}

	public static async Task<T> WithRetryAsync<T>(Func<Task<T>> call)
	{
		return await ClientWithBearerTokenBase.WithRetryAsync(call, (ApiException apiExpeception) => apiExpeception.StatusCode).ConfigureAwait(continueOnCapturedContext: false);
	}
}
