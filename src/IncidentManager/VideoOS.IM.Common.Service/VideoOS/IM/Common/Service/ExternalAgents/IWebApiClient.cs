using System;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.IM.Common.Service.Properties;

namespace VideoOS.IM.Common.Service.ExternalAgents;

public interface IWebApiClient
{
	Task<TResponse> ExecuteGetRequestAsync<TResponse>([NotNull] Uri apiHostName, [NotNull] string path, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : class;

	Task<TResponse> ExecutePostRequestAsync<TRequestBody, TResponse>([NotNull] Uri apiHostName, [NotNull] string path, [CanBeNull] TRequestBody requestBody = null, CancellationToken cancellationToken = default(CancellationToken)) where TRequestBody : class where TResponse : class;

	Task<TResponse> ExecutePutRequestAsync<TRequestBody, TResponse>([NotNull] Uri apiHostName, [NotNull] string path, [CanBeNull] TRequestBody requestBody = null, CancellationToken cancellationToken = default(CancellationToken)) where TRequestBody : class where TResponse : class;

	Task<TResponse> ExecuteDeleteRequestAsync<TResponse>([NotNull] Uri apiHostName, [NotNull] string path, CancellationToken cancellationToken = default(CancellationToken)) where TResponse : class;
}
