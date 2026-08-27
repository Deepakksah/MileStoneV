using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.IdentityServer.Common;

public interface IHttpGet
{
	CancellationToken CancelToken { get; set; }

	Task<IHttpGetResult> GetResponseAsync(Uri uri);

	Task<IHttpGetResult> GetResponseAsync(Uri uri, int timeout);
}
