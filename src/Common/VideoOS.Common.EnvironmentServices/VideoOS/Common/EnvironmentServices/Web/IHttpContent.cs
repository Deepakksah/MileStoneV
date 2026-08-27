using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public interface IHttpContent
{
	IHttpContentHeaders Headers { get; }

	Task<string> ReadAsStringAsync();

	Task<string> ReadAsStringAsync(CancellationToken cancellationToken);

	Task<Stream> ReadAsStreamAsync();

	Task<Stream> ReadAsStreamAsync(CancellationToken cancellationToken);

	Task<T> ReadAsAsync<T, TFormatter>(IEnumerable<TFormatter> formatters);

	Task<T> ReadAsAsync<T, TFormatter>(IEnumerable<TFormatter> formatters, CancellationToken cancellationToken);
}
