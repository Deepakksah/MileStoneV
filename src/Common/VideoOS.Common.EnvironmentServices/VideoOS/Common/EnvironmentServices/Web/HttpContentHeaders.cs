using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpContentHeaders : HttpHeaders, IHttpContentHeaders, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
	public HttpContentHeaders(HttpContentHeaders requestHeaders)
		: base((HttpHeaders)(object)requestHeaders)
	{
	}
}
