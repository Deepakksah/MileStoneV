using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.WebApi;

public static class HttpRequestMessageExtensions
{
	public static async Task<HttpRequestMessage> CloneAsync(this HttpRequestMessage httpRequestMessage)
	{
		HttpRequestMessage httpRequestMessageClone = new HttpRequestMessage(httpRequestMessage.Method, httpRequestMessage.RequestUri);
		if (httpRequestMessage.Content != null)
		{
			MemoryStream ms = new MemoryStream();
			await httpRequestMessage.Content.CopyToAsync((Stream)ms);
			ms.Position = 0L;
			httpRequestMessageClone.Content = (HttpContent)new StreamContent((Stream)ms);
			((IEnumerable<KeyValuePair<string, IEnumerable<string>>>)httpRequestMessage.Content.Headers)?.ToList().ForEach(delegate(KeyValuePair<string, IEnumerable<string>> header)
			{
				((HttpHeaders)httpRequestMessageClone.Content.Headers).Add(header.Key, header.Value);
			});
		}
		httpRequestMessageClone.Version = httpRequestMessage.Version;
		httpRequestMessage.Properties.ToList().ForEach(delegate(KeyValuePair<string, object> props)
		{
			httpRequestMessageClone.Properties.Add(props);
		});
		((IEnumerable<KeyValuePair<string, IEnumerable<string>>>)httpRequestMessage.Headers).ToList().ForEach(delegate(KeyValuePair<string, IEnumerable<string>> header)
		{
			((HttpHeaders)httpRequestMessageClone.Headers).TryAddWithoutValidation(header.Key, header.Value);
		});
		return httpRequestMessageClone;
	}
}
