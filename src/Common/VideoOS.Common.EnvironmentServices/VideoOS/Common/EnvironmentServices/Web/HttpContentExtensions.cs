using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Common.EnvironmentServices.Web;

public static class HttpContentExtensions
{
	public static Task<T> ReadAsAsync<T, TFormatter>(this HttpContent inner, IEnumerable<TFormatter> genericFormatters)
	{
		IEnumerable<MediaTypeFormatter> enumerable = (IEnumerable<MediaTypeFormatter>)genericFormatters;
		return HttpContentExtensions.ReadAsAsync<T>(inner, enumerable);
	}

	public static Task<T> ReadAsAsync<T, TFormatter>(this HttpContent inner, IEnumerable<TFormatter> genericFormatters, CancellationToken cancellationToken)
	{
		IEnumerable<MediaTypeFormatter> enumerable = (IEnumerable<MediaTypeFormatter>)genericFormatters;
		return HttpContentExtensions.ReadAsAsync<T>(inner, enumerable, cancellationToken);
	}
}
