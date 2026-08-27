using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;

namespace VideoOS.Common.EnvironmentServices.Web;

public class HttpHeaders : IHttpResponseHeaders, IEnumerable<KeyValuePair<string, IEnumerable<string>>>, IEnumerable
{
	private readonly HttpHeaders _inner;

	public HttpHeaders(HttpHeaders requestHeaders)
	{
		_inner = requestHeaders;
	}

	public IEnumerator<KeyValuePair<string, IEnumerable<string>>> GetEnumerator()
	{
		foreach (KeyValuePair<string, IEnumerable<string>> item in _inner)
		{
			yield return item;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
