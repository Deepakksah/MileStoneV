using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpResponseHeaders : HttpHeaders, IHttpResponseHeaders, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
	public HttpResponseHeaders(HttpResponseHeaders requestHeaders)
		: base((HttpHeaders)(object)requestHeaders)
	{
	}
}
